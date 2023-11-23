using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class LaserTurret : Hazard, IUpdatable, IFireable, ICreateRemove
    {
        private Direction _direction;
        private bool _isActive;
        private const int FIRE_DELAY = 30;
        private int _currentFireDelayTick;

        public LaserTurret(SpaceHashGrid grid, Rectangle hitbox, Direction direction, bool IsActive, TurretType type) :
            base(grid, hitbox, type == TurretType.KEEN5 ? HazardType.KEEN5_LASER_TURRET : HazardType.KEEN6_LASER_TURRET)
        {
            _type = type;
            _direction = direction;
            _isActive = IsActive;
            SetSpriteFromType(type == TurretType.KEEN5 ? HazardType.KEEN5_LASER_TURRET : HazardType.KEEN6_LASER_TURRET);
        }

        public override bool IsDeadly
        {
            get
            {
                return false;
            }
        }

        protected override void SetSpriteFromType(HazardType type)
        {
            base.SetSpriteFromType(type);
            switch (_direction)
            {
                case Direction.RIGHT:
                    _sprite.Image = _type == TurretType.KEEN5 ? Properties.Resources.keen5_laser_turret_right : Properties.Resources.keen6_laser_turret_right;
                    this.StandingTile = new PlatformTile(_collisionGrid, new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width - 10, this.HitBox.Height));
                    break;
                case Direction.LEFT:
                    _sprite.Image = _type == TurretType.KEEN5 ? Properties.Resources.keen5_laser_turret_left : Properties.Resources.keen6_laser_turret_left;
                    this.StandingTile = new PlatformTile(_collisionGrid, new Rectangle(this.HitBox.X + 10, this.HitBox.Y, this.HitBox.Width - 10, this.HitBox.Height));
                    break;
                case Direction.UP:
                    _sprite.Image = _type == TurretType.KEEN5 ? Properties.Resources.keen5_laser_turret_up : Properties.Resources.keen6_laser_turret_up;
                    break;
                case Direction.DOWN:
                    _sprite.Image = _type == TurretType.KEEN5 ? Properties.Resources.keen5_laser_turret_down : Properties.Resources.keen6_laser_turret_down;
                    break;
            }
            this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
        }

        public PlatformTile StandingTile
        {
            get;
            private set;
        }

        public virtual void Update()
        {
            if (_isActive)
            {
                if (_currentFireDelayTick++ == FIRE_DELAY)
                {
                    _currentFireDelayTick = 0;
                    this.Fire();
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
                    fireLocation = new Point(this.HitBox.X, this.HitBox.Top - 24);
                    fireHitbox = new Size(32, 32);
                    break;
                case Direction.DOWN:
                    fireLocation = new Point(this.HitBox.X, this.HitBox.Bottom - 8);
                    fireHitbox = new Size(32, 32);
                    break;
                case Direction.LEFT:
                    fireLocation = new Point(this.HitBox.Left - 24, this.HitBox.Y);
                    fireHitbox = new Size(32, 32);
                    break;
                case Direction.RIGHT:
                    fireLocation = new Point(this.HitBox.Right - 8, this.HitBox.Y);
                    fireHitbox = new Size(32, 32);
                    break;
            }

            StraightShotTrajectory trajectory = new StraightShotTrajectory(_collisionGrid, new Rectangle(fireLocation, fireHitbox), _direction, EnemyTrajectoryType.KEEN5_LASER_TURRET_SHOT);
            trajectory.Create += new EventHandler<KeenEventArgs.ObjectEventArgs>(trajectory_Create);
            trajectory.Remove += new EventHandler<ObjectEventArgs>(trajectory_Remove);
            ObjectEventArgs e = new ObjectEventArgs()
            {
                ObjectSprite = trajectory
            };
            OnCreate(e);
        }

        void trajectory_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void trajectory_Create(object sender, KeenEventArgs.ObjectEventArgs e)
        {
            OnCreate(e);
        }

        public bool IsFiring
        {
            get { return _isActive; }
        }

        public int Ammo
        {
            get { return -1; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private TurretType _type;

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

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_direction.ToString()}|{_isActive}|{_type.ToString()}";
        }
    }

    public enum TurretType
    {
        KEEN5,
        KEEN6
    }
}
