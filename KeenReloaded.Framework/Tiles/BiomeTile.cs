using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Tiles
{
    public class BiomeTile : DebugTile, IBiomeTile
    {
        private BiomeType _biome;
        private TileType _type;
        private const int VERTICAL_OFFSET = 32, EDGE_HORIZONTAL_OFFSET_LEFT = 12, EDGE_HORIZONTAL_OFFSET_RIGHT=8, CEILING_VERTICAL_OFFSET = 8;
        public BiomeTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome, TileType type)
            : base(grid, hitbox)
        {
            _biome = biome;
            _type = type;
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

        public TileType FloorType
        {
            get
            {
                return _type;
            }
        }
        public BiomeType Biome
        {
            get
            {
                return _biome;
            }
        }

        private void Initialize()
        {
            SetSpriteFromBiome();
            if (_type == TileType.FLOOR_LEFT_EDGE)
                this.HitBox = new Rectangle(this.Sprite.Location.X + EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Height - VERTICAL_OFFSET);
            else if (_type == TileType.FLOOR_MIDDLE)
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width, this.Sprite.Height - VERTICAL_OFFSET);
            else if (_type == TileType.FLOOR_RIGHT_EDGE)
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_RIGHT, this.Sprite.Height - VERTICAL_OFFSET);
            else if (_type == TileType.WALL_RIGHT_EDGE)
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y , 24, this.Sprite.Height);
            else if (_type == TileType.WALL_MIDDLE || _type == TileType.WALL_LEFT_EDGE)
                this.HitBox = new Rectangle(this.Sprite.Location, this.Sprite.Size);
            else if (_type == TileType.CEILING)
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y, this.Sprite.Width, this.Sprite.Height - CEILING_VERTICAL_OFFSET);

            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Sprite.BackColor = Color.Transparent;
            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Direction.UP_RIGHT);
        }

        private void SetSpriteFromBiome()
        {
            switch (_biome)
            {
                case BiomeType.KEEN4_GREEN:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_floor_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_wall_middle;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_wall_right_edge;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_forest_wall_left_edge;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen4ForestTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_floor_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_wall_middle;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_wall_right_edge;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_mirage_wall_left_edge;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen4MirageTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN4_PYRAMID:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_wall_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_wall_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_pyramid_wall_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen4PyramidTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN4_CAVE:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_air_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_wall_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_wall_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen4_cave_wall_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen4CaveTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN5_BLACK:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_black_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_black_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_black_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_black_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_black_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_black_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen5BlackTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN5_GREEN:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_green_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_green_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_green_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_green_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_green_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_green_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen5GreenTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN5_RED:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_red_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_red_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_floor_red_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_red_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_red_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen5_wall_red_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen5RedTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN6_FOREST:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_wall_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_wall_middle1;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_forest_wall_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen6ForestTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_wall_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_wall_middle;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_industrial_wall_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen6IndustrialTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
                case BiomeType.KEEN6_DOME:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_floor_edge_left;
                            break;
                        case TileType.FLOOR_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_floor_middle;
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_floor_edge_right;
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_wall_edge_right;
                            break;
                        case TileType.WALL_MIDDLE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_wall_middle1;
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            this.Sprite.Image = Properties.Resources.keen6_dome_wall_edge_left;
                            break;
                        case TileType.CEILING:
                            var spriteSheet = SpriteSheet.Keen6DomeTilesBottom;
                            int randVal = _random.Next(0, spriteSheet.Length);
                            this.Sprite.Image = spriteSheet[randVal];
                            break;
                    }
                    break;
            }
        }


        public void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            RedrawSprites();
        }

        private void RedrawSprites()
        {
            SetSpriteFromBiome();
           
        }

        public Point SpriteLocation
        {
            get { return this.Sprite == null ? default(Point) : this.Sprite.Location; }
        }

        public override string ToString()
        {
            return $"{typeof(BiomeTile).Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Sprite.Width}|{this.Sprite.Height}|{this.Biome}|{this.FloorType}";
        }
    }
}
