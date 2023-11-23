using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class BlueEagleEgg : DestructibleObject, IUpdatable, IEnemy, ISprite, ICreateRemove
    {
        private System.Windows.Forms.PictureBox _sprite;
        private bool _hatched;
        private bool _animationComplete;
        private CommanderKeen _keen;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _spriteChangeDelayTick;
        private int _currentSprite = 0;
        private const int FALL_VELOCITY = 30;

        public BlueEagleEgg(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            this.Health = 1;
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Image = Properties.Resources.keen4_blue_eagle_egg;
            _sprite.Location = this.HitBox.Location;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
          
        }
    
        public override void  Die()
        {
            this.Hatch();
        }

        private void Hatch()
        {
            if (!_hatched)
            {
                _hatched = true;
                BlueEagle e = new BlueEagle(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Bottom - 62, 60, 62), _keen);
                OnCreate(e);
            }
        }

        private CollisionObject GetTopMostLandingTile()
        {
            CollisionObject topMostTile;
            Rectangle areaTocheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + 2);
            var items = this.CheckCollision(areaTocheck, true);

            if (!items.Any())
                return null;

            int minY = items.Select(c => c.HitBox.Top).Min();
            topMostTile = items.FirstOrDefault(t => t.HitBox.Top == minY);

            return topMostTile;
        }

        private void Fall()
        {
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + FALL_VELOCITY, this.HitBox.Width, this.HitBox.Height);
            this.UpdateCollisionNodes(Direction.DOWN);
        }

        private void Land(CollisionObject obj)
        {
            if (obj != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, obj.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                this.UpdateCollisionNodes(Direction.DOWN);
            }
        }

        private void UpdateHatchedSprite()
        {
            if (_spriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _spriteChangeDelayTick = 0;
                switch (_currentSprite)
                {
                    case 0:
                        _currentSprite++;
                        this.Sprite.Image = Properties.Resources.keen4_blue_eagle_egg_hatch1;
                        this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                        this.UpdateCollisionNodes(Direction.DOWN);
                        break;
                    case 1: 
                         _currentSprite++;
                        this.Sprite.Image = Properties.Resources.keen4_blue_eagle_egg_hatch2;
                        this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
                        this.UpdateCollisionNodes(Direction.DOWN);
                        _animationComplete = true;
                        break;
                }
            }
        }

        private void ExecuteGravity()
        {
            CollisionObject obj = GetTopMostLandingTile();
            if (obj != null)
            {
                this.Land(obj);
            }
            else
            {
                this.Fall();
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
                }
            }
        }
    
        public bool  DeadlyTouch
        {
            get { return false; }
        }

        public void  HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool  IsActive
        {
            get { return !_hatched; }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Update()
        {
            if (!_hatched && this.HitBox.IntersectsWith(_keen.HitBox))
            {
                this.Hatch();
            }
            else if (_hatched && !_animationComplete)
            {
                UpdateHatchedSprite();
            }
            ExecuteGravity();
        }

        protected void OnCreate(ISprite obj)
        {
            if (Create != null)
            {
                Create(this, new ObjectEventArgs() { ObjectSprite = obj });
            }
        }

        protected void OnRemove(ISprite obj)
        {
            if (Remove != null)
            {
                Remove(this, new ObjectEventArgs() { ObjectSprite = obj });
            }
        }

        public event EventHandler<ObjectEventArgs> Create;

        public event EventHandler<ObjectEventArgs> Remove;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
