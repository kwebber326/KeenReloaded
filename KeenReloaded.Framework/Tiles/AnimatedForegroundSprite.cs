using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Tiles
{
    public class AnimatedForegroundSprite : ForegroundSprite, IAnimation, IUpdatable
    {
        private Image[] _animationSprites;
        private int _animationDelayTick;
        private int _currentSprite;
        private bool _continueAnimation = true;
        private readonly List<string> _spriteFiles;
        private readonly AnimationAnchor _anchor;

        private readonly int _anchorLeft, _anchorRight, _anchorTop, _anchorBottom;

        public AnimatedForegroundSprite(Rectangle area, List<string> spriteFiles, AnimationAnchor animationAnchor)
            : base(area, null)
        {
            if (spriteFiles == null || !spriteFiles.Any())
                throw new ArgumentException("Missing Sprite information for animation foreground sprite");

            _spriteFiles = spriteFiles;
            _anchor = animationAnchor;
            _animationSprites = GetSpritesFromFiles();
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = _animationSprites[_currentSprite];
            _sprite.Location = area.Location;
            _anchorLeft = _sprite.Left;
            _anchorRight = _sprite.Right;
            _anchorTop = _sprite.Top;
            _anchorBottom = _sprite.Bottom;
        }

        private Image[] GetSpritesFromFiles()
        {
            Image[] images = new Image[_spriteFiles.Count];
            for (int i = 0; i < _spriteFiles.Count; i++)
            {
                Image img = Image.FromFile(_spriteFiles[i]);
                images[i] = img;
            }
            return images;
        }

        public int AnimationDelay => 2;

        public void GetNextAnimationImage()
        {
            if (++_currentSprite == _animationSprites.Length)
            {
                _currentSprite = 0;
            }
            this.Sprite.Image = _animationSprites[_currentSprite];
            AdjustImageLocationFromAnchor();
        }

        public void Reset()
        {
            _currentSprite = 0;
            _continueAnimation = true;
        }

        public void StartAnimation()
        {
            _continueAnimation = true;
        }

        public void StopAnimation()
        {
            _continueAnimation = false;
        }

        public void Update()
        {
            if (_continueAnimation)
            {
                if (_animationDelayTick++ == AnimationDelay)
                {
                    _animationDelayTick = 0;
                    GetNextAnimationImage();
                }
            }
        }

        private void AdjustImageLocationFromAnchor()
        {
            int xOffset = 0, yOffset = 0;

            var previousImageSize = GetPreviosImageSize();

            int widthDifference = _sprite.Width - previousImageSize.Width, heightDifference = _sprite.Height - previousImageSize.Height;
            switch (_anchor)
            {
                case AnimationAnchor.TOP_RIGHT:
                    xOffset = widthDifference;
                    _sprite.Location = new Point(_sprite.Location.X - xOffset, _sprite.Location.Y);
                    break;
                case AnimationAnchor.BOTTOM_LEFT:
                    yOffset = heightDifference;
                    _sprite.Location = new Point(_sprite.Location.X, _sprite.Location.Y - yOffset);
                    break;
                case AnimationAnchor.BOTTOM_RIGHT:
                    yOffset = heightDifference;
                    xOffset = widthDifference;
                    _sprite.Location = new Point(_sprite.Location.X - xOffset, _sprite.Location.Y - yOffset);
                    break;
                case AnimationAnchor.BOTTOM_CENTER:
                    xOffset = widthDifference / 2;
                    yOffset = heightDifference;
                    _sprite.Location = new Point(_sprite.Location.X - xOffset, _sprite.Location.Y - yOffset);
                    break;
                case AnimationAnchor.TOP_CENTER:
                    xOffset = widthDifference / 2;
                    _sprite.Location = new Point(_sprite.Location.X - xOffset, _sprite.Location.Y);
                    break;
                case AnimationAnchor.CENTER:
                    xOffset = widthDifference / 2;
                    yOffset = heightDifference / 2;
                    _sprite.Location = new Point(_sprite.Location.X - xOffset, _sprite.Location.Y - yOffset);
                    break;
            }
        }

        private Size GetPreviosImageSize()
        {
            int index = 0;
            int spriteSheetLength = _animationSprites.Length;
            if (_currentSprite == 0)
                index = spriteSheetLength - 1;
            else if (_currentSprite >= spriteSheetLength - 1)
                index = spriteSheetLength == 1 ? 0 : spriteSheetLength - 2;
            else
                index = _currentSprite - 1;

            Image img = _animationSprites[index];
            return img.Size;
        }
    }
}
