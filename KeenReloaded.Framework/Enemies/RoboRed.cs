using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Trajectories;

namespace KeenReloaded.Framework.Enemies
{
    public class RoboRed : CollisionObject, IUpdatable, ISprite, IEnemy, IFireable, ICreateRemove
    {
        private Direction _direction;

        public RoboRed(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen) :
            base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            this.BasicFall(FALL_VELOCITY);
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        protected virtual void BasicFall(int fallVelocity)
        {
            if (this.State != RoboRedState.FALLING)
            {
                this.State = RoboRedState.FALLING;
            }
            var landingTile = this.GetTopMostLandingTile(fallVelocity);
            if (landingTile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                this.Patrol();
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + fallVelocity, this.HitBox.Width, this.HitBox.Height);
            }
        }

        public void Update()
        {
            switch (this.State)
            {
                case RoboRedState.FIRING:
                    this.Fire();
                    break;
                case RoboRedState.PATROLLING:
                    this.Patrol();
                    break;
                case RoboRedState.ALERTED:
                    this.Alert();
                    break;
                case RoboRedState.FALLING:
                    this.BasicFall(FALL_VELOCITY);
                    break;
            }
        }

        public void Alert()
        {
            if (this.State != RoboRedState.ALERTED)
            {
                this.State = RoboRedState.ALERTED;
                _currentFireDelayTick = 0;
                this.Direction = this.SetDirectionFromObjectHorizontal(_keen, true);
            }
            if (_currentFireDelayTick++ == FIRE_DELAY)
            {
                _currentFireDelayTick = 0;
                this.Fire();
            }
        }

        private void Patrol()
        {
            if (this.State != RoboRedState.PATROLLING)
            {
                this.State = RoboRedState.PATROLLING;
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(FALL_VELOCITY);
                return;
            }

            if (_keen.HitBox.Right >= this.HitBox.Left - X_HEARING_DISTANCE
                && _keen.HitBox.Left <= this.HitBox.Right + X_HEARING_DISTANCE
                && (_keen.MoveState == MoveState.RUNNING || _keen.MoveState == MoveState.JUMPING || _keen.MoveState == MoveState.FALLING || _keen.IsFiring))
            {
                int _alertVal = _random.Next(1, SHOT_CHANCE_UPON_HEARING_NOISE + 1);
                if (_alertVal == SHOT_CHANCE_UPON_HEARING_NOISE)
                {
                    this.Alert();
                    return;
                }
            }

            int xOffset = this.Direction == Enums.Direction.RIGHT ? MOVE_VELOCITY : MOVE_VELOCITY * -1;
            int xPosCollisionCheck = this.Direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xPosCollisionCheck, this.HitBox.Y, this.HitBox.Width + MOVE_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (this.IsOnEdge(this.Direction))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                return;
            }

            if (tile != null)
            {
                int xPos = this.Direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    if (_direction == Enums.Direction.LEFT
                       && _keen.HitBox.Right >= this.HitBox.Left)
                    {
                        _keen.Die();
                    }
                    else if (_direction == Enums.Direction.RIGHT
                        && _keen.HitBox.Left <= this.HitBox.Right)
                    {
                        _keen.Die();
                    }
                }
            }
            else
            {
                this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
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
                if (_sprite != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(this.Direction);
                }
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
                _direction = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            switch (_direction)
            {
                case Enums.Direction.LEFT:
                    _sprite.Image = Properties.Resources.keen5_robo_red_left;
                    break;
                case Enums.Direction.RIGHT:
                    _sprite.Image = Properties.Resources.keen5_robo_red_right;
                    break;
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return true; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            if (this.State != RoboRedState.ALERTED && this.State != RoboRedState.FIRING)
                this.Alert();
        }

        public bool IsActive
        {
            get { return true; }
        }

        public void Fire()
        {
            if (this.State != RoboRedState.FIRING)
            {
                this.State = RoboRedState.FIRING;
                _currentFireTimeTick = 0;
                _shotNum = 1;
                _fireTick = 0;
            }

            if (_currentFireTimeTick++ == FIRE_TIME)
            {
                _currentFireTimeTick = 0;
                this.Patrol();
            }
            else if (_fireTick++ == _fireRate)
            {
                _fireTick = 0;
                bool upDirection = (_shotNum++ % 2 == 0);
                Direction shotDirectionUp = Enums.Direction.UP_LEFT;
                Direction shotDirectionDown = Enums.Direction.DOWN_LEFT;
                shotDirectionUp = this.Direction == Enums.Direction.LEFT ? Direction.UP_LEFT : Enums.Direction.UP_RIGHT;
                shotDirectionDown = this.Direction == Enums.Direction.LEFT ? Direction.DOWN_LEFT : Enums.Direction.DOWN_RIGHT;
                int xPos = this.Direction == Enums.Direction.LEFT ? this.HitBox.Left : this.HitBox.Right;

                //upshot
                RoboRedShot shot1 = new RoboRedShot(_collisionGrid, new Rectangle(xPos, this.HitBox.Y + 68, RoboRedShot.DEFAULT_WIDTH, RoboRedShot.DEFAULT_HEIGHT), shotDirectionUp);
                shot1.Create += new EventHandler<ObjectEventArgs>(shot_Create);
                shot1.Remove += new EventHandler<ObjectEventArgs>(shot_Remove);
                ObjectEventArgs args1 = new ObjectEventArgs()
                {
                    ObjectSprite = shot1
                };
                OnCreate(args1);
                //dowmshot
                RoboRedShot shot2 = new RoboRedShot(_collisionGrid, new Rectangle(xPos, this.HitBox.Y + 68, RoboRedShot.DEFAULT_WIDTH, RoboRedShot.DEFAULT_HEIGHT), shotDirectionDown);
                shot2.Create += new EventHandler<ObjectEventArgs>(shot_Create);
                shot2.Remove += new EventHandler<ObjectEventArgs>(shot_Remove);
                ObjectEventArgs args2 = new ObjectEventArgs()
                {
                    ObjectSprite = shot2
                };
                OnCreate(args2);
            }

            PerformKickBack();
        }

        private void PerformKickBack()
        {
            if (_kickBackDelayTick++ == KICK_BACK_DELAY)
            {
                _kickBackDelayTick = 0;
                if (_returningFromKickback)
                {
                    int kickBack = this.Direction == Enums.Direction.LEFT ? KICK_BACK_LENGTH : KICK_BACK_LENGTH * -1;
                    _sprite.Location = new Point(_sprite.Location.X + kickBack, _sprite.Location.Y);
                }
                else
                {
                    int kickBack = this.Direction == Enums.Direction.LEFT ? KICK_BACK_LENGTH * -1 : KICK_BACK_LENGTH;
                    _sprite.Location = new Point(_sprite.Location.X + kickBack, _sprite.Location.Y);
                }
                _returningFromKickback = !_returningFromKickback;
            }
        }

        void shot_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void shot_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        RoboRedState State
        {
            get;
            set;
        }

        public bool IsFiring
        {
            get { return this.State == RoboRedState.FIRING; }
        }

        public int Ammo
        {
            get { return -1; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private CommanderKeen _keen;
        private System.Windows.Forms.PictureBox _sprite;
        private const int FALL_VELOCITY = 20;
        private const int MOVE_VELOCITY = 5;
        private const int FIRE_DELAY = 15;
        private int _currentFireDelayTick;

        private const int FIRE_TIME = 30;
        private int _currentFireTimeTick;
        private const int SHOTS_PER_FIRE_TIME = 10;
        private int _fireRate = FIRE_TIME / SHOTS_PER_FIRE_TIME;
        private int _shotNum;
        private int _fireTick;

        private const int KICK_BACK_LENGTH = 4;
        private const int KICK_BACK_DELAY = 0;
        private int _kickBackDelayTick;
        private bool _returningFromKickback;
        private const int X_HEARING_DISTANCE = 400;
        private const int SHOT_CHANCE_UPON_HEARING_NOISE = 80;//set this to lower value for higher probability

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

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum RoboRedState
    {
        PATROLLING,
        ALERTED,
        FIRING,
        FALLING
    }
}
