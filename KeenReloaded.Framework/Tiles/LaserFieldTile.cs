using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class LaserFieldTile : DebugTile
    {
        private const int Y_BUFFER_UP = 10;
        private Direction _direction;
        private LaserFieldTileType _type;
        public LaserFieldTile(SpaceHashGrid grid, Rectangle hitbox, Direction direction, LaserFieldTileType type)
            : base(grid, hitbox)
        {
            _type = type;
            _direction = direction;
            Initialize();
        }

        private void Initialize()
        {
            if (_type == LaserFieldTileType.KEEN5)
            {
                this.Sprite.Image = _direction == Direction.UP
                    ? Properties.Resources.keen5_laser_field_up
                    : Properties.Resources.keen5_laser_field_down;
                this.Sprite.Location = this.HitBox.Location;
                if (_direction == Direction.UP)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + Y_BUFFER_UP, this.HitBox.Width, this.HitBox.Height - Y_BUFFER_UP);
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
            else if (_type == LaserFieldTileType.KEEN6)
            {
                this.Sprite.Image = _direction == Direction.UP
                    ? Properties.Resources.keen6_laser_field_bottom
                    : Properties.Resources.keen6_laser_field_top;
                this.Sprite.Location = this.HitBox.Location;
                if (_direction == Direction.UP)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + Y_BUFFER_UP, this.HitBox.Width, this.HitBox.Height - Y_BUFFER_UP);
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }

            this.Sprite.BackColor = Color.Transparent;
            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
    }

    public enum LaserFieldTileType
    {
        KEEN5,
        KEEN6
    }
}
