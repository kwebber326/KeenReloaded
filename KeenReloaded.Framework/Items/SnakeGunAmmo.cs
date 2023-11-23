using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Items
{
    public class SnakeGunAmmo : NeuralStunnerAmmo
    {
        public SnakeGunAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo) : base(grid, hitbox, ammo)
        {
        }

        protected override void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.snake_gun1,
                Properties.Resources.snake_gun2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.snake_gun_acquired
            };
            this.Sprite.Image = this.SpriteList.FirstOrDefault();
        }
    }
}
