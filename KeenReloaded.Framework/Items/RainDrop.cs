using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Items
{
    public class RainDrop : Item, IDropCollector
    {
        public RainDrop(SpaceHashGrid grid, Rectangle hitbox)
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
                Properties.Resources.keen4_drop_acquired1,
                Properties.Resources.keen4_drop_acquired2,
                Properties.Resources.keen4_drop_acquired3
            };

            this.SpriteList = new Image[] 
            {
                Properties.Resources.keen4_drop1,
                Properties.Resources.keen4_drop2,
                Properties.Resources.keen4_drop3
            };
            this.Sprite.Image = this.SpriteList[0];
        }

        public int DropVal
        {
            get { return 1; }
        }
    }
}
