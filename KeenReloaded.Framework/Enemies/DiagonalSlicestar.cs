using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class DiagonalSlicestar : Slicestar
    {
        public DiagonalSlicestar(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen _keen) :
            base(grid, hitbox, Direction.DOWN_LEFT, _keen)
        {
            
        }

        protected override void Initialize()
        {
            base.Initialize();
            VELOCITY = 20;
            int directionVal = _random.Next(1, 5);
            switch (directionVal)
            {
                case 1:
                    this.Direction = Enums.Direction.DOWN_LEFT;
                    break;
                case 2:
                    this.Direction = Enums.Direction.DOWN_RIGHT;
                    break;
                case 3:
                    this.Direction = Enums.Direction.UP_LEFT;
                    break;
                case 4:
                    this.Direction = Enums.Direction.UP_RIGHT;
                    break;
            }
        }

        protected override CollisionObject GetTopMostLandingTile(List<CollisionObject> collisions)
        {
            CollisionObject topMostTile;
            var landingTiles = collisions.Where(h => h.HitBox.Top >= this.HitBox.Top 
                && h.HitBox.Right >= this.HitBox.Left
                && h.HitBox.Left <= this.HitBox.Right);

            if (!landingTiles.Any())
                return null;

            int minY = landingTiles.Select(c => c.HitBox.Top).Min();
            topMostTile = landingTiles.FirstOrDefault(t => t.HitBox.Top == minY);

            return topMostTile;
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => !(c is PlatformTile) 
                && c.HitBox.Bottom <= this.HitBox.Top
                && c.HitBox.Right >= this.HitBox.Left
                && c.HitBox.Left <= this.HitBox.Right).ToList();
            if (debugTiles.Any())
            {
                int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
                CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
                return obj;
            }
            return null;
        }

        public override void Move()
        {
            int xOffset = this.IsLeftDirection(this.Direction) ? VELOCITY * -1 : VELOCITY;
            int yOffset = this.IsUpDirection(this.Direction) ? VELOCITY * -1 : VELOCITY;

            int xPos = this.IsLeftDirection(this.Direction) ? this.HitBox.X + xOffset : this.HitBox.X;
            int yPos = this.IsUpDirection(this.Direction) ? this.HitBox.Y + yOffset : this.HitBox.Y;

            Rectangle areaToCheck = new Rectangle(xPos, yPos, this.HitBox.Width + VELOCITY, this.HitBox.Height + VELOCITY);
            var collisions = this.CheckCollision(areaToCheck, true);

            var horizontalTile = this.IsLeftDirection(this.Direction) ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = this.IsUpDirection(this.Direction) ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
            //collision with walls/floors/ceilings
            if (horizontalTile != null || verticalTile != null)
            {
                int keenCollisionCheckX = xPos, keenCollisionCheckY = yPos;
                if (horizontalTile != null)
                {
                    int collideXPos = this.IsLeftDirection(this.Direction) ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                    this.HitBox = new Rectangle(collideXPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    keenCollisionCheckX = collideXPos;
                    this.Direction = ChangeHorizontalDirection(this.Direction);
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
                if (verticalTile != null)
                {
                    int collideYPos = this.IsUpDirection(this.Direction) ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                    this.HitBox = new Rectangle(this.HitBox.X, collideYPos, this.HitBox.Width, this.HitBox.Height);
                    keenCollisionCheckY = collideYPos;
                    this.Direction = ChangeVerticalDirection(this.Direction);
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
                }
                KillKeenIfColliding(new Rectangle(keenCollisionCheckX, keenCollisionCheckY, this.HitBox.Width + VELOCITY, this.HitBox.Height + VELOCITY));
            }
            else
            {//move freely
                KillKeenIfColliding(areaToCheck);
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
            }
        }

        protected override Direction ChangeHorizontalDirection(Direction direction)
        {
            if (direction == Enums.Direction.UP_LEFT)
            {
                direction = Enums.Direction.UP_RIGHT;
            }
            else if (direction == Enums.Direction.UP_RIGHT)
            {
                direction = Enums.Direction.UP_LEFT;
            }
            else if (direction == Enums.Direction.DOWN_LEFT)
            {
                direction = Enums.Direction.DOWN_RIGHT;
            }
            else if (direction == Enums.Direction.DOWN_RIGHT)
            {
                direction = Enums.Direction.DOWN_LEFT;
            }
            return direction;
        }

        protected override Direction ChangeVerticalDirection(Direction direction)
        {
            if (direction == Enums.Direction.UP_LEFT)
            {
                direction = Enums.Direction.DOWN_LEFT;
            }
            else if (direction == Enums.Direction.UP_RIGHT)
            {
                direction = Enums.Direction.DOWN_RIGHT;
            }
            else if (direction == Enums.Direction.DOWN_LEFT)
            {
                direction = Enums.Direction.UP_LEFT;
            }
            else if (direction == Enums.Direction.DOWN_RIGHT)
            {
                direction = Enums.Direction.UP_RIGHT;
            }
            return direction;
        }

        protected override void KillKeenIfColliding(Rectangle areaToCheck)
        {
            if (_keen.HitBox.Right >= areaToCheck.Left && _keen.HitBox.Left <= areaToCheck.Right)
            {
                double rise = areaToCheck.Height - this.HitBox.Height, run = areaToCheck.Width;
                double slope = rise / run;
                if (IsUpDirection(this.Direction))
                    slope *= -1.0;

                double xInputKeen = _keen.HitBox.Right - areaToCheck.Left;
                double yOutput = this.HitBox.Y + slope * xInputKeen;

                Rectangle killArea = new Rectangle(_keen.HitBox.Left, (int)yOutput, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(killArea))
                {
                    _keen.Die();
                }
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
