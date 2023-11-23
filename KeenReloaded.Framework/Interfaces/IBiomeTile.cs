using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using System.Drawing;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IBiomeTile
    {
        BiomeType Biome { get; }

        void ChangeBiome(BiomeType newBiome);

        Point SpriteLocation { get; }
    }
}
