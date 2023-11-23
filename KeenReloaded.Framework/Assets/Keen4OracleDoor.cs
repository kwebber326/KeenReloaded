using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Assets
{
    public class Keen4OracleDoor : Door, IUpdatable
    {
     
        private const int SPRITE_CHANGE_DELAY = 12;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private Image[] _sprites;
        public Keen4OracleDoor(SpaceHashGrid grid, Rectangle hitbox, int doorId, Door destinationDoor = null)
            : base(grid, hitbox, DoorType.KEEN4_ORACLE, doorId, destinationDoor)
        {
        }

        protected override void Initialize(DoorType type, Door destination)
        {
            base.Initialize(type, destination);
            this.HitBox = new Rectangle(_sprite.Location.X + LEFT_HITBOX_OFFSET, _sprite.Location.Y, _sprite.Width - LEFT_HITBOX_OFFSET - RIGHT_HITBOX_OFFSET, _sprite.Height);
            _sprites = SpriteSheet.Keen4OracleDoorImages;
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }

        public override Rectangle HitBox
        {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (value != null && _sprite != null)
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }
    }
}
