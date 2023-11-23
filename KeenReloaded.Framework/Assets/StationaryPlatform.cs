using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class StationaryPlatform : Platform
    {
        public StationaryPlatform(SpaceHashGrid grid, Rectangle hitbox, PlatformType type, CommanderKeen keen)
            : base(grid, hitbox, type, keen, Guid.Empty)
        {

        }
        public override void Activate()
        {
           
        }

        public override void Deactivate()
        {
         
        }

        public override void Update()
        {

        }
    }
}
