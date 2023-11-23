using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Items
{
    public class PointItem : Item
    {
        private PointItemType _type;
        public PointItem(SpaceHashGrid grid, Rectangle hitbox, PointItemType type)
            : base(grid, hitbox)
        {
            _type = type;
            Initialize();
        }

        public int PointValue
        {
            get;
            private set;
        }

        public PointItemType PointItemType
        {
            get
            {
                return _type;
            }
        }

        private void Initialize()
        {
            _canSteal = true;
            switch (_type)
            {
                case PointItemType.KEEN4_SHIKADI_SODA:
                    PointValue = 100;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_shikadi_soda1,
                        Properties.Resources.keen4_shikadi_soda2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_100
                    };
                    break;
                case PointItemType.KEEN4_THREE_TOOTH_GUM:
                    PointValue = 200;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_three_tooth_gum1,
                        Properties.Resources.keen4_three_tooth_gum2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_200
                    };
                    break;
                case PointItemType.KEEN4_SHIKKERS_CANDY_BAR:
                    PointValue = 500;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_candy_bar1,
                        Properties.Resources.keen4_candy_bar2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_500
                    };
                    break;
                case PointItemType.KEEN4_JAWBREAKER:
                    PointValue = 1000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_jawbreaker1,
                        Properties.Resources.keen4_jawbreaker2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_1000
                    };
                    break;
                case PointItemType.KEEN4_DOUGHNUT:
                    PointValue = 2000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_doughnut1,
                        Properties.Resources.keen4_doughnut1
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_2000
                    };
                    break;
                case PointItemType.KEEN4_ICECREAM_CONE:
                    PointValue = 5000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen4_icecream_cone1,
                        Properties.Resources.keen4_icecream_cone2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_5000
                    };
                    break;
                case PointItemType.KEEN5_SHIKADI_GUM:
                    PointValue = 100;
                      SpriteList = new Image[]{
                        Properties.Resources.keen5_shikadi_gum1,
                        Properties.Resources.keen5_shikadi_gum2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_100
                    };
                    break;
                case PointItemType.KEEN5_MARSHMALLOW:
                    PointValue = 200;
                    SpriteList = new Image[]{
                        Properties.Resources.keen5_marshmallow1,
                        Properties.Resources.keen5_marshmallow2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_200
                    };
                    break;
                case PointItemType.KEEN5_CHOCOLATE_MILK:
                    PointValue = 500;
                    SpriteList = new Image[]{
                        Properties.Resources.keen5_chocolate_milk1,
                        Properties.Resources.keen5_chocolate_milk2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_500
                    };
                    break;
                case PointItemType.KEEN5_TART_STIX:
                    PointValue = 1000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen5_tart_stix1,
                        Properties.Resources.keen5_tart_stix1
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_1000
                    };
                    break;
                case PointItemType.KEEN5_SUGAR_STOOPIES_CEREAL:
                    PointValue = 2000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen5_sugar_stoopies_cereal1,
                        Properties.Resources.keen5_sugar_stoopies_cereal2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_2000
                    };
                    break;
                case PointItemType.KEEN5_BAG_O_SUGAR:
                    PointValue = 5000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen5_bag_o_sugar1,
                        Properties.Resources.keen5_bag_o_sugar2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_5000
                    };
                    break;

                //TODO: change all code below for keen 6 point items
                case PointItemType.KEEN6_BLOOG_SODA:
                    PointValue = 100;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_bloog_soda1,
                        Properties.Resources.keen6_bloog_soda2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_100
                    };
                    break;
                case PointItemType.KEEN6_ICE_CREAM_BAR:
                    PointValue = 200;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_ice_cream_bar1,
                        Properties.Resources.keen6_ice_cream_bar2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_200
                    };
                    break;
                case PointItemType.KEEN6_PUDDING:
                    PointValue = 500;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_pudding1,
                        Properties.Resources.keen6_pudding2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_500
                    };
                    break;
                case PointItemType.KEEN6_ROOT_BEER_FLOAT:
                    PointValue = 1000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_root_beer_float1,
                        Properties.Resources.keen6_root_beer_float2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_1000
                    };
                    break;
                case PointItemType.KEEN6_BANANA_SPLIT:
                    PointValue = 2000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_banana_split1,
                        Properties.Resources.keen6_banana_split2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_2000
                    };
                    break;
                case PointItemType.KEEN6_PIZZA_SLICE:
                    PointValue = 5000;
                    SpriteList = new Image[]{
                        Properties.Resources.keen6_pizza_slice1,
                        Properties.Resources.keen6_pizza_slice2
                    };
                    AcquiredSpriteList = new Image[]{
                        Properties.Resources.keen_points_5000
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
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.PointItemType.ToString()}";
        }
    }
}
