using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class Vitalin : Item, IDropCollector
    {
        public Vitalin(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            Initialize();
        }

        private void Initialize()
        {
            _canSteal = true;
            _moveUp = false;
            this.AcquiredSpriteList = new Image[] 
            {
                Properties.Resources.keen5_vitalin_acquired1,
                Properties.Resources.keen5_vitalin_acquired2,
                Properties.Resources.keen5_vitalin_acquired3,
                Properties.Resources.keen5_vitalin_acquired4
            };

            this.SpriteList = new Image[] 
            {
                Properties.Resources.keen5_vitalin1,
                Properties.Resources.keen5_vitalin2,
                Properties.Resources.keen5_vitalin3,
                Properties.Resources.keen5_vitalin4
            };
            this.Sprite.Image = this.SpriteList[0];
        }

        public int DropVal
        {
            get { return 1; }
        }
    }
}
