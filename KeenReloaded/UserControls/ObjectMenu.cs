using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Enums;
using System.Diagnostics;
using System.IO;
using KeenReloaded.HelperClasses;
using System.Drawing.Imaging;

namespace KeenReloaded.UserControls
{
    public enum Mode
    {
        PLACEMENT,
        EDIT
    }
    public partial class ObjectMenu : UserControl
    {
        Dictionary<string, string> _categoryFolderDict;
        private const int SEPARATOR_DISTANCE = 4;
        private const int MAX_WIDTH_PER_ROW = 500;
        private int _currentCatX, _currentCatY;

        private KeenReloadedItemPic _selectedItem;
        private KeenReloadedItemPic _cursorItem;

        #region category item lists

        #endregion
        public ObjectMenu()
        {
            InitializeComponent();
        }

        public KeenReloadedItemPic CursorItem
        {
            get
            {
                return _cursorItem;
            }
        }

        private void ObjectMenu_Load(object sender, EventArgs e)
        {
            InitializeCategoryDictionary();
            LoadEpisodes();
            LoadBiomeTypes();
            LoadEditModes();
            LoadCategoryItems();
        }

        private void LoadEditModes()
        {
            var modes = Enum.GetValues(typeof(Mode));
            foreach (var mode in modes)
            {
                cmbEditMode.Items.Add(mode);
            }
            cmbEditMode.SelectedIndex = 0;
            cmbEditMode.SelectedIndexChanged += CmbEditMode_SelectedIndexChanged;
        }

        private void CmbEditMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Mode)
            {
                case Mode.EDIT:
                    var parent = this.Parent as MapMaker;
                    ClearParentCursorObject(parent);
                    AttachSelectionEvent(parent);
                    break;
                case Mode.PLACEMENT:

                    break;
            }
        }

        private void ClearParentCursorObject(MapMaker parent)
        {
            if (parent != null)
            {
                this.Parent.Controls.Remove(_cursorItem);
                _cursorItem = null;
            }
        }

        private void AttachSelectionEvent(MapMaker parent)
        {
            foreach (var control in parent.CanvasObjects)
            {
                var item = control as KeenReloadedItemPic;

                if (item != null && !item.IsDelegateHandling(nameof(Pic_Selected)))
                {
                    item.Selected += Pic_Selected;
                }
            }
        }

        public Mode Mode
        {
            get
            {
                return (Mode)Enum.Parse(typeof(Mode), cmbEditMode.SelectedItem.ToString());
            }
        }

        private void LoadBiomeTypes()
        {
            cmbBiome.Items.Clear();
            var selectedEpisode = Convert.ToInt32(cmbEpisode.SelectedItem);
            var types = Enum.GetValues(typeof(BiomeType));
            foreach (var t in types)
            {
                if (t.ToString().Contains(selectedEpisode.ToString()))
                {
                    cmbBiome.Items.Add(t);
                }
            }
            cmbBiome.SelectedIndex = 0;
        }

        private bool IsControlInPanel(Control control)
        {
            return pnlItems.Controls.Contains(control);
        }

        public void AttachObjectToSelectedEvent(KeenReloadedItemPic item)
        {
            item.Selected += Pic_Selected;
        }

        public void DetachObjectFromSelectedEvent(KeenReloadedItemPic item)
        {
            item.Selected -= Pic_Selected;
        }

        public void DeselectItem()
        {
            if (_selectedItem != null)
            {
                _selectedItem = null;
            }
        }

        public KeenReloadedItemPic SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            private set
            {
                _selectedItem = value;
            }
        }

        private void Pic_Selected(object sender, EventArgs e)
        {
            var item = sender as KeenReloadedItemPic;
            if (item != null && item.IsSelected)
            {
                //if we click on items on the map canvas in placement mode, just place
                //new item on map
                if (!IsControlInPanel(item) && Mode == Mode.PLACEMENT)
                {
                    var parent = this.Parent as MapMaker;
                    item.IsSelected = false;
                    if (parent != null)
                    {
                        parent.AddCursorItemToCanvas();
                    }
                    return;
                }

                if (_selectedItem != null)
                    _selectedItem.IsSelected = false;
                _selectedItem = item;
                pbSelectedItem.Image = _selectedItem.Image;

                if (this.Mode == Mode.PLACEMENT)
                {
                    SetNewCursorItem();
                }
                else if (this.Mode == Mode.EDIT)
                {
                    if (IsControlInPanel(item))
                    {
                        SetEditMode(Mode.PLACEMENT);
                        SetNewCursorItem();
                    }
                    else
                    {
                        SetPropertyValueControlPanel(item);
                    }
                }
            }
        }

        private void SetEditMode(Mode editMode)
        {
            cmbEditMode.SelectedIndex = (int)editMode;
            cmbEditMode.SelectedValue = editMode.ToString();
            cmbEditMode.Text = editMode.ToString();
            cmbEditMode.SelectedItem = cmbEditMode.Items[cmbEditMode.SelectedIndex];
        }

        private void SetPropertyValueControlPanel(KeenReloadedItemPic item)
        {
            pnlSpecialProperties.Controls.Clear();
            PopulatePropertyControlPanel(item.RawObjectCreationData);
        }

        private void SetNewCursorItem()
        {
            var imgFactory = ImageToObjectCreationFactory.Instance;
            string filename = _selectedItem.FileName.Substring(_selectedItem.FileName.LastIndexOf('\\') + 1).Replace(".png", "");
            pnlSpecialProperties.Controls.Clear();

            if (imgFactory.TryGetTypeItem(filename, out TypeConstructorValues val))
            {
                PopulatePropertyControlPanel(val);
            }
            if (_cursorItem != null)
                this.Parent.Controls.Remove(_cursorItem);

            if (val != null)
            {
                _cursorItem = new KeenReloadedItemPic(_selectedItem.Image, _selectedItem.FileName);
                _cursorItem.BackColor = Color.Transparent;
                _cursorItem.Image = _selectedItem.Image;
                _cursorItem.RawObjectCreationData = new TypeConstructorValues(val.Type, val.ConstructorParamNames, val.Values);
                this.Parent.Controls.Add(_cursorItem);
                _cursorItem.BringToFront();
            }
            else
            {
                _cursorItem = null;
                _selectedItem = null;
            }
        }

        private void PopulatePropertyControlPanel(TypeConstructorValues val)
        {
            int currentY = 0;
            for (int i = 0; i < val.ConstructorParamNames.Length; i++)
            {
                string propertyName = Convert.ToString(val.ConstructorParamNames[i]);
                object propertyValue = val.Values[i];
                PropertyValueControl pvc = new PropertyValueControl(propertyName, propertyValue);
                pvc.PropertyValueChanged += Pvc_PropertyValueChanged;
                pnlSpecialProperties.Controls.Add(pvc);
                pvc.Location = new Point(pvc.Location.X, currentY);
                currentY += pvc.Height + 10;
            }
        }

        private void Pvc_PropertyValueChanged(object sender, KeenReloadedEventArgs.PropertyValueControlValueChangedEventArgs e)
        {
            if (this.Mode == Mode.PLACEMENT)
            {
                EditFromPlacementMode(sender, e);
            }
            else if (this.Mode == Mode.EDIT)
            {
                EditFromEditMode(sender, e);
            }
        }

        private void EditFromEditMode(object sender, KeenReloadedEventArgs.PropertyValueControlValueChangedEventArgs e)
        {
            if (this.SelectedItem?.RawObjectCreationData != null)
            {
                var pvc = sender as PropertyValueControl;
                if (pvc != null)
                {
                    int index = pnlSpecialProperties.Controls.IndexOf(pvc);
                    if (index != -1)
                    {
                        //set the new value to the appropriate type since Activator.CreateInstance
                        //cannot find the constructor if the types are not correctly set
                        Type t = this.SelectedItem.RawObjectCreationData.Values[index].GetType();
                        var type = t.Name;
                        var fullName = t.FullName;
                        object newVal = GetTypeCastedObjectValue(type, fullName, e.NewValue);
                        this.SelectedItem.RawObjectCreationData.Values[index] = newVal;
                        var objectcreationdata = this.SelectedItem.RawObjectCreationData;
                        var parent = this.Parent as MapMaker;
                        if (parent != null)
                        {
                            parent.RemoveItemFromCanvas(this.SelectedItem);
                            var location = this.SelectedItem.Location;
                            this.SelectedItem = new KeenReloadedItemPic(this.SelectedItem.Image, this.SelectedItem.FileName);
                            this.SelectedItem.Location = location;
                            this.SelectedItem.RawObjectCreationData = objectcreationdata;
                            this.SelectedItem.CreateInstanceFromRawObjectCreationData();
                            this.SetPropertyValueControlPanel(this.SelectedItem);
                            parent.AddItemToCanvas(this.SelectedItem);
                        }
                        
                    }
                }
            }
        }

        private void EditFromPlacementMode(object sender, KeenReloadedEventArgs.PropertyValueControlValueChangedEventArgs e)
        {
            if (_cursorItem?.RawObjectCreationData != null)
            {
                var pvc = sender as PropertyValueControl;
                if (pvc != null)
                {
                    int index = pnlSpecialProperties.Controls.IndexOf(pvc);
                    if (index != -1)
                    {
                        //set the new value to the appropriate type since Activator.CreateInstance
                        //cannot find the constructor if the types are not correctly set
                        Type t = _cursorItem.RawObjectCreationData.Values[index].GetType();
                        var type = t.Name;
                        var fullName = t.FullName;
                        object newVal = GetTypeCastedObjectValue(type, fullName, e.NewValue);
                        _cursorItem.RawObjectCreationData.Values[index] = newVal;
                        //var objectCreationData = _cursorItem.RawObjectCreationData;
                        //this.Parent.Controls.Remove(_cursorItem);
                        //_cursorItem = new KeenReloadedItemPic(_cursorItem.Image, _cursorItem.FileName);
                        //this.Parent.Controls.Add(_cursorItem);
                    }
                }
            }
        }

        private List<Point> ParsePointListFromText(string text)
        {
            try
            {
                List<string> splitItems = text.Split(',').ToList();
                List<Point> items = new List<Point>();
                if (!splitItems.Any())
                {
                    return new List<Point>() { new Point(0, 0) };
                }

                for (int i = 0; i < splitItems.Count; i += 2)
                {
                    int x = Convert.ToInt32(splitItems[i]);
                    int y = Convert.ToInt32(splitItems[i + 1]);
                    Point p = new Point(x, y);
                    items.Add(p);
                }
                return items;
            }
            catch
            {
                return new List<Point>() { new Point(0, 0) };
            }
        }

        private object GetTypeCastedObjectValue(string typeName, string fullName, object value)
        {
            if (fullName.Contains("List"))
            {
                if (fullName.Contains("Point"))
                {
                    return ParsePointListFromText(value.ToString());
                }
                else if (fullName.Contains("Guid"))
                {
                    return ParseGuidListFromText(value.ToString());
                }
            }
            else if (typeName == typeof(Point).Name)
            {
                return ParsePointFromText(value.ToString());
            }
            else if (typeName == typeof(Rectangle).Name)
            {
                return ParseRectangleFromText(value.ToString());
            }
            if (typeName == typeof(string).Name)
            {
                return Convert.ToString(value);
            }
            if (typeName == typeof(bool).Name)
            {
                return bool.TryParse(Convert.ToString(value), out bool result) ? result : default(bool);
            }
            if (typeName == typeof(int).Name)
            {
                return int.TryParse(Convert.ToString(value), out int result) ? result : default(int);
            }
            if (typeName == typeof(short).Name)
            {
                return short.TryParse(Convert.ToString(value), out short result) ? result : default(short);
            }
            if (typeName == typeof(long).Name)
            {
                return long.TryParse(Convert.ToString(value), out long result) ? result : default(long);
            }
            if (typeName == typeof(double).Name)
            {
                return double.TryParse(Convert.ToString(value), out double result) ? result : default(double);
            }
            if (typeName == typeof(float).Name)
            {
                return float.TryParse(Convert.ToString(value), out float result) ? result : default(float);
            }
            if (typeName == typeof(decimal).Name)
            {
                return decimal.TryParse(Convert.ToString(value), out decimal result) ? result : default(decimal);
            }

            return value;
        }

        private object ParseRectangleFromText(string value)
        {
            try
            {
                string[] rectValues = value.Replace("{", "").Replace("}", "").Replace(" ", "")
                    .Replace("X", "").Replace("Y", "").Replace("=", "")
                    .Replace("Width","").Replace("Height","").Split(',');
                int x = Convert.ToInt32(rectValues[0]);
                int y = Convert.ToInt32(rectValues[1]);
                int width = Convert.ToInt32(rectValues[2]);
                int height = Convert.ToInt32(rectValues[3]);
                return new Rectangle(x, y, width, height);
            }
            catch
            {
                return new Rectangle();
            }
        }

        private object ParsePointFromText(string value)
        {
            try
            {
                string[] xyCoord = value.Replace("{", "").Replace("}", "").Replace(" ", "")
                    .Replace("X","").Replace("Y","").Replace("=","").Split(',');
                int x = Convert.ToInt32(xyCoord[0]);
                int y = Convert.ToInt32(xyCoord[1]);
                return new Point(x, y);
            }
            catch
            {
                return new Point();
            }
        }

        private object ParseGuidListFromText(string value)
        {
            try
            {
                List<string> items = value.Split(',').ToList();
                List<Guid> guids = items.Select(i => Guid.Parse(i)).ToList();
                return guids;
            }
            catch
            {
                return new List<Guid>();
            }
        }

        private void LoadCategoryItems()
        {

            foreach (var category in _categoryFolderDict.Keys)
            {
                cmbCategory.Items.Add(category);
            }
            cmbCategory.SelectedIndex = 0;
        }

        private void LoadItemsOntoPanel()
        {
            try
            {
                _currentCatX = SEPARATOR_DISTANCE;
                _currentCatY = SEPARATOR_DISTANCE;
                pnlItems.Controls.Clear();
                int maxPicHeight = 0;
                string category = cmbCategory.SelectedItem.ToString();
                int episode = (int)cmbEpisode.SelectedItem;
                BiomeType biome = (BiomeType)Enum.Parse(typeof(BiomeType), cmbBiome.SelectedItem.ToString());

                string episodeText = $"Keen{episode}";

                string basePath = Environment.CurrentDirectory + @"\MapMakerObjects\";
                string path = basePath + $@"{category}" + $@"\{episodeText}" + $@"\{biome.ToString()}";

                switch (category)
                {
                    case "Player":
                    case "Gems":
                    case "Weapons":
                        path = basePath + $@"\{category}";
                        break;
                    case "Extra Lives":
                    case "Enemies":
                    case "Point Items":
                    case "Hazards":
                    case "Constructs":
                    case "Backgrounds":
                    case "Foregrounds":
                        path = basePath + $@"\{category}" + $@"\{episodeText}";
                        break;

                }

                var files = Directory.GetFiles(path, "*.png");

                foreach (var file in files)
                {
                    Image img = Image.FromFile(file);

                    KeenReloadedItemPic pic = new KeenReloadedItemPic(img, file);
                    pic.Selected += Pic_Selected;
                    pic.Location = new Point(_currentCatX, _currentCatY);
                    pnlItems.Controls.Add(pic);
                    if (pic.Height > maxPicHeight)
                    {
                        maxPicHeight = pic.Height;
                    }
                    _currentCatX += pic.Width + SEPARATOR_DISTANCE;
                    if (_currentCatX >= MAX_WIDTH_PER_ROW)
                    {
                        _currentCatX = SEPARATOR_DISTANCE;
                        _currentCatY += SEPARATOR_DISTANCE + maxPicHeight;
                        maxPicHeight = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void LoadEpisodes()
        {
            cmbEpisode.Items.Add(4);
            cmbEpisode.Items.Add(5);
            cmbEpisode.Items.Add(6);

            cmbEpisode.SelectedIndex = 0;
        }

        private void InitializeCategoryDictionary()
        {
            _categoryFolderDict = new Dictionary<string, string>()
            {
                { "Tiles", "Tiles" },
                { "Enemies", "Enemies" },
                { "Point Items", "Items" },
                { "Extra Lives", "Items" },
                { "Weapons", "Items" },
                { "Gems", "Items" },
                { "Constructs", "Assets" },
                { "Player", "" },
                { "Hazards", "Hazards" },
                { "Backgrounds", "Backgrounds" },
                { "Foregrounds", "Foregrounds" }
            };
        }

        private void CmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshItemsInPanel();

        }

        private void RefreshItemsInPanel()
        {
            LoadItemsOntoPanel();
        }

        private void CmbEpisode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBiomeTypes();
            if (cmbCategory.SelectedItem != null)
                RefreshItemsInPanel();
        }

        private void CmbBiome_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedItem != null)
                RefreshItemsInPanel();
        }
    }
}
