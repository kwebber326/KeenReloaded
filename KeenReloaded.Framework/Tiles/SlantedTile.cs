using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Windows.Forms;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class SlantedTile : CollisionObject, ISprite
    {
        PictureBox _sprite;
        public readonly int Angle;
        public readonly Direction Direction;
        public SlantedTile(SpaceHashGrid grid, Rectangle hitbox, int angle, Direction direction) : base (grid, hitbox)
        {
            _sprite = new PictureBox();
            _sprite.BackColor = Color.Blue;
            _sprite.Location = this.HitBox.Location;
            _sprite.Size = this.HitBox.Size;
            _sprite.BorderStyle = BorderStyle.Fixed3D;
            this.Angle = angle;
            this.Direction = direction;
        }

       

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }
    }
}
