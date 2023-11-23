using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System.Drawing;

namespace KeenReloaded.Framework.Enemies
{
    public class NospikeQuestionMark : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        public const int WIDTH = 16;
        public const int HEIGHT = 32;

        public NospikeQuestionMark(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            Initialize();
        }

        protected override void HandleCollision(CollisionObject obj)
        {
     
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = Properties.Resources.keen6_nospike_confused;
        }

        public void Update()
        {
            
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;

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
    }
}
