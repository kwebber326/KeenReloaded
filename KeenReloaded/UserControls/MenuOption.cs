using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.UserControls
{
    public partial class MenuOption : UserControl
    {
        private bool _isSelected;
        private string _optionText;
        private Action _selectAction;
        private Dictionary<string, Image> _imageMap;

      

        public MenuOption(string optionText, Action selectAction, Dictionary<string, Image> characterImageMap, string selectedCharacter)
        {
            _optionText = optionText;
            _selectAction = selectAction;
            _imageMap = characterImageMap;
            this.SelectedCharacter = selectedCharacter;
            InitializeComponent();
        }
        public string SelectedCharacter { get; set; }

        public event EventHandler Selected;

        protected void OnSelected(object sender, EventArgs e)
        {
            this.Selected?.Invoke(sender, e);
        } 

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            private set
            {
                if (!_isSelected && value)
                {
                    _isSelected = value;
                    pbKeen.Image = _imageMap.TryGetValue(this.SelectedCharacter, out Image img) ? img : Properties.Resources.keen_shoot_right_standing;
                    OnSelected(this, EventArgs.Empty);

                }
                else if (_isSelected && !value)
                {
                    _isSelected = value;
                    pbKeen.Image = null;
                }
            }
        }

        public void UpdateSelectionImage()
        {
            if (this.IsSelected)
            {
                pbKeen.Image = _imageMap.TryGetValue(this.SelectedCharacter, out Image img) ? img : Properties.Resources.keen_shoot_right_standing;
            }
        }

        public void SelectOption()
        {
            IsSelected = true;
        }
        public void DeselectOption()
        {
            IsSelected = false;
        }

        private void MenuOption_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_optionText))
            {
                lblOptionText.Text = _optionText;
            }
        }

        private void LblOptionText_DoubleClick(object sender, EventArgs e)
        {
            InvokeAction();
        }

        public void InvokeAction()
        {
            SelectOption();
            _selectAction?.Invoke();
        }

        private void MenuOption_MouseHover(object sender, EventArgs e)
        {
            SelectOption();
        }
    }
}
