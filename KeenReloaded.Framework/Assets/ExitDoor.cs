﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Assets
{
    public class ExitDoor : Door, IUpdatable
    {
        private const int OPEN_DOOR_SPRITE_CHANGE_DELAY = 4;
        private int _currentSprite;
        private int _currentSpriteChangeDelay;
        private Image[] _doorOpenSprites;
        private bool _isOpening;
        private bool _isOpened;
        public ExitDoor(SpaceHashGrid grid, Rectangle hitbox) 
            : base(grid, hitbox, DoorType.KEEN5_EXIT, -1, null)
        {
            _doorOpenSprites = SpriteSheet.Keen5ExitDoorOpenImages;
        }

        public bool IsOpened
        {
            get
            {
                return _isOpened;
            }
        }

        public bool IsOpening
        {
            get
            {
                return _isOpening;
            }
        }

        public void Open()
        {
            _isOpening = true;
        }

        public void Update()
        {
            if (_isOpening && !_isOpened)
            {
                this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelay, ref _currentSprite, OPEN_DOOR_SPRITE_CHANGE_DELAY, UpdateSprite);
            }
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _doorOpenSprites.Length)
            {
                _isOpened = true;
                return;
            }
            _sprite.Image = _doorOpenSprites[_currentSprite];
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
