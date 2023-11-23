using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen5LaserField : Hazard, IUpdatable
    {
        private LaserFieldState _state;
        private const int OFF_TIME = 20;
        private int _currentOffTimeTick;
        private const int STATE_CHANGE_DELAY = 2;
        private int _currentStateChangeDelayTick;

        private const int SKINNY_WIDTH = 6, MEDIUM_WIDTH = 14, FAT_WIDTH = 32;

        private const int WIDTH = 32, UP_HEIGHT = 44, Y_OFFSET = 4, DOWN_HEIGHT=32;
        private CommanderKeen _keen;

        public Keen5LaserField(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, LaserFieldState initialState) 
            : base(grid, hitbox, Enums.HazardType.KEEN5_LASER_FIELD)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _sprite.Size = this.HitBox.Size;
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         

            UpTile = new LaserFieldTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Bottom - Y_OFFSET, WIDTH, UP_HEIGHT), Enums.Direction.UP, LaserFieldTileType.KEEN5);
            DownTile = new LaserFieldTile(_collisionGrid, new Rectangle(this.HitBox.X , this.HitBox.Top - DOWN_HEIGHT, WIDTH, DOWN_HEIGHT), Enums.Direction.DOWN, LaserFieldTileType.KEEN5);

            this.State = initialState;
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

        public override bool IsDeadly
        {
            get
            {
                return _state == LaserFieldState.PHASE2 || _state == LaserFieldState.PHASE3 || _state == LaserFieldState.PHASE4;
            }
        }

        public LaserFieldTile UpTile
        {
            get;
            private set;
        }

        public LaserFieldTile DownTile
        {
            get;
            private set;
        }

        LaserFieldState State
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
            switch (_state)
            {
                case LaserFieldState.OFF:
                    _sprite.SendToBack();
                    _sprite.Image = null;
                    break;
                case LaserFieldState.PHASE1:
                case LaserFieldState.PHASE5:
                    _sprite.Image = Properties.Resources.keen5_laser_field_laser1;
                    _sprite.BringToFront();
                    this.UpdateHitboxWidth(SKINNY_WIDTH);
                    break;
                case LaserFieldState.PHASE2:
                case LaserFieldState.PHASE4:
                    _sprite.Image = Properties.Resources.keen5_laser_field_laser2;
                    _sprite.BringToFront();
                    this.UpdateHitboxWidth(MEDIUM_WIDTH);
                    break;
                case LaserFieldState.PHASE3:
                    _sprite.Image = Properties.Resources.keen5_laser_field_laser3;
                    _sprite.BringToFront();
                    this.UpdateHitboxWidth(FAT_WIDTH);
                    break;
            }
        }

        public virtual void Update()
        {
            switch (_state)
            {
                case LaserFieldState.OFF:
                    this.UpdateDoormantState();
                    break;
                default:
                    this.UpdateLaserPhase();
                    break;
            }
        }

        private void UpdateLaserPhase()
        {
            switch (_state)
            {
                case LaserFieldState.OFF:
                    this.State = LaserFieldState.PHASE1;
                    break;
                case LaserFieldState.PHASE1:
                    if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
                    {
                        _currentStateChangeDelayTick = 0;
                        this.State = LaserFieldState.PHASE2;
                    }
                    break;
                case LaserFieldState.PHASE2:
                    if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
                    {
                        _currentStateChangeDelayTick = 0;
                        this.State = LaserFieldState.PHASE3;
                    }
                    TryKillKeen();
                    break;
                case LaserFieldState.PHASE3:
                    if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
                    {
                        _currentStateChangeDelayTick = 0;
                        this.State = LaserFieldState.PHASE4;
                    }
                    TryKillKeen();
                    break;
                case LaserFieldState.PHASE4:
                    if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
                    {
                        _currentStateChangeDelayTick = 0;
                        this.State = LaserFieldState.PHASE5;
                    }
                    TryKillKeen();
                    break;
                case LaserFieldState.PHASE5:
                    if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
                    {
                        _currentStateChangeDelayTick = 0;
                        this.UpdateDoormantState();
                    }
                    break;
            }
        }

        private void UpdateHitboxWidth(int width)
        {
            if (width > WIDTH)
                width = WIDTH;
            this.HitBox = new Rectangle(UpTile.HitBox.X + ((WIDTH - width) / 2), this.HitBox.Y, width, this.HitBox.Height);
            _sprite.Size = this.HitBox.Size;
        }

        private void TryKillKeen()
        {
            if (this.IsDeadly && _keen.HitBox.IntersectsWith(this.HitBox))
                _keen.Die();
        }

        private void UpdateDoormantState()
        {
            if (this.State != LaserFieldState.OFF)
            {
                this.State = LaserFieldState.OFF;
                _currentOffTimeTick = 0;
            }
            if (_currentOffTimeTick++ == OFF_TIME)
            {
                this.UpdateLaserPhase();
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.HitBox.X}|{this.HitBox.Y}|{this.HitBox.Width}|{this.HitBox.Height}|{this.State.ToString()}";
        }
    }

    public enum LaserFieldState
    {
        OFF,
        PHASE1,
        PHASE2,
        PHASE3,
        PHASE4,
        PHASE5
    }
}
