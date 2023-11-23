using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class WallToPlatformTile : CollisionObject, ISprite, IBiomeTile
    {
        private System.Windows.Forms.PictureBox _sprite;
        private BiomeType _biome;
        private Direction _direction;
        private DebugTile _floorTile;
        private PlatformTile _platformTile;

        private int VERTICAL_OFFSET = 32, EDGE_HORIZONTAL_OFFSET_LEFT = 0, EDGE_HORIZONTAL_OFFSET_RIGHT = 0, WALL_WIDTH_RIGHT = 6, WALL_WIDTH_LEFT = 4, WALL_HEIGHT = 64;

        public WallToPlatformTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome, Direction direction)
            : base(grid, hitbox)
        {
            if (direction != Direction.LEFT && direction != Direction.RIGHT)
                throw new ArgumentException("Direction must be left or right for wall-to-platform tile type");

            _direction = direction;
            _biome = biome;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            switch (_biome)
            {
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = _direction == Direction.LEFT 
                        ? Properties.Resources.keen4_mirage_wall_to_platform_left 
                        : Properties.Resources.keen4_mirage_wall_to_platform_right;
                    break;
                case BiomeType.KEEN5_RED:
                    _direction = Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen5_red_wall_to_platform_left;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _direction = Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen5_black_wall_to_platform_left;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _direction = Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen5_green_wall_to_platform_left;
                    VERTICAL_OFFSET = 36;
                    break;
                case BiomeType.KEEN6_DOME:
                    _direction = Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen6_dome_wall_to_platform;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _direction = Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen6_industrial_wall_to_platform_left;
                    break;
                default:
                    throw new ArgumentException("Wall-to-platform tile type does not currently have an image for that biome tile");
            }

            if (_direction == Direction.LEFT)
            {
                _platformTile = new PlatformTile(_collisionGrid, new Rectangle(this.HitBox.X + WALL_WIDTH_LEFT + EDGE_HORIZONTAL_OFFSET_LEFT, this.HitBox.Y + VERTICAL_OFFSET, this.HitBox.Width - WALL_WIDTH_LEFT - EDGE_HORIZONTAL_OFFSET_LEFT, this.HitBox.Height - VERTICAL_OFFSET));
                _floorTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Y, WALL_WIDTH_LEFT, this.HitBox.Height));
            }
            else
            {
                _floorTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.Right - WALL_WIDTH_RIGHT + EDGE_HORIZONTAL_OFFSET_RIGHT, this.HitBox.Y, WALL_WIDTH_RIGHT - EDGE_HORIZONTAL_OFFSET_RIGHT, this.HitBox.Height));
                _platformTile = new PlatformTile(_collisionGrid, new Rectangle(this.HitBox.X + EDGE_HORIZONTAL_OFFSET_RIGHT, this.HitBox.Y + VERTICAL_OFFSET, this.HitBox.Width - WALL_WIDTH_RIGHT - EDGE_HORIZONTAL_OFFSET_RIGHT, this.HitBox.Height - VERTICAL_OFFSET));
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
       
        }

        public BiomeType Biome
        {
            get { return _biome; }
        }

        public Direction Direction
        {
            get
            {
                return _direction;
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
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Sprite.Width}|{this.Sprite.Height}|{this.Biome}|{this.Direction}";
        }
    }
}
