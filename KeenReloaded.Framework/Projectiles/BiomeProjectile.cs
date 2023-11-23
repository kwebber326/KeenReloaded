using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Hazards;

namespace KeenReloaded.Framework.Trajectories
{
    public class BiomeProjectile : CollisionObject, ITrajectory, IUpdatable, ISprite, ICreateRemove
    {
        protected int _verticalVelocity;
        protected int _horizontalVelocity;
        protected System.Windows.Forms.PictureBox _sprite;

        protected const int AIR_RESISTANCE = 2;
        protected const int GRAVITY_ACCELERATION = 5;
        private const int MAX_FALL_VELOCITY = 150;

        protected bool _removed;

        public BiomeProjectile(SpaceHashGrid grid, Rectangle hitbox, int initialVerticalVelocity, int initialHorizontalVelocity, BiomeType biome)
            : base(grid, hitbox)
        {
            _biome = biome;
            _verticalVelocity = initialHorizontalVelocity;
            _horizontalVelocity = initialHorizontalVelocity;
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
                if (_sprite != null && value != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }


        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => c is DebugTile || c is IBiomeTile).ToList();
            if (collisions.Any() && debugTiles.Any())
            {
                var rightTiles = debugTiles.Where(c => c.HitBox.Left > this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (rightTiles.Any())
                {
                    int minX = rightTiles.Select(t => t.HitBox.Left).Min();
                    CollisionObject obj = rightTiles.FirstOrDefault(x => x.HitBox.Left == minX);
                    return obj;
                }
            }
            return null;
        }

        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => c is DebugTile || c is IBiomeTile).ToList();
            if (collisions.Any() && debugTiles.Any())
            {
                var leftTiles = debugTiles.Where(c => c.HitBox.Left < this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (leftTiles.Any())
                {
                    int maxX = leftTiles.Select(t => t.HitBox.Right).Max();
                    CollisionObject obj = leftTiles.FirstOrDefault(x => x.HitBox.Right == maxX);
                    return obj;
                }
            }
            return null;
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            SetSprite();

            var collisions = this.CheckCollision(this.HitBox);

            if (collisions.Any())
            {
                var horizontalCollision = _horizontalVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
                var verticalCollision = _verticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);

                int verticalDist = int.MaxValue;
                int horizontalDist = int.MaxValue;

                if (horizontalCollision != null)
                {
                    horizontalDist = Math.Abs(this.HitBox.X - horizontalCollision.HitBox.X);
                }
                if (verticalCollision != null)
                {
                    verticalDist = Math.Abs(this.HitBox.Y - verticalCollision.HitBox.Y);
                }

                CollisionObject collision = verticalDist < horizontalDist ? verticalCollision : horizontalCollision;

                if (collision != null)
                {
                    this.HandleCollision(collision);
                }
            }
        }

        private void SetSprite()
        {
            switch (_biome)
            {
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_floor_middle;
                    break;
                case BiomeType.KEEN4_GREEN:
                    _sprite.Image = Properties.Resources.keen4_forest_floor_middle;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_floor_middle;
                    break;
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.keen4_pyramid_floor_middle;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_floor_black_middle;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_floor_green_middle;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_floor_red_middle;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_floor_middle;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_floor_middle;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_floor_middle;
                    break;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is IBiomeTile)
            {
                var biomeTile = (IBiomeTile)obj;
                biomeTile.ChangeBiome(_biome);
            }
            this.Stop();
        }

        public virtual int Damage
        {
            get { return -1; }
        }

        public virtual int Velocity
        {
            get { return Math.Abs(_horizontalVelocity) > Math.Abs(_verticalVelocity) ? _horizontalVelocity : _verticalVelocity; }
        }

        public virtual int Pierce
        {
            get { return -1; }
        }

        public virtual int Spread
        {
            get { return -1; }
        }

        public virtual int BlastRadius
        {
            get { return -1; }
        }

        public virtual int RefireDelay
        {
            get { return -1; }
        }

        public bool KillsKeen
        {
            get { return false; }
        }

        public void Move()
        {
            int speedX = Math.Abs(_horizontalVelocity);
            int speedY = Math.Abs(_verticalVelocity);
            int xPosCheck = _horizontalVelocity < 0 ? this.HitBox.X + _horizontalVelocity : this.HitBox.X;
            int yPosCheck = _verticalVelocity < 0 ? this.HitBox.Y + _verticalVelocity : this.HitBox.Y;

            Rectangle areaToCheck = new Rectangle(xPosCheck, yPosCheck, this.HitBox.Width + speedX, this.HitBox.Height + speedY);
            var collisions = this.CheckCollision(areaToCheck);

            var verticalTile = _verticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
            var horizontalTile = _horizontalVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);


            if (verticalTile != null && horizontalTile != null)
            {
                CollisionObject collisionTile = null;
                int xDistanceToCollisionTile = Math.Abs(horizontalTile.HitBox.X - this.HitBox.X), yDistanceToCollisionTile = Math.Abs(verticalTile.HitBox.Y - this.HitBox.Y);

                int xCollidePos = this.HitBox.X, yCollidePos = this.HitBox.Y;
                if (xDistanceToCollisionTile < yDistanceToCollisionTile)
                {
                    xCollidePos = _horizontalVelocity < 0 ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                    collisionTile = horizontalTile;
                }
                else
                {
                    yCollidePos = _verticalVelocity < 0 ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                    collisionTile = verticalTile;
                }
                this.HitBox = new Rectangle(xCollidePos, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                this.HandleCollision(collisionTile);
            }
            else if (verticalTile != null)
            {
                int yCollidePos = _verticalVelocity < 0 ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(this.HitBox.X, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                this.HandleCollision(verticalTile);
            }
            else if (horizontalTile != null)
            {
                int xCollidePos = _horizontalVelocity < 0 ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.HandleCollision(horizontalTile);
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + _horizontalVelocity, this.HitBox.Y + _verticalVelocity, this.HitBox.Width, this.HitBox.Height);
                DecelerateHorizontalVelocity();
                AccelerateGravity();
            }
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {
            //var debugTiles = collisions.Where(c => (c is IBiomeTile || c is DebugTile) && c.HitBox.Bottom <= this.HitBox.Top).ToList();
            //if (debugTiles.Any())
            //{
            //    //int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
            //    //CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
            //    //return obj;

            //}
            //return null;
            var debugs = collisions.Where(c => c is DebugTile && c.HitBox.Bottom <= this.HitBox.Top).ToList();
            var biomes = collisions.Where(c => c is IBiomeTile && c.HitBox.Bottom <= this.HitBox.Top).ToList();
            if (!debugs.Any() && !biomes.Any())
                return null;

            int maxBottomDebugTile = debugs.Any() ? debugs.Select(c => c.HitBox.Bottom).Max() : 0;
            int maxBottomBiomeTile = biomes.Any() ? biomes.Select(c => c.HitBox.Bottom).Max() : 0;

            if (maxBottomBiomeTile >= maxBottomDebugTile)
            {
                CollisionObject obj = biomes.FirstOrDefault(c => c.HitBox.Bottom == maxBottomBiomeTile);
                return obj;
            }
            else
            {
                CollisionObject obj = debugs.FirstOrDefault(c => c.HitBox.Bottom == maxBottomDebugTile);
                return obj;
            }
        }

        protected override CollisionObject GetTopMostLandingTile(List<CollisionObject> collisions)
        {
            //CollisionObject topMostTile;
            //var landingTiles = collisions.Where(h => (h is IBiomeTile || h is DebugTile)
            //    && h.HitBox.Top >= this.HitBox.Top);

            //if (!landingTiles.Any())
            //    return null;

            //int minY = landingTiles.Select(c => c.HitBox.Top).Min();
            //topMostTile = landingTiles.FirstOrDefault(t => t.HitBox.Top == minY);

            //return topMostTile;
            var debugs = collisions.Where(c => c is DebugTile && c.HitBox.Top >= this.HitBox.Top).ToList();
            var biomes = collisions.Where(c => c is IBiomeTile && c.HitBox.Top >= this.HitBox.Top).ToList();
            if (!debugs.Any() && !biomes.Any())
                return null;

            int minTopDebugTile = debugs.Any() ? debugs.Select(c => c.HitBox.Top).Min() : 0;
            int minTopBottomBiomeTile = biomes.Any() ? biomes.Select(c => c.HitBox.Top).Min() : 0;

            if (minTopBottomBiomeTile <= minTopDebugTile)
            {
                CollisionObject obj = biomes.FirstOrDefault(c => c.HitBox.Top == minTopBottomBiomeTile);
                return obj;
            }
            else
            {
                CollisionObject obj = debugs.FirstOrDefault(c => c.HitBox.Top == minTopDebugTile);
                return obj;
            }
        }

        private void AccelerateGravity()
        {
            if (_verticalVelocity < MAX_FALL_VELOCITY)
            {
                if (_verticalVelocity + GRAVITY_ACCELERATION < MAX_FALL_VELOCITY)
                {
                    _verticalVelocity += GRAVITY_ACCELERATION;
                }
                else
                {
                    _verticalVelocity = MAX_FALL_VELOCITY;
                }
            }
        }

        private void DecelerateHorizontalVelocity()
        {
            if (_horizontalVelocity != 0)
            {
                if (_horizontalVelocity < 0)
                {
                    if (_horizontalVelocity + AIR_RESISTANCE < 0)
                    {
                        _horizontalVelocity += AIR_RESISTANCE;
                    }
                    else
                    {
                        _horizontalVelocity = 0;
                    }
                }
                else if (_horizontalVelocity - AIR_RESISTANCE > 0)
                {
                    _horizontalVelocity -= AIR_RESISTANCE;
                }
                else
                {
                    _horizontalVelocity = 0;
                }
            }
        }

        public void Stop()
        {
            OnRemove(new ObjectEventArgs() { ObjectSprite = this });
            _stopped = true;
        }

        public Enums.MoveState MoveState
        {
            get;
            set;
        }

        public Enums.Direction Direction
        {
            get;
            set;
        }

        public void Update()
        {
            if (!_stopped)
                this.Move();
            else if (!_removed)
                OnRemove(new ObjectEventArgs() { ObjectSprite = this });
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        protected BiomeType _biome;
        private bool _stopped;

        protected void OnCreate(ObjectEventArgs e)
        {
            if (this.Create != null)
                this.Create(this, e);
        }

        protected void OnRemove(ObjectEventArgs e)
        {
            if (e.ObjectSprite == this)
            {
                foreach (var node in _collidingNodes)
                {
                    node.Objects.Remove(this);
                    node.NonEnemies.Remove(this);
                }
            }
            if (this.Remove != null)
            {
                this.Remove(this, e);
                _removed = true;
            }
        }
    }
}
