using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Items
{
    public class KeyCard : Item
    {
        public KeyCard(SpaceHashGrid grid, Rectangle hitbox) : base(grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _canSteal = false;
            this.SpriteList = SpriteSheet.Keen5KeyCardImages;
            this.AcquiredSpriteList = SpriteSheet.Keen5KeyCardAcquiredImages;

            this.Sprite.Image = this.SpriteList[0];
        }
    }
}
