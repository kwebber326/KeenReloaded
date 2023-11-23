using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Hazards
{
    public class PoisonSlugPoop : Hazard, IUpdatable, ISprite, ICreateRemove
    {
        private SlugPoopState _state;
        private const int ACTIVE_STATE_REMOVE_DELAY = 80;
        private int _currentActiveStateDelayTick = 0;

        private const int REMOVE_DELAY = 40;
        private const int FALL_VELOCITY = 40;
        private int _currentRemoveDelay;

        public static int POOP_HEIGHT
        {
            get
            {
                return 14;
            }
        }
        public static int POOP_WIDTH
        {
            get
            {
                return 30;
            }
        }

        public PoisonSlugPoop(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox, Enums.HazardType.KEEN4_SLUG_POOP)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.State = SlugPoopState.ACTIVE;
        }

        public void Update()
        {
            this.BasicFall(FALL_VELOCITY);
            if (this.State == SlugPoopState.ACTIVE)
            {
                if (_currentActiveStateDelayTick++ == ACTIVE_STATE_REMOVE_DELAY)
                {
                    _currentActiveStateDelayTick = 0;
                    this.State = SlugPoopState.FADING;
                }
            }
            else
            {
                if (_currentRemoveDelay++ == REMOVE_DELAY)
                {
                    _currentRemoveDelay = 0;
                    _sprite.Image = null;
                    OnRemove();
                }
            }
        }

        private void UpdateSprite()
        {
            switch (this.State)
            {
                case SlugPoopState.ACTIVE:
                    _sprite.Image = Properties.Resources.keen4_slug_poop_active;
                    break;
                case SlugPoopState.FADING:
                    _sprite.Image = Properties.Resources.keen4_slug_poop_fading;
                    break;
            }
        }

        internal SlugPoopState State
        {
            get
            {
                return _state;
            }
            private set
            {
                _state = value;
                UpdateSprite();
            }
        }

        public override bool IsDeadly
        {
            get
            {
                return this.State == SlugPoopState.ACTIVE;
            }
        }

        public override Rectangle HitBox
        {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (_sprite != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        System.Windows.Forms.PictureBox ISprite.Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnRemove()
        {
            if (Remove != null)
            {
                if (_collidingNodes != null && _collidingNodes.Any())
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
                Remove(this, args);
            }
        }

        protected void OnCreate()
        {
            if (Create != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                Create(this, args);
            }
        }
    }

    enum SlugPoopState
    {
        ACTIVE,
        FADING
    }
}
