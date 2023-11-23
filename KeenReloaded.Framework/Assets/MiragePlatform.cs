using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;
using System.Drawing;

namespace KeenReloaded.Framework.Assets
{
    public class MiragePlatform : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        private const int STANDING_OFFSET_Y = 30;
        private const int STANDING_OFFSET_X = 10;
        private const int SPRITE_CHANGE_DELAY = 30;
        private int _currentSpriteChangeDelayTick;

        private MiragePlatformState _state;
        private Dictionary<MiragePlatformState, Image> _spritesByState;

        public MiragePlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, MiragePlatformState initialState)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _state = initialState;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            _sprite.Location = this.HitBox.Location;
            _sprite.Size = this.HitBox.Size;
            this.HitBox = new Rectangle(this.HitBox.X + STANDING_OFFSET_X, this.HitBox.Y + STANDING_OFFSET_Y, this.HitBox.Width - (STANDING_OFFSET_X * 2), this.HitBox.Height - STANDING_OFFSET_Y);
            _spritesByState = new Dictionary<MiragePlatformState, Image>();
            _spritesByState.Add(MiragePlatformState.PHASE1, Properties.Resources.keen4_mirage_platform1);
            _spritesByState.Add(MiragePlatformState.PHASE2, Properties.Resources.keen4_mirage_platform2);
            _spritesByState.Add(MiragePlatformState.PHASE3, Properties.Resources.keen4_mirage_platform3);
            _spritesByState.Add(MiragePlatformState.PHASE4, Properties.Resources.keen4_mirage_platform4);
            SetImageByState();
            if (_state == MiragePlatformState.PHASE1)
            {
                SetStandingTile();
            }
        }

        private void SetImageByState()
        {
            if (_sprite != null)
                _sprite.Image = _spritesByState[_state];
        }

        public void Update()
        {
            switch (_state)
            {
                case MiragePlatformState.PHASE1:
                    UpdatePhase1();
                    break;
                case MiragePlatformState.PHASE2:
                    UpdatePhase2();
                    break;
                case MiragePlatformState.PHASE3:
                    UpdatePhase3();
                    break;
                case MiragePlatformState.PHASE4:
                    UpdatePhase4();
                    break;
            }
        }

        private void UpdatePhase4()
        {
            if (_state != MiragePlatformState.PHASE4)
            {
                _state = MiragePlatformState.PHASE4;
                SetStandingTile();
                SetImageByState();
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                UpdatePhase1();
            }
        }

        private void UpdatePhase3()
        {
            if (_state != MiragePlatformState.PHASE3)
            {
                _state = MiragePlatformState.PHASE3;
                RemoveStandingTile();
                SetImageByState();
                if (_keen.MoveState == Enums.MoveState.HANGING)
                {
                    _keen.Fall();
                }
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                UpdatePhase4();
            }
        }

     
        private void UpdatePhase2()
        {
            if (_state != MiragePlatformState.PHASE2)
            {
                _state = MiragePlatformState.PHASE2;
                SetImageByState();
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                UpdatePhase3();
            }
        }

        private void UpdatePhase1()
        {
            if (_state != MiragePlatformState.PHASE1)
            {
                _state = MiragePlatformState.PHASE1;
                SetStandingTile();
                SetImageByState();
            }

            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                UpdatePhase2();
            }
        }

        private void SetStandingTile()
        {
            if (!_firstTime)
            {
                OnCreate(new ObjectEventArgs() { ObjectSprite = this.StandingTile });
            }
            else
            {
                this.StandingTile = new DebugTile(_collisionGrid, new Rectangle(this.HitBox.Location, this.HitBox.Size));
                _firstTime = false;
            }
        }

        private void RemoveStandingTile()
        {
            OnRemove(new ObjectEventArgs() { ObjectSprite = this.StandingTile });
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

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public MiragePlatformState State
        {
            get
            {
                return _state;
            }
        }

        public DebugTile StandingTile
        {
            get;
            private set;
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;
        private bool _firstTime = true;
        private CommanderKeen _keen;

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

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.State}";
        }
    }

    public enum MiragePlatformState
    {
        PHASE1,
        PHASE2,
        PHASE3,
        PHASE4
    }
}
