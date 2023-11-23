using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.UserControls
{
    public class SelectableImageButton : PictureBox
    {
        private string _path;

        public SelectableImageButton(Image img, string path)
        {

            this.Image = img;
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.BorderStyle = BorderStyle.Fixed3D;
            this.DoubleClick += SelectableImageButton_DoubleClick;
            _path = path;
            if (this.Image != null)
                this.Size = this.Image.Size;
        }

        private void SelectableImageButton_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.FileName = _path;
            o.Filter = "Image Files|*.png";
            if (o.ShowDialog() == DialogResult.OK)
            {
                this.Image = Image.FromFile(o.FileName);
                if (this.Image != null)
                    this.Size = this.Image.Size;
            }

        }
    }
}
