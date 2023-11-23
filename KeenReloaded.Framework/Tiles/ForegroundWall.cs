using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Tiles
{
    public class ForegroundWall : BackgroundSprite
    {
        public ForegroundWall(Rectangle area, Image sprite, int zIndex, bool stretchImage) : base(area, sprite, zIndex, stretchImage)
        {
        }
    }
}
