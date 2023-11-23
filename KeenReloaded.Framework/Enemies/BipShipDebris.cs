using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class BipShipDebris : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        private System.Windows.Forms.PictureBox _sprite;
        private const int BASIC_FALL_VELOCITY = 30;
        public const int WIDTH = 64;
        public const int HEIGHT = 24;
        public BipShipDebris(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = Properties.Resources.keen6_bip_ship_debris;
            _sprite.Location = this.HitBox.Location;
            _sprite.BringToFront();
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
                    this.UpdateCollisionNodes(Enums.Direction.DOWN);
                }
            }
        }

        public void Update()
        {
            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
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

        protected override void HandleCollision(CollisionObject obj)
        {

        }
    }
}
