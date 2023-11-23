using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen6LaserField : Hazard, IUpdatable
    {
        private LaserFieldState _state;
        private const int OFF_TIME = 20;
        private int _currentOffTimeTick;
        private const int STATE_CHANGE_DELAY = 6;
        private int _currentStateChangeDelayTick;

        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private Image[] _laserSprites;

        private const int WIDTH = 32, UP_HEIGHT = 44, Y_OFFSET = 4, DOWN_HEIGHT = 32;
        private CommanderKeen _keen;

        public Keen6LaserField(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, LaserFieldState initialState)
            : base(grid, hitbox, Enums.HazardType.KEEN5_LASER_FIELD)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _sprite.Size = this.HitBox.Size;
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            _laserSprites = SpriteSheet.Keen6LaserFieldImages;

            UpTile = new LaserFieldTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Bottom - Y_OFFSET, WIDTH, UP_HEIGHT), Enums.Direction.UP, LaserFieldTileType.KEEN6);
            DownTile = new LaserFieldTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Top - DOWN_HEIGHT, WIDTH, DOWN_HEIGHT), Enums.Direction.DOWN, LaserFieldTileType.KEEN6);

            this.State = initialState; //initialState == LaserFieldState.OFF ? LaserFieldState.OFF : LaserFieldState.PHASE1;
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
                return _state == LaserFieldState.PHASE1;
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
                    if (_currentSprite >= _laserSprites.Length)
                    {
                        _currentSprite = 0;
                    }
                    _sprite.Image = _laserSprites[_currentSprite];
                    _sprite.BringToFront();
                    break;
            }
        }

        public void Update()
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
            this.State = LaserFieldState.PHASE1;
            if (_currentStateChangeDelayTick++ == STATE_CHANGE_DELAY)
            {
                _currentStateChangeDelayTick = 0;
                this.UpdateDoormantState();
                return;
            }
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
            TryKillKeen();
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
            if (_keen.HitBox.IntersectsWith(this.HitBox))
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
}
