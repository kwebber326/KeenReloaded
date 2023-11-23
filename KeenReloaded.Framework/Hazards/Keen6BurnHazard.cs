using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen6BurnHazard : Hazard, IUpdatable
    {
        private const int SPRITE_CHANGE_DELAY = 0;
        private int _currentSpriteChangeDelayTick = 0;
        private int _currentSprite = 0;

        public Keen6BurnHazard(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox, Enums.HazardType.KEEN6_BURN_HAZARD)
        {

        }
        public void Update()
        {
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            var spriteSet = SpriteSheet.Keen6BurnHazardImages;
            if (++_currentSprite >= spriteSet.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = spriteSet[_currentSprite];
        }
    }
}
