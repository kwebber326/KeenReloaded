using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class Spindred : CollisionObject, IUpdatable, ISprite, IEnemy
    {
        private CommanderKeen _keen;
        private Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private int _currentSprite;
        private const int INITIAL_BOUNCE_VELOCITY = 30;
        private const int BOUNCE_DECELERATION = 5;
        private int _currentVerticalVelocity;
        private const int INITIAL_FALL_VELOCITY = 0;
        private const int GRAVITY_ACCELERATION = 5;
        private const int MAX_FALL_VELOCITY = 80;
        private const int DIRECTION_CHANGE_VELOCITY = 80;
        private const int BOUNCES_UNTIL_GRAVITY_SWITCH = 2;
        private int _currentBounce;
        private const int SPRITE_CHANGE_DELAY = 2;
        private int _spriteChangeDelayTick;
        private bool _changingGravity;

        public Spindred(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, Direction direction)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            if (direction != Direction.UP && direction != Direction.DOWN)
                throw new ArgumentException("Spindreds can only move vertically");

            _direction = direction;
            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = SpriteSheet.SpindredImages[_currentSprite];
            _sprite.Location = this.HitBox.Location;
            _currentVerticalVelocity = _direction == Direction.UP ? INITIAL_FALL_VELOCITY * -1 : INITIAL_FALL_VELOCITY;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
           
            if (_direction == Direction.UP)
            {
                ExecuteReverseGravityMotion();
            }
            else
            {
                ExecuteNormalGravityMotion();
            }
            UpdateSprite();
        }

        private void ExecuteNormalGravityMotion()
        {
            int absoluteSpeed = Math.Abs(_currentVerticalVelocity);
            int yCollisionCheckHeight = this.HitBox.Height + absoluteSpeed;
            int yPosCollisionCheck = _currentVerticalVelocity < 0 ? this.HitBox.Y - absoluteSpeed : this.HitBox.Y;

            Rectangle areaToCheck = new Rectangle(this.HitBox.X, yPosCollisionCheck, this.HitBox.Width, yCollisionCheckHeight);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _currentVerticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
            if (tile != null)
            {
                _changingGravity = false;
                int yCollidePos = _currentVerticalVelocity < 0 ? tile.HitBox.Bottom + 1 : tile.HitBox.Top - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(this.HitBox.X, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                if (this.HitBox.IntersectsWith(_keen.HitBox))
                {
                    _keen.Die();
                }
                _currentVerticalVelocity = _currentVerticalVelocity < 0 ? INITIAL_BOUNCE_VELOCITY : INITIAL_BOUNCE_VELOCITY * -1;
                if (_currentBounce++ == BOUNCES_UNTIL_GRAVITY_SWITCH)
                {
                    _currentBounce = 0;
                    _direction = Direction.UP;
                    _currentVerticalVelocity = DIRECTION_CHANGE_VELOCITY * -1;
                    _changingGravity = true;
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
                if (!_changingGravity)
                {
                    if (_currentVerticalVelocity >= 0)
                    {
                        if (_currentVerticalVelocity + GRAVITY_ACCELERATION <= MAX_FALL_VELOCITY)
                        {
                            _currentVerticalVelocity += GRAVITY_ACCELERATION;
                        }
                        else
                        {
                            _currentVerticalVelocity = MAX_FALL_VELOCITY;
                        }
                    }
                    else
                    {
                        if (_currentVerticalVelocity + GRAVITY_ACCELERATION <= 0)
                        {
                            _currentVerticalVelocity += GRAVITY_ACCELERATION;
                        }
                        else
                        {
                            _currentVerticalVelocity = 0;
                        }
                    }
                }
            }
        }

        private void ExecuteReverseGravityMotion()
        {
            int absoluteSpeed = Math.Abs(_currentVerticalVelocity);
            int yCollisionCheckHeight = this.HitBox.Height + absoluteSpeed;
            int yPosCollisionCheck = _currentVerticalVelocity < 0 ? this.HitBox.Y - absoluteSpeed : this.HitBox.Y;

            Rectangle areaToCheck = new Rectangle(this.HitBox.X, yPosCollisionCheck, this.HitBox.Width, yCollisionCheckHeight);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _currentVerticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
            if (tile != null)
            {
                _changingGravity = false;
                int yCollidePos = _currentVerticalVelocity < 0 ? tile.HitBox.Bottom + 1 : tile.HitBox.Top - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(this.HitBox.X, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                if (this.HitBox.IntersectsWith(_keen.HitBox))
                {
                    _keen.Die();
                }
                _currentVerticalVelocity = _currentVerticalVelocity < 0 ? INITIAL_BOUNCE_VELOCITY : INITIAL_BOUNCE_VELOCITY * -1;
                if (_currentBounce++ == BOUNCES_UNTIL_GRAVITY_SWITCH)
                {
                    _currentBounce = 0;
                    _direction = Direction.DOWN;
                    _currentVerticalVelocity = DIRECTION_CHANGE_VELOCITY;
                    _changingGravity = true;
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
                if (!_changingGravity)
                {
                    if (_currentVerticalVelocity <= 0)
                    {
                        if (_currentVerticalVelocity - GRAVITY_ACCELERATION >= MAX_FALL_VELOCITY * -1)
                        {
                            _currentVerticalVelocity -= GRAVITY_ACCELERATION;
                        }
                        else
                        {
                            _currentVerticalVelocity = MAX_FALL_VELOCITY * -1;
                        }
                    }
                    else
                    {
                        if (_currentVerticalVelocity - GRAVITY_ACCELERATION >= 0)
                        {
                            _currentVerticalVelocity -= GRAVITY_ACCELERATION;
                        }
                        else
                        {
                            _currentVerticalVelocity = 0;
                        }
                    }
                }
            }
        }

        private void UpdateSprite()
        {
            if (_spriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _spriteChangeDelayTick = 0;
                if (++_currentSprite >= SpriteSheet.SpindredImages.Length)
                {
                    _currentSprite = 0;
                }
                _sprite.Image = SpriteSheet.SpindredImages[_currentSprite];
            }
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
                    if (_currentVerticalVelocity < 0)
                    {
                        this.UpdateCollisionNodes(Direction.UP);
                    }
                    else if (_currentVerticalVelocity > 0)
                    {
                        this.UpdateCollisionNodes(Direction.DOWN);
                    }
                }
            }
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

        public Direction Direction
        {
            get
            {
                return _direction;
            }
        }

        public bool IsActive
        {
            get { return true; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Direction.ToString()}";
        }
    }
}
