using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;

namespace KeenReloaded.Framework.Hazards
{
    public class Smasher : Hazard, IUpdatable
    {
        private const int OFF_TIME = 25;
        private int _offTimeTick;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private CommanderKeen _keen;
        private SmasherState _state;
        private bool _firstTime = true;
        private Dictionary<SmasherState, Image> _stateSprites;

        public Smasher(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, SmasherState initialState)
            : base(grid, hitbox, Enums.HazardType.KEEN6_SMASHER)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("Keen was not properly set");
            }
            _state = initialState;
            _keen = keen;
            Initialize();
        }

        public override Rectangle HitBox
        {
            get
            {
                return base.HitBox;
            }
            protected set
            {
                base.HitBox = value;
                if (_collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public override bool IsDeadly
        {
            get
            {
                return _state != SmasherState.OFF;
            }
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _stateSprites = new Dictionary<SmasherState, Image>();
            _stateSprites.Add(SmasherState.OFF, Properties.Resources.keen6_smasher_off);
            _stateSprites.Add(SmasherState.PHASE1, Properties.Resources.keen6_smasher_1);
            _stateSprites.Add(SmasherState.PHASE2, Properties.Resources.keen6_smasher_2);
            _stateSprites.Add(SmasherState.PHASE3, Properties.Resources.keen6_smasher_3);
            _stateSprites.Add(SmasherState.PHASE4, Properties.Resources.keen6_smasher_2);
            _stateSprites.Add(SmasherState.PHASE5, Properties.Resources.keen6_smasher_1);

            this.UpdateSprite();
        }

        public void Update()
        {
            switch (_state)
            {
                case SmasherState.OFF:
                    this.UpdateOffState();
                    break;
                default:
                    this.UpdateOnState();
                    break;
            }
        }

        private void UpdateOffState()
        {
            if (_firstTime || this.State != SmasherState.OFF)
            {
                this.State = SmasherState.OFF;
                _firstTime = false;
                _offTimeTick = 0;
            }

            if (_offTimeTick++ == OFF_TIME)
            {
                this.UpdateOnState();
            }
        }

        private void UpdateOnState()
        {
            if (_firstTime || this.State == SmasherState.OFF)
            {
                this.State = _firstTime ? _state : SmasherState.PHASE1;
                _firstTime = false;
            }
            if (this.CollidesWith(_keen))
                _keen.Die();

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                int stateVal = (int)_state;
                if (stateVal < 5)
                {
                    stateVal++;
                    _state = (SmasherState)stateVal;
                    UpdateSprite();
                }
                else
                {
                    this.UpdateOffState();
                }
            }
        }

        SmasherState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            _sprite.Image = _stateSprites[_state];
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, _sprite.Height);
        }
    }

    public enum SmasherState
    {
        OFF = 0,
        PHASE1,
        PHASE2,
        PHASE3,
        PHASE4,
        PHASE5
    }
}
