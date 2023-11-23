using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Enums;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Trajectories
{
    public class RoboRedShot : CollisionObject, ITrajectory, IUpdatable, ISprite, ICreateRemove
    {
        private Direction _direction;
        private System.Windows.Forms.PictureBox _sprite;
        private bool _hit;
        private bool _isLeftDirection, _isUpDirection;
        public static readonly int DEFAULT_WIDTH = 28;
        public static readonly int DEFAULT_HEIGHT = 26;
        private const int HIT_SPRITE_CHANGE_DELAY = 1;
        private int _currentHitSpriteChangeDelayTick;

        private Image[] _fireSprites = new Image[]
        {
            Properties.Resources.keen5_robo_red_shot1,
            Properties.Resources.keen5_robo_red_shot2
        };

        private int _currentHitSprite;
        private Image[] _hitSprites = new Image[]
        {
            Properties.Resources.keen5_robo_red_shot_hit1,
            Properties.Resources.keen5_robo_red_shot_hit2
        };

        public RoboRedShot(SpaceHashGrid grid, Rectangle hitbox, Direction direction)
            : base(grid, hitbox)
        {
            _direction = direction;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            int sprite = _random.Next(0, _fireSprites.Length);
            _sprite.Image = _fireSprites[sprite];
            _isLeftDirection = this.IsLeftDirection(this.Direction);
            _isUpDirection = this.IsUpDirection(this.Direction);
        }
      
        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is CommanderKeen)
            {
                ((CommanderKeen)obj).Die();
            }
        }

        public override System.Drawing.Rectangle HitBox
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
                    if (_isUpDirection)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                    else
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                }
            }
        }

        public int Damage
        {
            get { return 1; }
        }

        public int Velocity
        {
            get { return 60; }
        }

        public int Pierce
        {
            get { return 0; }
        }

        public int Spread
        {
            get { return 5; }
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
            get { return true; }
        }

        public void Move()
        {
          
            int xOffset = _isLeftDirection ? this.Velocity * -1 : this.Velocity;
            int yOffset = _isUpDirection ? this.Spread * -1 : this.Spread;
            int xCheck = _isLeftDirection ? this.HitBox.X + xOffset : this.HitBox.X;
            int yCheck = _isUpDirection ? this.HitBox.Y + yOffset : this.HitBox.Y;
            Rectangle areaToCheck = new Rectangle(xCheck, yCheck, this.HitBox.Width + this.Velocity, this.HitBox.Height + this.Spread);
            var collisions = this.CheckCollision(areaToCheck);
            var keens = collisions.OfType<CommanderKeen>();
            bool keensToKill = keens.Any();

            var horizontalTile = _isLeftDirection ? this.GetRightMostLeftTile(collisions) : this.GetLeftMostRightTile(collisions);
            var verticalTile = _isUpDirection ? this.GetCeilingTile(collisions) : this.GetTopMostLandingTile(collisions);

            if (horizontalTile != null)
            {
                int xPos = _isLeftDirection ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (keensToKill)
                {
                    KillKeensIfColliding(keens);
                    //HandleHorizontalKeenCollisions(keens, xPos);
                }
                this.Stop();
                return;
            }
            if (verticalTile != null)
            {
                int yPos = _isUpDirection ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                if (horizontalTile == null)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, yPos, this.HitBox.Width, this.HitBox.Height);
                }
                if (keensToKill)
                {
                    //HandleVerticalKeenCollisions(keens, yPos);
                    KillKeensIfColliding(keens);
                }
                this.Stop();
                return;
            }
            else if (verticalTile == null && horizontalTile == null)
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
                if (keensToKill)
                {
                    HandleOpenKeenCollision(areaToCheck, keens);
                }
            }
        }

        private void KillKeensIfColliding(IEnumerable<CommanderKeen> keens)
        {
            foreach (var keen in keens)
            {
                if (keen.HitBox.IntersectsWith(this.HitBox))
                {
                    keen.Die();
                }
            }
        }

        private void HandleOpenKeenCollision(Rectangle areaToCheck, IEnumerable<CommanderKeen> keens)
        {
            foreach (var keen in keens)
            {
                if (keen.HitBox.IntersectsWith(areaToCheck))
                {
                    keen.Die();
                }
            }
        }

        private void HandleVerticalKeenCollisions(IEnumerable<CommanderKeen> keens, int yPos)
        {
            foreach (var keen in keens)
            {
                if (_isUpDirection)
                {
                    if (keen.HitBox.Bottom >= yPos)
                    {
                        HandleCollision(keen);
                    }
                }
                else if (keen.HitBox.Bottom <= yPos + this.HitBox.Height)
                {
                    HandleCollision(keen);
                }
            }
        }

        private void HandleHorizontalKeenCollisions(IEnumerable<CommanderKeen> keens, int xPos)
        {
            foreach (var keen in keens)
            {
                if (_isLeftDirection)
                {
                    if (keen.HitBox.Right >= xPos)
                    {
                        HandleCollision(keen);
                    }
                }
                else if (keen.HitBox.Left <= xPos)
                {
                    HandleCollision(keen);
                }
            }
        }

        public void Stop()
        {
            _hit = true;
            if (_currentHitSpriteChangeDelayTick++ == HIT_SPRITE_CHANGE_DELAY)
            {
                _currentHitSpriteChangeDelayTick = 0;
                if (_currentHitSprite < _hitSprites.Length)
                {
                    _sprite.Image = _hitSprites[_currentHitSprite++];
                }
                else
                {
                    OnRemove(new ObjectEventArgs() { ObjectSprite = this });
                }
            }
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

        public Enums.Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                
            }
        }

        public void Update()
        {
            if (!_hit)
            {
                this.Move();
            }
            else
            {
                this.Stop();
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

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
                        node.NonEnemies.Remove(this);
                        node.Objects.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }
    }
}
