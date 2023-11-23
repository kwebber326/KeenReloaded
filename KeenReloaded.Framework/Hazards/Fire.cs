using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Hazards
{
    public class Fire : Hazard, IUpdatable, ISprite
    {
        public Fire(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox, Enums.HazardType.KEEN4_FIRE)
        {
            Initialize(direction);
        }

        private void Initialize(Direction direction)
        {
            _direction = direction == Direction.LEFT ? Direction.LEFT : Direction.RIGHT;
            if (_direction == Direction.LEFT)
            {
                _images = new Image[] 
                {
                    Properties.Resources.keen4_fire_left1,
                    Properties.Resources.keen4_fire_left2,
                    Properties.Resources.keen4_fire_left3
                };
            }
            else
            {
                _images = new Image[] 
                {
                    Properties.Resources.keen4_fire_right1,
                    Properties.Resources.keen4_fire_right2,
                    Properties.Resources.keen4_fire_right3
                };
            }
        }

        private Direction _direction;
        private Image[] _images;

        private int _currentSprite;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick = 0;

        public void Update()
        {
            if (_sprite == null)
            {
                _sprite = new PictureBox();
                _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
                if (this.HitBox != null)
                    _sprite.Location = this.HitBox.Location;
            }
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                if (_images != null && _images.Any())
                {
                    _sprite.Image = _images[_currentSprite];
                    _currentSprite = _currentSprite < _images.Length - 1 ? _currentSprite + 1 : 0;
                }
                _currentSpriteChangeDelayTick = 0;
            }
        }
    }
}
