using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Hazards
{
    public class DartGun : Hazard, IUpdatable, IFireable, ICreateRemove
    {
        public DartGun(SpaceHashGrid grid, Rectangle hitbox, Direction direction, bool isFiring = true)
            : base(grid, hitbox, Enums.HazardType.DART_GUN)
        {
            Initialize(direction, isFiring);
        }

        private void Initialize(Direction direction, bool isFiring)
        {
            _isFiring = isFiring;
            _direction = direction;
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            switch (_direction)
            {
                case Direction.DOWN:
                    _sprite.Image = Properties.Resources.keen4_dart_gun_down;
                    break;
                case Direction.UP:
                    _sprite.Image = Properties.Resources.keen4_dart_gun_up;
                    break;
                case Direction.RIGHT:
                    _sprite.Image = Properties.Resources.keen4_dart_gun_right;
                    break;
                case Direction.LEFT:
                    _sprite.Image = Properties.Resources.keen4_dart_gun_left;
                    break;
            }
        }

        private Direction _direction;
        private bool _isFiring;
        private const int SHOT_DELAY = 40;
        private int _currentShotDelayTick = 0;

        public void Update()
        {
            if (_isFiring)
            {
                if (_currentShotDelayTick == SHOT_DELAY)
                {
                    _currentShotDelayTick = 0;
                    this.Fire();
                }
                else
                {
                    _currentShotDelayTick++;
                }
            }
        }

        public void Fire()
        {
            Point fireLocation = new Point();
            Size fireHitbox = new Size();
            switch (_direction)
            {
                case Direction.UP:
                    fireLocation = new Point(this.HitBox.X + 16, this.HitBox.Top - 32);
                    fireHitbox = new Size(10, 32);
                    break;
                case Direction.DOWN:
                    fireLocation = new Point(this.HitBox.X + 16, this.HitBox.Bottom);
                    fireHitbox = new Size(10, 32);
                    break;
                case Direction.LEFT:
                    fireLocation = new Point(this.HitBox.Left - 32, this.HitBox.Y + 16);
                    fireHitbox = new Size(32, 10);
                    break;
                case Direction.RIGHT:
                    fireLocation = new Point(this.HitBox.Right, this.HitBox.Y + 16);
                    fireHitbox = new Size(32, 10);
                    break;
            }
            Dart dart = new Dart(_collisionGrid, new Rectangle(fireLocation, fireHitbox), _direction);
            dart.Create += new EventHandler<KeenEventArgs.ObjectEventArgs>(dart_Create);
            dart.Remove += new EventHandler<ObjectEventArgs>(dart_Remove);
            ObjectEventArgs e = new ObjectEventArgs()
            {
                ObjectSprite = dart
            };
            OnCreate(e);
        }

        void dart_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void dart_Create(object sender, KeenEventArgs.ObjectEventArgs e)
        {
            OnCreate(e);
        }

        private void OnRemove(ObjectEventArgs e)
        {
            if (Remove != null)
            {
                Remove(this, e);
            }
        }
       
        private void OnCreate(ObjectEventArgs e)
        {
            if (Create != null)
            {
                Create(this, e);
            }
        }

        public bool IsFiring
        {
            get { return _isFiring; }
        }

        public int Ammo
        {
            get { return -1; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_direction}";
        }
    }
}
