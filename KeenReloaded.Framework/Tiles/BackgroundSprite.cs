using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class BackgroundSprite : ISprite, IBackground
    {
        private readonly PictureBox _sprite;
        private readonly Rectangle _coverageArea;
        private readonly bool _stretchImage;

        public BackgroundSprite(Rectangle area, Image sprite, int zIndex, bool stretchImage)
        {
            _coverageArea = area;
            _stretchImage = stretchImage;
            _sprite = new PictureBox();
            _sprite.SizeMode = _stretchImage ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.AutoSize;
            _sprite.Location = area.Location;
            _sprite.Size = area.Size;
            _sprite.Image = sprite;
            this.ZIndex = zIndex;
            if (!_stretchImage)
                InitializeSpriteMap(sprite.Size);
        }

        protected virtual void InitializeSpriteMap(Size imageDimensions)
        {
            //image width and height
            int width = imageDimensions.Width;
            int height = imageDimensions.Height;
            //location to write image (0,0 is the top left corner of the picture box, not the form)
            int currentX = 0;
            int currentY = 0;
            //declare a bitmap for the image
            var bitmap = new Bitmap(_coverageArea.Width, _coverageArea.Height);
            //update the size of the picture box to accomodate background size
            _sprite.Size = _coverageArea.Size;
            //for each subsequent length and height, draw the image onto the bitmap
            //to fill out the background rectangle
            for (int i = 0; i < _coverageArea.Width; i += width)
            {
                for (int j = 0; j < _coverageArea.Height; j += height)
                {
                    Graphics.FromImage(bitmap).DrawImage(_sprite.Image, new Rectangle(currentX, currentY, _sprite.Image.Width, _sprite.Image.Height));
                    currentY += height;
                }
                currentY = 0;
                currentX += width;
            }
            //assign the resulting bitmap to the picture box's image property so it loads as 
            //one image onto the form
            _sprite.Image = bitmap;
        }

        public bool StretchImage
        {
            get
            {
                return _stretchImage;
            }
        }

        public PictureBox Sprite => _sprite;

        public int ZIndex { get; private set; }
    }
}
