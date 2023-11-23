using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using KeenReloaded.HelperClasses;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.UserControls
{
    public partial class KeenReloadedItemPic : PictureBox
    {
        protected bool _selected;
        private string _fileName;
        private TypeConstructorValues _rawObjectCreationData;

        public event EventHandler Selected;

        public KeenReloadedItemPic(Image image, string fileName)
        {

            this.Image = image;
            _fileName = fileName;


            InitializeComponent();
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Click += KeenReloadedItemPic_Click;

            this.BorderStyle = BorderStyle.None;
            this.BackColor = Color.Red;
        }

        public ISprite SpriteObject
        {
            get;set;
        }

        public TypeConstructorValues RawObjectCreationData
        {
            get
            {
                return _rawObjectCreationData;
            }
            set {
                _rawObjectCreationData = value;
            }
        }

        private Delegate[] GetSubscribers()
        {
            var listeners = this.Selected?.GetInvocationList();
            return listeners;
        }

        private void KeenReloadedItemPic_Click(object sender, EventArgs e)
        {
            this.IsSelected = true;
        }

        public bool IsDelegateHandling(string  methodName)
        {
            var listeners = GetSubscribers();
            return listeners != null && listeners.Select(d => d.Method.Name).Contains(methodName);
        }

        public virtual bool IsSelected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected)
                {
                    this.BorderStyle = BorderStyle.Fixed3D;
                    OnSelected();
                }
                else
                {
                    this.BorderStyle = BorderStyle.None;
                }
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }

        public void CreateInstanceFromRawObjectCreationData()
        {
            if (this.RawObjectCreationData != null)
            {
               this.SpriteObject = Activator.CreateInstance(
                   this.RawObjectCreationData.Type,
                   this.RawObjectCreationData.Values) as ISprite;
            }
        }

        protected void OnSelected()
        {
            this.Selected?.Invoke(this, EventArgs.Empty);
        }
        private void KeenReloadedItemPic_Load(object sender, EventArgs e)
        {


        }
    }
}
