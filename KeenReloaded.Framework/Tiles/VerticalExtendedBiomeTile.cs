using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Utilities;

namespace KeenReloaded.Framework.Tiles
{
    public class VerticalExtendedBiomeTile : DebugTile, IBiomeTile
    {
       private BiomeType _biome;
        private TileType _type;
        private string[] _files;
        private const int VERTICAL_OFFSET = 32, EDGE_HORIZONTAL_OFFSET_LEFT = 12, EDGE_HORIZONTAL_OFFSET_RIGHT=8, CEILING_VERTICAL_OFFSET = 8;
        private int _lengths;
        public VerticalExtendedBiomeTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome, TileType type, int lengths = 1)
            : base(grid, hitbox)
        {
            if (lengths <= 0)
                throw new ArgumentException("lengths value must be greater than zero");

            _biome = biome;
            _type = type;
            _lengths = lengths;
            Initialize();
        }
        public TileType TileType
        {
            get
            {
                return _type;
            }
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
                    _collidingNodes.Clear();
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }

        private void Initialize()
        {
            SetImageFilesFromBiome();

            if (_type == TileType.FLOOR_LEFT_EDGE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location.X + EDGE_HORIZONTAL_OFFSET_LEFT, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_LEFT, (this.Sprite.Height * _lengths) - VERTICAL_OFFSET);
            }
            else if (_type == TileType.FLOOR_MIDDLE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width, (this.Sprite.Height * _lengths) - VERTICAL_OFFSET);
            }
            else if (_type == TileType.FLOOR_RIGHT_EDGE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_RIGHT, (this.Sprite.Height * _lengths) - VERTICAL_OFFSET);
            }
            else if (_type == TileType.WALL_RIGHT_EDGE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.Sprite.Width - EDGE_HORIZONTAL_OFFSET_RIGHT, (this.Sprite.Height * _lengths) - VERTICAL_OFFSET);
            }
            else if (_type == TileType.WALL_LEFT_EDGE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location, new Size(this.Sprite.Width, this.Sprite.Height * _lengths));
            }
            else if (_type == TileType.WALL_MIDDLE)
            {
                this.HitBox = new Rectangle(this.Sprite.Location, new Size(this.Sprite.Width, this.Sprite.Height * _lengths));
            }
            else if (_type == TileType.CEILING)
            {
                this.HitBox = new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y, this.Sprite.Width, (this.Sprite.Height * _lengths) - CEILING_VERTICAL_OFFSET);
            }

            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Sprite.BackColor = Color.Transparent;
            this.Sprite.Image = GetImageFromFiles();
            this.Sprite.SizeMode = PictureBoxSizeMode.AutoSize;

            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Direction.UP_RIGHT);

            foreach (var node in _collidingNodes)
            {
                node.Tiles.Add(this);
                node.NonEnemies.Add(this);
            }
        }

        private void SetImageFilesFromBiome()
        {
            string file = FileIOUtilities.GetResourcesPath();
            switch (_biome)
            {
                case BiomeType.KEEN4_GREEN:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen4_forest_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen4_forest_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen4_forest_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen4_forest_wall_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen4_forest_wall_right_edge.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen4_forest_wall_left_edge.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                 file + nameof(Properties.Resources.keen4_forest_wall_bottom1) + ".png",
                                 file + nameof(Properties.Resources.keen4_forest_wall_bottom2) + ".png"
                            };
                            break;
                    }
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen4_mirage_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen4_mirage_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen4_mirage_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen4_mirage_wall_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen4_mirage_wall_right_edge.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen4_mirage_wall_left_edge.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                file + nameof(Properties.Resources.keen4_mirage_wall_bottom1) + ".png",
                                file + nameof(Properties.Resources.keen4_mirage_wall_bottom2) + ".png",
                                file + nameof(Properties.Resources.keen4_mirage_wall_bottom3) + ".png",
                                file + nameof(Properties.Resources.keen4_mirage_wall_bottom4) + ".png"
                            };
                            break;
                    }
                    break;
                case BiomeType.KEEN4_PYRAMID:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen4_pyramid_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen4_pyramid_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen4_pyramid_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen4_pyramid_wall_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen4_pyramid_wall_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen4_pyramid_wall_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                 file + nameof(Properties.Resources.keen4_pyramid_wall_bottom1) + ".png",
                                 file + nameof(Properties.Resources.keen4_pyramid_wall_bottom2) + ".png"
                            };
                            break;
                    }
                    break;
                case BiomeType.KEEN4_CAVE:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen4_cave_air_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen4_cave_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen4_cave_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen4_cave_wall_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen4_cave_wall_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen4_cave_wall_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom1) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom2) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom3) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom4) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom5) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom6) + ".png",
                                file + nameof(Properties.Resources.keen4_cave_wall_bottom7) + ".png"
                            };
                            break;
                    }
                    break;

                case BiomeType.KEEN5_BLACK:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen5_floor_black_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen5_floor_black_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen5_floor_black_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen5_wall_black_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen5_wall_black_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen5_wall_black_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[] { file + nameof(Properties.Resources.keen5_wall_black_bottom) + ".png" };
                            break;
                    }
                    break;
                case BiomeType.KEEN5_GREEN:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen5_floor_green_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen5_floor_green_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen5_floor_green_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen5_wall_green_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen5_wall_green_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen5_wall_green_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[] { file + nameof(Properties.Resources.keen5_ceiling_green) + ".png" };
                            break;
                    }
                    break;
                case BiomeType.KEEN5_RED:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen5_floor_red_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen5_floor_red_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen5_floor_red_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen5_wall_red_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen5_wall_red_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen5_wall_red_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[] { file + nameof(Properties.Resources.keen5_ceiling_red) + ".png" };
                            break;
                    }
                    break;
                case BiomeType.KEEN6_FOREST:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen6_forest_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen6_forest_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen6_forest_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen6_forest_wall_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen6_forest_wall_middle1.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen6_forest_wall_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                  file + nameof(Properties.Resources.keen6_forest_ceiling1) + ".png",
                                  file + nameof(Properties.Resources.keen6_forest_ceiling2) + ".png"
                            };
                            break;
                    }
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen6_industrial_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen6_industrial_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen6_industrial_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen6_industrial_wall_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen6_industrial_wall_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen6_industrial_wall_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[] { file + nameof(Properties.Resources.keen6_industrial_ceiling) + ".png" };
                            break;
                    }
                    break;
                case BiomeType.KEEN6_DOME:
                    switch (_type)
                    {
                        case TileType.FLOOR_LEFT_EDGE:
                            file += "keen6_dome_floor_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_MIDDLE:
                            file += "keen6_dome_floor_middle.png";
                            _files = new string[] { file };
                            break;
                        case TileType.FLOOR_RIGHT_EDGE:
                            file += "keen6_dome_floor_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_RIGHT_EDGE:
                            file += "keen6_dome_wall_edge_right.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_MIDDLE:
                            file += "keen6_dome_wall_middle1.png";
                            _files = new string[] { file };
                            break;
                        case TileType.WALL_LEFT_EDGE:
                            file += "keen6_dome_wall_edge_left.png";
                            _files = new string[] { file };
                            break;
                        case TileType.CEILING:
                            _files = new string[]
                            {
                                file + nameof(Properties.Resources.keen6_dome_ceiling2) + ".png",
                                file + nameof(Properties.Resources.keen6_dome_ceiling3) + ".png"
                            };
                            break;
                    }
                    break;
            }
        }

        private Image GetImageFromFiles()
        {
            if (_files == null || !_files.Any())
                return null;

            var fileList = new string[_lengths];
            int imgIndex = 0;
            for (int i = 0; i < fileList.Length; i++)
            {
                fileList[i] = _files[imgIndex];
                if (++imgIndex >= _files.Length)
                    imgIndex = 0;
            }
            var image = BitMapTool.CombineBitmap(fileList, 1, Color.Transparent);
            return image;
        }

        public BiomeType Biome
        {
            get { return _biome; }
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            this.RedrawSprites();
        }

        private void RedrawSprites()
        {
            SetImageFilesFromBiome();
            this.Sprite.Image = GetImageFromFiles();
        }

        public Point SpriteLocation
        {
            get { return this.Sprite == null ? default(Point) : this.Sprite.Location; }
        }
    }
}
