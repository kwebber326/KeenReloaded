using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Hazards
{
    public class Spear : Hazard, IUpdatable, ISprite
    {
        private Direction _direction;
        private Image[] _images;

        private const int STAB_UPDATE_DELAY = 1;
        private const int STAB_DELAY = 35;
        private int _currentStabDelayTick = 0;
        private int _currentStabUpdateDelayTick = 0;
        private int _currentStabState = 0;
        private bool _retracting;
        private int _currentYOffset;
        private int _currentXOffset;
        private int _currentWidth;
        private int _currentHeight;
        private Point _originalLocation;

        public Spear(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox, HazardType.KEEN4_SPEAR)
        {
            Initialize(direction);
        }

        private void Initialize(Direction direction)
        {
            _direction = direction;
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

            switch (_direction)
            {
                case Direction.LEFT:
                    _sprite.Image = Properties.Resources.keen4_spear_wait_left;
                    _images = new Image[]
                    {
                        Properties.Resources.keen4_spear_wait_left,
                        Properties.Resources.keen4_spear_stab_left1,
                        Properties.Resources.keen4_spear_stab_left2
                    };
                    break;
                case Direction.RIGHT:
                    _sprite.Image = Properties.Resources.keen4_spear_wait_right;
                    _images = new Image[]
                    {
                        Properties.Resources.keen4_spear_wait_right,
                        Properties.Resources.keen4_spear_stab_right1,
                        Properties.Resources.keen4_spear_stab_right2
                    };
                    break;
                case Direction.UP:
                    _sprite.Image = Properties.Resources.keen4_spear_wait_up;
                    _images = new Image[]
                    {
                        Properties.Resources.keen4_spear_wait_up,
                        Properties.Resources.keen4_spear_stab_up1,
                        Properties.Resources.keen4_spear_stab_up2
                    };
                    break;
                case Direction.DOWN:
                    _sprite.Image = Properties.Resources.keen4_spear_wait_down;
                    _images = new Image[]
                    {
                        Properties.Resources.keen4_spear_wait_down,
                        Properties.Resources.keen4_spear_stab_down1,
                        Properties.Resources.keen4_spear_stab_down2
                    };
                    break;
            }
            this.Sprite.Location = this.HitBox.Location;
            _originalLocation = this.HitBox.Location;
        }

        public override Rectangle HitBox
        {
            get
            {
                return base.HitBox;
            }
            protected set
            {
                base.HitBox = value;
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(_direction);
                }
            }
        }

        public override bool IsDeadly
        {
            get
            {
                return _currentStabState > 0;
            }
        }

        public void Update()
        {
            switch (_currentStabState)
            {
                case 0:
                    if (_currentStabDelayTick++ == STAB_DELAY)
                    {
                        _currentStabDelayTick = 0;
                        _sprite.Image = _images[++_currentStabState];
                        SetHitboxBasedOnDirectionAndState();
                    }
                    break;
                case 1:
                    if (_currentStabUpdateDelayTick++ == STAB_UPDATE_DELAY)
                    {
                        _currentStabUpdateDelayTick = 0;
                        if (!_retracting)
                        {
                            _sprite.Image = _images[++_currentStabState];
                            SetHitboxBasedOnDirectionAndState();
                        }
                        else
                        {
                            _sprite.Image = _images[--_currentStabState];
                            _retracting = false;
                            SetHitboxBasedOnDirectionAndState();
                        }
                    }
                    break;
                case 2:
                    if (_currentStabUpdateDelayTick++ == STAB_UPDATE_DELAY)
                    {
                        _currentStabUpdateDelayTick = 0;

                        if (!_retracting)
                        {
                            _retracting = true;
                        }
                        else
                        {
                            _sprite.Image = _images[--_currentStabState];
                            SetHitboxBasedOnDirectionAndState();
                        }
                    }
                    break;
            }
            if (this.IsDeadly)
                CheckKeenCollision();
        }

        private void CheckKeenCollision()
        {
            var collisionItems = this.CheckCollision(this.HitBox);
            var collisionKeens = collisionItems.OfType<CommanderKeen>();
            if (collisionKeens.Any())
            {
                foreach (var keen in collisionKeens)
                {
                    keen.Die();
                }
            }

        }

        private void SetHitboxBasedOnDirectionAndState()
        {
            switch (this._direction)
            {
                case Direction.DOWN:
                    _currentYOffset = 0;
                    break;
                case Direction.UP:
                    if (_currentStabState == 1)
                    {
                        _currentYOffset = _retracting ? 26 : -38;
                    }
                    else if (_currentStabState == 2)
                    {
                        _currentYOffset = _retracting ? 26 : -26;
                    }
                    break;
                case Direction.LEFT:
                    if (_currentStabState == 1)
                    {
                        _currentXOffset = _retracting ? 42 : -46;
                    }
                    else if (_currentStabState == 2)
                    {
                        _currentXOffset = _retracting ? 42 : -42;
                    }
                    break;
                case Direction.RIGHT:
                    _currentXOffset = 0;
                    break;
            }
            if (_currentStabState == 0)
            {
                this.HitBox = new Rectangle(_originalLocation, _sprite.Size);
            }
            else
            {
                this.HitBox = new Rectangle(new Point(this.HitBox.X + _currentXOffset, this.HitBox.Y + _currentYOffset), _sprite.Size);
            }
        }

        System.Windows.Forms.PictureBox ISprite.Sprite
        {
            get { return _sprite; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_direction.ToString()}";
        }
    }
}
