using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class BoobusBombLauncherAmmo : NeuralStunnerAmmo
    {
        public BoobusBombLauncherAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
            : base(grid, hitbox, ammo)
        {

        }
        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.keen_dreams_boobus_bomb1,
                Properties.Resources.keen_dreams_boobus_bomb2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.keen_dreams_boobus_bomb_acquired
            };
            this.Sprite.Image = Properties.Resources.keen_dreams_boobus_bomb1;
        }
    }

}
