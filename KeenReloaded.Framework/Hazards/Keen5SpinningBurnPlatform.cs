using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen5SpinningBurnPlatform : Hazard, IUpdatable, ICreateRemove
    {
        private Image[] _sprites = SpriteSheet.SpinningBurnPlatformImages;
        private bool _hasBurner;
        private int _currentIndex;
        private const int UPDATE_DELAY = 2;
        private int _currentUpdateDelayTick;
        private Keen5PlatformBurner _burner;


        public Keen5SpinningBurnPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, bool hasBurner, int startIndex = 6)
            : base(grid, hitbox, Enums.HazardType.KEEN5_SPINNING_BURN_PLATFORM)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("Keen was not properly set");
            }
            _keen = keen;

            _hasBurner = hasBurner;
            if (!_hasBurner)
            {
                _currentIndex = 7;
            }
            else
            {
                _currentIndex = startIndex;
                _burner = new Keen5PlatformBurner(_collisionGrid, new Rectangle(hitbox.Location, hitbox.Size));
                _burner.SpinSequence = _currentIndex;
                OnCreate(new ObjectEventArgs() { ObjectSprite = _burner });
            }
            this.Tile = new PlatformTile(grid, new Rectangle(hitbox.Location, hitbox.Size));
        }

        public PlatformTile Tile
        {
            get;
            private set;
        }

        public int CurrentIndex
        {
            get
            {
                return _currentIndex;
            }
        }

        public override bool IsDeadly
        {
            get
            {
                return false;
            }
        }

        public void Update()
        {
            if (_hasBurner && _currentUpdateDelayTick++ == UPDATE_DELAY)
            {
                UpdateSprite();
                UpdateDeadlyBurnerPosition();
            }
        }

        private void UpdateDeadlyBurnerPosition()
        {
            int nextSpriteHeight = _sprites[_currentIndex].Height;
            int referenceHeight = _sprites[6].Height;
            int yOffset = nextSpriteHeight < referenceHeight ? 0 : nextSpriteHeight - referenceHeight;
            if (_currentIndex >= 3 && _currentIndex <= 5)
            {
                Rectangle newHitbox = new Rectangle(new Point(_burner.HitBox.X, Tile.HitBox.Bottom), new Size(Tile.HitBox.Width, Math.Abs(yOffset)));
                _burner.UpdateLocation(newHitbox);
            }
            else
            {
                Rectangle newHitbox = new Rectangle(new Point(_burner.HitBox.X, Tile.HitBox.Y - Math.Abs(yOffset)), new Size(Tile.HitBox.Width, Math.Abs(yOffset)));
                _burner.UpdateLocation(newHitbox);
            }
            _burner.SpinSequence = _currentIndex;
            if (_keen.HitBox.IntersectsWith(_burner.HitBox))
            {
                _keen.Die();
            }
        }

        private void UpdateSprite()
        {
            _currentUpdateDelayTick = 0;
            int nextSpriteIndex = _currentIndex + 1;
            if (nextSpriteIndex >= _sprites.Length)
            {
                nextSpriteIndex = 0;
            }

            int nextSpriteHeight = _sprites[nextSpriteIndex].Height;
            int referenceHeight = _sprites[6].Height;
            int yOffset = nextSpriteHeight < referenceHeight ? 0 : nextSpriteHeight - referenceHeight;
            if (nextSpriteIndex < 3 || nextSpriteIndex == 7)
            {
                _sprite.Location = new Point(_sprite.Location.X, Tile.HitBox.Y - yOffset);
            }
            _sprite.Image = _sprites[nextSpriteIndex];
            _currentIndex = nextSpriteIndex;
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private CommanderKeen _keen;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (this.Remove != null)
            {
                if (args.ObjectSprite == this)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }

        public override string ToString()
        {
            if (!_hasBurner)
            {
                return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_hasBurner}|{_currentIndex}";
            }
            else
            {
                return $"{this.GetType().Name}|{this.Tile.HitBox.X}|{this.Tile.HitBox.Y}|{_hasBurner}|{_currentIndex}";
            }
        }
    }
}
