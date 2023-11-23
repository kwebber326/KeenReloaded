using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Assets
{
    public class Door :CollisionObject, ISprite
    {
        protected DoorType _doorType;
        protected System.Windows.Forms.PictureBox _sprite;
        protected const int LEFT_HITBOX_OFFSET = 32;
        protected const int RIGHT_HITBOX_OFFSET = 32;
        protected Door _destinationDoor;
        public Door(SpaceHashGrid grid, Rectangle hitbox, DoorType type, int doorId, Door destinationDoor = null)
            : base(grid, hitbox)
        {
            this.Id = doorId;
            Initialize(type, destinationDoor);
        }

        protected virtual void Initialize(DoorType type, Door destination)
        {
            _destinationDoor = destination;  
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _doorType = type;
            switch (_doorType)
            {
                case DoorType.KEEN4_BLUE:
                    _sprite.Image = Properties.Resources.keen4_blue_door;
                    break;
                case DoorType.KEEN4_GRAY:
                    _sprite.Image = Properties.Resources.keen4_gray_door;
                    break;
                case DoorType.KEEN4_ORACLE:
                    _sprite.Image = Properties.Resources.keen4_oracle_door1;
                    break;
                case DoorType.KEEN5_REGULAR:
                    _sprite.Image = Properties.Resources.keen5_door;
                    break;
                case DoorType.KEEN5_EXIT:
                    _sprite.Image = Properties.Resources.keen5_exit_door_closed;
                    break;
                case DoorType.KEEN6:
                    _sprite.Image = Properties.Resources.keen6_door;
                    break;
                case DoorType.CHUTE:
                    _sprite.Image = Properties.Resources.chute;
                    _destinationDoor = null;
                    break;

            }
            _sprite.Location = this.HitBox.Location;
        }

       

        public long Id { get; private set; }

        public Door DestinationDoor
        {
            get
            {
                return _destinationDoor;
            }
            set
            {
                _destinationDoor = value;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public override string ToString()
        {
            var destinationDoorString = this.DestinationDoor != null ? this.DestinationDoor.Id.ToString() : string.Empty;
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this._doorType.ToString()}|{this.Id.ToString()}|{destinationDoorString}";
        }
    }
}
