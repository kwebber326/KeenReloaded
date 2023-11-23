using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class VerticalSlicestar : Slicestar
    {
        private Point _startPoint;
        private Point _endPoint;
        public VerticalSlicestar(SpaceHashGrid grid, Rectangle hitbox, Direction direction, 
            Point startPoint, Point endPoint, CommanderKeen keen)
            : base(grid, hitbox, direction, keen)
        {
            _startPoint = startPoint;
            _endPoint = endPoint;
            ValidateStartAndEndPoints();
        }

        private void ValidateStartAndEndPoints()
        {
            if (_startPoint.X != _endPoint.X)
            {
                throw new ArgumentException("X position of start and endpoint must be the same for vertical slicestars");
            }

            if (_startPoint.Y >= _endPoint.Y)
            {
                throw new ArgumentException("Y position of end point must be less than the Y position of the start point for vertical slicestars");
            }

            if (this.Direction != Enums.Direction.UP && this.Direction != Enums.Direction.DOWN)
            {
                throw new ArgumentException("Direction must be set to 'up' or 'down' for vertical slicestars");
            }
        }

        public override void Move()
        {
            int yOffset = this.Direction == Enums.Direction.UP ? VELOCITY * -1 : VELOCITY;
            int yPos = this.Direction == Enums.Direction.UP ? this.HitBox.Y + yOffset : this.HitBox.Y;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, yPos, this.HitBox.Width, this.HitBox.Height + VELOCITY);

            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = this.Direction == Enums.Direction.UP ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
            if (tile != null)
            {
                int collideYPos = this.Direction == Enums.Direction.UP ? tile.HitBox.Bottom + 1 : tile.HitBox.Top - this.HitBox.Height - 1;
                Rectangle collisionCheck = new Rectangle(this.HitBox.X,
                    this.Direction == Enums.Direction.UP ? collideYPos : this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + Math.Abs(this.HitBox.Y - collideYPos));
                KillKeenIfColliding(collisionCheck);

                this.HitBox = new Rectangle(this.HitBox.X, collideYPos, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeVerticalDirection(this.Direction);
            }
            else
            {
                //get the remaining distance to the endpoint
                int remainingYDistance = this.Direction == Enums.Direction.UP
                    ? this.HitBox.Y - _startPoint.Y
                    : _endPoint.Y - this.HitBox.Y;
                //how far to move is limited first by distance to end point, then by the slicestar's velocity
                int moveDistance = remainingYDistance < VELOCITY 
                    ? this.Direction == Enums.Direction.UP 
                        ? remainingYDistance * -1 
                        : remainingYDistance 
                    : yOffset;

                //kill keen if colliding
                KillKeenIfColliding(areaToCheck);

                if (remainingYDistance > 0)
                {
                    //finally, move the slicestar
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + moveDistance, this.HitBox.Width, this.HitBox.Height);
                }
                else
                {
                    //change direction if we reached the end
                    this.Direction = this.ChangeVerticalDirection(this.Direction);
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + $"|{this.Direction}|{this._startPoint.X}|{this._startPoint.Y}|{this._endPoint.X}|{this._endPoint.Y}";
        }
    }
}
