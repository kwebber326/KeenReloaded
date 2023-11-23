﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Enums;
using System.Drawing;
using KeenReloaded.Framework.Trajectories;

namespace KeenReloaded.Framework.Enemies
{
    public class BipShip : DestructibleObject, IUpdatable, ISprite, IEnemy, IFireable, ICreateRemove, IZombieBountyEnemy
    {
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private BipShipState _state;
        private System.Windows.Forms.PictureBox _sprite;
        private Direction _direction;
        private CommanderKeen _keen;

        private Image[] _turnLeftSprites, _turnRightSprites;
        private int _currentTurnSprite;
        private const int TURN_SPRITE_CHANGE_DELAY = 1;
        private int _currentTurnSpriteChangeDelayTick;

        private const int HOVER_OVER_GROUND_DISTANCE = 50;
        private const int FLY_VELOCITY = 12;
        private const int EDGE_OFFSET = -50;

        private const int FIRE_CHANCE = 10;
        private const int FIRE_RANGE_X = 150;
        private bool _isFiring;
        private bool _exploded;

        private const int DESTROYED_FALLING_MAX_VELOCITY = 40;
        private const int DESTROYED_FALLING_INITIAL_VELOCITY = 0;
        private const int GRAVITY_ACCELERATION = 3;
        private const int AIR_RESISTANCE = 1;
        private int _fallVerticalVelocity;
        private int _fallHorizontalVelocity;

        public BipShip(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen) :
            base(grid, hitbox)
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

            _turnLeftSprites = SpriteSheet.BipShipTurnLeftImages;
            _turnRightSprites = SpriteSheet.BipShipTurnRightImages;

            var directionVal = _random.Next(0, 2);
            this.Direction = directionVal == 0 ? Enums.Direction.LEFT : Enums.Direction.RIGHT;

            this.Fly();
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
            this.Destroy();
        }

        public void Update()
        {
            if (_exploded)
            {
                OnRemove(new ObjectEventArgs() { ObjectSprite = this });
            }
            else
            {
                switch (_state)
                {
                    case BipShipState.FLYING:
                        this.Fly();
                        break;
                    case BipShipState.TURNING:
                        this.Turn();
                        break;
                    case BipShipState.DESTROYED:
                        this.Destroy();
                        break;
                }
            }
        }

        private void Destroy()
        {
            if (this.State != BipShipState.DESTROYED)
            {
                this.State = BipShipState.DESTROYED;
                _fallHorizontalVelocity = _direction == Enums.Direction.LEFT ? FLY_VELOCITY * -1 : FLY_VELOCITY;
                _fallVerticalVelocity = DESTROYED_FALLING_INITIAL_VELOCITY;
            }

            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + _fallHorizontalVelocity : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + Math.Abs(_fallHorizontalVelocity), this.HitBox.Height + _fallVerticalVelocity);
            var collisions = this.CheckCollision(areaToCheck, true);
            var horizontalTile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = GetTopMostLandingTile(collisions);
            if (horizontalTile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                _fallHorizontalVelocity = 0;
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + _fallHorizontalVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }

            DecelerateHorizontalMovement();

            if (verticalTile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, verticalTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                this.Explode();
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _fallVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
            }

            AccelerateGravity();
        }

        private void AccelerateGravity()
        {
            if (_fallVerticalVelocity + GRAVITY_ACCELERATION <= DESTROYED_FALLING_MAX_VELOCITY)
            {
                _fallVerticalVelocity += GRAVITY_ACCELERATION;
            }
            else
            {
                _fallVerticalVelocity = DESTROYED_FALLING_MAX_VELOCITY;
            }
        }

        private void Explode()
        {
            if (!_exploded)
            {
                int xPos = this.HitBox.X + (this.HitBox.Width / 2) - (Bip.WIDTH / 2);
                int yPos = this.HitBox.Bottom - Bip.HEIGHT;
                Bip bip = new Bip(_collisionGrid, new Rectangle(xPos, yPos, Bip.WIDTH, Bip.HEIGHT), _keen);
                bip.Create += new EventHandler<ObjectEventArgs>(object_Create);
                bip.Remove += new EventHandler<ObjectEventArgs>(object_Remove);
                OnCreate(new ObjectEventArgs() { ObjectSprite = bip });

                BipShipDebris debris = new BipShipDebris(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Bottom - BipShipDebris.HEIGHT, BipShipDebris.WIDTH, BipShipDebris.HEIGHT));
                debris.Create += new EventHandler<ObjectEventArgs>(object_Create);
                debris.Remove += new EventHandler<ObjectEventArgs>(object_Remove);
                OnCreate(new ObjectEventArgs() { ObjectSprite = debris });

                int heightDiff = BipShipExplosion.HEIGHT - this.HitBox.Height;
                int explodeX = this.HitBox.X, explodeY = this.HitBox.Y - heightDiff;
                BipShipExplosion explosion = new BipShipExplosion(new Point(explodeX, explodeY));
                explosion.Create += new EventHandler<ObjectEventArgs>(object_Create);
                explosion.Remove += new EventHandler<ObjectEventArgs>(object_Remove);
                OnCreate(new ObjectEventArgs() { ObjectSprite = explosion });

                _exploded = true;
            }
        }

        private void DecelerateHorizontalMovement()
        {
            if (_fallHorizontalVelocity != 0)
            {
                if (_direction == Enums.Direction.LEFT)
                {
                    if (_fallHorizontalVelocity + AIR_RESISTANCE <= 0)
                    {
                        _fallHorizontalVelocity += AIR_RESISTANCE;
                    }
                    else
                    {
                        _fallHorizontalVelocity = 0;
                    }
                }
                else if (_fallHorizontalVelocity - AIR_RESISTANCE >= 0)
                {
                    _fallHorizontalVelocity -= AIR_RESISTANCE;
                }
                else
                {
                    _fallHorizontalVelocity = 0;
                }
            }
        }

        private void Turn()
        {
            if (this.State != BipShipState.TURNING)
            {
                this.State = BipShipState.TURNING;
                _currentTurnSprite = 0;
            }
            var spriteSet = Direction == Enums.Direction.LEFT ? _turnRightSprites : _turnLeftSprites;
            if (_currentTurnSprite < spriteSet.Length)
            {
                this.UpdateSpriteByDelayBase(ref _currentTurnSpriteChangeDelayTick, ref _currentTurnSprite, TURN_SPRITE_CHANGE_DELAY, UpdateSprite);
            }
            else
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                this.Fly();
            }
        }

        protected override bool IsOnEdge(Direction directionToCheck, int edgeOffset = 0)
        {
            if (directionToCheck == Direction.LEFT)
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.Left - this.HitBox.Width + edgeOffset, this.HitBox.Bottom, this.HitBox.Width, HOVER_OVER_GROUND_DISTANCE + 2);
                var tiles = this.CheckCollision(areaToCheck, true);
                return !tiles.Any();
            }
            else if (directionToCheck == Direction.RIGHT)
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.Right - edgeOffset, this.HitBox.Bottom, this.HitBox.Width, HOVER_OVER_GROUND_DISTANCE + 2);
                var tiles = this.CheckCollision(areaToCheck, true);
                return !tiles.Any();
            }
            return false;
        }

        private bool IsKeenBehindThis()
        {
            if (_keen.HitBox.Top < this.HitBox.Bottom && _keen.HitBox.Bottom > this.HitBox.Top)
            {
                if (_keen.HitBox.Right < this.HitBox.Left + this.HitBox.Width / 2 && _direction == Enums.Direction.RIGHT)
                    return true;

                if (_keen.HitBox.Left > this.HitBox.Left + this.HitBox.Width / 2 && _direction == Enums.Direction.LEFT)
                    return true;
            }

            return false;
        }

        private bool IsInFireRange()
        {
            Rectangle areaToCheck = new Rectangle(this.HitBox.Left - FIRE_RANGE_X, this.HitBox.Y, this.HitBox.Width + (FIRE_RANGE_X * 2), this.HitBox.Height);
            bool inRange = _keen.HitBox.IntersectsWith(areaToCheck);
            return inRange;
        }

        private void Fly()
        {
            if (this.State != BipShipState.FLYING)
            {
                this.State = BipShipState.FLYING;
                if (IsOnEdge(this.Direction, EDGE_OFFSET))
                {
                    this.BasicFall(1);
                    return;
                }
            }

            if (IsOnEdge(this.Direction, EDGE_OFFSET) || IsKeenBehindThis())
            {
                this.Turn();
                return;
            }
            else if (IsInFireRange())
            {
                int fireVal = _random.Next(1, FIRE_CHANCE + 1);
                if (fireVal == FIRE_CHANCE)
                {
                    this.Fire();
                }
            }

            int xOffset = _direction == Enums.Direction.LEFT ? (FLY_VELOCITY + EDGE_OFFSET * -1) * -1 : FLY_VELOCITY + (EDGE_OFFSET * -1);
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + Math.Abs(xOffset), this.HitBox.Height + HOVER_OVER_GROUND_DISTANCE);

            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT
                    ? tile.HitBox.Right + (EDGE_OFFSET * -1) + 1
                    : tile.HitBox.Left - this.HitBox.Width - (EDGE_OFFSET * -1) - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Turn();
            }
            else
            {
                int velocity = _direction == Enums.Direction.LEFT ? FLY_VELOCITY * -1 : FLY_VELOCITY;
                this.HitBox = new Rectangle(this.HitBox.X + velocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
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
            get { return _state != BipShipState.DESTROYED; }
        }

        BipShipState State
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
                case BipShipState.FLYING:
                    _sprite.Image = _direction == Enums.Direction.LEFT
                        ? Properties.Resources.keen6_bip_ship_left
                        : Properties.Resources.keen6_bip_ship_right;
                    break;
                case BipShipState.TURNING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? _turnRightSprites : _turnLeftSprites;
                    if (_currentTurnSprite < spriteSet.Length)
                        _sprite.Image = spriteSet[_currentTurnSprite];
                    break;
                case BipShipState.DESTROYED:
                    _sprite.Image = _direction == Enums.Direction.LEFT
                      ? Properties.Resources.keen6_bip_ship_destroyed_left
                      : Properties.Resources.keen6_bip_ship_destroyed_right;
                    break;
            }
        }



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

        public void Fire()
        {
            _isFiring = true;
            int xPos = this.Direction == Enums.Direction.LEFT ? this.HitBox.Left : this.HitBox.Right;
            int yPos = this.HitBox.Y + 15;
            BipShipShot shot = new BipShipShot(_collisionGrid, new Rectangle(xPos, yPos, 26, 8), this.Direction, _keen);
            shot.Create += new EventHandler<ObjectEventArgs>(object_Create);
            shot.Remove += new EventHandler<ObjectEventArgs>(object_Remove);

            OnCreate(new ObjectEventArgs() { ObjectSprite = shot });
            _isFiring = false;
        }

        void object_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void object_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        public bool IsFiring
        {
            get { return _isFiring; }
        }

        public int Ammo
        {
            get { return -1; }
        }

        public PointItemType PointItem => PointItemType.KEEN6_PUDDING;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
    enum BipShipState
    {
        FLYING,
        TURNING,
        DESTROYED
    }
}
