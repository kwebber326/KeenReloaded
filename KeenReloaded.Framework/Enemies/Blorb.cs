using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class Blorb : CollisionObject, IUpdatable, ISprite, IEnemy
    {
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private Image[] _sprites;
        private const int FIRST_DELAY = 8, SECOND_DELAY = 3;
        private int SPRITE_CHANGE_DELAY = 5;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private const int VELOCITY = 5;

        public Blorb(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base (grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
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
                    this.UpdateCollisionNodes(this.Direction);
                }
            }
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprites = SpriteSheet.BlorbImages;
            _sprite.Image = _sprites[_currentSprite];
            RandomizeDirection();
        }

        private void RandomizeDirection()
        {
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

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }

        public void Update()
        {
            //will determine how/if we should bounce (default no bounce)
            bool bounceHorizontal = false, bounceVertical = false;
            //determine which diagonal direction we are moving in
            bool isLeftDirection = IsLeftDirection(this.Direction);
            bool isUpDirection = IsUpDirection(this.Direction);

            //get the velocities on horizontal and vertical plane
            int xOffset = isLeftDirection ? VELOCITY * -1 : VELOCITY;
            int yOffset = isUpDirection ? VELOCITY * -1 : VELOCITY;

            //get the distance away from hitbox to check for
            int xPosCheck = isLeftDirection ? this.HitBox.X + xOffset : this.HitBox.X;
            int yPosCheck = isUpDirection ? this.HitBox.Y + yOffset : this.HitBox.Y;

            //get collisions
            Rectangle areaToCheck = new Rectangle(xPosCheck, yPosCheck, this.HitBox.Width + VELOCITY, this.HitBox.Height + VELOCITY);
            var collisions = this.CheckCollision(areaToCheck, true);

            var horizontalTile = isLeftDirection ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = isUpDirection ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);

            //determine if we hit anything
            //by default, our new positions reflect a non collision
            int newXPos = this.HitBox.X + xOffset;
            int newYPos = this.HitBox.Y + yOffset;
            int xDistFromCollision = -1;
            int yDistFromCollision = -1;
            //set x,y coordinates from collisions
            if (horizontalTile != null)
            {
                newXPos = isLeftDirection ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                xDistFromCollision = isLeftDirection ? this.HitBox.Left - horizontalTile.HitBox.Right : horizontalTile.HitBox.Left - this.HitBox.Right;
                if (verticalTile == null)
                    bounceHorizontal = true;
            }
            if (verticalTile != null && !IsThroughPlatformTileVertically(verticalTile, isUpDirection))
            {
                newYPos = isUpDirection ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                yDistFromCollision = isUpDirection ? this.HitBox.Top - verticalTile.HitBox.Bottom : this.HitBox.Bottom - verticalTile.HitBox.Top;
                if (horizontalTile == null)
                    bounceVertical = true;
            }
            //in the event of collisions on both the horizontal and vertical plane, prioritize the one that is closer to the object
            //and update both x,y positions accordingly
            if (verticalTile != null && horizontalTile != null && horizontalTile != verticalTile && !IsThroughPlatformTileVertically(verticalTile, isUpDirection))
            {
                if (xDistFromCollision < yDistFromCollision)
                {
                    newYPos = isUpDirection ? this.HitBox.Y - xDistFromCollision : this.HitBox.Y + xDistFromCollision;
                    bounceHorizontal = true;
                }
                else
                {
                    newXPos = isLeftDirection ? this.HitBox.X - yDistFromCollision : this.HitBox.X + yDistFromCollision;
                    bounceVertical = true;
                }
            }//priority to horizontal bounce
            else if (horizontalTile != null && verticalTile != null && horizontalTile == verticalTile && !IsThroughPlatformTileVertically(verticalTile, isUpDirection))
            {
                newYPos = isUpDirection ? this.HitBox.Y - xDistFromCollision : this.HitBox.Y + xDistFromCollision;
                bounceHorizontal = true;
            }

            //check for keen collisions
            CheckForKeenCollisions(isLeftDirection, isUpDirection, newXPos, newYPos);

            //update the location to the new x,y position
            this.HitBox = new Rectangle(newXPos, newYPos, this.HitBox.Width, this.HitBox.Height);


            //bounce if applicable to the situation (priority to horizontal)
            if (bounceHorizontal)
            {
                BounceHorizontal();
            }
            else if (bounceVertical)
            {
                BounceVertical();
            }

            //update sprite
            SPRITE_CHANGE_DELAY = _currentSprite == 0 ? SECOND_DELAY : FIRST_DELAY;
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void CheckForKeenCollisions(bool isLeftDirection, bool isUpDirection, int newXPos, int newYPos)
        {
            int xMovement = Math.Abs(this.HitBox.X - newXPos);
            int yMovement = Math.Abs(this.HitBox.Y - newYPos);
            int xCheck = isLeftDirection ? this.HitBox.X - xMovement : this.HitBox.X;
            int yCheck = isUpDirection ? this.HitBox.Y - yMovement : this.HitBox.Y;
            Rectangle areaToCheckForKeen = new Rectangle(xCheck, yCheck, this.HitBox.Width + xMovement, this.HitBox.Height + yMovement);
            KillKeenIfColliding(areaToCheckForKeen);
        }

        protected void KillKeenIfColliding(Rectangle areaToCheck)
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

        private void BounceHorizontal()
        {
            switch (this.Direction)
            {
                case Enums.Direction.DOWN_LEFT:
                    this.Direction = Enums.Direction.DOWN_RIGHT;
                    break;
                case Enums.Direction.DOWN_RIGHT:
                    this.Direction = Enums.Direction.DOWN_LEFT;
                    break;
                case Enums.Direction.UP_LEFT:
                    this.Direction = Enums.Direction.UP_RIGHT;
                    break;
                case Enums.Direction.UP_RIGHT:
                    this.Direction = Enums.Direction.UP_LEFT;
                    break;
            }
        }

        private void BounceVertical()
        {
            switch (this.Direction)
            {
                case Enums.Direction.DOWN_LEFT:
                    this.Direction = Enums.Direction.UP_LEFT;
                    break;
                case Enums.Direction.DOWN_RIGHT:
                    this.Direction = Enums.Direction.UP_RIGHT;
                    break;
                case Enums.Direction.UP_LEFT:
                    this.Direction = Enums.Direction.DOWN_LEFT;
                    break;
                case Enums.Direction.UP_RIGHT:
                    this.Direction = Enums.Direction.DOWN_RIGHT;
                    break;
            }
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }

        private bool IsThroughPlatformTileVertically(CollisionObject platformTile, bool isupDirection)
        {
            if (platformTile == null)
                return false;

            if (this.HitBox.Bottom < platformTile.HitBox.Top && !isupDirection)
                return false;
            if (this.HitBox.Top > platformTile.HitBox.Bottom && isupDirection)
                return false;

            bool isPlatformTile = (platformTile is SingleMaskedPlatformTile) || (platformTile is MaskedPlatformTile);
            return isPlatformTile;
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return true; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            
        }

        public bool IsActive
        {
            get { return false; }
        }

        public Enums.Direction Direction { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
