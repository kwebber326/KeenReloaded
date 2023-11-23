﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class Lick : DestructibleObject, IEnemy, ISprite, IGravityObject, IMoveable, IUpdatable, IFireable, IZombieBountyEnemy
    {
        private PictureBox _sprite;
        private CommanderKeen _keen;
        private const int LONG_JUMP_DISTANCE = 80;
        private Enums.MoveState _moveState;
        private Enums.Direction _direction;
        private LickMoveState _state;
        private bool _shortJump;
        private int _currentFireBreathingState;
        private const int MAX_FALL_VELOCITY = 60;
        private const int BASIC_FALL_VELOCITY = 30;
        private int _currentFallVelocity = 0;
        private const int FALL_ACCELERATION = 15;
        private const int LONG_JUMP_VELOCITY = 30;
        private const int SHORT_JUMP_VELOCITY = 5;
        private int _moveVelocity = LONG_JUMP_VELOCITY;
        private const int JUMP_DELAY = 5;
        private int _currentJumpDelayTick = 0;
        private const int MAX_JUMP_HEIGHT = 30;
        private const int JUMP_ACCELLERATION = 10;
        private int _currentJumpHeight;
        private const int BREATH_FIRE_LENGTH = 10;
        private int _breathFireTick = 0;
        private Rectangle _originalLocation;
        private const int FIRE_DISTANCE = 30;
        private CollisionObject _previousLandingTile;
        private int _currentStunSprite = 0;

        private Image[] _fireBreathImagesLeft = new Image[]
        {
            Properties.Resources.keen4_lick_left_fire1,
            Properties.Resources.keen4_lick_left_fire2,
            Properties.Resources.keen4_lick_left_fire3,
        };

        private Image[] _fireBreathImagesRight = new Image[]
        {
            Properties.Resources.keen4_lick_right_fire1,
            Properties.Resources.keen4_lick_right_fire2,
            Properties.Resources.keen4_lick_right_fire3,
        };

        private Image[] _stunImages = new Image[]
        {
            Properties.Resources.keen4_lick_stun2,
            Properties.Resources.keen4_lick_stun3,
            Properties.Resources.keen4_lick_stun4,
        };
        private bool _isFiring;

        public Lick(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Commander Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            SetDirectionFromKeenLocation();
            this.Health = 1;
            if (IsNothingBeneath())
            {
                this.Fall();
            }
        }

        protected override bool IsNothingBeneath()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Bottom, this.HitBox.Width, 1);
            var items = this.CheckCollision(areaToCheck, true);
            return !items.Any();
        }

        private void SetDirectionFromKeenLocation()
        {
            int center = this.HitBox.Right - ((this.HitBox.Right - this.HitBox.Left) / 2);
            int keenCenter = _keen.HitBox.Right - ((_keen.HitBox.Right - _keen.HitBox.Left) / 2);
            if (keenCenter < center)
            {
                this.Direction = Enums.Direction.LEFT;
            }
            else
            {
                this.Direction = Enums.Direction.RIGHT;
            }
        }

        private LickMoveState State
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

        protected override void HandleCollision(CollisionObject obj)
        {

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
                if (this.Sprite != null)
                    this.Sprite.Location = base.HitBox.Location;

                if (_collidingNodes != null)
                {
                    this.UpdateCollisionNodes(this.Direction);
                    if (this.MoveState == Enums.MoveState.FALLING)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                }
            }
        }

        public bool DeadlyTouch
        {
            get { return this.State == LickMoveState.BREATHING_FIRE; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return !this.IsDead(); }
        }

        public PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Jump()
        {
            if (this.State != LickMoveState.JUMPING)
            {
                this.State = LickMoveState.JUMPING;
            }
            int xOffset = this.Direction == Enums.Direction.LEFT ? _moveVelocity * -1 : _moveVelocity;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y - JUMP_ACCELLERATION, this.HitBox.Width, JUMP_ACCELLERATION);
            var items = this.CheckCollision(areaToCheck, true);

            CollisionObject tile = GetCeilingTile(items);
            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
                this.Fall();
                _currentJumpHeight = 0;
            }
            else if (_currentJumpHeight < MAX_JUMP_HEIGHT)
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - JUMP_ACCELLERATION, this.HitBox.Width, this.HitBox.Height);
                _currentJumpHeight += JUMP_ACCELLERATION;
                this.Move();
            }
            else
            {
                _currentJumpHeight = 0;
                this.Fall();
            }
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => c is DebugTile).ToList();
            if (debugTiles.Any())
            {
                int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
                CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
                return obj;
            }
            return null;
        }

        public bool CanJump
        {
            get { return this.MoveState == Enums.MoveState.STANDING; }
        }

        public void Fall()
        {
            if (this.State != LickMoveState.FALLING)
            {
                this.State = LickMoveState.FALLING;
            }
            CollisionObject landingTile = GetTopMostLandingTile();
            if (landingTile != null)
            {
                this.State = LickMoveState.LANDING;
                this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Top - this.HitBox.Height, this.HitBox.Width, this.HitBox.Height);
                _currentFallVelocity = 0;
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentFallVelocity, this.HitBox.Width, this.HitBox.Height);
                if (_currentFallVelocity + FALL_ACCELERATION <= MAX_FALL_VELOCITY)
                {
                    _currentFallVelocity += FALL_ACCELERATION;
                }
            }
            this.Move();
        }

        private CollisionObject GetTopMostLandingTile()
        {
            CollisionObject topMostTile;
            Rectangle areaTocheck = new Rectangle(this.HitBox.X, this.HitBox.Bottom, this.HitBox.Width, _currentFallVelocity);
            var items = this.CheckCollision(areaTocheck, true);

            if (!items.Any(c1 => c1.HitBox.Top >= this.HitBox.Top))
                return null;

            int minY = items.Where(c1 => c1.HitBox.Top >= this.HitBox.Top).ToList().Select(c => c.HitBox.Top).Min();
            topMostTile = items.FirstOrDefault(t => t.HitBox.Top == minY);

            return topMostTile;
        }

        public void Move()
        {
            _moveVelocity = _shortJump ? SHORT_JUMP_VELOCITY : LONG_JUMP_VELOCITY;
            int xOffset = this.Direction == Enums.Direction.RIGHT ? _moveVelocity : _moveVelocity * -1;
            Rectangle areaToCheck = new Rectangle(this.Direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X, this.HitBox.Y, this.HitBox.Width + _moveVelocity, this.HitBox.Height);
            var items = this.CheckCollision(areaToCheck, true);

            CollisionObject tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(items) : GetLeftMostRightTile(items);
            if (tile != null)
            {
                if (this.Direction == Enums.Direction.LEFT)
                {
                    this.HitBox = new Rectangle(tile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
                else
                {
                    this.HitBox = new Rectangle(tile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
        }

        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            if (collisions.Any() && collisions.OfType<DebugTile>().Any())
            {
                int maxX = collisions.OfType<DebugTile>().Select(t => t.HitBox.Right).Max();
                CollisionObject obj = collisions.FirstOrDefault(x => x.HitBox.Right == maxX);
                return obj;
            }
            return null;
        }

        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            if (collisions.Any() && collisions.OfType<DebugTile>().Any())
            {
                int minX = collisions.OfType<DebugTile>().Select(t => t.HitBox.Left).Min();
                CollisionObject obj = collisions.FirstOrDefault(x => x.HitBox.Left == minX);
                return obj;
            }
            return null;
        }
        public void Stop()
        {
            throw new NotImplementedException();
        }


        private void UpdateSprite()
        {
            if (_state != LickMoveState.STUNNED && IsDead())
            {
                this.Sprite.Image = Properties.Resources.keen4_lick_stun1;
                return;
            }
            switch (_state)
            {
                case LickMoveState.FALLING:
                    this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? Properties.Resources.keen4_lick_left_fall
                        : Properties.Resources.keen4_lick_right_fall;
                    break;
                case LickMoveState.PREPARING_JUMP:
                    this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? Properties.Resources.keen4_lick_left_jump1
                     : Properties.Resources.keen4_lick_right_jump1;
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - (this.HitBox.Height / 6), this.Sprite.Size.Width, this.Sprite.Size.Height);
                    break;
                case LickMoveState.JUMPING:
                    this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? Properties.Resources.keen4_lick_left_jump2
                        : Properties.Resources.keen4_lick_right_jump2;
                    break;
                case LickMoveState.LANDING:
                    this.Sprite.Image = this.Direction == Enums.Direction.LEFT ? Properties.Resources.keen4_lick_left_land :
                        Properties.Resources.keen4_lick_right_land;
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + (this.HitBox.Height / 6), this.Sprite.Size.Width, this.Sprite.Size.Height);
                    break;
                case LickMoveState.BREATHING_FIRE:
                    if (this.Direction == Enums.Direction.LEFT)
                    {
                        if (_currentFireBreathingState == 0)
                        {
                            _originalLocation = new Rectangle(this.HitBox.Location, this.HitBox.Size);
                            this.Sprite.Image = _fireBreathImagesLeft[0];
                            this.HitBox = new Rectangle(_originalLocation.X - (_originalLocation.Width / 6), _originalLocation.Y + _originalLocation.Height / 9, _sprite.Width, _sprite.Height);
                        }
                        else if (_currentFireBreathingState % 2 == 1)
                        {
                            this.Sprite.Image = _fireBreathImagesLeft[1];
                            this.HitBox = new Rectangle(_originalLocation.X - (_originalLocation.Width + (_originalLocation.Width * (42 / 74))), this.HitBox.Y, _sprite.Width, _sprite.Height);
                        }
                        else
                        {
                            this.Sprite.Image = _fireBreathImagesLeft[2];
                            this.HitBox = new Rectangle(_originalLocation.X - (_originalLocation.Width + (_originalLocation.Width * (42 / 78))), this.HitBox.Y, _sprite.Width, _sprite.Height);
                        }
                    }
                    else
                    {
                        if (_currentFireBreathingState == 0)
                        {
                            _originalLocation = new Rectangle(this.HitBox.Location, this.HitBox.Size);
                            this.Sprite.Image = _fireBreathImagesRight[0];
                            this.HitBox = new Rectangle(_originalLocation.X, _originalLocation.Y + _originalLocation.Height / 9, _sprite.Width, _sprite.Height);
                        }
                        else if (_currentFireBreathingState % 2 == 1)
                        {
                            this.Sprite.Image = _fireBreathImagesRight[1];
                            this.HitBox = new Rectangle(_originalLocation.X, this.HitBox.Y, _sprite.Width, _sprite.Height);
                        }
                        else
                        {
                            this.Sprite.Image = _fireBreathImagesRight[2];
                            this.HitBox = new Rectangle(_originalLocation.X, this.HitBox.Y, _sprite.Width, _sprite.Height);
                        }
                    }
                    break;
                case LickMoveState.STUNNED:
                    if (_currentStunSprite >= _stunImages.Length)
                    {
                        _currentStunSprite = 0;
                    }
                    this.Sprite.Image = _stunImages[_currentStunSprite++];
                    this.HitBox = new Rectangle(this.Sprite.Location, this.Sprite.Size);
                    break;
            }
        }

        public Enums.MoveState MoveState
        {
            get
            {
                return _moveState;
            }
            set
            {
                _moveState = value;
                UpdateSprite();
            }
        }

        public Enums.Direction Direction
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

        public void Update()
        {
            switch (_state)
            {
                case LickMoveState.FALLING:
                    if (this.IsDead())
                    {
                        this.BasicFall(BASIC_FALL_VELOCITY);
                    }
                    else
                    {
                        this.Fall();
                    }
                    break;
                case LickMoveState.LANDING:
                    if (_currentJumpDelayTick++ == JUMP_DELAY && !IsDead())
                    {
                        _currentJumpDelayTick = 0;
                        this.State = LickMoveState.PREPARING_JUMP;
                        SetDirectionFromKeenLocation();
                    }
                    else if (IsDead())
                    {
                        this.State = LickMoveState.STUNNED;
                        var tile = GetTopMostLandingTile();
                        if (tile != null)
                        {
                            this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Y - this.HitBox.Height, this.HitBox.Width, this.HitBox.Height);
                        }
                    }
                    break;
                case LickMoveState.PREPARING_JUMP:
                    _currentJumpDelayTick = 0;
                    SetDirectionFromKeenLocation();
                    _shortJump = IsKeenClose();
                    if (IsKeenInRange())
                    {
                        this.Fire();
                    }
                    else
                    {
                        this.Jump();
                    }
                    break;
                case LickMoveState.JUMPING:
                    this.Jump();
                    break;
                case LickMoveState.BREATHING_FIRE:
                    this.Fire();
                    break;
                case LickMoveState.STUNNED:
                    UpdateSprite();
                    if (IsNothingBeneath())
                    {
                        this.Fall();
                    }
                    break;
            }
        }

        private bool IsKeenClose()
        {
            return (_keen.HitBox.X >= this.HitBox.X - LONG_JUMP_DISTANCE && _keen.HitBox.X < this.HitBox.Right)
                || (_keen.HitBox.X <= this.HitBox.Right + LONG_JUMP_DISTANCE && _keen.HitBox.X > this.HitBox.Right);
        }

        private bool IsKeenInRange()
        {
            return ((_keen.HitBox.Right >= this.HitBox.X - FIRE_DISTANCE && _keen.HitBox.Right < this.HitBox.Left)
                || (_keen.HitBox.X <= this.HitBox.Right + FIRE_DISTANCE && _keen.HitBox.X >= this.HitBox.X))
                && (_keen.HitBox.Bottom > this.HitBox.Top && _keen.HitBox.Top < this.HitBox.Bottom);
        }

        public override void Die()
        {
            UpdateSprite();
        }

        public void Fire()
        {
            if (!this.IsDead())
            {
                if (!_isFiring)
                {
                    _isFiring = true;

                    this.State = LickMoveState.BREATHING_FIRE;
                }
                if (_currentFireBreathingState++ == BREATH_FIRE_LENGTH)
                {
                    _currentFireBreathingState = 0;
                    _isFiring = false;
                    this.State = LickMoveState.PREPARING_JUMP;
                    this.HitBox = _originalLocation;
                    if (IsNothingBeneath())
                    {
                        this.Fall();
                    }
                }
                else
                {
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
                    }
                    UpdateSprite();
                }
            }
        }

        public bool IsFiring
        {
            get { return _isFiring; }
        }

        public int Ammo
        {
            get { return -1; }
        }

        public PointItemType PointItem => PointItemType.KEEN4_SHIKKERS_CANDY_BAR;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum LickMoveState
    {
        LANDING,
        PREPARING_JUMP,
        JUMPING,
        FALLING,
        BREATHING_FIRE,
        STUNNED
    }
}
