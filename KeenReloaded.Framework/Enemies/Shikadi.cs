using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System.Windows.Forms;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Trajectories;

namespace KeenReloaded.Framework.Enemies
{
    public class Shikadi : DestructibleObject, IUpdatable, ISprite, IEnemy, IFireable, ICreateRemove, IZombieBountyEnemy
    {
        private int _currentMoveSprite;
        private int _currentLookSprite;
        private int _currentStunnedSprite;

        private const int BASIC_FALL_VELOCITY = 40;

        private const int WALK_VELOCITY = 10;
        private const int CHASE_KEEN_CHANCE = 30;
        private const int WALK_SPRITE_CHANGE_DELAY = 2;
        private int _currentWalkSpriteChangeDelayTick;

        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelayTick;

        private const int LOOK_TIME = 50;
        private const int LOOK_CHANCE = 300;
        private int _currentLookTimeTick;
        private const int LOOK_SPRITE_CHANGE_DELAY = 1;
        private int _currentLookSpriteChangeDelayTick;

        private Pole _currentPole;
        private const int POLE_FIRE_TIME = 15;
        private int _currentPoleFireTimeTick;
        private const int POLE_FIRE_HORIZONTAL_OFFSET = 14;

        private bool _hitAnimation;
        private const int HIT_ANIMATION_TIME = 1;
        private int _hitAnimationTimeTick;

        public Shikadi(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
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
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            this.Direction = this.SetDirectionFromObjectHorizontal(_keen, true);
            this.Walk();
        }

        public override void Die()
        {
            this.Stun();
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
                case ShikadiState.LOOKING:
                    this.Look();
                    break;
                case ShikadiState.WALKING:
                    this.Walk();
                    break;
                case ShikadiState.FIRING:
                    this.Fire();
                    break;
                case ShikadiState.STUNNED:
                    this.Stun();
                    break;
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
                    this.UpdateCollisionNodes(this.Direction);
                }
            }
        }

        private void Stun()
        {
            if (this.State != ShikadiState.STUNNED)
            {
                this.State = ShikadiState.STUNNED;
                AdjustYPositionFromFloorTile();
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }

            var spriteIndex = _currentStunnedSprite;
            this.UpdateSpriteByDelay(ref _currentStunnedSpriteChangeDelayTick, ref _currentStunnedSprite, STUNNED_SPRITE_CHANGE_DELAY);
            if (_currentStunnedSprite != spriteIndex)
            {
                var image = SpriteSheet.ShikadiStunnedImages[_currentStunnedSprite];
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - (image.Size.Height - this.HitBox.Height), image.Width, image.Height);
            }
        }

        private void UpdateSpriteByDelay(ref int delayTicker, ref int spriteIndex, int delayThreshold)
        {
            if (delayTicker++ == delayThreshold)
            {
                delayTicker = 0;
                spriteIndex++;
                UpdateSprite();
            }
        }

        protected override Direction ChangeHorizontalDirection(Direction direction)
        {
            _currentPole = null;
            return direction == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
        }

        private void Walk()
        {
            if (this.State != ShikadiState.WALKING)
            {
                this.State = ShikadiState.WALKING;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                AdjustYPositionFromFloorTile();
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
                return;
            }
            else
            {
                int lookVal = _random.Next(1, LOOK_CHANCE + 1);
                if (lookVal == LOOK_CHANCE)
                {
                    this.Look();
                    return;
                }
            }


            int chaseVal = _random.Next(1, CHASE_KEEN_CHANCE + 1);
            if (chaseVal == CHASE_KEEN_CHANCE)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            }

            if (this.IsOnEdge(this.Direction, 2))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }

            int xOffset = _direction == Enums.Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);

            var collisions = this.CheckCollision(areaToCheck);
            //pole collisions
            var poles = collisions.OfType<Pole>();

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                if (this.HitBox.IntersectsWith(_keen.HitBox))
                {
                    _keen.Die();
                }
            }
            else if (poles.Any(p => p != _currentPole))
            {
                if (_direction == Enums.Direction.LEFT)
                {
                    Pole pole = poles.Where(p => p != _currentPole).OrderByDescending(p1 => p1.HitBox.X).FirstOrDefault();
                    if (pole != null)
                    {
                        _currentPole = pole;
                        this.HitBox = new Rectangle(pole.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        this.Fire();
                    }
                }
                else
                {
                    Pole pole = poles.Where(p => p != _currentPole).OrderBy(p1 => p1.HitBox.X).FirstOrDefault();
                    if (pole != null)
                    {
                        _currentPole = pole;
                        this.HitBox = new Rectangle(pole.HitBox.Right - this.HitBox.Width - POLE_FIRE_HORIZONTAL_OFFSET, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        this.Fire();
                    }
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
                if (!poles.Any())
                {
                    _currentPole = null;
                }
            }

            if (_currentWalkSpriteChangeDelayTick++ == WALK_SPRITE_CHANGE_DELAY)
            {
                _currentWalkSpriteChangeDelayTick = 0;
                _currentMoveSprite++;
                UpdateSprite();
            }
        }

        private void AdjustYPositionFromFloorTile()
        {
            var landingTile = GetTopMostLandingTile(1);
            if (landingTile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Y - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
            }
        }

        private void Look()
        {
            if (this.State != ShikadiState.LOOKING)
            {
                _currentLookTimeTick = 0;
                this.State = ShikadiState.LOOKING;
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }

            if (_currentLookTimeTick++ == LOOK_TIME)
            {
                this.Direction = this.SetDirectionFromObjectHorizontal(_keen, true);
                this.Walk();
            }
            else if (_currentLookSpriteChangeDelayTick++ == LOOK_SPRITE_CHANGE_DELAY)
            {
                _currentLookSpriteChangeDelayTick = 0;
                _currentLookSprite++;
                UpdateSprite();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return _state != ShikadiState.STUNNED; }
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
            get { return _state != ShikadiState.STUNNED; }
        }

        public void Fire()
        {
            if (this.State != ShikadiState.FIRING)
            {
                _currentPoleFireTimeTick = 0;
                this.State = ShikadiState.FIRING;
                //TODO: Fire electric charge
                if (_currentPole != null)
                {
                    int shockWidth = 26, shockHeight = 40;
                    int xOffset = (shockWidth - _currentPole.HitBox.Width) / 2;
                    ShikadiShock shock = new ShikadiShock(_collisionGrid, new Rectangle(_currentPole.HitBox.X - xOffset, this.HitBox.Top - shockHeight, shockWidth, shockHeight), Enums.Direction.UP, _currentPole);
                    shock.Create += new EventHandler<ObjectEventArgs>(shock_Create);
                    shock.Remove += new EventHandler<ObjectEventArgs>(shock_Remove);

                    OnCreate(new ObjectEventArgs() { ObjectSprite = shock });
                }
            }

            if (_currentPoleFireTimeTick++ == POLE_FIRE_TIME)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
                this.Walk();
            }
        }

        void shock_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void shock_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        public bool IsFiring
        {
            get { return _state == ShikadiState.FIRING; }
        }

        public int Ammo
        {
            get { return -1; }
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

        ShikadiState State
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

        public PointItemType PointItem => PointItemType.KEEN5_TART_STIX;

        private void UpdateSprite()
        {
            switch (_state)
            {
                case ShikadiState.WALKING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? SpriteSheet.ShikadWalkLeftImages : SpriteSheet.ShikadWalkRightImages;
                    if (_currentMoveSprite >= spriteSet.Length)
                    {
                        _currentMoveSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentMoveSprite];
                    break;
                case ShikadiState.LOOKING:
                    spriteSet = SpriteSheet.ShikadiLookImages;
                    if (_currentLookSprite >= spriteSet.Length)
                    {
                        _currentLookSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentLookSprite];
                    break;
                case ShikadiState.STUNNED:
                    spriteSet = SpriteSheet.ShikadiStunnedImages;
                    if (_currentStunnedSprite >= spriteSet.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    else if (_currentStunnedSprite == 0)
                    {
                        var firstStunnedSprite = spriteSet[0];
                        int yDiff = this.HitBox.Height - firstStunnedSprite.Height;
                        int xDiff = this.HitBox.Width - firstStunnedSprite.Width;
                        this.HitBox = new Rectangle(this.HitBox.X - (xDiff / 2), this.HitBox.Y + yDiff, _sprite.Width, _sprite.Height);
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                    _sprite.Image = spriteSet[_currentStunnedSprite];
                    break;
                case ShikadiState.FIRING:
                    _sprite.Image = _direction == Enums.Direction.LEFT
                      ? Properties.Resources.keen5_standard_shikadi_shoot_left
                      : Properties.Resources.keen5_standard_shikadi_shoot_right;
                    break;
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private ShikadiState _state;
        private PictureBox _sprite;
        private CommanderKeen _keen;
        private Direction _direction;

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

    enum ShikadiState
    {
        LOOKING,
        WALKING,
        FIRING,
        STUNNED
    }
}
