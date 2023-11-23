using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Hazards
{

    public class Keen6Drill : Hazard, IUpdatable
    {
        private const int SPRITE_CHANGE_DELAY = 2;
        private int _currentSpriteChangeDelayTick = 0;
        private int _currentSprite = 0;
        private Image[] _sprites = SpriteSheet.Keen6DrillSprites;
        public Keen6Drill(SpaceHashGrid grid, Rectangle hitbox) 
            : base(grid, hitbox, HazardType.KEEN6_DRILL)
        {
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(
                  ref _currentSpriteChangeDelayTick
                , ref _currentSprite
                , SPRITE_CHANGE_DELAY
                , UpdateSprite);
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }
    }
}
