using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class Keen5LaserFieldTile : DebugTile
    {
        private const int Y_BUFFER_UP = 10;
        private Direction _direction;
        public Keen5LaserFieldTile(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox)
        {
            _direction = direction;
            Initialize();
        }

        private void Initialize()
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

            this.Sprite.BackColor = Color.Transparent;
            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
        }
    }
}
