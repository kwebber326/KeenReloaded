using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Assets
{
    public class FlippingPlatform : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        private System.Windows.Forms.PictureBox _sprite;
        private Dictionary<FlippingPlatformState, Image> _spritesByState;
        private Dictionary<FlippingPlatformState, Point> _offsetsByState;
        private const int STILL_TIME = 60;
        private int _stillTimeTick;
        Point _originalLocation;
        private bool _firstTime = true;

        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;

        private const int STANDING_OFFSET_Y = 9;

        private FlippingPlatformState _currentState;
        public FlippingPlatform(SpaceHashGrid grid, Rectangle hitbox, FlippingPlatformState initialState)
            : base(grid, hitbox)
        {
            _currentState = initialState;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _originalLocation = new Point(this.HitBox.X, this.HitBox.Y);
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + STANDING_OFFSET_Y, this.HitBox.Width, this.HitBox.Height - STANDING_OFFSET_Y);
            _spritesByState = new Dictionary<FlippingPlatformState, Image>();
            _spritesByState.Add(FlippingPlatformState.STILL, Properties.Resources.flipping_platform_still);
            _spritesByState.Add(FlippingPlatformState.FLIP_45, Properties.Resources.flipping_platform_flip_45);
            _spritesByState.Add(FlippingPlatformState.FLIP_90, Properties.Resources.flipping_platform_flip_90);
            _spritesByState.Add(FlippingPlatformState.FLIP_135, Properties.Resources.flipping_platform_flip_135);

            _offsetsByState = new Dictionary<FlippingPlatformState, Point>();
            _offsetsByState.Add(FlippingPlatformState.STILL, new Point(0, 15));
            _offsetsByState.Add(FlippingPlatformState.FLIP_45, new Point(2, -15));
            _offsetsByState.Add(FlippingPlatformState.FLIP_90, new Point(15, 0));
            _offsetsByState.Add(FlippingPlatformState.FLIP_135, new Point(-15, 2));
            _sprite.Image = _spritesByState[_currentState];

          
        }

        protected override void HandleCollision(CollisionObject obj)
        {
           
        }

        public void Update()
        {
            switch (_currentState)
            {
                case FlippingPlatformState.STILL:
                    this.UpdateStillState();
                    break;
                case FlippingPlatformState.FLIP_45:
                    this.Flip45();
                    break;
                case FlippingPlatformState.FLIP_90:
                    this.FLip90();
                    break;
                case FlippingPlatformState.FLIP_135:
                    this.FLip135();
                    break;
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
                if (this.HitBox != null && _collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public PlatformTile StandingTile
        {
            get;
            private set;
        }

        private void UpdateStillState()
        {
            if (this.State != FlippingPlatformState.STILL)
            {
                this.State = FlippingPlatformState.STILL;
                _stillTimeTick = 0;
                _sprite.Location = _originalLocation;
                if (!_firstTime)
                {
                    OnCreate(new ObjectEventArgs() { ObjectSprite = this.StandingTile });
                }
                else
                {
                    this.StandingTile = new PlatformTile(_collisionGrid, new Rectangle(this.HitBox.Location, this.HitBox.Size));
                    _firstTime = false;
                }
            }

            if (_stillTimeTick++ == STILL_TIME)
            {
                this.Flip45();
            }
        }

        private void SetOffsetByCurrentState()
        {
            int offsetX = 0, offsetY=0;
            offsetX = _offsetsByState[_currentState].X;
            offsetY = _offsetsByState[_currentState].Y;
            _sprite.Location = new Point(_sprite.Location.X + offsetX, _sprite.Location.Y + offsetY);
        }

        private void Flip45()
        {
            if (this.State != FlippingPlatformState.FLIP_45)
            {
                this.State = FlippingPlatformState.FLIP_45;
                SetOffsetByCurrentState();
                OnRemove(new ObjectEventArgs() { ObjectSprite = this.StandingTile });
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                this.FLip90();
            }
        }

        private void FLip90()
        {
            if (this.State != FlippingPlatformState.FLIP_90)
            {
                this.State = FlippingPlatformState.FLIP_90;
                SetOffsetByCurrentState();
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                this.FLip135();
            }
        }

        private void FLip135()
        {
            if (this.State != FlippingPlatformState.FLIP_135)
            {
                this.State = FlippingPlatformState.FLIP_135;
                SetOffsetByCurrentState();
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                this.UpdateStillState();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        FlippingPlatformState State
        {
            get
            {
                return _currentState;
            }
            set
            {
                _currentState = value;
                _sprite.Image = _spritesByState[_currentState];
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
               // Create(this, args);
                if (args.ObjectSprite == this.StandingTile)
                {
                    foreach (var node in _collidingNodes)
                    {
                            node.Objects.Add(this.StandingTile);
                            node.Tiles.Add(this.StandingTile);
                    }
                }
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
                        node.NonEnemies.Remove(this);
                    }
                }
                else if (args.ObjectSprite == this.StandingTile)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this.StandingTile);
                        node.Tiles.Remove(this.StandingTile);
                    }
                }
                this.Remove(this, args);
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_currentState.ToString()}";
        }
    }

    public enum FlippingPlatformState
    {
        STILL,
        FLIP_45,
        FLIP_90,
        FLIP_135
    }
}
