using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Tiles
{
    public class MapEdgeTile : DebugTile
    {
        private readonly MapEdgeBehavior _behavior;

        public MapEdgeTile(SpaceHashGrid grid, Rectangle hitbox, MapEdgeBehavior behavior) : base(grid, hitbox)
        {
            _sprite.Image = Properties.Resources.edge_of_map_tile_debug;
            _behavior = behavior;
        }

        public MapEdgeBehavior Behavior
        {
            get
            {
                return _behavior;
            }
        }
    }
}
