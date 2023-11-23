using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Weapons
{
    public class SMGNeuralStunner : NeuralStunner
    {
        public SMGNeuralStunner(SpaceHashGrid grid, Rectangle hitbox, int ammo = 10)
            : base(grid, hitbox, ammo)
        {
            SPREAD = 12;
            PIERCE = 1;
            DAMAGE = 1;
            REFIRE_DELAY = 3;
            IS_AUTO = true;
        }
    }
}
