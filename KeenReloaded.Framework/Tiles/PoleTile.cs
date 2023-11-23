using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Tiles
{
    public class PoleTile : CollisionObject, ISprite
    {
        private System.Windows.Forms.PictureBox _sprite;
        private BiomeType _type;

        public PoleTile(SpaceHashGrid grid, Rectangle hitbox, PictureBox sprite)
            : base(grid, hitbox)
        {
            _sprite = sprite;
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
