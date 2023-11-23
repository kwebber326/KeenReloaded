using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Hazards;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class ThunderCloud : CollisionObject, IUpdatable, IMoveable, ISprite
    {
        public ThunderCloud(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {
            this.MotionState = ThunderCloudMoveState.DORMANT;
            _strikeRange = new Rectangle(new Point(this.HitBox.X + HORIZONTAL_STRIKE_RANGE_OFFSET, this.HitBox.Y),
                new Size(40, VERTICAL_STRIKE_RANGE));
        }
        private const int DIRECTION_CHANGE_RANGE = 100;
        private const int KEEN_LOCATION_CHECK_DELAY = 30;
        private int _currentLocationCheckDelay =0;

        private const int VELOCITY = 3;
        private const int PURSUE_DELAY = 10;
        private int _currentPursueDelayTick = 0;
        private const int HORIZONTAL_STRIKE_RANGE_OFFSET = 30;
        private const int VERTICAL_STRIKE_RANGE = 186;

        private const int STRIKE_DELAY = 5;
        private int _currentStrikeTick = 0;

        private int _currentStrikingSpriteTick;
        private const int STRIKE_SPRITE_CHANGE_DELAY = 2;
        private bool _thunderSprite = true;

        private bool _striking = false;

        private Rectangle _strikeRange;
        private CommanderKeen _keen;

        public event EventHandler<ObjectEventArgs> BoltCreated;
        public event EventHandler<ObjectEventArgs> BoltRemoved;

        protected void OnBoltCreated(LightningBolt bolt)
        {
            if (BoltCreated != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = bolt
                };
                BoltCreated(this, args);
            }
        }

        protected void OnBoltRemoved(LightningBolt bolt)
        {
            if (BoltRemoved != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = bolt
                };
                BoltRemoved(this, args);
            }
        }

        public void Update()
        {
            switch (MotionState)
            {
                case ThunderCloudMoveState.DORMANT:
                    var collisionItems = this.CheckCollision(this.HitBox);
                    if (collisionItems.OfType<CommanderKeen>().Any())
                    {
                        this.MotionState = ThunderCloudMoveState.PURSUING;
                        var keen = collisionItems.OfType<CommanderKeen>().First();
                        _keen = keen;
                        this.Direction = GetDirectionToKeen(keen);
                    }
                    break;
                case ThunderCloudMoveState.PURSUING:
                    this.Move();
                    break;
                case ThunderCloudMoveState.STRIKING:
                    UpdateLightningStrike();
                    break;
            }
        }

        private void UpdateLightningStrike()
        {
            if (_currentStrikeTick < STRIKE_DELAY)
            {
                UpdateSprite();
            }
            else
            {
                //TODO: here strikes a lightning bolt 
                if (!_striking)
                {
                    _striking = true;
                    LightningBolt bolt = new LightningBolt(_collisionGrid, new Rectangle(this.HitBox.X + 26, this.HitBox.Bottom + 1, 48, 142));
                    bolt.Removed += new EventHandler<KeenEventArgs.ObjectEventArgs>(bolt_Removed);
                    OnBoltCreated(bolt);
                }
            }
        }

        void bolt_Removed(object sender, KeenEventArgs.ObjectEventArgs e)
        {
            _currentStrikeTick = 0;
            _striking = false;
            this.MotionState = ThunderCloudMoveState.PURSUING;
            OnBoltRemoved(e.ObjectSprite as LightningBolt);
        }

        private Enums.Direction GetDirectionToKeen(CommanderKeen keen)
        {
            if (keen == null)
                return Enums.Direction.RIGHT;

            var center = this.HitBox.X + (this.HitBox.Width / 2);
            var keenCenter = keen.HitBox.X + (keen.HitBox.Width / 2);
            var direction = keenCenter < center ? Enums.Direction.LEFT : Enums.Direction.RIGHT;
            return direction;
        }

        public void Move()
        {
            if (_currentPursueDelayTick == PURSUE_DELAY)
            {
                if (this.Direction == Enums.Direction.LEFT)
                {
                    Rectangle areaToCheck = new Rectangle(this.HitBox.X - VELOCITY, this.HitBox.Y, this.HitBox.Width + VELOCITY, this.HitBox.Height);
                    var collisionKeens = this.CheckCollision(_strikeRange);
                    var keens = collisionKeens.OfType<CommanderKeen>();
                    var collisionItems = this.CheckCollision(areaToCheck);
                    var walls = collisionItems.OfType<DebugTile>();
                    if (walls.Any() || IsKeenEscaping(_keen))
                    {
                        ChangeDirection();
                    }
                    else if (keens.Any())
                    {
                        this.MotionState = ThunderCloudMoveState.STRIKING;
                    }
                    else
                    {
                        this.HitBox = new Rectangle(this.HitBox.X - VELOCITY, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                }
                else
                {
                    Rectangle areaToCheck = new Rectangle(this.HitBox.X + VELOCITY, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    var collisionKeens = this.CheckCollision(_strikeRange);
                    var keens = collisionKeens.OfType<CommanderKeen>();
                    var collisionItems = this.CheckCollision(areaToCheck);
                    var walls = collisionItems.OfType<DebugTile>();
                    if (walls.Any())
                    {
                        ChangeDirection();
                    }
                    else if (keens.Any())
                    {
                        this.MotionState = ThunderCloudMoveState.STRIKING;
                    }
                    else
                    {
                        this.HitBox = new Rectangle(this.HitBox.X + VELOCITY, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                }
            }
            else
            {
                _currentPursueDelayTick++;
            }
        }

        private bool IsKeenEscaping(CommanderKeen keen)
        {
            if (keen == null)
                return false;

            if (_currentLocationCheckDelay < KEEN_LOCATION_CHECK_DELAY)
            {
                _currentLocationCheckDelay++;
                return false;
            }

            _currentLocationCheckDelay = 0;
            if (this.Direction == Enums.Direction.LEFT)
            {
                bool isKeenEscaping = keen.HitBox.Left >= this.HitBox.Right + DIRECTION_CHANGE_RANGE;
                return isKeenEscaping;
            }
            else
            {
                bool isKeenEscaping = keen.HitBox.Right <= this.HitBox.X - DIRECTION_CHANGE_RANGE;
                return isKeenEscaping;
            }
        }

        private void ChangeDirection()
        {
            this.Direction = this.Direction == Enums.Direction.LEFT ? Enums.Direction.RIGHT : Enums.Direction.LEFT;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public Enums.MoveState MoveState
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        private ThunderCloudMoveState _moveState;

        public ThunderCloudMoveState MotionState
        {
            get
            {
                return _moveState;
            }
            private set
            {
                _moveState = value;
                UpdateSprite();
            }
        }

        public void PursueKeen(CommanderKeen keen)
        {
            if (keen != null)
            {
                _keen = keen;
                this.Direction = GetDirectionToKeen(keen);
                this.MotionState = ThunderCloudMoveState.PURSUING;
            }
        }

        private void UpdateSprite()
        {
            if (this.Sprite == null)
            {
                _sprite = new PictureBox();
            }
            switch (_moveState)
            {
                case ThunderCloudMoveState.DORMANT:
                    this.Sprite.Image = Properties.Resources.keen4_thundercloud_dormant;
                    break;
                case ThunderCloudMoveState.PURSUING:
                    this.Sprite.Image = Properties.Resources.keen4_thundercloud_no_thunder;
                    break;
                case ThunderCloudMoveState.STRIKING:
                    if (_currentStrikingSpriteTick < STRIKE_SPRITE_CHANGE_DELAY)
                    {
                        _currentStrikingSpriteTick++;
                    }
                    else
                    {
                        _currentStrikingSpriteTick = 0;
                        _currentStrikeTick++;
                        this.Sprite.Image = _thunderSprite ? Properties.Resources.keen4_thundercloud_thunder : Properties.Resources.keen4_thundercloud_no_thunder;
                        _thunderSprite = !_thunderSprite;
                    }
                    break;
            }
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            if (this.HitBox != null)
                _sprite.Location = this.HitBox.Location;
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
                if (this.Sprite != null && this.HitBox != null)
                {
                    this.Sprite.Location = this.HitBox.Location;
                    _strikeRange.Location = new Point(this.HitBox.X + HORIZONTAL_STRIKE_RANGE_OFFSET, this.HitBox.Y);
                    this.UpdateCollisionNodes(this.Direction);
                }
            }
        }

        public Enums.Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                _direction = value == Enums.Direction.LEFT ? Enums.Direction.LEFT : Enums.Direction.RIGHT;
            }
        }

        private Enums.Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }
        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    public enum ThunderCloudMoveState
    {
        DORMANT,
        PURSUING,
        STRIKING
    }
}
