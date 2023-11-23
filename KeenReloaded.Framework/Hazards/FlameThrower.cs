using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class FlameThrower : Hazard, IUpdatable
    {
        public Image[] _sprites;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentOnSprite;
        private CommanderKeen _keen;
        private FlameThrowerState _state;
        private int _originalBottom, _originalLeft, _originalWidth;

        private const int OFF_TIME = 25;
        private int _offTimeTick;
        private bool _firstTime = true;

        private const int SPRITE_WIDTH = 26;
        private const int STANDING_TILE_WIDTH=36, STANDING_TILE_HEIGHT = 32;

        public FlameThrower(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, FlameThrowerState initialState)
            : base(grid, hitbox, Enums.HazardType.KEEN6_FLAME_THROWER)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _originalBottom = this.HitBox.Bottom;
            _originalLeft = this.HitBox.Left;
            _originalWidth = this.HitBox.Width;
            Initialize(initialState);
          
        }

        private void Initialize(FlameThrowerState initialState)
        {
            _sprites = SpriteSheet.Keen6FlameThrowerBurnImages;

            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            StandingTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.X - ((STANDING_TILE_WIDTH - SPRITE_WIDTH) /2), this.HitBox.Bottom - STANDING_TILE_HEIGHT, STANDING_TILE_WIDTH, STANDING_TILE_HEIGHT));
            this.State = initialState;
        }

        public override bool IsDeadly
        {
            get
            {
                return _state != FlameThrowerState.OFF;
            }
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
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        DebugTile StandingTile
        {
            get;
            set;
        }

        FlameThrowerState State
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
            _sprite.Image = _sprites[(int)_state];
            UpdatePosition();
        }

        public void Update()
        {
            if (_state == FlameThrowerState.OFF)
            {
                this.UpdateOffState();
            }
            else
            {
                this.UpdateOnState();
            }
        }

        private void UpdateOnState()
        {
            switch (_state)
            {
                case FlameThrowerState.ON_PHASE1:
                    if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                    {
                        _currentSpriteChangeDelayTick = 0;
                        this.State = FlameThrowerState.ON_PHASE2;
                    }
                    break;
                case FlameThrowerState.ON_PHASE2:
                    if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                    {
                        _currentSpriteChangeDelayTick = 0;
                        this.State = FlameThrowerState.ON_PHASE3;
                    }
                    break;
                case FlameThrowerState.ON_PHASE3:
                    if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                    {
                        _currentSpriteChangeDelayTick = 0;
                        this.State = FlameThrowerState.ON_PHASE4;
                    }
                    break;
                case FlameThrowerState.ON_PHASE4:
                    if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                    {
                        _currentSpriteChangeDelayTick = 0;
                        this.State = FlameThrowerState.ON_PHASE5;
                    }
                    break;
                case FlameThrowerState.ON_PHASE5:
                    if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                    {
                        _currentSpriteChangeDelayTick = 0;
                        this.UpdateOffState();
                    }
                    break;
            }
            if (_state != FlameThrowerState.OFF && this.CollidesWith(_keen))
            {
                _keen.Die();
            }
        }

        private void UpdatePosition()
        {
            int widthOffset = _sprite.Width - _originalWidth;
            if (_state == FlameThrowerState.OFF)
            {
                this.HitBox = new Rectangle(_originalLeft - widthOffset, _originalBottom - _sprite.Height, _sprite.Width, _sprite.Height);
            }
            else
            {
                this.HitBox = new Rectangle(_originalLeft - widthOffset, _originalBottom - _sprite.Height, _sprite.Width, _sprite.Height - STANDING_TILE_HEIGHT);
            }
        }

        private void UpdateOffState()
        {
            if (this.State != FlameThrowerState.OFF || _firstTime)
            {
                this.State = FlameThrowerState.OFF;
                this.UpdatePosition();
                _offTimeTick = 0;
                _firstTime = false;
            }

            if (_offTimeTick++ == OFF_TIME)
            {
                this.State = FlameThrowerState.ON_PHASE1;
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.State}";
        }
    }

    public enum FlameThrowerState
    {
        OFF,
        ON_PHASE1,
        ON_PHASE2,
        ON_PHASE3,
        ON_PHASE4,
        ON_PHASE5
    }
}
