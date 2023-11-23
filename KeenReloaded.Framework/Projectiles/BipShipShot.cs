using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Trajectories
{
    public class BipShipShot : CollisionObject, IUpdatable, ISprite, ITrajectory, ICreateRemove
    {
        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private bool _shotHit;
        public BipShipShot(SpaceHashGrid grid, Rectangle hitbox, Direction direction, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("Keen was not properly set");
            }
            _keen = keen;
            this.Direction = direction == Enums.Direction.LEFT ? Enums.Direction.LEFT : Enums.Direction.RIGHT;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = Properties.Resources.keen6_bip_ship_laser;
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

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Update()
        {
            if (!_shotHit)
                this.Move();
            else
                OnRemove(new ObjectEventArgs() { ObjectSprite = this });
        }

        public int Damage
        {
            get { return int.MaxValue; }
        }

        public int Velocity
        {
            get { return 50; }
        }

        public int Pierce
        {
            get { return 0; }
        }

        public int Spread
        {
            get { return 10; }
        }

        public int BlastRadius
        {
            get { return 0; }
        }

        public int RefireDelay
        {
            get { return 0; }
        }

        public bool KillsKeen
        {
            get { return true; }
        }

        public void Move()
        {
            int xOffset = this.Direction == Enums.Direction.LEFT ? this.Velocity * -1 : this.Velocity;
            int xPosCheck = this.Direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + this.Velocity, this.HitBox.Height + this.Spread);

            var collisions = this.CheckCollision(areaToCheck, true);
            var horizontalTile = Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = GetTopMostLandingTile(collisions);
            if (horizontalTile != null || verticalTile != null)
            {
                if (horizontalTile != null)
                {
                    int xCollidePos = Direction == Enums.Direction.LEFT ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                    this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    if (verticalTile != null && this.HitBox.IntersectsWith(verticalTile.HitBox))
                    {
                        this.HitBox = new Rectangle(this.HitBox.X, verticalTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                    }
                }
                else if (verticalTile != null)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, verticalTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                }
                if (this.HitBox.IntersectsWith(_keen.HitBox))
                {
                    _keen.Die();
                }
                this.Stop();
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y + this.Spread, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }
        }

        public void Stop()
        {
            _shotHit = true;
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
                        node.NonEnemies.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }
    }
}
