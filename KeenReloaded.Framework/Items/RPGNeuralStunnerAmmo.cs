using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class RPGNeuralStunnerAmmo : NeuralStunnerAmmo
    {
        public RPGNeuralStunnerAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
            : base(grid, hitbox, ammo)
        {

        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_rocket_launcher1,
                Properties.Resources.neural_stunner_rocket_launcher2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_rocket_launcher_acquired
            };
            this.Sprite.Image = Properties.Resources.neural_stunner_rocket_launcher1;
        }
    }
}
