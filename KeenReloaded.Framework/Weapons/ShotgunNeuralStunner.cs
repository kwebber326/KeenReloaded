using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Weapons
{
    public class ShotgunNeuralStunner : NeuralStunner
    {
        public ShotgunNeuralStunner(SpaceHashGrid grid, Rectangle hitbox, int ammo = 25)
            : base(grid, hitbox, ammo)
        {
            VELOCITY = 90;
            SPREAD = 10;
            REFIRE_DELAY = 10;
            SHOTS_PER_FIRE = 5;
        }
    }
}
