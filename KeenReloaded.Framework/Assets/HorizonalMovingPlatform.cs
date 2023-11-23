using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class HorizonalMovingPlatform : Platform
    {
        private bool _isActive;
        private int _currentSprite;
        private int _maxMoveDistance;
        private int _currentMoveDistance;
        private bool _isKeenStandingOnPlatform;
        private bool _wasStandingOnPlatform;
        private Image[] _leftImages;
        private Image[] _rightImages;

        public HorizonalMovingPlatform(SpaceHashGrid grid, Rectangle hitbox, PlatformType type, CommanderKeen keen, Direction initialDirection, int maxMoveDistance, Guid activationId, bool initiallyActive)
            : base(grid, hitbox, type, keen, activationId)
        {
            _direction = initialDirection == Direction.LEFT ? Direction.LEFT : Direction.RIGHT;
            _isActive = initiallyActive;
            _maxMoveDistance = maxMoveDistance;
            InitializeSpriteSheet();
        }

        private void InitializeSpriteSheet()
        {
            switch (_type)
            {
                case PlatformType.KEEN4:
                    _leftImages = SpriteSheet.Keen4MovingPlatformLeftSprites;
                    _rightImages = SpriteSheet.Keen4MovingPlatformRightSprites;
                    break;
                case PlatformType.KEEN5_ORANGE:
                    _leftImages = SpriteSheet.Keen5OrangePlatformSprites;
                    _rightImages = SpriteSheet.Keen5OrangePlatformSprites;
                    break;
                case PlatformType.KEEN5_PINK:
                    _leftImages = SpriteSheet.Keen5PinkPlatformSprites;
                    _rightImages = SpriteSheet.Keen5PinkPlatformSprites;
                    break;
                case PlatformType.KEEN6:
                    _leftImages = SpriteSheet.Keen6MovingPlatformSprites;
                    _rightImages = SpriteSheet.Keen6MovingPlatformSprites;
                    break;
            }
        }

        public override void Activate()
        {
            _isActive = true;
        }

        public override void Deactivate()
        {
            _isActive = false;
            SetInitialSprite();
        }

        public override void Update()
        {
            if (_isActive)
            {
                this.Move();
            }
        }

        private void Move()
        {
            UpdateSprite();
            _isKeenStandingOnPlatform = IsKeenStandingOnPlatform();
            int xOffset = _direction == Direction.LEFT ? _moveVelocity * -1 : _moveVelocity;
            int xLocation = _direction == Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            int originalX = this.HitBox.X;
            if (_direction == Direction.LEFT)
            {
                MoveLeft(xLocation);
            }
            else
            {
                MoveRight(xLocation);
            }

            if (_keen.IsLookingDown && _wasStandingOnPlatform)
            {
                this.UpdateKeenVerticalPosition();
                if (this.HitBox.X != originalX)
                    this.UpdateKeenHorizontalPosition();
            }
            else
            {
                if (_isKeenStandingOnPlatform)
                {
                    if (this.HitBox.X != originalX)
                        this.UpdateKeenHorizontalPosition();
                    _wasStandingOnPlatform = true;
                }
                else
                {
                    _wasStandingOnPlatform = false;
                }
            }
            //this.UpdateCollisionNodes(_direction);
        }

        private void MoveLeft(int xLocation)
        {
            Rectangle areaToCheck = new Rectangle(xLocation, this.HitBox.Y, this.HitBox.Width + _moveVelocity, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = GetRightMostLeftTile(collisions);
            int moveDistance;
            if (tile != null)
            {
                moveDistance = this.HitBox.Left - tile.HitBox.Right - 1;
                if (_currentMoveDistance + moveDistance <= _maxMoveDistance)
                {
                    this.HitBox = new Rectangle(tile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    _currentMoveDistance += moveDistance;
                }
                else
                {
                    moveDistance = (_currentMoveDistance + moveDistance) - _maxMoveDistance;
                    this.HitBox = new Rectangle(this.HitBox.X - moveDistance, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    SwitchDirection();
                }
            }
            else if (_currentMoveDistance + _moveVelocity <= _maxMoveDistance)
            {
                this.HitBox = new Rectangle(this.HitBox.X - _moveVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                _currentMoveDistance += _moveVelocity;
            }
            else
            {
                moveDistance = (_currentMoveDistance + _moveVelocity) - _maxMoveDistance;
                this.HitBox = new Rectangle(this.HitBox.X - moveDistance, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                SwitchDirection();
            }
        }

        private void MoveRight(int xLocation)
        {
            Rectangle areaToCheck = new Rectangle(xLocation, this.HitBox.Y, this.HitBox.Width + _moveVelocity, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = GetLeftMostRightTile(collisions);
            int moveDistance;
            if (tile != null)
            {
                moveDistance = this.HitBox.Right - tile.HitBox.Left - 1;
                if (_currentMoveDistance + moveDistance <= _maxMoveDistance)
                {
                    this.HitBox = new Rectangle(tile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    _currentMoveDistance += moveDistance;
                }
                else
                {
                    moveDistance = (_currentMoveDistance + moveDistance) - _maxMoveDistance;
                    this.HitBox = new Rectangle(this.HitBox.X + moveDistance, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    SwitchDirection();
                }
            }
            else if (_currentMoveDistance + _moveVelocity <= _maxMoveDistance)
            {
                this.HitBox = new Rectangle(this.HitBox.X + _moveVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                _currentMoveDistance += _moveVelocity;
            }
            else
            {
                moveDistance = (_currentMoveDistance + _moveVelocity) - _maxMoveDistance;
                this.HitBox = new Rectangle(this.HitBox.X + moveDistance, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                SwitchDirection();
            }
        }

        private void SwitchDirection()
        {
            _direction = _direction == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
            _currentMoveDistance = 0;
        }

        private void UpdateSprite()
        {
            var spriteSet = _direction == Direction.RIGHT ? _rightImages : _leftImages;
            if (++_currentSprite >= spriteSet.Length)
            {
                _currentSprite = 0;
            }

            this.Sprite.Image = spriteSet[_currentSprite];
        }

        public override string ToString()
        {
            return base.ToString() + $"{this.HitBox.Width}|{this.HitBox.Height}|{this._direction}|{this._maxMoveDistance}|{this.ActivationID}|{_isActive}";
        }
    }
}
