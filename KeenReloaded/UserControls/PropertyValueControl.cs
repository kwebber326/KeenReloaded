using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.KeenReloadedEventArgs;

namespace KeenReloaded.UserControls
{
    public partial class PropertyValueControl : UserControl
    {
        private const int CTRL_MARGIN = 12;
        private const int TEXT_BOX_WIDTH = 300;
        private readonly string _propertyName;
        private readonly object _value;


        public event EventHandler<PropertyValueControlValueChangedEventArgs> PropertyValueChanged;
        public PropertyValueControl(string propertyName, object value)
        {
            InitializeComponent();
            _propertyName = propertyName;
            _value = value;
        }

        private void SetObjectControlFromType()
        {
            if (!string.IsNullOrWhiteSpace(_propertyName))
            {
                lblPropertyName.Text = _propertyName.Substring(0, 1).ToUpper() + _propertyName.Substring(1) + ": ";//capitalize first letter
            }
            if (_value != null)
            {
                string valAsString = Convert.ToString(_value);
                if (int.TryParse(valAsString, out int result))
                {
                    TextBox txt = new TextBox();
                    txt.Text = result.ToString();
                    txt.Width = TEXT_BOX_WIDTH;
                    txt.TextChanged += Txt_TextChanged;
                    txt.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    this.Controls.Add(txt);
                }
                else if (bool.TryParse(valAsString, out bool boolResult))
                {
                    CheckBox chk = new CheckBox();
                    chk.Checked = boolResult;
                    chk.CheckedChanged += Chk_CheckedChanged;
                    chk.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    this.Controls.Add(chk);
                }
                else if (_value is Enum)
                {
                    ComboBox cmb = new ComboBox();
                    cmb.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmb.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    Type t = Type.GetType(_value.GetType().AssemblyQualifiedName);
                    var items = Enum.GetValues(t);
                    foreach (var item in items)
                    {
                        cmb.Items.Add(item);
                    }
                    cmb.SelectedItem = _value;
                    if (_value is BiomeType || _value is TileType)
                    {
                        cmb.Enabled = false;
                    }
                    cmb.SelectedIndexChanged += Cmb_SelectedIndexChanged;
                    this.Controls.Add(cmb);
                }
                else if (_value is Image)
                {
                    var img = (Image)_value;
                    PictureBox btn = new PictureBox()
                    {
                        SizeMode = PictureBoxSizeMode.AutoSize,
                        Image = img
                    };
                    btn.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    this.Controls.Add(btn);
                    int height = this.Height > img.Height ? this.Height : img.Height;
                    this.Size = new Size(this.Width + img.Width, height);
                }
                else if (_value is List<Point>)
                {
                    var points = (List<Point>)_value;
                    string totaltxt = string.Empty;
                    if (points.Any())
                    {
                       
                        for (int i = 0; i < points.Count; i++)
                        {
                            string pointTxt = $"{points[i].X},{points[i].Y}";
                            if (i < points.Count - 1)
                            {
                                pointTxt += ",";
                            }
                            totaltxt += pointTxt;
                        }
                    }
                    TextBox txt = new TextBox();
                    txt.Text = totaltxt;
                    txt.Width = TEXT_BOX_WIDTH;
                    txt.TextChanged += Txt_TextChanged;
                    txt.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    this.Controls.Add(txt);
                }
                else
                {
                    TextBox txt = new TextBox();
                    txt.Text = valAsString;
                    txt.Width = TEXT_BOX_WIDTH;
                    txt.TextChanged += Txt_TextChanged;
                    txt.Location = new Point(lblPropertyName.Right + CTRL_MARGIN, lblPropertyName.Top);
                    this.Controls.Add(txt);
                }
            }
        }

        protected void OnPropertyValueChanged(PropertyValueControlValueChangedEventArgs e)
        {
            this.PropertyValueChanged?.Invoke(this, e);
        }

        private void Cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null)
            {
                PropertyValueControlValueChangedEventArgs pvce = new PropertyValueControlValueChangedEventArgs()
                {
                    NewValue = cmb.SelectedItem
                };
                OnPropertyValueChanged(pvce);
            }
        }

        private void Chk_CheckedChanged(object sender, EventArgs e)
        {
            var chk = sender as CheckBox;
            if (chk != null)
            {
                PropertyValueControlValueChangedEventArgs pvce = new PropertyValueControlValueChangedEventArgs()
                {
                    NewValue = chk.Checked
                };
                OnPropertyValueChanged(pvce);
            }
        }

        private void Txt_TextChanged(object sender, EventArgs e)
        {
            var txt = sender as TextBox;
            if (txt != null)
            {
                PropertyValueControlValueChangedEventArgs pvce = new PropertyValueControlValueChangedEventArgs()
                {
                    NewValue = txt.Text
                };
                OnPropertyValueChanged(pvce);
            }
        }

        private void PropertyValueControl_Load(object sender, EventArgs e)
        {
            SetObjectControlFromType();
        }
    }
}
