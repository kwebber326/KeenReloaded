using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class NeuralStunnerAmmo : Item
    {
        private int _ammoAmmount;
        public NeuralStunnerAmmo(SpaceHashGrid grid, Rectangle hitbox, int ammo)
            : base(grid, hitbox)
        {
            Initialize();
            _ammoAmmount = ammo;
        }

        protected virtual void Initialize()
        {
            this.SpriteList = new Image[]
            {
                Properties.Resources.neural_stunner1,
                Properties.Resources.neural_stunner2
            };

            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.neural_stunner_acquired
            };
            this.Sprite.Image = Properties.Resources.neural_stunner1;
        }

        public int AmmoAmount
        {
            get
            {
                return _ammoAmmount;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.AmmoAmount}";
        }
    }
}
