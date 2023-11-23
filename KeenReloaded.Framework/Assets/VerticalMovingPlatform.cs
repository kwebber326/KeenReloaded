using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class VerticalMovingPlatform : Platform
    {
        private bool _isActive;
        private int _currentSprite;
        private int _maxMoveDistance;
        private int _currentMoveDistance;
        private bool _isKeenStandingOnPlatform;
        private bool _wasStandingOnPlatform;

        public VerticalMovingPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, PlatformType type, Direction initialDirection, int maxMoveDistance, bool initiallyActive, Guid activationId)
            : base(grid, hitbox, type, keen, activationId)
        {
            _direction = initialDirection == Direction.UP ? Direction.UP : Direction.DOWN;
            _isActive = initiallyActive;
            _maxMoveDistance = maxMoveDistance;
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
            int yOffset = _direction == Direction.UP ? _moveVelocity * -1 : _moveVelocity;
            int yLocation = _direction == Direction.UP ? this.HitBox.Y + yOffset : this.HitBox.Y;

            if (_direction == Direction.UP)
            {
                MoveUp(yLocation);
            }
            else
            {
                MoveDown(yLocation);
            }

            if (_keen.IsLookingDown && _wasStandingOnPlatform)
            {
                this.UpdateKeenVerticalPosition();
            }
            else
            {
                if (_isKeenStandingOnPlatform)
                {
                    this.UpdateKeenVerticalPosition();
                    _wasStandingOnPlatform = true;
                }
                else
                {
                    _wasStandingOnPlatform = false;
                }
            }
            this.UpdateCollisionNodes(Direction.DOWN);
            this.UpdateCollisionNodes(Direction.UP);
        }

        private void MoveUp(int yLocation)
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, yLocation, this.HitBox.Width, this.HitBox.Height + _moveVelocity);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = this.GetCeilingTile(collisions);
            int moveDistance;
            if (tile != null)
            {
                moveDistance = (tile.HitBox.Bottom - this.HitBox.Top) - 1;
                if (_currentMoveDistance + moveDistance <= _maxMoveDistance)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
                    _currentMoveDistance += moveDistance;
                }
                else
                {
                    moveDistance = (_currentMoveDistance + moveDistance) - _maxMoveDistance;
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - moveDistance, this.HitBox.Width, this.HitBox.Height);
                    SwitchDirection();
                }
            }
            else if (_currentMoveDistance + _moveVelocity <= _maxMoveDistance)
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - _moveVelocity, this.HitBox.Width, this.HitBox.Height);
                _currentMoveDistance += _moveVelocity;
            }
            else
            {
                moveDistance = (_currentMoveDistance + _moveVelocity) - _maxMoveDistance;
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - moveDistance, this.HitBox.Width, this.HitBox.Height);
                SwitchDirection();
            }
        }

        private void MoveDown(int yLocation)
        {
            var tile = this.GetTopMostLandingTile(_moveVelocity);
            int moveDistance;
            if (tile != null)
            {
                moveDistance = (this.HitBox.Bottom - tile.HitBox.Top) - 1;
                if (_currentMoveDistance + moveDistance <= _maxMoveDistance)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                    _currentMoveDistance += moveDistance;
                }
                else
                {
                    moveDistance = (_currentMoveDistance + moveDistance) - _maxMoveDistance;
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + moveDistance, this.HitBox.Width, this.HitBox.Height);
                    SwitchDirection();
                }
            }
            else if (_currentMoveDistance + _moveVelocity <= _maxMoveDistance)
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _moveVelocity, this.HitBox.Width, this.HitBox.Height);
                _currentMoveDistance += _moveVelocity;
            }
            else
            {
                moveDistance = (_currentMoveDistance + _moveVelocity) - _maxMoveDistance;
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + moveDistance, this.HitBox.Width, this.HitBox.Height);
                SwitchDirection();
            }
        }

        private void SwitchDirection()
        {
            _direction = _direction == Direction.UP ? Direction.DOWN : Direction.UP;
            _currentMoveDistance = 0;
        }

        private void UpdateSprite()
        {
            if (++_currentSprite >= _images.Length)
            {
                _currentSprite = 0;
            }
            this.Sprite.Image = _images[_currentSprite];
        }
    }
}
