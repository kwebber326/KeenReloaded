using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using System.Drawing;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class MadMushroom : CollisionObject, IUpdatable, ISprite, IGravityObject, IEnemy
    {
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private bool _atPeakJump;
        private int _jumpState;

        private const int MAX_FALL_VELOCITY = 28;
        private const int FALL_ACCELERATION = 7;
        private int _currentFallVelocity = -1;
        private const int MAX_JUMP_VELOCITY = 25;
        private int _currentJumpVelocity;

        private const int SMALL_JUMP_MAX_HEIGHT = 40;
        private const int HIGH_JUMP_MAX_HEIGHT = 70;
        private const int JUMP_ACCELERATION = 10;
        private int _maxJumpHeight;
        private int _currentJumpHeight;

        public MadMushroom(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen) :
            base(grid, hitbox)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _keen = keen;
            SetDirectionBasedOnKeenLocation();
            if (IsNothingBelow())
            {
                this.Fall();
            }
            else
            {
                this.Jump();
            }
        }

        private bool IsNothingBelow()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + 1);
            var collisionNodes = this.CheckCollision(areaToCheck);
            var floors = collisionNodes.OfType<DebugTile>();
            return !floors.Any();
        }

        private void SetDirectionBasedOnKeenLocation()
        {
            if (_keen != null)
            {
                if (_keen.HitBox.X < this.HitBox.X)
                {
                    this.Direction = Enums.Direction.LEFT;
                }
                else
                {
                    this.Direction = Enums.Direction.RIGHT;
                }
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
                    if (_moveState == MoveState.JUMPING)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                }
            }
        }

        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            if (this.Direction == Enums.Direction.RIGHT)
            {
                _sprite.Image = _atPeakJump ? Properties.Resources.keen4_mad_mushroom_right2 : Properties.Resources.keen4_mad_mushroom_right1;
            }
            else
            {
                _sprite.Image = _atPeakJump ? Properties.Resources.keen4_mad_mushroom_left2 : Properties.Resources.keen4_mad_mushroom_left1;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
           
        }

        public void Update()
        {
            if (this._moveState == MoveState.JUMPING)
            {
                this.Jump();
            }
            else
            {
                this.Fall();
            }
            KillKeenIfColliding();
            SetDirectionBasedOnKeenLocation();
        }

        private void KillKeenIfColliding()
        {
            if (_keen != null && _keen.HitBox.IntersectsWith(this.HitBox))
            {
                _keen.Die();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        private MoveState _moveState;
        private Enums.Direction _direction;

        public void Jump()
        {
            _moveState = MoveState.JUMPING;
            if (!_atPeakJump)
            {
                _maxJumpHeight = _jumpState < 2 ? SMALL_JUMP_MAX_HEIGHT : HIGH_JUMP_MAX_HEIGHT;
                _currentJumpHeight += JUMP_ACCELERATION;
                Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y - _currentJumpHeight, this.HitBox.Width, this.HitBox.Height + _currentJumpHeight);
                var collisions = this.CheckCollision(areaToCheck);
                DebugTile tile = GetLowestCeilingTile(collisions);
                if (tile != null)
                {
                    _currentJumpHeight = 0;
                    this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Bottom, this.HitBox.Width, this.HitBox.Height);
                    this._moveState = MoveState.FALLING;
                    UpdateJumpState();
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - _currentJumpHeight, this.HitBox.Width, this.HitBox.Height);
                    if (_currentJumpHeight >= _maxJumpHeight)
                    {
                        _atPeakJump = true;
                    }
                }
            }
            else
            {
                _atPeakJump = false;
                _currentJumpHeight = 0;
                _moveState = MoveState.FALLING;
                UpdateJumpState();
            }
        }

        private void UpdateJumpState()
        {
            if (_jumpState < 2)
            {
                _jumpState++;
            }
            else
            {
                _jumpState = 0;
            }
        }

        public DebugTile GetLowestCeilingTile(List<CollisionObject> collisions)
        {
            if (collisions != null && collisions.Any())
            {
                var tile = collisions.OfType<DebugTile>().OrderByDescending(c => c.HitBox.Bottom).FirstOrDefault();
                return tile;
            }
            return null;
        }

        public bool CanJump
        {
            get { return true; }
        }

        public void Fall()
        {
            _moveState = MoveState.FALLING;
            if (_currentFallVelocity == -1)
                _currentFallVelocity = 0;
            else if (_currentFallVelocity < MAX_FALL_VELOCITY)
                _currentFallVelocity += FALL_ACCELERATION;
                
             Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + _currentFallVelocity);
             var collisions = this.CheckCollision(areaToCheck);
            var tile = GetTopMostLandingTile(collisions);
             if (tile != null)
             {
                 this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height, this.HitBox.Width, this.HitBox.Height);
                 this._moveState = MoveState.JUMPING;
                 _currentFallVelocity = 0;
             }
             else
             {
                 this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentFallVelocity, this.HitBox.Width, this.HitBox.Height);
             }
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
            get { return true; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
