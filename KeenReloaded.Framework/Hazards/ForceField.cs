using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Utilities;

namespace KeenReloaded.Framework.Hazards
{
    public class ForceField : DestructibleObject, ISprite, IEnemy
    {
        public const int GENERATOR_WIDTH = 32;
        public const int GENERATOR_HEIGHT = 64;

        private bool _isActive;
        private ForceFieldTop _top;
        private ForceFieldBottom _bottom;
        private ForceFieldBarrier _barrier;
        private int _health;
        public ForceField(SpaceHashGrid grid, Rectangle hitbox, int health) : base(grid, hitbox)
        {
            _health = health;
            this.Activate();
        }

        public bool IsActive => _isActive;

        public ISprite Barrier => _barrier;

        public ISprite Top => _top;

        public ISprite Bottom => _bottom;

        public PictureBox Sprite => _barrier.Sprite;

        public ProgressBar HealthBar => _barrier.HealthBar;

        public bool DeadlyTouch => false;


        public void Activate()
        {
            _isActive = true;

            int bottomX = this.HitBox.X;
            int bottomY = this.HitBox.Bottom - GENERATOR_HEIGHT;
            int barrierX = this.HitBox.X;
            int barrierY = this.HitBox.Y + GENERATOR_HEIGHT;
            int barrierWidth = this.HitBox.Width;
            int barrierHeight = this.HitBox.Height - (GENERATOR_HEIGHT * 2);

            Rectangle generatorAreaTop = new Rectangle(this.HitBox.X, this.HitBox.Y, GENERATOR_WIDTH, GENERATOR_HEIGHT);
            Rectangle generatorAreaBottom = new Rectangle(bottomX, bottomY, GENERATOR_WIDTH, GENERATOR_HEIGHT);
            Rectangle barrierArea = new Rectangle(barrierX, barrierY, barrierWidth, barrierHeight);
            _top = new ForceFieldTop(_collisionGrid, generatorAreaTop);
            _bottom = new ForceFieldBottom(_collisionGrid, generatorAreaBottom);
            _barrier = new ForceFieldBarrier(_collisionGrid, barrierArea, _health);
            _barrier.Killed += _barrier_Killed;
        }

        private void _barrier_Killed(object sender, ObjectEventArgs e)
        {
            this.Deactivate();
        }

        public void Deactivate()
        {
            _isActive = false;
        }

        public override string ToString()
        {
            return base.ToString() + $"{_health}";
        }

        public override void Die()
        {
        }

        public override void TakeDamage(int damage)
        {
            _barrier.TakeDamage(damage);
        }

        public override void TakeDamage(ITrajectory trajectory)
        {

        }

        public void HandleHit(ITrajectory trajectory)
        {

        }
    }

    internal class ForceFieldBarrier : DestructibleObject, ISprite, ICreateRemove, IHealthBar
    {
        private PictureBox _sprite;
        private HealthBarTile _collisionTile;
        private Timer _animationTimer;
        private bool _isDestroyed;

        private const int EXPLOSION_ANIMATIONS = 8;
        private int _currentExplosionAnimation;
        public ForceFieldBarrier(SpaceHashGrid grid, Rectangle hitbox, int health) : base(grid, hitbox)
        {
            Initialize(grid, hitbox, health);
        }

        private void Initialize(SpaceHashGrid grid, Rectangle hitbox, int health)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            _sprite.Size = hitbox.Size;
            _sprite.Location = hitbox.Location;
          
            DrawForceFieldImage(hitbox);
            this.Health = health;

            _collisionTile = new HealthBarTile(grid, hitbox, this.Health, false);
            OnCreate(_collisionTile);

            _animationTimer = new Timer();
            _animationTimer.Interval = 300;
            _animationTimer.Tick += _animationTimer_Tick;
            SetHealthBarValue();
        }


        private void DrawForceFieldImage(Rectangle hitbox)
        {
            string filePath = FileIOUtilities.GetResourcesPath() + nameof(Properties.Resources.keen5_laser_field_laser3) + ".png";
            int imgHeight = Properties.Resources.keen5_laser_field_laser3.Height;
            int imageCount = hitbox.Height / imgHeight;
            if (hitbox.Height % imgHeight != 0)
                imageCount++;

            var fileList = new string[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                fileList[i] = filePath;
            }
            _sprite.Image = BitMapTool.CombineBitmap(fileList, 1, Color.White);
        }

        private void DrawExplosionImage(Rectangle hitbox)
        {
            int imgVal = _random.Next(1, 3);
            string filePath1 = FileIOUtilities.GetResourcesPath() + nameof(Properties.Resources.keen5_force_field_explosion1) + ".png";
            string filePath2 = FileIOUtilities.GetResourcesPath() + nameof(Properties.Resources.keen5_force_field_explosion2) + ".png";
            int imgHeight = Properties.Resources.keen5_force_field_explosion1.Height; //same height, can pick either one
            int imageCount = hitbox.Height / imgHeight;

            if (hitbox.Height % imgHeight != 0)
                imageCount++;

            var fileList = new string[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                fileList[i] = imgVal == 1 ? filePath1 : filePath2;
                imgVal = _random.Next(1, 3);
            }
            _sprite.Image = BitMapTool.CombineBitmap(fileList, 1, Color.White);
        }

        private void _animationTimer_Tick(object sender, EventArgs e)
        {

            if (!this.IsDead())
            {
                DrawForceFieldImage(this.HitBox);
                _animationTimer.Stop();
            }
            else if (++_currentExplosionAnimation < EXPLOSION_ANIMATIONS)
            {
                DrawExplosionImage(this.HitBox);
            }
            else
            {
                _animationTimer.Stop();
                this.Sprite.Image = null;
                _isDestroyed = true;
                OnRemove(this);
            }
        }

        private void SetHealthBarValue()
        {
            _collisionTile?.SetHealthBarValue(this.Health);
        }

        private void SetHealthBarVisible()
        {
            _collisionTile?.SetHealthBarVisiblity(true);
        }

        public override void TakeDamage(ITrajectory trajectory)
        {
            base.TakeDamage(trajectory);
            ExecuteHitLogic();
            SetHealthBarValue();
            SetHealthBarVisible();
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            ExecuteHitLogic();
            SetHealthBarValue();
            SetHealthBarVisible();
        }

        private void ExecuteHitLogic()
        {
            if (!this.IsDead())
                ExecuteHitAnimation();
            else if (!_isDestroyed)
            {
                DrawExplosionImage(this.HitBox);
                this.Sprite.BackColor = Color.Transparent;
                ExecuteDestructionAnimation();
            }
        }

        private void ExecuteHitAnimation()
        {
            this.Sprite.Image = null;
            this.Sprite.BackColor = Color.White;
            if (!_animationTimer.Enabled)
            {
                _animationTimer.Start();
            }
        }

        private void ExecuteDestructionAnimation()
        {
            _animationTimer.Interval = 100;
            if (!_animationTimer.Enabled)
            {
                _animationTimer.Start();
            }
        }

        public PictureBox Sprite => _sprite;

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

        public ProgressBar HealthBar => _collisionTile?.HealthBar;

        public event EventHandler<ObjectEventArgs> Create;
        public event EventHandler<ObjectEventArgs> Remove;


        protected void OnCreate(ISprite obj)
        {
            ObjectEventArgs eventArgs = new ObjectEventArgs()
            {
                ObjectSprite = obj
            };
            this.Create?.Invoke(this, eventArgs);
        }

        protected void OnRemove(ISprite obj)
        {
            ObjectEventArgs eventArgs = new ObjectEventArgs()
            {
                ObjectSprite = obj
            };
            foreach (var node in _collidingNodes)
            {
                node.Tiles.Remove(_collisionTile);
                node.NonEnemies.Remove(_collisionTile);
                node.Objects.Remove(_collisionTile);
            }
            this.Remove?.Invoke(this, eventArgs);
            _collisionTile = null;
        }

        public override void Die()
        {
            OnRemove(_collisionTile);
            this.Sprite.Image = null;
        }
    }



    internal class ForceFieldTop : DebugTile
    {

        public ForceFieldTop(SpaceHashGrid grid, Rectangle hitbox) : base(grid, hitbox)
        {
            Initialize(hitbox);
        }

        private void Initialize(Rectangle hitbox)
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = hitbox.Location;
            _sprite.Size = hitbox.Size;
            _sprite.Image = Properties.Resources.keen5_force_field_top;
        }
    }
    internal class ForceFieldBottom : DebugTile
    {
        public ForceFieldBottom(SpaceHashGrid grid, Rectangle hitbox) : base(grid, hitbox)
        {
            Initialize(hitbox);
        }

        private void Initialize(Rectangle hitbox)
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = hitbox.Location;
            _sprite.Size = hitbox.Size;
            _sprite.Image = Properties.Resources.keen5_force_field_bottom;
        }
    }
}
