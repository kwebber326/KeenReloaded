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
    public class Bip : CollisionObject, IUpdatable, ISprite, IEnemy, ISquashable, ICreateRemove, IZombieBountyEnemy
    {
        private System.Windows.Forms.PictureBox _sprite;
        private Enums.Direction _direction;
        private BipState _state;
        private Image[] _walkLeftSprites, _walkRightSprites;
        private CommanderKeen _keen;

        public const int WIDTH = 12, HEIGHT = 16;

        private const int WALK_SPRITE_CHANGE_DELAY = 0;
        private int _currentWalkSpriteChangeDelayTick;
        private int _currentWalkSprite;

        private const int WALK_VELOCITY = 5;
        private const int BASIC_FALL_VELOCITY = 30;
        private const int DISTANCE_TIL_KEEN_CHASE = 16;

        private const int LOOK_CHANCE = 30;
        private const int LOOK_TIME = 5;
        private int _currentLookTimeTick;

        public Bip(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
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
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            _walkLeftSprites = SpriteSheet.BipLeftImages;
            _walkRightSprites = SpriteSheet.BipRightImages;

            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            this.Fall();
            _sprite.BringToFront();
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
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    if (_state == BipState.FALLING)
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

        public void Update()
        {
            switch (_state)
            {
                case BipState.FALLING:
                    this.Fall();
                    break;
                case BipState.CHASING:
                    this.Chase();
                    break;
                case BipState.WANDERING:
                    this.Wander();
                    break;
                case BipState.LOOKING:
                    this.Look();
                    break;
                case BipState.SQUASHED:
                    this.UpdateSquashedState();
                    break;
            }
        }

        private void UpdateSquashedState()
        {
            if (this.State != BipState.SQUASHED)
            {
                this.State = BipState.SQUASHED;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                this.BasicFall(BASIC_FALL_VELOCITY);
                OnSquashed();
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }
        }

        private void Look()
        {
            if (this.State != BipState.LOOKING)
            {
                this.State = BipState.LOOKING;
                _currentLookTimeTick = 0;
            }
            if (IsNothingBeneath())
            {
                this.Fall();
            }

            if (_currentLookTimeTick++ == LOOK_TIME)
            {
                if (IsAlignedWithKeenVertically())
                {
                    this.Chase();
                }
                else
                {
                    this.Direction = this.ChangeHorizontalDirection(this.Direction);
                    this.Wander();
                }
            }
        }

        private void Wander()
        {
            if (this.State != BipState.WANDERING)
            {
                this.State = BipState.WANDERING;
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (IsOnEdge(this.Direction))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }

            if (IsAlignedWithKeenVertically())
            {
                this.Chase();
                return;
            }

            int lookVal = _random.Next(1, LOOK_CHANCE + 1);
            if (lookVal == LOOK_CHANCE)
            {
                this.Look();
                return;
            }

            int xOffset = _direction == Enums.Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }

            this.UpdateSpriteByDelayBase(ref _currentWalkSpriteChangeDelayTick, ref _currentWalkSprite, WALK_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void Chase()
        {
            if (this.State != BipState.CHASING)
            {
                this.State = BipState.CHASING;
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (IsOnEdge(this.Direction))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }

            if (!IsAlignedWithKeenVertically())
            {
                this.Wander();
                return;
            }

            int xOffset = _direction == Enums.Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (IsKeenBehindThis())
                {
                    this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
                }
            }

            this.UpdateSpriteByDelayBase(ref _currentWalkSpriteChangeDelayTick, ref _currentWalkSprite, WALK_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private bool IsKeenBehindThis()
        {
            if (_keen.HitBox.Top < this.HitBox.Bottom && _keen.HitBox.Bottom > this.HitBox.Top)
            {
                if (_keen.HitBox.Right < this.HitBox.Left - DISTANCE_TIL_KEEN_CHASE && _direction == Enums.Direction.RIGHT)
                    return true;

                if (_keen.HitBox.Left > this.HitBox.Right + DISTANCE_TIL_KEEN_CHASE && _direction == Enums.Direction.LEFT)
                    return true;
            }

            return false;
        }

        private void Fall()
        {
            if (this.State != BipState.FALLING)
            {
                this.State = BipState.FALLING;
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }
            else if (IsAlignedWithKeenVertically())
            {
                this.Chase();
            }
            else
            {
                this.Wander();
            }
        }

        private bool IsAlignedWithKeenVertically()
        {
            return _keen.HitBox.Bottom == this.HitBox.Bottom;
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

        }

        public bool IsActive
        {
            get { return false; }
        }

        BipState State
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

        private void UpdateSprite()
        {
            switch (_state)
            {
                case BipState.FALLING:
                case BipState.CHASING:
                case BipState.WANDERING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? _walkLeftSprites : _walkRightSprites;
                    if (_currentWalkSprite >= spriteSet.Length)
                    {
                        _currentWalkSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentWalkSprite];
                    break;
                case BipState.LOOKING:
                    _sprite.Image = Properties.Resources.keen6_bip_look;
                    break;
                case BipState.SQUASHED:
                    _sprite.Image = Properties.Resources.keen6_bip_squashed;
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

        public void Squash()
        {
            this.UpdateSquashedState();

        }

        public bool IsSquashed
        {
            get { return _state == BipState.SQUASHED; }
        }

        public bool CanSquash
        {
            get { return _state != BipState.SQUASHED && _state != BipState.FALLING; }
        }

        public PointItemType PointItem => PointItemType.KEEN6_BLOOG_SODA;

        public event EventHandler<ObjectEventArgs> Squashed;
        public event EventHandler<ObjectEventArgs> Killed;

        protected void OnSquashed()
        {
            var eventArgs = new KeenEventArgs.ObjectEventArgs() { ObjectSprite = this };
            Squashed?.Invoke(this, eventArgs);
            Killed?.Invoke(this, eventArgs);
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum BipState
    {
        WANDERING,
        CHASING,
        LOOKING,
        SQUASHED,
        FALLING
    }
}
