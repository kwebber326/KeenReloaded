using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class FloorToPlatformTile : CollisionObject, ISprite, IBiomeTile
    {
        private BiomeType _biome;
        private System.Windows.Forms.PictureBox _sprite;
        private DebugTile _floorTile;
        private PlatformTile _platformTile;

        private const int VERTICAL_OFFSET = 16, EDGE_HORIZONTAL_OFFSET_LEFT = 0, EDGE_HORIZONTAL_OFFSET_RIGHT = 0, FLOOR_WIDTH = 12;

        public FloorToPlatformTile(SpaceHashGrid grid, Rectangle hitbox, BiomeType biome)
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

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;

            switch (_biome)
            {
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_floor_to_platform_left;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_floor_to_platform_left;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_floor_to_platform_left;
                    break;
                default:
                    throw new ArgumentException("Wall-to-platform tile type does not currently have an image for that biome tile");
            }

            _floorTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.X + EDGE_HORIZONTAL_OFFSET_LEFT, this.HitBox.Y + VERTICAL_OFFSET, FLOOR_WIDTH, this.HitBox.Height - VERTICAL_OFFSET));
            _platformTile = new PlatformTile(_collisionGrid, new Rectangle(_floorTile.HitBox.Right + 1, this.HitBox.Y + VERTICAL_OFFSET, this.HitBox.Width - FLOOR_WIDTH - EDGE_HORIZONTAL_OFFSET_LEFT - EDGE_HORIZONTAL_OFFSET_RIGHT, this.HitBox.Height - VERTICAL_OFFSET));
        }

        protected override void HandleCollision(CollisionObject obj)
        {
         
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            Initialize();
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public BiomeType Biome => _biome;

        public Point SpriteLocation =>  this.Sprite == null ? default(Point) : this.Sprite.Location;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.Sprite.Width}|{this.Sprite.Height}|{this.Biome}";
        }
    }
}
