using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class SMGNeuralStunnerAmmo : NeuralStunnerAmmo
    {
        public SMGNeuralStunnerAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo = 10)
            : base(grid, hitbox, ammo)
        {

        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_smg_1,
                Properties.Resources.neural_stunner_smg_2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_smg_acquired
            };
            this.Sprite.Image = Properties.Resources.neural_stunner_smg_1;
        }
    }
}
