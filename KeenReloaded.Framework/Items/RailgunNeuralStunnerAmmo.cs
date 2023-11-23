using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class RailgunNeuralStunnerAmmo : NeuralStunnerAmmo
    {
        public RailgunNeuralStunnerAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
            : base(grid, hitbox, ammo)
        {

        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_railgun1,
                Properties.Resources.neural_stunner_railgun2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_railgun_acquired
            };
            this.Sprite.Image = Properties.Resources.neural_stunner_railgun1;
        }
    }
}
