using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class RemovableTile : CollisionObject, ICreateRemove, ISprite
    {
        protected PictureBox _sprite;

        public RemovableTile(SpaceHashGrid grid, Rectangle hitbox) : base(grid, hitbox)
        {
        }

        public PictureBox Sprite => _sprite;

        public event EventHandler<ObjectEventArgs> Create;
        public event EventHandler<ObjectEventArgs> Remove;

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }

        protected virtual void OnCreate()
        {
            ObjectEventArgs e = new ObjectEventArgs()
            {
                ObjectSprite = this
            };
            this.Create?.Invoke(this, e);
        }

        protected virtual void OnRemove()
        {
            ObjectEventArgs e = new ObjectEventArgs()
            {
                ObjectSprite = this
            };
            this.Remove?.Invoke(this, e);
        }
    }
}
