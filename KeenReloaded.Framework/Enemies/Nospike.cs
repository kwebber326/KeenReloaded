using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class Nospike : DestructibleObject, IUpdatable, ISprite, IEnemy, ICreateRemove, IZombieBountyEnemy
    {
        #region sprite and state variables
        private CommanderKeen _keen;
        private NospikeState _state;
        private NospikeQuestionMark _questionMark;
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private Image[] _patrolLeftSprites, _patrolRightSprites, _chargeLeftSprites, _chargeRightSprites, _stunnedSprites;
        private int _currentPatrolSprite, _currentChargeSprite, _currentStunnedSprite;
        private const int PATROL_SPRITE_CHANGE_DELAY = 2, CHARGE_SPRITE_CHANGE_DELAY = 0, STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentPatrolSpriteChangeDelayTick, _currentChargeSpriteChangeDelayTick, _currentStunnedSpriteChangeDelayTick;
        #endregion

        #region behavior logic variables
        private const int DIRECTION_EDGE_BUFFER = 5;
        private const int RANGE_OF_VISION = 200;
        private const int CHARGE_CHANCE = 20;
        private const int CHARGE_MIN_DISTANCE = 100;
        private int _currentChargeDist;
        private const int CHARGE_STOP_CHANCE = 60;

        private const int LOOK_CHANCE = 80;
        private const int LOOK_TIME = 20;
        private int _lookTimeTick;

        private const int CONFUSED_STAGE_1_END = 5;
        private const int CONFUSED_STAGE_2_END = 25;
        private const int CONFUSED_STAGE_3_END = 30;
        private int _confusedTimeTick;
        private const int QUESTION_MARK_DISTANCE_OVER_HEAD = 10;

        private bool _hitAnimation;
        private const int HIT_ANIMATION_TIME = 1;
        private int _hitAnimationTimeTick;
        #endregion

        #region physics variables

        private const int MAX_FALL_VELOCITY = 40;
        private const int GRAVITY_ACCELERATION = 5;
        private int _verticalVelocity;

        private const int PATROL_VELOCITY = 5;

        private const int CHARGE_VELOCITY = 20;

        #endregion

        public Nospike(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
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

            _patrolLeftSprites = SpriteSheet.NospikePatrolLeftImages;
            _patrolRightSprites = SpriteSheet.NospikePatrolRightImages;
            _chargeLeftSprites = SpriteSheet.NospikeChargeLeftImages;
            _chargeRightSprites = SpriteSheet.NospikeChargeRightImages;
            _stunnedSprites = SpriteSheet.NospikeStunnedImages;

            int directionVal = _random.Next(0, 2);
            this.Direction = directionVal == 0 ? Direction.LEFT : Enums.Direction.RIGHT;
            this.Fall();
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
                    this.UpdateCollisionNodes(Enums.Direction.DOWN);
                }
            }
        }

        public override void Die()
        {
            this.UpdateStunnedState();
            this.OnKilled();
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
                case NospikeState.LOOKING:
                    this.Look();
                    break;
                case NospikeState.PATROLLING:
                    this.Patrol();
                    break;
                case NospikeState.CHARGING:
                    this.Charge();
                    break;
                case NospikeState.CONFUSED:
                    this.UpdateConfusedState();
                    break;
                case NospikeState.FALLING:
                    this.Fall();
                    break;
                case NospikeState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void Look()
        {
            if (this.State != NospikeState.LOOKING)
            {
                _lookTimeTick = 0;
                this.State = NospikeState.LOOKING;
                AdjustHitboxAndSpriteHeight();
            }

            if (IsNothingBeneath())
            {
                this.UpdateConfusedState();
                return;
            }

            if (_lookTimeTick++ == LOOK_TIME)
            {
                this.Patrol();
            }
        }

        private void AdjustHitboxAndSpriteHeight()
        {
            if (_sprite.Height > this.HitBox.Height)
            {
                int heightDiff = _sprite.Height - this.HitBox.Height;
                this.HitBox = new Rectangle(_sprite.Location.X, _sprite.Location.Y - heightDiff, _sprite.Width, _sprite.Height);
            }
        }

        private bool IsKeenInRangeOfVision()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.Left - RANGE_OF_VISION, this.HitBox.Y, this.HitBox.Width + (RANGE_OF_VISION * 2), this.HitBox.Height);
            return (_keen.HitBox.IntersectsWith(areaToCheck));
        }

        private void Patrol()
        {
            if (this.State != NospikeState.PATROLLING)
            {
                this.State = NospikeState.PATROLLING;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                this.BasicFall(MAX_FALL_VELOCITY);
            }

            if (IsNothingBeneath())
            {
                this.UpdateConfusedState();
                return;
            }

            if (IsOnEdge(this.Direction, DIRECTION_EDGE_BUFFER))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }

            int lookVal = _random.Next(1, LOOK_CHANCE + 1);
            if (lookVal == LOOK_CHANCE)
            {
                this.Look();
                return;
            }

            if (IsKeenInRangeOfVision())
            {
                int chargeVal = _random.Next(1, CHARGE_CHANCE + 1);
                if (chargeVal == CHARGE_CHANCE)
                {
                    this.Charge();
                    return;
                }
            }

            int xOffset = _direction == Enums.Direction.LEFT ? PATROL_VELOCITY * -1 : PATROL_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + PATROL_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }
            var spriteSet = _direction == Enums.Direction.LEFT ? _patrolLeftSprites : _patrolRightSprites;
            this.UpdateHitboxBasedOnStunnedImage(spriteSet, ref _currentPatrolSprite, ref _currentPatrolSpriteChangeDelayTick, PATROL_SPRITE_CHANGE_DELAY, UpdateSprite);
            //this.UpdateSpriteByDelayBase(ref _currentPatrolSpriteChangeDelayTick, ref _currentPatrolSprite, PATROL_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void UpdateConfusedState()
        {
            if (this.State != NospikeState.CONFUSED)
            {
                this.State = NospikeState.CONFUSED;
                AdjustHitboxAndSpriteHeight();
                _confusedTimeTick = 0;
            }

            _confusedTimeTick++;
            if (_confusedTimeTick == CONFUSED_STAGE_1_END)
            {
                //create question mark over head
                SetQuestionMark();
            }
            else if (_confusedTimeTick == CONFUSED_STAGE_2_END)
            {
                //remove question mark
                RemoveQuestionMark();
            }
            else if (_confusedTimeTick == CONFUSED_STAGE_3_END)
            {
                //fall
                this.Fall();
            }
        }

        private void RemoveQuestionMark()
        {
            OnRemove(new ObjectEventArgs() { ObjectSprite = _questionMark });
            _questionMark.Remove -= _questionMark_Remove;
            _questionMark.Create -= _questionMark_Create;
            _questionMark = null;
        }

        private void SetQuestionMark()
        {
            int xPos = _sprite.Location.X + (_sprite.Width / 2) - (NospikeQuestionMark.WIDTH / 2);
            int yPos = _sprite.Location.Y - NospikeQuestionMark.HEIGHT - QUESTION_MARK_DISTANCE_OVER_HEAD;
            _questionMark = new NospikeQuestionMark(_collisionGrid, new Rectangle(xPos, yPos, NospikeQuestionMark.WIDTH, NospikeQuestionMark.HEIGHT));
            _questionMark.Create += new EventHandler<ObjectEventArgs>(_questionMark_Create);
            _questionMark.Remove += new EventHandler<ObjectEventArgs>(_questionMark_Remove);

            OnCreate(new ObjectEventArgs() { ObjectSprite = _questionMark });
        }

        void _questionMark_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void _questionMark_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        private void Fall()
        {
            if (this.State != NospikeState.FALLING)
            {
                this.State = NospikeState.FALLING;
                _verticalVelocity = 0;
            }

            var tile = this.BasicFallReturnTile(_verticalVelocity);
            if (tile != null)
            {
                if (_verticalVelocity == MAX_FALL_VELOCITY)
                {
                    this.Die();
                }
                else
                {
                    this.Patrol();
                }
            }
            else if (_verticalVelocity + GRAVITY_ACCELERATION <= MAX_FALL_VELOCITY)
            {
                _verticalVelocity += GRAVITY_ACCELERATION;
            }
            else
            {
                _verticalVelocity = MAX_FALL_VELOCITY;
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != NospikeState.STUNNED)
            {
                this.State = NospikeState.STUNNED;
            }

            this.UpdateHitboxBasedOnStunnedImage(
              _stunnedSprites
              , ref _currentStunnedSprite
              , ref _currentStunnedSpriteChangeDelayTick
              , STUNNED_SPRITE_CHANGE_DELAY
              , UpdateSprite);

            if (IsNothingBeneath())
            {
                this.BasicFall(MAX_FALL_VELOCITY);
            }
        }


        private void Charge()
        {
            if (this.State != NospikeState.CHARGING)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
                _currentChargeDist = 0;
                this.State = NospikeState.CHARGING;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                //avoid getting stuck in a wall when charging left with back to said wall
                if (this.Direction == Enums.Direction.LEFT)
                {
                    var wallCollisions = this.CheckCollision(this.HitBox, true);
                    if (wallCollisions.Any())
                    {
                        var tileWall = GetLeftMostRightTile(wallCollisions);
                        this.HitBox = new Rectangle(tileWall.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                }
                this.BasicFall(MAX_FALL_VELOCITY);
            }

            //go through potential cases to stop charging first
            if (IsNothingBeneath())
            {
                this.UpdateConfusedState();
                return;
            }

            if (_currentChargeDist >= CHARGE_MIN_DISTANCE)
            {
                int chargeStopVal = _random.Next(1, CHARGE_STOP_CHANCE + 1);
                if (chargeStopVal == CHARGE_STOP_CHANCE)
                {
                    this.Patrol();
                    return;
                }
            }

            //charge
            int xOffset = _direction == Enums.Direction.LEFT ? CHARGE_VELOCITY * -1 : CHARGE_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + CHARGE_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                int chargeDist = Math.Abs(xCollidePos - this.HitBox.X);
                _currentChargeDist += chargeDist;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                this.Look();
                return;
            }
            else
            {
                _currentChargeDist += Math.Abs(xOffset);
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }

            this.UpdateSpriteByDelayBase(ref _currentChargeSpriteChangeDelayTick, ref _currentChargeSprite, CHARGE_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return _state == NospikeState.PATROLLING || _state == NospikeState.CHARGING || _state == NospikeState.LOOKING; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);

        }

        public override void TakeDamage(ITrajectory trajectory)
        {
            base.TakeDamage(trajectory);
            if (!IsDead())
            {
                this.Charge();
                _sprite.BackColor = Color.White;
                _hitAnimation = true;
            }
        }

        public override void TakeDamage(int damage)
        {
            if (damage > 0)
            {
                base.TakeDamage(damage);
                if (!IsDead())
                {
                    this.Charge();
                }
            }
        }

        public bool IsActive
        {
            get { return _state != NospikeState.STUNNED && _state != NospikeState.FALLING && _state != NospikeState.CONFUSED; }
        }

        NospikeState State
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

        public PointItemType PointItem => PointItemType.KEEN6_PIZZA_SLICE;

        private void UpdateSprite()
        {
            switch (_state)
            {
                case NospikeState.LOOKING:
                case NospikeState.CONFUSED:
                case NospikeState.FALLING:
                    _sprite.Image = Properties.Resources.keen6_nospike_look;
                    break;
                case NospikeState.PATROLLING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? _patrolLeftSprites : _patrolRightSprites;
                    if (_currentPatrolSprite >= spriteSet.Length)
                    {
                        _currentPatrolSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentPatrolSprite];
                    break;
                case NospikeState.CHARGING:
                    spriteSet = _direction == Enums.Direction.LEFT ? _chargeLeftSprites : _chargeRightSprites;
                    if (_currentChargeSprite >= spriteSet.Length)
                    {
                        _currentChargeSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentChargeSprite];
                    break;
                case NospikeState.STUNNED:
                    spriteSet = _stunnedSprites;
                    if (_currentStunnedSprite >= spriteSet.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = spriteSet[_currentStunnedSprite];
                    break;
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (this.Remove != null)
            {
                if (args.ObjectSprite == this)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum NospikeState
    {
        LOOKING,
        PATROLLING,
        CHARGING,
        CONFUSED,
        FALLING,
        STUNNED
    }
}
