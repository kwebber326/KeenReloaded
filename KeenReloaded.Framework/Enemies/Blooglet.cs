using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System.Drawing;
using KeenReloaded.Framework.Items;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Enemies
{
    public class Blooglet : DestructibleObject, IUpdatable, ISprite, IEnemy, ICreateRemove, IZombieBountyEnemy
    {
        private Color _color;
        private Image[] _walkRightSprites, _walkLeftSprites, _stunnedSprites;
        private bool _holdsItem;
        private System.Windows.Forms.PictureBox _sprite;
        private BloogletState _state;
        private Enums.Direction _direction;
        private int _currentStunnedSprite;
        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelayTick;
        private const int MOVE_SPRITE_CHANGE_DELAY = 0;
        private int _currentMoveSpriteChangeDelayTick;
        private int _currentMoveSprite;
        private const int FALL_VELOCITY = 30;
        private const int MOVE_VELOCITY = 17;
        private const int CHASE_KEEN_CHANCE = 20;
        private CommanderKeen _keen;
        private bool _spawnedGem;

        public Blooglet(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, Color color, bool holdsItem)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _color = color;
            _holdsItem = holdsItem;
            Initialize();
        }

        private void Initialize()
        {
            //default to red
            _walkLeftSprites = SpriteSheet.BloogletRedLeftImages;
            _walkRightSprites = SpriteSheet.BloogletRedRightImages;
            _stunnedSprites = SpriteSheet.BloogletRedStunnedImages;

            //TODO: once sprites are in, set the sprite arrays inside these 'if/else' conditions
            if (_color == Color.Blue)
            {
                _walkLeftSprites = SpriteSheet.BloogletBlueLeftImages;
                _walkRightSprites = SpriteSheet.BloogletBlueRightImages;
                _stunnedSprites = SpriteSheet.BloogletBlueStunnedImages;
            }
            else if (_color == Color.Green)
            {
                _walkLeftSprites = SpriteSheet.BloogletGreenLeftImages;
                _walkRightSprites = SpriteSheet.BloogletGreenRightImages;
                _stunnedSprites = SpriteSheet.BloogletGreenStunnedImages;
            }
            else if (_color == Color.Yellow)
            {
                _walkLeftSprites = SpriteSheet.BloogletYellowLeftImages;
                _walkRightSprites = SpriteSheet.BloogletYellowRightImages;
                _stunnedSprites = SpriteSheet.BloogletYellowStunnedImages;
            }
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;

            UpdateSpriteLocation();

            int directionVal = _random.Next(0, 2);
            this.Direction = directionVal == 0 ? Enums.Direction.LEFT : Enums.Direction.RIGHT;

            this.Fall();
        }

        public override void Die()
        {
            this.UpdateStunnedState();
            if (_holdsItem && !_spawnedGem)
            {
                SpawnGem();
            }
        }

        private void SpawnGem()
        {
            GemColor color = GetGemColorFromThisColor();
            Gem gem = new Gem(_collisionGrid, new Rectangle(this.HitBox.X + this.HitBox.Width / 2 - 13, this.HitBox.Y, 26, 22), color, true);
            gem.Create += new EventHandler<ObjectEventArgs>(gem_Create);
            gem.Remove += new EventHandler<ObjectEventArgs>(gem_Remove);

            OnCreate(new ObjectEventArgs() { ObjectSprite = gem });
            _spawnedGem = true;
        }

        void gem_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void gem_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        private GemColor GetGemColorFromThisColor()
        {
            if (_color == Color.Red)
                return GemColor.RED;
            if (_color == Color.Yellow)
                return GemColor.YELLOW;
            if (_color == Color.Green)
                return GemColor.GREEN;
            if (_color == Color.Blue)
                return GemColor.BLUE;

            return GemColor.RED;
        }

        public void Update()
        {
            switch (_state)
            {
                case BloogletState.MOVING:
                    this.Move();
                    break;
                case BloogletState.STUNNED:
                    this.UpdateStunnedState();
                    break;
                case BloogletState.FALLING:
                    this.Fall();
                    break;
            }
        }

        private void Fall()
        {
            if (this.State != BloogletState.FALLING)
            {
                this.State = BloogletState.FALLING;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            }

            var tile = this.BasicFallReturnTile(FALL_VELOCITY);

            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                if (!this.IsDead())
                {
                    this.Move();
                }
                else
                {
                    this.UpdateStunnedState();
                }
            }
            else if (this.IsDead())
            {
                this.UpdateHitboxBasedOnStunnedImage(
               _stunnedSprites
               , ref _currentStunnedSprite
               , ref _currentStunnedSpriteChangeDelayTick
               , STUNNED_SPRITE_CHANGE_DELAY
               , UpdateSprite);
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
                    UpdateSpriteLocation();
                    if (this.State != BloogletState.FALLING)
                    {
                        this.UpdateCollisionNodes(this.Direction);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                }
            }
        }

        private void UpdateSpriteLocation()
        {
            if (_sprite.Width > this.HitBox.Width)
            {
                int widthDif = _sprite.Width - this.HitBox.Width;
                _sprite.Location = new Point(this.HitBox.X - widthDif / 2, this.HitBox.Y);
            }
            else
            {
                _sprite.Location = this.HitBox.Location;
            }
        }

        Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                UpdateSprite();
            }
        }

        BloogletState State
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

        public Color Color
        {
            get
            {
                return _color;
            }
        }

        public bool HoldsItem
        {
            get
            {
                return _holdsItem;
            }
        }

        private void UpdateSprite()
        {
            switch (_state)
            {
                case BloogletState.MOVING:
                case BloogletState.FALLING:
                    if (!this.IsDead())
                    {
                        var spriteSet = this.Direction == Enums.Direction.LEFT ? _walkLeftSprites : _walkRightSprites;
                        if (_currentMoveSprite >= spriteSet.Length || _currentMoveSprite < 0)
                        {
                            _currentMoveSprite = 0;
                        }

                        this.Sprite.Image = spriteSet[_currentMoveSprite];
                    }
                    break;
                case BloogletState.STUNNED:
                    if (_currentStunnedSprite >= _stunnedSprites.Length || _currentStunnedSprite < 0)
                    {
                        _currentStunnedSprite = 1;
                    }
                    this.Sprite.Image = _stunnedSprites[_currentStunnedSprite];
                    break;
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != BloogletState.STUNNED)
            {
                this.State = BloogletState.STUNNED;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                SetInitialStunnedSprite();
                return;
            }

            this.UpdateHitboxBasedOnStunnedImage(
                _stunnedSprites
                , ref _currentStunnedSprite
                , ref _currentStunnedSpriteChangeDelayTick
                , STUNNED_SPRITE_CHANGE_DELAY
                , UpdateSprite);

            if (IsNothingBeneath())
            {
                this.Fall();
            }
        }

        private void SetInitialStunnedSprite()
        {
            if (_color == Color.Red)
                _sprite.Image = Properties.Resources.keen6_blooglet_red_stunned1;
            else if (_color == Color.Blue)
                _sprite.Image = Properties.Resources.keen6_blooglet_blue_stunned1;
            else if (_color == Color.Green)
                _sprite.Image = Properties.Resources.keen6_blooglet_green_stunned1;
            else if (_color == Color.Yellow)
                _sprite.Image = Properties.Resources.keen6_blooglet_yellow_stunned1;
        }

        private void Move()
        {
            if (this.State != BloogletState.MOVING)
            {
                this.State = BloogletState.MOVING;
                if (_keen.HitBox.IntersectsWith(this.HitBox) && !IsKeenInFrontOfThis())
                {
                    _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), true, this);
                }
                else
                {
                    _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), false, this);
                }
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }
            int chaseKeenVal = _random.Next(1, CHASE_KEEN_CHANCE + 1);
            if (chaseKeenVal == CHASE_KEEN_CHANCE)
            {
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            }

            if (IsOnEdge(this.Direction, 3))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            }

            int xOffset = _direction == Enums.Direction.LEFT ? MOVE_VELOCITY * -1 : MOVE_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;

            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + MOVE_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                xOffset = _direction == Enums.Direction.LEFT ? MOVE_VELOCITY * -1 : MOVE_VELOCITY;
                ExecuteFreeMoveLogic(xOffset, areaToCheck);
            }
            else
            {
                ExecuteFreeMoveLogic(xOffset, areaToCheck);
            }

            this.UpdateSpriteByDelayBase(ref _currentMoveSpriteChangeDelayTick, ref _currentMoveSprite, MOVE_SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void ExecuteFreeMoveLogic(int xOffset, Rectangle areaToCheck)
        {
            _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
            _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);

            Rectangle pushAreaToCheck = this.Direction == Enums.Direction.LEFT
                ? new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, xOffset * -1, this.HitBox.Height)
                : new Rectangle(this.HitBox.Right, this.HitBox.Y, xOffset, this.HitBox.Height);
            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);

            if (!(_keen.MoveState == MoveState.ON_POLE || _keen.IsDead()))
            {

                if (_keen.HitBox.IntersectsWith(pushAreaToCheck))
                {
                    _keen.SetKeenPushState(this.Direction, true, this);
                    _keen.GetMovedHorizontally(this, this.Direction, MOVE_VELOCITY);
                }
                else
                {
                    _keen.SetKeenPushState(this.Direction, false, this);
                }
            }
        }

        private bool IsKeenInFrontOfThis()
        {
            if (!_keen.HitBox.IntersectsWith(this.HitBox))
                return false;
            if (_keen.Direction == Enums.Direction.LEFT)
            {
                if (_keen.HitBox.Left <= this.HitBox.Right - 15)
                    return true;
            }
            else
            {
                if (_keen.HitBox.Right >= this.HitBox.Left + 15)
                    return true;
            }

            return false;
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return false; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return _state != BloogletState.STUNNED; }
        }

        public PointItemType PointItem => PointItemType.KEEN6_BLOOG_SODA;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;


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
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_color.Name}|{_holdsItem.ToString()}"; 
        }
    }

    enum BloogletState
    {
        MOVING,
        STUNNED,
        FALLING
    }
}
