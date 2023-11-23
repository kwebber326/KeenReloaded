using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Windows.Forms;
using System.Drawing;

namespace KeenReloaded.Framework.Tiles
{
    public class DebugTile : CollisionObject, ISprite
    {
        public DebugTile(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            _sprite = new PictureBox();
            _sprite.BackColor = Color.Red;
            _sprite.Location = this.HitBox.Location;
            _sprite.Size = this.HitBox.Size;
            _sprite.BorderStyle = BorderStyle.Fixed3D;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
           
        }

        protected PictureBox _sprite;

        public PictureBox Sprite
        {
            get { return _sprite; }
        }
    }
}
