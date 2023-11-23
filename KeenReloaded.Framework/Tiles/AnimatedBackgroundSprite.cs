using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class AnimatedBackgroundSprite : ISprite, IUpdatable, IBackground
    {
        private readonly Image[] _images;
        private readonly Point _location;
        private readonly int _delayTicks;
        private readonly int _zIndex;
        private readonly List<string> _imageNames;
        private PictureBox _sprite;
        private int _currentDelayTicks = 0;
        private int _currentImageIndex = 0;

        public AnimatedBackgroundSprite(Point location, Image[] images, int delayTicks, int zIndex)
        {
            if (images == null || !images.Any())
                throw new ArgumentException("Must supply a valid list of images");

            if (delayTicks < 0)
                throw new ArgumentException("animation delay must be a non-negative number");

            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = location;
            _sprite.Image = images[0];
            _imageNames = GetImageNamesFromImageList(images).ToList();
            _images = images;
            _location = location;
            _delayTicks = delayTicks;
            _zIndex = zIndex;
        }

        public PictureBox Sprite => _sprite;

        public int ZIndex => _zIndex;

        public void Update()
        {
            if (_currentDelayTicks++ == _delayTicks)
            {
                _currentDelayTicks = 0;
                if (++_currentImageIndex >= _images.Length)
                {
                    _currentImageIndex = 0;
                }
                _sprite.Image = _images[_currentImageIndex];
            }
        }

        public override string ToString()
        {
            string imageListStr = string.Join(",", _imageNames);
            return $"{this.GetType().Name}|{_location.X}|{_location.Y}|{_zIndex}|{_delayTicks}|[{imageListStr}]";
        }

        private IEnumerable<string> GetImageNamesFromImageList(Image[] images)
        {
            foreach (var image in images)
            {
                PictureBox p = new PictureBox();
                p.Image = image;
                yield return p.Tag?.ToString();
            }
        }
    }
}
