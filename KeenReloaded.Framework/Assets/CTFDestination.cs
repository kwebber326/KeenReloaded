using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Assets
{
    public class CTFDestination : CollisionObject, ISprite
    {
        private readonly GemColor _color;
        private PictureBox _sprite;

        public CTFDestination(SpaceHashGrid grid, Rectangle hitbox, GemColor color)
            : base (grid, hitbox)
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = hitbox.Location;
            _sprite.Image = SpriteSheet.CTFDestinations[(int)color];

            _color = color;
        }

        public PictureBox Sprite => _sprite;
        public GemColor Color => _color;

        public override string ToString()
        {
            return base.ToString() + "|" + _color.ToString();
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            
        }
    }
}
