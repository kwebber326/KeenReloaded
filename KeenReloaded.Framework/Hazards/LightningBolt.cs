using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Hazards
{
    public class LightningBolt : Hazard, IUpdatable
    {
        public LightningBolt(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox, HazardType.KEEN4_LIGHTNING_BOLT)
        {
            OnCreated();
        }

        private const int SPRITE_SWITCHES = 10;
        private int _currentSpriteSwitchCount = 0;

        public event EventHandler<ObjectEventArgs> Removed;
        public event EventHandler<ObjectEventArgs> Created;

        private Image[] _images = new Image[] 
        {
             Properties.Resources.keen4_lightning_bolt1,
             Properties.Resources.keen4_lightning_bolt2
        };

        protected void OnRemoved()
        {
            if (Removed != null)
            {
                if (_collidingNodes != null)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }

                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };

                Removed(this, args);
            }
        }

        protected void OnCreated()
        {
            if (Created != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                Created(this, args);
            }
        }

        public void Update()
        {
            if (_currentSpriteSwitchCount < SPRITE_SWITCHES)
            {
                this.Sprite.Image = _images[++_currentSpriteSwitchCount % 2];
                var collisionItems = this.CheckCollision(this.HitBox);
                var keens = collisionItems.OfType<CommanderKeen>();
                foreach (var keen in keens)
                {
                    keen.Die();
                }
            }
            else
            {
                OnRemoved();
            }
        }
    }
}
