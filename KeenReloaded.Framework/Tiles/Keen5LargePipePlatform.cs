using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class Keen5LargePipePlatform : SingleMaskedPlatformTile
    {
        public Keen5LargePipePlatform(SpaceHashGrid grid, Rectangle hitbox) 
            : base(grid, hitbox, BiomeType.KEEN5_BLACK)
        {
        }

        protected override void SetSprite()
        {
            this.Sprite.Image = Properties.Resources.keen5_pipe_platform;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.SpriteLocation.X}|{this.SpriteLocation.Y}|{this.Sprite.Width}|{this.Sprite.Height}";
        }
    }
}
