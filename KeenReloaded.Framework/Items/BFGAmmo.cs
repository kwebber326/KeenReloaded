using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Items
{
    public class BFGAmmo : NeuralStunnerAmmo
    {
        public BFGAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
           : base(grid, hitbox, ammo)
        {

        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.BFG1,
                Properties.Resources.BFG2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.BFG_acquired
            };
            this.Sprite.Image = this.SpriteList.FirstOrDefault();
        }
    }
}
