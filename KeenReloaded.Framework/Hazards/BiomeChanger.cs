using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Hazards
{
    public class BiomeChanger : DestructibleObject, ISprite, IUpdatable, ICreateRemove, IFireable, IEnemy
    {
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private int _fireDelay;
        private System.Windows.Forms.PictureBox _sprite;
        private bool _isFiring;
        private int _maxVelocity;
        private int _currentFireDelayTick;

        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;

        private const int FIRE_TIME = 3;
        private int _currentFireTimeTick;

        private Image[] _sprites;

        private bool _hitAnimation;
        private const int HIT_ANIMATION_TIME = 3;
        private int _hitAnimationTimeTick;
        private int _maxDelay;
        private int _minDelay;

        public BiomeChanger(SpaceHashGrid grid, Rectangle hitbox, int minDelay, int maxDelay, int maxVelocty, int health)
            : base(grid, hitbox)
        {
            this.Health = health;
            _minDelay = minDelay;
            _maxDelay = maxDelay < minDelay ? minDelay : maxDelay;
            _fireDelay = _random.Next(_minDelay, _maxDelay + 1);
            _maxVelocity = maxVelocty;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprites = SpriteSheet.BiomeChangerImages;
            _currentSprite = _random.Next(0, _sprites.Length);
            _sprite.Image = _sprites[_currentSprite];
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
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public override void Die()
        {
            _sprite.Image = Properties.Resources.keen5_quantum_dynamo_sphere_destroyed;
            ResetHitAnimation();
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public void Update()
        {
            if (!this.IsDead())
            {
                if (_hitAnimation)
                {
                    if (_hitAnimationTimeTick++ == 0)
                    {
                        _sprite.BackColor = Color.White;
                    }
                    else if (_hitAnimationTimeTick == HIT_ANIMATION_TIME)
                    {
                        ResetHitAnimation();
                    }
                }
                if (_isFiring)
                {
                    this.Fire();
                }
                else if (_currentFireDelayTick++ == _fireDelay)
                {
                    _currentFireDelayTick = 0;
                    this.Fire();
                }

                this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
            }
        }

        private void ResetHitAnimation()
        {
            _hitAnimationTimeTick = 0;
            _hitAnimation = false;
            _sprite.BackColor = Color.Transparent;
        }

        private void UpdateSprite()
        {
            if (++_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite];
        }

        public void Fire()
        {
            if (!_isFiring)
                _isFiring = true;

            if (_currentFireTimeTick++ == 0)
            {
                this.Sprite.BackColor = Color.Red;
                //fire shot
                CreateBiomeShot();
            }
            else if (_currentFireTimeTick == FIRE_TIME)
            {
                this.Sprite.BackColor = Color.Transparent;
                _currentFireTimeTick = 0;
                _isFiring = false;
                _fireDelay = _random.Next(_minDelay, _maxDelay);
            }
        }

        private void CreateBiomeShot()
        {
            //random velocity
            int randHorizontalVelocity = _random.Next(_maxVelocity * -1, _maxVelocity + 1);
            if (randHorizontalVelocity == 0)
                randHorizontalVelocity = 1;//don't divide by zero, genius
            _random = new Random();
            int randVerticalVelocity = _random.Next(_maxVelocity * -1, _maxVelocity + 1);
            if (randVerticalVelocity == 0)
                randVerticalVelocity = 1;//don't divide by zero, genius
            //random biome
            int biomeCount = typeof(BiomeType).GetEnumValues().Length;
            int randVal = _random.Next(0, biomeCount);
            BiomeType tileType = (BiomeType)randVal;

            //block projectile should begin at a point on the sphere that is indicative of its intended trajectory
            int xPos = randHorizontalVelocity < 0 ? this.HitBox.X - 48 : this.HitBox.Right + 48;
            double yPosOffset = ((double)this.HitBox.Height / 2 / (double)randVerticalVelocity);
            int yOffsetVal =  (int)((this.HitBox.Height / 2) * yPosOffset);
            int yPos = this.HitBox.Y + (this.HitBox.Height / 2) + yOffsetVal;
            xPos = randVerticalVelocity < 0 ? xPos + Math.Abs(yOffsetVal) : xPos - Math.Abs(yOffsetVal);

            //instantiate projectile
            BiomeProjectile biomeChangerProjectile = new BiomeProjectile(_collisionGrid, new Rectangle(xPos, yPos, 48, 64), randVerticalVelocity, randHorizontalVelocity, tileType);
            biomeChangerProjectile.Create += new EventHandler<ObjectEventArgs>(biomeChangerProjectile_Create);
            biomeChangerProjectile.Remove += new EventHandler<ObjectEventArgs>(biomeChangerProjectile_Remove);

            OnCreate(new ObjectEventArgs() { ObjectSprite = biomeChangerProjectile });
        }

        void biomeChangerProjectile_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void biomeChangerProjectile_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        public bool IsFiring
        {
            get { return _isFiring; }
        }

        public int Ammo
        {
            get { return -1; }
        }

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
                this.Remove(this, args);
            }
        }

        public bool DeadlyTouch
        {
            get { return false; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
            _hitAnimation = true;
        }

        public bool IsActive
        {
            get { return !this.IsDead(); }
        }
    }
}
