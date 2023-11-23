using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class ShotgunNeuralStunnerAmmo : NeuralStunnerAmmo
    {
        public ShotgunNeuralStunnerAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
            : base(grid, hitbox, ammo)
        {

        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_shotgun,
                Properties.Resources.neural_stunner_shotgun2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_shotgun_acquired
            };
            this.Sprite.Image = Properties.Resources.neural_stunner_shotgun;
        }
    }
}
