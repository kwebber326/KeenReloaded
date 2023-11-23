using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public abstract class Slicestar : DestructibleObject, IUpdatable, ISprite, IEnemy, ICreateRemove, IZombieBountyEnemy
    {
        protected CommanderKeen _keen;
        private const int DEATH_TIME = 15;
        private int _deathTimeTick;

        protected int VELOCITY = 10;

        public Slicestar(SpaceHashGrid grid, Rectangle hitbox, Direction direction, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("keen was not properly set");
            }
            this.Direction = direction;
            _keen = keen;
            Initialize();
        }

        protected virtual void Initialize()
        {
            this.Health = 20;
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = Properties.Resources.keen5_slicestar;
            _sprite.Location = this.HitBox.Location;
        }

        protected virtual void KillKeenIfColliding(Rectangle areaToCheck)
        {
            if (_keen.HitBox.IntersectsWith(areaToCheck))
            {
                _keen.Die();
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

        protected System.Windows.Forms.PictureBox _sprite;
        protected virtual Enums.Direction Direction
        {
            get;
            set;
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public override void Die()
        {
            _sprite.Image = Properties.Resources.keen5_slicestar_destroyed;
            UpdateDeathSprite();
        }

        public void Update()
        {
            if (!this.IsDead())
            {
                this.Move();
            }
            else
            {
                UpdateDeathSprite();
            }
        }

        private void UpdateDeathSprite()
        {
            if (_deathTimeTick++ == DEATH_TIME)
            {
                ObjectEventArgs e = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                OnRemove(e);
            }
        }

        public abstract void Move();

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite;  }
        }

        public bool DeadlyTouch
        {
            get { return !IsDead(); }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return !IsDead(); }
        }

        public PointItemType PointItem => PointItemType.KEEN5_BAG_O_SUGAR;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        private void OnRemove(ObjectEventArgs e)
        {
            if (Remove != null)
            {
                if (e.ObjectSprite == this)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }
                Remove(this, e);
            }
        }

        private void OnCreate(ObjectEventArgs e)
        {
            if (Create != null)
            {
                Create(this, e);
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
