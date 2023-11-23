using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System.Diagnostics;

namespace KeenReloaded.Framework.Tiles
{
    public class SingleMaskedPlatformTile : PlatformTile, IBiomeTile
    {
        private BiomeType _biome;
        private const int VERTICAL_OFFSET = 16, EDGE_HORIZONTAL_OFFSET_LEFT = 8, EDGE_HORIZONTAL_OFFSET_RIGHT = 8;
        public SingleMaskedPlatformTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome)
            : base(grid, hitbox)
        {
            _biome = biome;
            Initialize();
        }

        public override Rectangle HitBox
        {
            get
            {

                return base.HitBox;
            }
            protected set
            {
                base.HitBox = value;
                if (_collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }

        protected virtual void Initialize()
        {
            SetSprite();

            this.HitBox = new Rectangle(
                this.HitBox.X + EDGE_HORIZONTAL_OFFSET_LEFT //x
              , this.HitBox.Y + VERTICAL_OFFSET   //y
              , this.HitBox.Width - EDGE_HORIZONTAL_OFFSET_LEFT - EDGE_HORIZONTAL_OFFSET_RIGHT //width
              , this.HitBox.Height - VERTICAL_OFFSET); //height

            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Sprite.BackColor = Color.Transparent;
        }

        protected virtual void SetSprite()
        {
            switch (_biome)
            {
                case BiomeType.KEEN4_MIRAGE:
                    this.Sprite.Image = Properties.Resources.keen4_mirage_platform_single;
                    break;
                case BiomeType.KEEN4_GREEN:
                    this.Sprite.Image = Properties.Resources.keen4_forest_platform_single;
                    break;
                case BiomeType.KEEN5_RED:
                    this.Sprite.Image = Properties.Resources.keen5_single_platform_red;
                    break;
                case BiomeType.KEEN5_BLACK:
                    this.Sprite.Image = Properties.Resources.keen5_single_platform_blue;
                    break;
                case BiomeType.KEEN6_DOME:
                    this.Sprite.Image = Properties.Resources.keen6_dome_platform_single;
                    break;
                case BiomeType.KEEN6_FINAL:
                    this.Sprite.Image = Properties.Resources.keen6_eyeball_platform;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    this.Sprite.Image = Properties.Resources.keen6_industrial_single_masked_platform;
                    break;
                default:
                    string errorMessage = string.Format("Currently there is no single masked platform tile for biome {0}", _biome.ToString());
                    Debug.WriteLine(errorMessage);
                    break;
            }
        }


        public BiomeType Biome
        {
            get { return _biome; }
        }

        public virtual void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            SetSprite();
        }

        public Point SpriteLocation
        {
            get { return this.Sprite == null ? default(Point) : this.Sprite.Location; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Sprite.Width}|{this.Sprite.Height}|{this.Biome}"; ;
        }
    }
}
