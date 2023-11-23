using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Trajectories
{
    public class BoobusBombExplosion : CollisionObject, IUpdatable, IExplodable, ISprite, ICreateRemove
    {
        private int _currentSprite;
        private Image[] _explosionImages = new Image[]
        {
            Properties.Resources.keen_dreams_boobus_bomb_explode1,
            Properties.Resources.keen_dreams_boobus_bomb_explode2
        };

        private const int EXPLOSION_LENGTH = 4;
        private int _explosionTick;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _spriteChangeTick;

        public BoobusBombExplosion(SpaceHashGrid grid, Rectangle hitbox, int blastRadius, int damage)
            : base(grid, hitbox)
        {
            _damage = damage;
            Initialize();
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
                if (this.HitBox != null && _sprite != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    _sprite.Size = this.HitBox.Size;
                }
            }
        }


        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Image = _explosionImages[_currentSprite];
            _sprite.Location = this.HitBox.Location;
            _explosionState = Enums.ExplosionState.EXPLODING;
            this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
        }

        public void Update()
        {
            this.Explode();
        }

        public void Explode()
        {
            var collisions = this.CheckCollision(this.HitBox);
            foreach (var collision in collisions)
            {
                this.HandleCollision(collision);
            }
            if (_explosionTick < EXPLOSION_LENGTH)
            {
                if (_spriteChangeTick++ == SPRITE_CHANGE_DELAY)
                {
                    _spriteChangeTick = 0;
                    _explosionTick++;
                    UpdateSprite();
                }
            }
            else
            {
                _explosionState = Enums.ExplosionState.DONE;
                OnRemove();
            }
        }

        private void UpdateSprite()
        {
            if (_currentSprite < _explosionImages.Length - 1)
            {
                _currentSprite++;
            }
            else
            {
                _currentSprite = 0;
            }
            _sprite.Image = _explosionImages[_currentSprite];
        }

        public Enums.ExplosionState ExplosionState
        {
            get { return _explosionState; }
            //set
            //{
            //    _explosionState = value;
            //}
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;
        private Enums.ExplosionState _explosionState;
        private int _damage;

        protected void OnRemove()
        {
            if (this.Remove != null)
            {
                _explosionState = Enums.ExplosionState.DONE;
                foreach (var node in _collidingNodes)
                {
                    node.Objects.Remove(this);
                    node.NonEnemies.Remove(this);
                }
                ObjectEventArgs args = new ObjectEventArgs() { ObjectSprite = this };
                this.Remove(this, args);
            }
        }

        protected void OnCreate()
        {
            if (this.Create != null)
            {
                ObjectEventArgs args = new ObjectEventArgs() { ObjectSprite = this };
                this.Create(this, args);
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (!(obj is CommanderKeen))
            {
                if (obj is IStunnable)
                {
                    ((IStunnable)obj).Stun();
                }
                else if (obj is DestructibleObject)
                {
                    ((DestructibleObject)obj).TakeDamage(this._damage);
                }
            }
        }
    }
}
