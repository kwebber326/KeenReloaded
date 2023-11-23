using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Windows.Forms;
using System.Drawing;

namespace KeenReloaded.Framework.Tiles
{
    public class PlatformTile : CollisionObject, ISprite
    {
        public PlatformTile(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            _sprite = new PictureBox();
            _sprite.BackColor = Color.Black;
            _sprite.Location = this.HitBox.Location;
            _sprite.Size = this.HitBox.Size;
            _sprite.BorderStyle = BorderStyle.Fixed3D;
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public void UpdateLocation(Point p)
        {
            this.HitBox = new Rectangle(p.X, p.Y, this.HitBox.Width, this.HitBox.Height);
            if (_sprite != null)
                _sprite.Location = this.HitBox.Location;

            this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
        }

        private PictureBox _sprite;

        public PictureBox Sprite
        {
            get { return _sprite; }
        }
    }
}
