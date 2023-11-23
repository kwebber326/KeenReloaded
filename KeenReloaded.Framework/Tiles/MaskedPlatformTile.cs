using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Tiles
{
    public class MaskedPlatformTile : PlatformTile, IBiomeTile
    {
        private BiomeType _biome;
        private TileType _floorType;
        private const int VERTICAL_OFFSET = 16, EDGE_HORIZONTAL_OFFSET_LEFT = 8, EDGE_HORIZONTAL_OFFSET_RIGHT = 8;
        public MaskedPlatformTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome, TileType floorType)
            : base(grid, hitbox)
        {
            _biome = biome;
            _floorType = floorType;
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

        private void Initialize()
        {
            SetSprite();
            if (_biome == BiomeType.KEEN5_RED || _biome == BiomeType.KEEN5_GREEN || _biome == BiomeType.KEEN5_BLACK)
            {
                if (_floorType == TileType.FLOOR_LEFT_EDGE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X + EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Location.Y + (VERTICAL_OFFSET / 2), this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Height - (VERTICAL_OFFSET / 2));
                else if (_floorType == TileType.FLOOR_MIDDLE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + (VERTICAL_OFFSET / 2), this.Sprite.Width, this.Sprite.Height - (VERTICAL_OFFSET / 2));
                else if (_floorType == TileType.FLOOR_RIGHT_EDGE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + (VERTICAL_OFFSET / 2), this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_RIGHT, this.Sprite.Height - (VERTICAL_OFFSET / 2));
            }
            else
            {
                if (_floorType == TileType.FLOOR_LEFT_EDGE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X + EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Height - VERTICAL_OFFSET);
                else if (_floorType == TileType.FLOOR_MIDDLE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width, this.Sprite.Height - VERTICAL_OFFSET);
                else if (_floorType == TileType.FLOOR_RIGHT_EDGE)
                    this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_RIGHT, this.Sprite.Height - VERTICAL_OFFSET);
            }

            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Sprite.BackColor = Color.Transparent;
        }

        private void SetSprite()
        {
            switch (_biome)
            {
                case BiomeType.KEEN4_MIRAGE:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_platform_left_edge;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_platform_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_platform_right_edge;
                            break;
                    }
                    break;
                case BiomeType.KEEN4_GREEN:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_platform_left_edge;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_platform_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_platform_right_edge;
                            break;
                    }
                    break;
                case BiomeType.KEEN4_CAVE:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_platform_left_edge;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_platform_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_platform_right_edge;
                            break;
                    }
                    break;
                case BiomeType.KEEN5_GREEN:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_green_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_green_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_green_edge_right;
                            break;
                    }
                    break;
                case BiomeType.KEEN5_BLACK:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_blue_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_blue_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_blue_edge_right;
                            break;
                    }
                    break;
                case BiomeType.KEEN5_RED:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_red_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_red_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_platform_red_edge_right;
                            break;
                    }
                    break;
                case BiomeType.KEEN6_DOME:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_platform_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_platform_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_platform_edge_right;
                            break;
                    }
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    switch (_floorType)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_platform_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_platform_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_platform_right;
                            break;
                    }
                    break;
            }
        }

        public BiomeType Biome
        {
            get { return _biome; }
        }

        public TileType FloorType
        {
            get
            {
                return _floorType;
            }
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            Initialize();
        }

        public Point SpriteLocation
        {
            get { return this.Sprite == null ? default(Point) : this.Sprite.Location; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Sprite.Width}|{this.Sprite.Height}|{this.Biome}|{this.FloorType}";
        }
    }
}
