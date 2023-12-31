﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class Shockshund : DestructibleObject, IUpdatable, IEnemy, IFireable, ISprite, ICreateRemove, IZombieBountyEnemy
    {
        private int _currentMoveSprite;
        private int _currentShootSprite;
        private int _currentLookSprite;
        private int _currentStunnedSprite;

        private const int BASIC_FALL_VELOCITY = 40;

        private const int STUNNED_SPRITE_OFFSET_Y = 6;
        private const int STUNNED_SPRITE_OFFSET_X = 6;
        private bool _firstStunSpriteRotation = true;
        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelayTick;

        private const int WALK_VELOCITY = 5;
        private const int CHASE_KEEN_CHANCE = 40;
        private const int WALK_SPRITE_CHANGE_DELAY = 2;
        private int _currentWalkSpriteChangeDelayTick;

        private const int JUMP_CHANCE_EDGE = 3;
        private const int JUMP_CHANCE = 40;
        private const int INITIAL_JUMP_VELOCITY_VERTICAL = 20;
        private const int INITIAL_JUMP_VELOCITY_HORIZONTAL = 45;
        private const int MIN_HORIZONTAL_VELOCITY = 20;
        private const int GRAVITY_ACCELERATION = 5;
        private const int AIR_RESISTANCE = 3;
        private const int MAX_VERTICAL_VELOCITY = 50;
        private int _currentVerticalVelocity, _currentHorizontalVelocity;

        private const int LOOK_CHANCE = 400;
        private const int LOOK_TIME = 50;
        private const int LOOK_SPRITE_CHANGE_DELAY = 1;
        private int _currentLookTimeTick;
        private int _currentLookSpriteChangeDelayTick;

        private const int FIRE_CHANCE = 30;
        private const int FIRE_SPRITE_CHANGE_DELAY = 2;
        private int _currentFireSpriteChangeDelayTick;

        private bool _hitAnimation;
        private const int HIT_ANIMATION_TIME = 1;
        private int _hitAnimationTimeTick;

        public Shockshund(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");


            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            this.Health = 2;
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            this.Walk();
            int seed = new Random().Next(0, int.MaxValue);
            _random = new Random(seed);
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

        public override void Die()
        {
            if (this.State != ShockshundState.STUNNED)
            {
                this.State = ShockshundState.STUNNED;
            }
        }

        private Direction _direction;

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

        ShockshundState _state;

        ShockshundState State
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
                case ShockshundState.LOOKING:
                    if (_currentLookSprite >= SpriteSheet.ShockShundLookSprites.Length)
                    {
                        _currentLookSprite = 0;
                    }
                    _sprite.Image = SpriteSheet.ShockShundLookSprites[_currentLookSprite];
                   
                    break;
                case ShockshundState.JUMPING:
                    _sprite.Image = _direction == Enums.Direction.LEFT ? Properties.Resources.keen5_shockshund_jump_left : Properties.Resources.keen5_shockshund_jump_right;
                    break;
                case ShockshundState.WALKING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? SpriteSheet.ShockShundWalkLeftSprites : SpriteSheet.ShockShundWalkRightSprites;
                    if (_currentMoveSprite >= spriteSet.Length)
                    {
                        _currentMoveSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentMoveSprite];
                    break;
                case ShockshundState.SHOOTING:
                    spriteSet = _direction == Enums.Direction.LEFT ? SpriteSheet.ShockShundShootLeftSprites : SpriteSheet.ShockShundShootRightSprites;
                    if (_currentShootSprite >= spriteSet.Length)
                    {
                        _currentShootSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentShootSprite];
                    break;
                case ShockshundState.STUNNED:
                    if (_currentStunnedSprite >= SpriteSheet.ShockShundStunnedSprites.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = SpriteSheet.ShockShundStunnedSprites[_currentStunnedSprite];
                   
                    break;
            }
        }

        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;



        public void Fire()
        {
            if (this.State != ShockshundState.SHOOTING)
            {
                this.State = ShockshundState.SHOOTING;
                _currentFireSpriteChangeDelayTick = 0;
                _currentShootSprite = 0;
                FireShot();
            }

            var spriteSet = _direction == Enums.Direction.LEFT ? SpriteSheet.ShockShundShootLeftSprites : SpriteSheet.ShockShundShootRightSprites;

            if (_currentFireSpriteChangeDelayTick++ == FIRE_SPRITE_CHANGE_DELAY)
            {
                _currentFireSpriteChangeDelayTick = 0;
                if (++_currentShootSprite < spriteSet.Length)
                {
                    UpdateSprite();
                }
                else
                {
                    this.Walk();
                }
            }
        }

        private void FireShot()
        {
            int width = 24, height = 24;
            Point p = _direction == Enums.Direction.LEFT ? new Point(this.HitBox.X - width, this.HitBox.Y) : new Point(this.HitBox.Right, this.HitBox.Y);
            ShockshundTrajectory shot = new ShockshundTrajectory(_collisionGrid, new Rectangle(p, new Size(width, height)), this.Direction);
            shot.Create += new EventHandler<ObjectEventArgs>(shot_Create);
            shot.Remove += new EventHandler<ObjectEventArgs>(shot_Remove);

            OnCreate(new ObjectEventArgs() { ObjectSprite = shot });
        }

        void shot_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void shot_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        public bool IsFiring
        {
            get { return _state == ShockshundState.SHOOTING; }
        }

        public int Ammo
        {
            get { return -1; }
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
                case ShockshundState.LOOKING:
                    this.Look();
                    break;
                case ShockshundState.WALKING:
                    this.Walk();
                    break;
                case ShockshundState.SHOOTING:
                    this.Fire();
                    break;
                case ShockshundState.JUMPING:
                    this.Jump();
                    break;
                case ShockshundState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != ShockshundState.STUNNED)
            {
                this.State = ShockshundState.STUNNED;
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
                return;
            }

            if (_currentStunnedSpriteChangeDelayTick++ == STUNNED_SPRITE_CHANGE_DELAY)
            {
                _currentStunnedSpriteChangeDelayTick = 0;
                if (++_currentStunnedSprite == 1 && _firstStunSpriteRotation)
                {
                    _firstStunSpriteRotation = false;
                    this.HitBox = new Rectangle(this.HitBox.X - STUNNED_SPRITE_OFFSET_X / 2, this.HitBox.Y - STUNNED_SPRITE_OFFSET_Y, this.HitBox.Width + STUNNED_SPRITE_OFFSET_X, this.HitBox.Height + STUNNED_SPRITE_OFFSET_Y);
                    this.UpdateCollisionNodes(Enums.Direction.LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP);
                }
                UpdateSprite();
            }
        }

        private void Jump()
        {
            if (this.State != ShockshundState.JUMPING)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
                this.State = ShockshundState.JUMPING;
                _currentVerticalVelocity = INITIAL_JUMP_VELOCITY_VERTICAL * -1;
                _currentHorizontalVelocity = _direction == Enums.Direction.LEFT ? INITIAL_JUMP_VELOCITY_HORIZONTAL * -1 : INITIAL_JUMP_VELOCITY_HORIZONTAL;
            }

            Rectangle areaToCheck = new Rectangle(
               _currentHorizontalVelocity < 0 ? this.HitBox.X + _currentHorizontalVelocity : this.HitBox.X //X
             , _currentVerticalVelocity < 0 ? this.HitBox.Y + _currentVerticalVelocity : this.HitBox.Y //Y
             , this.HitBox.Width + Math.Abs(_currentHorizontalVelocity)//width
             , this.HitBox.Height + Math.Abs(_currentVerticalVelocity));//height

            var collisions = this.CheckCollision(areaToCheck, true);

            var horizontalTile = _currentHorizontalVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = _currentVerticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);

            if (horizontalTile != null)
            {
                int collisionXPos = _currentHorizontalVelocity < 0 ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(collisionXPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                DecelerateHorizontalMovement();
            }
            else
            {
                Rectangle areaToCheckToKillKeen = new Rectangle(
                    _currentHorizontalVelocity < 0 ? this.HitBox.X + _currentHorizontalVelocity : this.HitBox.X,
                    this.HitBox.Y, this.HitBox.Width + Math.Abs(_currentHorizontalVelocity), this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheckToKillKeen))
                {
                    _keen.Die();
                }
                this.HitBox = new Rectangle(this.HitBox.X + _currentHorizontalVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                DecelerateHorizontalMovement();
            }

            if (verticalTile != null)
            {
                int _collisionYPos = _currentVerticalVelocity < 0 ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(this.HitBox.X, _collisionYPos, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                this.Walk();
            }
            else
            {
                Rectangle areaToCheckToKillKeen = new Rectangle(
                   this.HitBox.X,
                   _currentVerticalVelocity < 0 ? this.HitBox.Y + _currentVerticalVelocity : this.HitBox.Y
                   , this.HitBox.Width, this.HitBox.Height + Math.Abs(_currentVerticalVelocity));

                if (_keen.HitBox.IntersectsWith(areaToCheckToKillKeen))
                {
                    _keen.Die();
                }
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                AccelerateVerticalMovement();
            }
        }

        private void AccelerateVerticalMovement()
        {
            if (_currentVerticalVelocity + GRAVITY_ACCELERATION <= MAX_VERTICAL_VELOCITY)
            {
                _currentVerticalVelocity += GRAVITY_ACCELERATION;
            }
            else
            {
                _currentVerticalVelocity = MAX_VERTICAL_VELOCITY;
            }
        }

        private void DecelerateHorizontalMovement()
        {
            if (_currentHorizontalVelocity < 0)
            {
                if (_currentHorizontalVelocity + AIR_RESISTANCE <= MIN_HORIZONTAL_VELOCITY * -1)
                {
                    _currentHorizontalVelocity += AIR_RESISTANCE;
                }
                else
                {
                    _currentHorizontalVelocity = 0;
                }
            }
            else if (_currentHorizontalVelocity > 0)
            {
                if (_currentHorizontalVelocity - AIR_RESISTANCE >= MIN_HORIZONTAL_VELOCITY)
                {
                    _currentHorizontalVelocity -= AIR_RESISTANCE;
                }
                else
                {
                    _currentHorizontalVelocity = 0;
                }
            }
        }

        private void Walk()
        {
            if (this.State != ShockshundState.WALKING)
            {
                this.State = ShockshundState.WALKING;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                var landingTile = GetTopMostLandingTile(1);
                if (landingTile != null)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Y - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                }
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
                return;
            }

            int chaseVal = this.GenerateRandomInteger(1, CHASE_KEEN_CHANCE);//_random.Next(1, CHASE_KEEN_CHANCE + 1);
            if (chaseVal == CHASE_KEEN_CHANCE)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            }

            if (IsOnEdge(_direction, 3))
            {
                int jumpVal = this.GenerateRandomInteger(1, JUMP_CHANCE_EDGE); //_random.Next(1, JUMP_CHANCE_EDGE + 1);
                if (jumpVal == JUMP_CHANCE_EDGE)
                {
                    this.Jump();
                    return;
                }
                else
                {
                    this.Direction = ChangeHorizontalDirection(this.Direction);
                }
            }
            else
            {
                int jumpVal = this.GenerateRandomInteger(1, JUMP_CHANCE);//_random.Next(1, JUMP_CHANCE + 1);
                if (jumpVal == JUMP_CHANCE)
                {
                    this.Jump();
                    return;
                }
            }

            int lookVal = this.GenerateRandomInteger(1, LOOK_CHANCE);// _random.Next(1, LOOK_CHANCE + 1);
            if (lookVal == LOOK_CHANCE)
            {
                this.Look();
                return;
            }

            int fireVal = this.GenerateRandomInteger(1, FIRE_CHANCE);//_random.Next(1, FIRE_CHANCE + 1);
            if (fireVal == FIRE_CHANCE)
            {
                this.Fire();
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
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }

            if (_currentWalkSpriteChangeDelayTick++ == WALK_SPRITE_CHANGE_DELAY)
            {
                _currentWalkSpriteChangeDelayTick = 0;
                _currentMoveSprite++;
                UpdateSprite();
            }
        }

        private void Look()
        {
            if (this.State != ShockshundState.LOOKING)
            {
                this.State = ShockshundState.LOOKING;
                this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                _currentLookTimeTick = 0;
                _currentLookSprite = 0;
            }

            if (_currentLookTimeTick++ == LOOK_TIME)
            {
                this.Walk();

            }
            else
            {
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
            get { return _state != ShockshundState.STUNNED; }
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
            get { return _state != ShockshundState.STUNNED; }
        }

        public PointItemType PointItem => PointItemType.KEEN5_SUGAR_STOOPIES_CEREAL;

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

    enum ShockshundState
    {
        LOOKING,
        WALKING,
        SHOOTING,
        JUMPING,
        STUNNED
    }
}
