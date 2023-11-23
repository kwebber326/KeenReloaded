using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class HorizontalTrickPlatform : Platform
    {
        private const int VERTICAL_VISION = 100;
        private const int VERTICAL_VISION_OFFSET = 100;
        private const int QUICK_MOVE_SPEED = 50;
        private const int SLOW_MOVE_SPEED = 10;
        private const int MAX_LUNGE_DISTANCE = 250;

        private TrickPlatformState _state;
        private int _currentLungeDistance;
        private Direction _returnDirection;
        private int _originalX;

        public HorizontalTrickPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, PlatformType type)
            : base(grid, hitbox, type, keen, Guid.Empty)
        {
            _direction = SetDirectionFromKeenLocation();
            _state = TrickPlatformState.WAITING;
            _originalX = this.HitBox.X;
            _moveVelocity = SLOW_MOVE_SPEED;
        }

        private Direction SetDirectionFromKeenLocation()
        {
            if (_keen.HitBox.X < this.HitBox.X + this.HitBox.Width / 2)
            {
                return Direction.RIGHT;
            }
            return Direction.LEFT;
        }

        public override void Activate()
        {

        }

        public override void Deactivate()
        {

        }

        public override void Update()
        {
            switch (_state)
            {
                case TrickPlatformState.WAITING:
                    this.Wait();
                    break;
                case TrickPlatformState.LUNGING:
                    this.Lunge();
                    break;
                case TrickPlatformState.RETURNING:
                    this.ReturnToOriginalLocation();
                    break;
            }
        }

        private void ReturnToOriginalLocation()
        {
            if (_state != TrickPlatformState.RETURNING)
            {
                _state = TrickPlatformState.RETURNING;
                _currentLungeDistance = 0;
                _returnDirection = _direction == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
                _moveVelocity = SLOW_MOVE_SPEED;
            }
            int originalX = this.HitBox.X;
            bool returned = _returnDirection == Direction.LEFT ? this.HitBox.X - SLOW_MOVE_SPEED <= _originalX : this.HitBox.X + SLOW_MOVE_SPEED >= _originalX;
            if (returned)
            {
                this.HitBox = new Rectangle(_originalX, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Wait();
                return;
            }

            int xOffset = _returnDirection == Direction.LEFT ? SLOW_MOVE_SPEED * -1 : SLOW_MOVE_SPEED;
            int xLocation = _returnDirection == Direction.LEFT ? this.HitBox.X - QUICK_MOVE_SPEED : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xLocation, this.HitBox.Y, this.HitBox.Width + SLOW_MOVE_SPEED, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck);
            var hitTile = _returnDirection == Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (hitTile == null)
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (IsKeenStandingOnPlatform() && (this.HitBox.X != originalX))
                    UpdateKeenHorizontalPosition(_returnDirection);
            }
        }

        private void UpdateKeenHorizontalPosition(Direction direction)
        {
            if (!_keen.IsDead())
                _keen.GetMovedHorizontally(this, direction, _moveVelocity);
        }

        private void Lunge()
        {
            if (_state != TrickPlatformState.LUNGING)
            {
                _state = TrickPlatformState.LUNGING;
                _moveVelocity = QUICK_MOVE_SPEED;
            }
            int originalX = this.HitBox.X;
            int xOffset = _direction == Direction.LEFT ? QUICK_MOVE_SPEED * -1 : QUICK_MOVE_SPEED;
            int xLocation = _direction == Direction.LEFT ? this.HitBox.X - QUICK_MOVE_SPEED : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xLocation, this.HitBox.Y, this.HitBox.Width + QUICK_MOVE_SPEED, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck);
            var hitTile = _direction == Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (hitTile != null)
            {
                int newXLocation = _direction == Direction.LEFT ? hitTile.HitBox.Right + 1 : hitTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(newXLocation, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.ReturnToOriginalLocation();
            }
            else if (_currentLungeDistance + QUICK_MOVE_SPEED <= MAX_LUNGE_DISTANCE)
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                _currentLungeDistance += QUICK_MOVE_SPEED;
            }
            else
            {
                int moveDistance = MAX_LUNGE_DISTANCE - _currentLungeDistance;
                int newXLocation = _direction == Direction.LEFT ? this.HitBox.X - moveDistance : this.HitBox.X + moveDistance;
                this.HitBox = new Rectangle(newXLocation, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.ReturnToOriginalLocation();
            }
            if (IsKeenStandingOnPlatform() && (this.HitBox.X != originalX))
                UpdateKeenHorizontalPosition(_direction);
        }

        private void Wait()
        {
            if (_state != TrickPlatformState.WAITING)
            {
                _state = TrickPlatformState.WAITING;
            }

            _direction = SetDirectionFromKeenLocation();

            bool keenInVision = IsKeenInVision();
            bool keenStandingOnPlatform = IsKeenStandingOnPlatform();
            if (keenInVision && !keenStandingOnPlatform && (_keen.MoveState == MoveState.FALLING || _keen.MoveState == MoveState.JUMPING))
            {
                this.Lunge();
            }
        }

        private bool IsKeenInVision()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Top - VERTICAL_VISION - VERTICAL_VISION_OFFSET, this.HitBox.Width, VERTICAL_VISION);
            bool inVision = _keen.HitBox.IntersectsWith(areaToCheck);
            return inVision;
        }
    }

    enum TrickPlatformState
    {
        WAITING,
        LUNGING,
        RETURNING
    }
}