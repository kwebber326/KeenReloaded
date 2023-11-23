using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class Keen6EyeBallTile : SingleMaskedPlatformTile
    {
        public Keen6EyeBallTile(SpaceHashGrid grid, Rectangle hitbox) 
            : base(grid, hitbox, BiomeType.KEEN6_FINAL)
        {
        }

        public override void ChangeBiome(BiomeType newBiome)
        {
           
        }
    }
}
