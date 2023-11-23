using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Trajectories
{
    public class ShikadiMasterShockwave : CollisionObject, IUpdatable, ISprite, ITrajectory, ICreateRemove
    {
        private const int UPDATE_SPRITE_DELAY = 3;
        private int _currentUpdateSpriteDelayTick;
        private int _currentLifeTimeTick;
        private Direction _direction;
        private CommanderKeen _keen;
        public ShikadiMasterShockwave(SpaceHashGrid grid, Rectangle hitbox, Direction direction, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _direction = direction;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Image = SpriteSheet.ShikadiMasterShockwaveImages[_currentSprite];
            _sprite.Location = this.HitBox.Location;
            _currentHorizontalVelocity = _direction == Enums.Direction.LEFT ? this.Velocity * -1 : this.Velocity;
            var collisions = this.CheckCollision(this.HitBox, true);
            if (collisions.Any())
            {
                this.Stop();
            }
        }
        protected override void HandleCollision(CollisionObject obj)
        {

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
                    this.UpdateCollisionNodes(this.Direction);
                    this.UpdateCollisionNodes(Enums.Direction.UP);
                    this.UpdateCollisionNodes(Enums.Direction.DOWN);
                }
            }
        }

        public void Update()
        {
            this.Move();
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public int Damage
        {
            get { return int.MaxValue; }
        }

        public int Velocity
        {
            get { return 40; }
        }

        public int Pierce
        {
            get { return 0; }
        }

        public int Spread
        {
            get { return 0; }
        }

        public int BlastRadius
        {
            get { return 0; }
        }

        public int RefireDelay
        {
            get { return -1; }
        }

        public bool KillsKeen
        {
            get { return !_shotHit; }
        }

        public override void DetachFromCollisionGrid()
        {
            foreach (var node in _collidingNodes)
            {
                if (node != null)
                {
                    if (node.Objects != null)
                        node.Objects.Remove(this);
                    if (node.NonEnemies != null)
                        node.NonEnemies.Remove(this);
                }
            }
        }

        public void Move()
        {
            UpdateSpriteByDelay();

            if (IsOnEdge(_direction))
            {
                this.Stop();
                return;
            }

            int xPosCheck = _currentHorizontalVelocity < 0 ? this.HitBox.X + _currentHorizontalVelocity : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + Math.Abs(_currentHorizontalVelocity), this.HitBox.Height);

            var collisions = this.CheckCollision(areaToCheck, true);

            var tile = _currentHorizontalVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                this.Stop();
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + _currentHorizontalVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }
        }

        private void UpdateSpriteByDelay()
        {
            if (_currentUpdateSpriteDelayTick++ == UPDATE_SPRITE_DELAY)
            {
                _currentUpdateSpriteDelayTick = 0;
                if (++_currentSprite >= SpriteSheet.ShikadiMasterShockwaveImages.Length)
                {
                    _currentSprite = 0;
                }
                _sprite.Image = SpriteSheet.ShikadiMasterShockwaveImages[_currentSprite];
            }
        }

        private void SwitchDirection()
        {
            _direction = ChangeHorizontalDirection(_direction);
            _currentHorizontalVelocity *= -1;
        }

        public void Stop()
        {
            _shotHit = true;
            DetachFromCollisionGrid();
            OnRemove(new ObjectEventArgs() { ObjectSprite = this });
        }

        public MoveState MoveState
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

        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {

            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;
        private int _currentHorizontalVelocity;
        private int _currentSprite;
        private bool _shotHit;

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
                        node.NonEnemies.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }
    }
}
