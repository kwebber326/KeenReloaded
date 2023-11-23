using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class Flect : DestructibleObject, IUpdatable, ISprite, IEnemy, IDeflector, IZombieBountyEnemy
    {
        private CommanderKeen _keen;
        private FlectState _state;
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private Image[] _walkLeftSprites, _walkRightSprites, _stunnedSprites;
        private int _currentWalkSprite, _currentStunnedSprite;
        private const int WALK_SPRITE_CHANGE_DELAY = 2;
        private int _currentWalkSpriteChangeDelayTick;
        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelayTick;

        private const int BASIC_FALL_VELOCITY = 30;
        private const int WALK_VELOCITY = 5;
        private const int CHASE_KEEN_CHANCE = 20;
        private bool _walkingOffWallCollision;
        private const int TURN_TIME = 2;
        private int _turnTimeTick;
        private const int MIN_TURN_OFF_COLLISION_TIME = 2;
        private int _collisionTimeTick;

        public Flect(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
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
                    if (_state == FlectState.FALLING)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(this.Direction);
                    }
                }
            }
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            _walkLeftSprites = SpriteSheet.FlectWalkLeftImages;
            _walkRightSprites = SpriteSheet.FlectWalkRightImages;
            _stunnedSprites = SpriteSheet.FlectStunnedImages;

            this.Direction = this.SetDirectionFromObjectHorizontal(_keen, true);
            this.Fall();
        }

        public override void Die()
        {
            _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
            _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            this.UpdateStunnedState();
        }

        public void Update()
        {
            switch (_state)
            {
                case FlectState.FALLING:
                    this.Fall();
                    break;
                case FlectState.TURNING:
                    this.Turn();
                    break;
                case FlectState.WALKING:
                    this.Walk();
                    break;
                case FlectState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void Fall()
        {
            if (this.State != FlectState.FALLING)
            {
                this.State = FlectState.FALLING;
            }

            var tile = this.BasicFallReturnTile(BASIC_FALL_VELOCITY);
            if (tile != null)
            {
                this.Walk();
            }
        }

        private void Turn()
        {
            if (this.State != FlectState.TURNING)
            {
                this.State = FlectState.TURNING;
                _turnTimeTick = 0;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                _collisionTimeTick = 0;
                CheckPushFromThisDirection();
            }

            if (_turnTimeTick++ == TURN_TIME)
            {
                this.Direction = this.SetDirectionFromObjectHorizontal(_keen, true);
                this.Walk();
            }
        }

        private void Walk()
        {
            if (this.State != FlectState.WALKING)
            {
                this.State = FlectState.WALKING;
                CheckPushFromThisDirection();
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (IsOnEdge(this.Direction, 2))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                CheckPushFromThisDirection();
                _walkingOffWallCollision = true;
            }

            if (IsKeenBehindThis())
            {
                if (!_walkingOffWallCollision && _collisionTimeTick++ == MIN_TURN_OFF_COLLISION_TIME)
                {
                    this.Turn();
                    return;
                }
                else
                {
                    int chaseVal = _random.Next(1, CHASE_KEEN_CHANCE + 1);
                    if (chaseVal == CHASE_KEEN_CHANCE || (IsKeenIntersectingOnVerticalPlane() && _collisionTimeTick ++ == MIN_TURN_OFF_COLLISION_TIME))
                    {
                        _walkingOffWallCollision = false;
                        this.Turn();
                        return;
                    }
                }
            }

            int xOffset = _direction == Enums.Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                _walkingOffWallCollision = true;
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                ExecuteFreeMoveLogic(xOffset);
            }
            else
            {
                ExecuteFreeMoveLogic(xOffset);
            }

            this.UpdateSpriteByDelayBase(ref _currentWalkSpriteChangeDelayTick, ref _currentWalkSprite, WALK_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void CheckPushFromThisDirection()
        {
            if (_keen.HitBox.IntersectsWith(this.HitBox) && !IsKeenInFrontOfThis())
            {
                _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), true, this);
            }
            else
            {
                _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), false, this);
            }
        }

        private bool IsKeenBehindThis()
        {
            if (_keen.HitBox.Right < this.HitBox.Left + this.HitBox.Width / 2 && _direction == Enums.Direction.RIGHT)
                return true;

            if (_keen.HitBox.Left > this.HitBox.Left + this.HitBox.Width / 2 && _direction == Enums.Direction.LEFT)
                return true;

            return false;
        }

        private bool IsKeenInFrontOfThis()
        {
            if (!_keen.HitBox.IntersectsWith(this.HitBox))
                return false;
            if (_keen.Direction == Enums.Direction.LEFT)
            {
                if (_keen.HitBox.Left <= this.HitBox.Right)
                    return true;
            }
            else
            {
                if (_keen.HitBox.Right >= this.HitBox.Left)
                    return true;
            }

            return false;
        }


        private bool IsKeenIntersectingOnVerticalPlane()
        {
            return _keen.HitBox.Bottom >= this.HitBox.Top && _keen.HitBox.Top <= this.HitBox.Bottom;
        }

        private void ExecuteFreeMoveLogic(int xOffset)
        {
            _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
            _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);

            Rectangle pushAreaToCheck = this.Direction == Enums.Direction.LEFT
                ? new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, xOffset * -1, this.HitBox.Height)
                : new Rectangle(this.HitBox.Right, this.HitBox.Y, xOffset, this.HitBox.Height);
            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);

            if (!(_keen.MoveState == MoveState.ON_POLE || _keen.IsDead() || _keen.IsStunned))
            {

                if (_keen.HitBox.IntersectsWith(pushAreaToCheck))
                {
                    _keen.SetKeenPushState(this.Direction, true, this);
                    _keen.GetMovedHorizontally(this, this.Direction, WALK_VELOCITY);
                }
                else
                {
                    _keen.SetKeenPushState(this.Direction, false, this);
                }
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != FlectState.STUNNED)
            {
                this.State = FlectState.STUNNED;
            }

            this.UpdateHitboxBasedOnStunnedImage(
               _stunnedSprites
               , ref _currentStunnedSprite
               , ref _currentStunnedSpriteChangeDelayTick
               , STUNNED_SPRITE_CHANGE_DELAY
               , UpdateSprite);

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return false; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return _state != FlectState.STUNNED; }
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

        FlectState State
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
                case FlectState.FALLING:
                case FlectState.WALKING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? _walkLeftSprites : _walkRightSprites;
                    if (_currentWalkSprite >= spriteSet.Length)
                    {
                        _currentWalkSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentWalkSprite];
                    break;
                case FlectState.TURNING:
                    _sprite.Image = Properties.Resources.keen6_flect_look;
                    break;
                case FlectState.STUNNED:
                    spriteSet = _stunnedSprites;
                    if (_currentStunnedSprite >= spriteSet.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = spriteSet[_currentStunnedSprite];
                    break;
            }
        }

        public bool DeflectsHorizontally
        {
            get { return true; }
        }

        public bool DeflectsVertically
        {
            get { return false; }
        }

        public PointItemType PointItem => PointItemType.KEEN6_PIZZA_SLICE;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
    enum FlectState
    {
        WALKING,
        TURNING,
        STUNNED,
        FALLING
    }
}
