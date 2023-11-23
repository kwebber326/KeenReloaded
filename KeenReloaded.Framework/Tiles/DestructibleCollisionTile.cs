using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Tiles
{
    public class DestructibleCollisionTile : DebugTile
    {
        public DestructibleCollisionTile(SpaceHashGrid grid, Rectangle hitbox) : base(grid, hitbox)
        {
        }
    }
}
