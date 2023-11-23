using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using System.Drawing;

namespace KeenReloaded.Framework.Enemies
{
    public class Fleex : DestructibleObject, IUpdatable, ISprite, IEnemy, IZombieBountyEnemy
    {
        private FleexState _state;
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private Image[] _lookSprites, _moveLeftSprites, _moveRightSprites, _stunnedSprites;

        #region sprite variables

        private const int LOOK_SPRITE_CHANGE_DELAY = 10;
        private int _currentLookSpriteChangeDelayTick;
        private int _currentLookSprite;

        private const int MOVE_SPRITE_CHANGE_DELAY = 1;
        private int _currentMoveSpriteChangeDelayTick;
        private int _currentMoveSprite;

        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelayTick;
        private int _currentStunnedSprite;

        #endregion

        #region behavior/physics variables

        private const int BASIC_FALL_VELOCITY = 30;

        private int _animationsToLook = 2;
        private const int ANIMATION_COUNT_LIMIT = 10;
        private int _animationCount;
        private bool _offChangeInDirection;
        private int _movementOffChangeInDirection;
        private int _distanceToMoveBeforeChase;
        private const int MAX_MOVEMENT_OFF_DIRECTION_CHANGE = 80;
        private const int MIN_MOVEMENT_OFF_DIRECTION_CHANGE = 40;

        private const int CHASE_VELOCITY = 14;
        private bool _doneLooking;

        private bool _hitAnimation;
        private const int HIT_ANIMATION_TIME = 1;
        private int _hitAnimationTimeTick;

        #endregion

        public Fleex(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            this.Health = 4;
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            _lookSprites = SpriteSheet.FleexLookImages;
            _moveLeftSprites = SpriteSheet.FleexLeftImages;
            _moveRightSprites = SpriteSheet.FleexRightImages;
            _stunnedSprites = SpriteSheet.FleexStunnedImages;

            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);

            this.Fall();
        }

       
        public override void Die()
        {
            this.UpdateStunnedState();
        }

        public void Update()
        {
            if (_hitAnimation && _hitAnimationTimeTick++ == HIT_ANIMATION_TIME)
            {
                _hitAnimationTimeTick = 0;
                _hitAnimation = false;
                _sprite.BackColor = Color.Transparent;
            }
            switch (_state)
            {
                case FleexState.FALLING:
                    this.Fall();
                    break;
                case FleexState.LOOKING:
                    this.Look();
                    break;
                case FleexState.CHASING:
                    this.Chase();
                    break;
                case FleexState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void Fall()
        {
            if (this.State != FleexState.FALLING)
            {
                this.State = FleexState.FALLING;
            }

            var tile = this.BasicFallReturnTile(BASIC_FALL_VELOCITY);
            if (tile != null)
            {
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                this.Chase();
            }
            else
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + BASIC_FALL_VELOCITY);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != FleexState.STUNNED)
            {
                this.State = FleexState.STUNNED;
            }

            this.BasicFall(BASIC_FALL_VELOCITY);

            this.UpdateHitboxBasedOnStunnedImage(_stunnedSprites, ref _currentStunnedSprite, ref _currentStunnedSpriteChangeDelayTick, STUNNED_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void Chase()
        {
            if (this.State != FleexState.CHASING)
            {
                this.State = FleexState.CHASING;
                _currentMoveSprite = 0;
                _animationCount = 0;
                _animationsToLook = 2;
                _offChangeInDirection = false;
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
               // this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                var initialCollisions = this.CheckCollision(this.HitBox, true);
                var colTile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(initialCollisions) : GetLeftMostRightTile(initialCollisions);
                this.BasicFall(1);
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
                if (colTile != null)
                {
                    int xCollidePos = _direction == Enums.Direction.LEFT ? colTile.HitBox.Right + 1 : colTile.HitBox.Left - this.HitBox.Width - 1;
                    this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                }
               
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (IsOnEdge(this.Direction))
            {
                ChangeDirection();
                return;
            }

            int xOffset = _direction == Enums.Direction.LEFT ? CHASE_VELOCITY * -1 : CHASE_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + CHASE_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                int movement = Math.Abs(xCollidePos - this.HitBox.X);
                ChangeDirection();
                _movementOffChangeInDirection += movement;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                _movementOffChangeInDirection += Math.Abs(xOffset);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }

            this.UpdateSpriteByDelayBase(ref _currentMoveSpriteChangeDelayTick, ref _currentMoveSprite, MOVE_SPRITE_CHANGE_DELAY, UpdateSprite);

            if (IsKeenInStationaryMoveState() && _animationCount >= _animationsToLook)
            {
               this.Look();
            }
            else if (_offChangeInDirection)
            {
                if (_movementOffChangeInDirection >= _distanceToMoveBeforeChase)
                {
                    _movementOffChangeInDirection = 0;
                    this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
                    _offChangeInDirection = false;
                }
            }
            else
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            }
        }

        private bool IsKeenInStationaryMoveState()
        {
            return _keen.MoveState == MoveState.STANDING 
                || _keen.MoveState == MoveState.HANGING 
                || _keen.MoveState == MoveState.ON_POLE 
                || _keen.MoveState == MoveState.STUNNED
                || _keen.IsDead();
        }

        private void ChangeDirection()
        {
            this.Direction = this.ChangeHorizontalDirection(this.Direction);
            _offChangeInDirection = true;
            _animationCount = 0;
            _movementOffChangeInDirection = 0;
            _distanceToMoveBeforeChase = _random.Next(MIN_MOVEMENT_OFF_DIRECTION_CHANGE, MAX_MOVEMENT_OFF_DIRECTION_CHANGE + 1);
            _animationsToLook = _random.Next(2, 5);
        }

        private void Look()
        {
            if (this.State != FleexState.LOOKING)
            {
                this.State = FleexState.LOOKING;
                _currentLookSprite = 0;
                _doneLooking = false;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                this.BasicFall(1);
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            this.UpdateSpriteByDelayBase(ref _currentLookSpriteChangeDelayTick, ref _currentLookSprite, LOOK_SPRITE_CHANGE_DELAY, UpdateSprite);
            if (_doneLooking)
            {
                this.Chase();
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
                    if (this.State == FleexState.FALLING || this.State == FleexState.STUNNED)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(_direction);
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
            get { return !IsDead(); }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);

            if (this.Health > 0)
            {
                _sprite.BackColor = Color.White;
                _hitAnimation = true;
            }
        }

        public bool IsActive
        {
            get { return _state != FleexState.STUNNED; }
        }

        FleexState State
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

        Direction Direction
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

        public PointItemType PointItem => PointItemType.KEEN6_BANANA_SPLIT;

        private void UpdateSprite()
        {
            switch (_state)
            {
                case FleexState.LOOKING:
                    if (_currentLookSprite >= _lookSprites.Length)
                    {
                        _currentLookSprite = 0;
                        _doneLooking = true;
                    }
                    _sprite.Image = _lookSprites[_currentLookSprite];
                    break;
                case FleexState.CHASING:
                case FleexState.FALLING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? _moveLeftSprites : _moveRightSprites;
                    if (_currentMoveSprite >= spriteSet.Length)
                    {
                        _currentMoveSprite = 0;
                        if (_animationCount < ANIMATION_COUNT_LIMIT)
                        {
                            _animationCount++;
                        }
                    }
                    _sprite.Image = spriteSet[_currentMoveSprite];
                    
                    break;
                case FleexState.STUNNED:
                    if (_currentStunnedSprite >= _stunnedSprites.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = _stunnedSprites[_currentStunnedSprite];
                    break;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum FleexState
    {
        CHASING,
        LOOKING,
        STUNNED,
        FALLING
    }
}
