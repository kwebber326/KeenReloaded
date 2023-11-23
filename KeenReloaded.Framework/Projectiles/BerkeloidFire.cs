﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;
using System.Drawing;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Trajectories
{
    public class BerkeloidFire : CollisionObject, IUpdatable, ITrajectory, ISprite, IGravityObject, ICreateRemove
    {
        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;

        private Image[] _collidedImages = new Image[] 
        {
            Properties.Resources.keen4_berkeloid_fire_hit1,
            Properties.Resources.keen4_berkeloid_fire_hit2,
            Properties.Resources.keen4_berkeloid_fire_hit3
        };
        private bool _collided;
        private int _currentCollidedSprite = 0;
        private bool _reverse;
        private int _velocity;
        private const int INITIAL_MOVE_VELOCITY = 60;
        private const int INITIAL_FALL_VELOCITY = 0;
        private int _fallVelocity;
        private const int FALL_ACCELERATION = 5;
        private const int VELOCITY_DECREASE = 2;
        private const int MAX_FALL_VELOCITY = 100;

        public static int INITIAL_WIDTH
        {
            get
            {
                return 28;
            }
        }

        public static int INITIAL_HEIGHT
        {
            get
            {
                return 30;
            }
        }

        public BerkeloidFire(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox)
        {
            this.Direction = direction;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = Properties.Resources.keen4_berkeloid_fire;
            if (this.HitBox != null)
                _sprite.Location = this.HitBox.Location;

            _velocity = INITIAL_MOVE_VELOCITY;
            _fallVelocity = INITIAL_FALL_VELOCITY;
        }

        public int Damage
        {
            get { return 1; }
        }

        public int Velocity
        {
            get { return _velocity; }
        }

        public int Pierce
        {
            get { return 0; }
        }

        public int Spread
        {
            get { return 0; }
        }

        public int BlastRadius
        {
            get { return 0; }
        }

        public int RefireDelay
        {
            get { return -1; }
        }

        public bool KillsKeen
        {
            get { return true; }
        }


        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            var tiles = collisions.Where(c => c is DebugTile || c is PlatformTile);
            if (tiles.Any())
            {
                return tiles.OrderByDescending(c => c.HitBox.Right).FirstOrDefault();
            }
            return null;
        }

        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            var tiles = collisions.Where(c => c is DebugTile || c is PlatformTile);
            if (tiles.Any())
            {
                return tiles.OrderBy(c => c.HitBox.Left).FirstOrDefault();
            }
            return null;
        }

        public void Move()
        {
            if (this.IsOutOfBounds())
            {
                this.Stop();
                return;
            }
            int velocity = this.Direction == Enums.Direction.LEFT ? _velocity * -1 : _velocity;
            Rectangle areaToCheck = this.Direction == Enums.Direction.LEFT ?
                new Rectangle(this.HitBox.Left + velocity, this.HitBox.Y, _velocity, this.HitBox.Height + _fallVelocity) :
                new Rectangle(this.HitBox.Right, this.HitBox.Y, _velocity, this.HitBox.Height + _fallVelocity);
            var collisions = this.CheckCollision(areaToCheck);
            var keens = collisions.OfType<CommanderKeen>();

            CollisionObject tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);


            if (tile != null)
            {
                if (this.Direction == Enums.Direction.LEFT)
                {
                    if (this.HitBox.Left > tile.HitBox.Right)
                        this.HitBox = new Rectangle(tile.HitBox.Right, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    if (keens.Any())
                    {
                        foreach (var keen in keens)
                        {
                            if (keen.HitBox.Right > tile.HitBox.Right)
                            {
                                keen.Die();
                            }
                        }
                    }
                }
                else
                {
                    if (this.HitBox.Right < tile.HitBox.Left)
                        this.HitBox = new Rectangle(tile.HitBox.Left - this.HitBox.Width, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    if (keens.Any())
                    {
                        foreach (var keen in keens)
                        {
                            if (keen.HitBox.Left < tile.HitBox.Left)
                            {
                                keen.Die();
                            }
                        }
                    }
                }
                _collided = true;
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + velocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_velocity >= VELOCITY_DECREASE)
                {
                    _velocity -= VELOCITY_DECREASE;
                }
                foreach (var keen in keens)
                {
                    keen.Die();
                }
            }
        }

        public void Stop()
        {
            _collided = true;
        }

        public MoveState MoveState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Jump()
        {
        }

        public bool CanJump
        {
            get { return false; }
        }

        public void Fall()
        {
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _fallVelocity, this.HitBox.Width, this.HitBox.Height);
            if (_fallVelocity < MAX_FALL_VELOCITY)
            {
                _fallVelocity += FALL_ACCELERATION;
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
                if (this.HitBox != null && _sprite != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(this.Direction);
                    this.UpdateCollisionNodes(Enums.Direction.DOWN);
                }
            }
        }

        public void Update()
        {
            if (!_collided)
            {
                this.Move();
                if (!_collided)
                    this.Fall();
            }
            else
            {
                UpdateCollisionSprite();
            }
        }

        private void UpdateCollisionSprite()
        {
            int sprite = _currentCollidedSprite;
            if (!_reverse)
            {
                if (_currentCollidedSprite == _collidedImages.Length)
                {
                    _reverse = true;
                    _currentCollidedSprite--;
                }
                else
                {
                    this.Sprite.Image = _collidedImages[_currentCollidedSprite++];
                }
            }
            else
            {
                if (_currentCollidedSprite < 0)
                {
                    OnRemove();
                    return;
                }
                else
                {
                    this.Sprite.Image = _collidedImages[_currentCollidedSprite--];
                }
            }
            int yOffset;
            if (sprite == 0)
            {
                yOffset = _reverse ? 0 : 2;
            }
            else
            {
                yOffset = _reverse ? 0 : 25;
            }
            this.HitBox = new Rectangle(new Point(this.HitBox.X, this.HitBox.Y - yOffset), this.Sprite.Size);
            if (!_reverse)
            {
                this.UpdateCollisionNodes(Direction.UP);
            }
            else
            {
                this.UpdateCollisionNodes(Direction.DOWN);
            }
        }

        protected void OnCreate()
        {
            if (Create != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                Create(this, args);
            }
        }

        protected void OnRemove()
        {
            if (Remove != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                if (_collidingNodes != null && _collidingNodes.Any())
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                        node.NonEnemies.Remove(this);
                    }
                }
                Remove(this, args);
            }
        }

        public event EventHandler<ObjectEventArgs> Create;

        public event EventHandler<ObjectEventArgs> Remove;

    }
}
