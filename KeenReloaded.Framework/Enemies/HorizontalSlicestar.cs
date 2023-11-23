using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class HorizontalSlicestar : Slicestar
    {
        private Point _startPoint;
        private Point _endPoint;
        public HorizontalSlicestar(SpaceHashGrid grid, Rectangle hitbox, Direction direction, 
            Point startPoint, Point endPoint, CommanderKeen keen)
            : base(grid, hitbox, direction, keen)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            ValidateStartAndEndPoints();
        }

        private void ValidateStartAndEndPoints()
        {
            if (_startPoint.Y != _endPoint.Y)
            {
                throw new ArgumentException("Y position of start and endpoint must be the same for horizontal slicestars");
            }

            if (_startPoint.X >= _endPoint.X)
            {
                throw new ArgumentException("X position of end point must be greater than the X position of the start point for horizontal slicestars");
            }

            if (this.Direction != Enums.Direction.LEFT && this.Direction != Enums.Direction.RIGHT)
            {
                throw new ArgumentException("Direction must be set to 'left' or 'right' for horizontal slicestars");
            }
        }

        public override void Move()
        {
            int xOffset = this.Direction == Enums.Direction.LEFT ? VELOCITY * -1 : VELOCITY;
            int xPos = this.Direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xPos, this.HitBox.Y, this.HitBox.Width + VELOCITY, this.HitBox.Height);

            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (tile != null)
            {
                int collideXPos = this.Direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                Rectangle collisionCheck = new Rectangle(this.Direction == Enums.Direction.LEFT ? collideXPos : this.HitBox.X,
                    this.HitBox.Y, this.HitBox.Width + Math.Abs(this.HitBox.X - collideXPos), this.HitBox.Height);
                KillKeenIfColliding(collisionCheck);

                this.HitBox = new Rectangle(collideXPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = ChangeHorizontalDirection(this.Direction);
            }
            else
            {
                //get the remaining distance to the endpoint
                int remainingXDistance = this.Direction == Enums.Direction.LEFT
                    ? this.HitBox.X - _startPoint.X
                    : _endPoint.X - this.HitBox.X;
                //how far to move is limited first by distance to end point, then by the slicestar's velocity
                int moveDistance = remainingXDistance < VELOCITY 
                    ? this.Direction == Enums.Direction.LEFT 
                        ? remainingXDistance * -1 
                        : remainingXDistance 
                    : xOffset;

                //kill keen if colliding
                KillKeenIfColliding(areaToCheck);
                if (remainingXDistance > 0)
                {
                    //finally, move the slicestar
                    this.HitBox = new Rectangle(this.HitBox.X + moveDistance, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
                else
                {
                    this.Direction = this.ChangeHorizontalDirection(this.Direction);
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + $"|{this.Direction}|{this._startPoint.X}|{this._startPoint.Y}|{this._endPoint.X}|{this._endPoint.Y}";
        }
    }
}
