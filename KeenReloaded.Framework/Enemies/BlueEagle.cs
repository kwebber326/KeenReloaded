using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Enemies
{
    public class BlueEagle : CollisionObject, IUpdatable, ISprite, IEnemy, IStunnable
    {
        private BirdMoveState _state;
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;

        private const int SPRITE_CHANGE_DELAY = 2;
        private int _spriteChangeDelayTick;

        private int _currentWalkSprite = 0;
        private Image[] _walkRightImages = new Image[]
        {
            Properties.Resources.keen4_blue_eagle_walk_right1,
            Properties.Resources.keen4_blue_eagle_walk_right2,
            Properties.Resources.keen4_blue_eagle_walk_right3,
            Properties.Resources.keen4_blue_eagle_walk_right4
        };

        private Image[] _walkLeftImages = new Image[]
        {
            Properties.Resources.keen4_blue_eagle_walk_left1,
            Properties.Resources.keen4_blue_eagle_walk_left2,
            Properties.Resources.keen4_blue_eagle_walk_left3,
            Properties.Resources.keen4_blue_eagle_walk_left4
        };

        private int _currentFlyingImage = 0;
        private Image[] _flyImages = new Image[]
        {
            Properties.Resources.keen4_blue_eagle_fly1,
            Properties.Resources.keen4_blue_eagle_fly2,
            Properties.Resources.keen4_blue_eagle_fly3,
            Properties.Resources.keen4_blue_eagle_fly4,
        };
        private Direction _horizontalDirection;

        private const int STUN_TIME = 100;
        private int _currentStunTimeTick = 0;

        private const int FALL_VELOCITY = 30;
        private const int WALK_VELOCITY = 10;
        private const int FLY_VELOCITY = 10;

        private const int WAIT_TIME = 30;
        private int _currentWaitTimeTick;

        private const int WALK_TIME = 50;
        private int _currentWalkTime = 0;

        public BlueEagle(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            SetDirectionFromKeenLocation();
            _sprite.Location = this.HitBox.Location;
            this.VerticalDirection = Direction.UP;
        }

        private void SetDirectionFromKeenLocation()
        {
            if (_keen.HitBox.X < this.HitBox.X)
            {
                this.HorizontalDirection = Direction.LEFT;
            }
            else
            {
                this.HorizontalDirection = Direction.RIGHT;
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
                    this.UpdateCollisionNodes(this.HorizontalDirection);
                    this.UpdateCollisionNodes(this.VerticalDirection);
                }
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public Direction HorizontalDirection
        {
            get
            {
                return _horizontalDirection;
            }
            set
            {
                _horizontalDirection = value;
                UpdateSprite();
            }
        }

        public void Update()
        {
            switch (_state)
            {
                case BirdMoveState.WAITING:
                    this.UpdateWaitState();
                    break;
                case BirdMoveState.WALKING:
                    this.Walk();
                    break;
                case BirdMoveState.FLYING:
                    this.Fly();
                    break;
                case BirdMoveState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void UpdateWaitState()
        {
            if (_currentWaitTimeTick++ == WAIT_TIME)
            {
                _currentWaitTimeTick = 0;
                this.Walk();
            }
            this.KillKeenIfColliding();
        }

        private void UpdateStunnedState()
        {
            if (_currentStunTimeTick++ == STUN_TIME)
            {
                _currentStunTimeTick = 0;
                this.Fly();
            }
            else
            {
                this.ExecuteGravity();
            }
        }

        private void ExecuteGravity()
        {
            CollisionObject obj = GetTopMostLandingTile(FALL_VELOCITY);
            if (obj != null)
            {
                this.Land(obj);
            }
            else
            {
                this.Fall();
            }
        }

        private void Fall()
        {
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + FALL_VELOCITY, this.HitBox.Width, this.HitBox.Height);
            this.UpdateCollisionNodes(Direction.DOWN);
        }

        private void Land(CollisionObject obj)
        {
            if (obj != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, obj.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                this.UpdateCollisionNodes(Direction.DOWN);
            }
        }

        //private CollisionObject GetTopMostLandingTile()
        //{
        //    CollisionObject topMostTile;
        //    Rectangle areaTocheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + 2);
        //    var items = this.CheckCollision(areaTocheck, true);

        //    if (!items.Any())
        //        return null;

        //    int minY = items.Select(c => c.HitBox.Top).Min();
        //    topMostTile = items.FirstOrDefault(t => t.HitBox.Top == minY);

        //    return topMostTile;
        //}

        private void Walk()
        {
            if (this.State != BirdMoveState.WALKING)
            {
                this.State = BirdMoveState.WALKING;
                _spriteChangeDelayTick = 0;
            }

            SetDirectionFromKeenLocation();

            var spriteSet = this.HorizontalDirection == Direction.LEFT ? _walkLeftImages : _walkRightImages;
            if (_currentWalkSprite++ == spriteSet.Length)
            {
                _currentWalkSprite = 0;
            }

            if (_spriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _spriteChangeDelayTick = 0;
                UpdateSprite();

            }

            if (this.HorizontalDirection == Direction.LEFT)
            {
                var collisions = this.CheckCollision(new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height - 1), true);
                CollisionObject leftTile = GetRightMostLeftTile(collisions);
                if (leftTile != null)
                {
                    this.HitBox = new Rectangle(leftTile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
            }

            var landingTile = GetTopMostLandingTile(FALL_VELOCITY);
            if (landingTile == null)
            {
                this.Fly();
                return;
            }

            if (_currentWalkTime++ == WALK_TIME)
            {
                this.Fly();
                return;
            }

            this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);

            int xOffset = this.HorizontalDirection == Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xlocation = this.HorizontalDirection == Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xlocation, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);
            var items = this.CheckCollision(areaToCheck, true);
            CollisionObject tile = this.HorizontalDirection == Direction.LEFT ? GetRightMostLeftTile(items) : GetLeftMostRightTile(items);

            if (tile != null)
            {
                int location = this.HorizontalDirection == Direction.LEFT ? tile.HitBox.Right - 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(location, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
            KillKeenIfColliding();
        }

        private void Fly()
        {
            if (this.State != BirdMoveState.FLYING)
            {
                this.State = BirdMoveState.FLYING;
                _spriteChangeDelayTick = 0;
                SetDirectionFromKeenLocation();
            }
            _currentWalkTime = 0;

            if (_spriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                if (_currentFlyingImage++ == _flyImages.Length)
                {
                    _currentFlyingImage = 0;
                }
                _spriteChangeDelayTick = 0;
                UpdateSprite();
            }

            var collisions = this.CheckCollision(new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height - 1), true);
            CollisionObject rightTile = GetLeftMostRightTile(collisions);
            if (this.HorizontalDirection == Direction.RIGHT)
            {
                if (rightTile != null)
                {
                    this.HitBox = new Rectangle(rightTile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
            }

            if (this.HorizontalDirection == Direction.LEFT)
            {
                CollisionObject leftTile = GetRightMostLeftTile(collisions);
                if (leftTile != null)
                {
                    this.HitBox = new Rectangle(leftTile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
            }

            CollisionObject tile = GetTopMostLandingTile(FLY_VELOCITY);
            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
            }

            if (this.HitBox.Top >= _keen.HitBox.Top)
            {
                MoveUp();
            }
            else if (this.HitBox.Bottom <= _keen.HitBox.Top + 5)
            {
                MoveDown();
            }

            if (this.HitBox.Left > _keen.HitBox.Left)
            {
                MoveLeft();
            }
            else if (this.HitBox.Right < _keen.HitBox.Right)
            {
                MoveRight();
            }
            KillKeenIfColliding();
        }

        private void MoveDown()
        {
            int yOffset = FLY_VELOCITY;
            this.VerticalDirection = Direction.DOWN;
            CollisionObject tile = GetTopMostLandingTile(FLY_VELOCITY);
            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
            }
        }

        private void MoveUp()
        {
            int yOffset = FLY_VELOCITY * -1;
            this.VerticalDirection = Direction.UP;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height + FLY_VELOCITY);
            var collisions = this.CheckCollision(areaToCheck, true);
            CollisionObject tile = GetCeilingTile(collisions);
            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
            }
        }

        private void MoveLeft()
        {
            this.HorizontalDirection = Direction.LEFT;
            int xOffset = FLY_VELOCITY * -1;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, FLY_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            CollisionObject horizontalTile = GetRightMostLeftTile(collisions);
            if (horizontalTile != null)
            {
                this.HitBox = new Rectangle(horizontalTile.HitBox.Right - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
        }

        private void MoveRight()
        {
            this.HorizontalDirection = Direction.RIGHT;
            int xOffset = FLY_VELOCITY;
            Rectangle areaToCheck = new Rectangle(this.HitBox.Right, this.HitBox.Y, FLY_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            CollisionObject horizontalTile = GetLeftMostRightTile(collisions);
            if (horizontalTile != null)
            {
                this.HitBox = new Rectangle(horizontalTile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
        }

        private void KillKeenIfColliding()
        {
            if (this.CollidesWith(_keen))
            {
                _keen.Die();
            }
        }

        private void KillKeenIfColliding(Rectangle areaToCheck)
        {
            if (this.HitBox.IntersectsWith(areaToCheck))
            {
                _keen.Die();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return this.State != BirdMoveState.STUNNED; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.Stun();
        }

        public bool IsActive
        {
            get { return true; }
        }

        public void Stun()
        {
            _currentStunTimeTick = 0;
            this.State = BirdMoveState.STUNNED;
        }

        public bool IsStunned
        {
            get { return this.State == BirdMoveState.STUNNED; }
        }

        private BirdMoveState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            switch (_state)
            {
                case BirdMoveState.WAITING:
                    this.Sprite.Image = this.HorizontalDirection == Direction.LEFT ? _walkLeftImages[0] : _walkRightImages[0];
                    break;
                case BirdMoveState.WALKING:
                    var spriteSet = this.HorizontalDirection == Direction.LEFT ? _walkLeftImages : _walkRightImages;
                    if (_currentWalkSprite >= spriteSet.Length)
                    {
                        _currentWalkSprite = 0;
                    }
                    this.Sprite.Image = spriteSet[_currentWalkSprite];
                    break;
                case BirdMoveState.STUNNED:
                    this.Sprite.Image = Properties.Resources.keen4_blue_eagle_stunned;
                    break;
                case BirdMoveState.FLYING:
                    if (_currentFlyingImage >= _flyImages.Length)
                    {
                        _currentFlyingImage = 0;
                    }
                    this.Sprite.Image = _flyImages[_currentFlyingImage];
                    break;
            }
            this.HitBox = new Rectangle(this.HitBox.Location, _sprite.Size);
        }

        public Direction VerticalDirection { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum BirdMoveState
    {
        WAITING,
        WALKING,
        FLYING,
        STUNNED
    }
}
