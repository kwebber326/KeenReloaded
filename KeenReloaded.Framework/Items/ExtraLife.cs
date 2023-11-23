using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Items
{
    public class ExtraLife : Item
    {
        private ExtraLifeType _type;
        public ExtraLife(SpaceHashGrid grid, Rectangle hitbox, ExtraLifeType type)
            : base(grid, hitbox)
        {
            _type = type;
            Initialize();
        }

        public ExtraLifeType ExtraLifeType
        {
            get
            {
                return _type;
            }
        }

        private void Initialize()
        {
            _canSteal = true;
            this.AcquiredSpriteList = new Image[]
            {
                Properties.Resources.keen_1up
            };
            switch (_type)
            {
                case ExtraLifeType.KEEN4_LIFEWATER_FLASK:
                    this.SpriteList = new Image[]
                    {
                        Properties.Resources.keen4_lifewater_flask1,
                        Properties.Resources.keen4_lifewater_flask2
                    };
                    break;
                case ExtraLifeType.KEEN5_KEG_O_VITALIN:
                    this.SpriteList = new Image[]
                    {
                        Properties.Resources.keen5_keg_o_vitalin1,
                        Properties.Resources.keen5_keg_o_vitalin2
                    };
                    break;
                case ExtraLifeType.KEEN6_VIVA_QUEEN:
                    this.SpriteList = new Image[]
                    {
                        Properties.Resources.keen6_viva_queen1,
                        Properties.Resources.keen6_viva_queen2
                    };
                    break;
            }
            if (this.SpriteList != null && this.SpriteList.Length > 0)
            {
                this.Sprite.Image = this.SpriteList[0];
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.ExtraLifeType.ToString()}";
        }
    }
}
