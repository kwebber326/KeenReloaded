using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class RocketPropelledPlatform : Hazard, IUpdatable
    {
        private Image[] _sprites = SpriteSheet.RocketPropelledPlatformImages;
        private const int SPRITE_CHANGE_DELAY = 0;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;

        private const int VERTICAL_OFFSET_FOR_DEATH_COLLISION = 6;
        private const int HORIZONTAL_OFFSET_FOR_DEATH_COLLISION = 8;
        private Rectangle _deathHitBox;
        private CommanderKeen _keen;

        public RocketPropelledPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox, Enums.HazardType.KEEN4_ROCKET_PROPELLED_PLATFORM)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        public override bool IsDeadly
        {
            get
            {
                return _deathHitBox.IntersectsWith(_keen.HitBox);
            }
        }

        DebugTile LandingTile { get; set; }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            UpdateSprite();
            _deathHitBox = new Rectangle(this.HitBox.X + VERTICAL_OFFSET_FOR_DEATH_COLLISION, this.HitBox.Y + VERTICAL_OFFSET_FOR_DEATH_COLLISION,
                this.HitBox.Width - (HORIZONTAL_OFFSET_FOR_DEATH_COLLISION * 2), this.HitBox.Height - VERTICAL_OFFSET_FOR_DEATH_COLLISION);
            LandingTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, VERTICAL_OFFSET_FOR_DEATH_COLLISION));
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        public void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }
    }
}
