using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class ForegroundSprite : ISprite
    {
        protected PictureBox _sprite;

        public ForegroundSprite(Rectangle area, Image sprite)
        {

            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.StretchImage;
            _sprite.Location = area.Location;
            _sprite.Size = area.Size;
            _sprite.Image = sprite;
            _sprite?.BringToFront();
        }

        public PictureBox Sprite => _sprite;
    }
}
