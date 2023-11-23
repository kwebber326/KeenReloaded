using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;

namespace KeenReloaded.Framework.Items
{
    public class Viva : Item, IDropCollector
    {
        private bool _perch;
        public Viva(SpaceHashGrid grid, Rectangle hitbox, bool perch)
            : base(grid, hitbox)
        {
            _perch = perch;
            Initialize();
        }
        public int DropVal
        {
            get { return 1; }
        }

        public bool Perched
        {
            get
            {
                return _perch;
            }
        }

        private void Initialize()
        {
            _canSteal = true;
            _moveUp = false;
            this.AcquiredSpriteList = new Image[] 
            {
                Properties.Resources.keen6_viva_acquired1,
                Properties.Resources.keen6_viva_acquired2,
                Properties.Resources.keen6_viva_acquired3,
                Properties.Resources.keen6_viva_acquired4
            };

            if (_perch)
            {
                this.SpriteList = new Image[] 
                {
                    Properties.Resources.keen6_viva_perched1,
                    Properties.Resources.keen6_viva_perched2,
                    Properties.Resources.keen6_viva_perched3,
                    Properties.Resources.keen6_viva_perched4
                };
            }
            else
            {
                this.SpriteList = new Image[] 
                {
                    Properties.Resources.keen6_viva_flying1,
                    Properties.Resources.keen6_viva_flying2,
                    Properties.Resources.keen6_viva_flying3
                };
            }
            this.Sprite.Image = this.SpriteList[0];
        }

        public override string ToString()
        {
            return base.ToString() + $"|{this.Perched}"; ;
        }
    }
}
