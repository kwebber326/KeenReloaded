using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using System.Windows.Forms;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Trajectories
{
    public class StraightShotTrajectory : CollisionObject, ITrajectory, ISprite, IUpdatable, ICreateRemove
    {
        private System.Windows.Forms.PictureBox _sprite;
        private EnemyTrajectoryType _trajectoryType;
        private Enums.Direction _direction;
        protected int _damage;
        protected int _velocity;
        protected int _pierce;
        protected int _spread;
        protected int _blastRadius;
        protected int _refireDelay;

        protected Image[] _shotSprites;
        protected Image[] _shotCompleteSprites;
        protected bool _shotComplete;
        protected int _spreadOffset;
        private int _currentShootSprite;
        private int _currentCompleteSprite;
        private int _currentSpriteDelay;
        private int UPDATE_SPRITE_DELAY = 1;

        public StraightShotTrajectory(SpaceHashGrid grid, Rectangle hitbox, Direction direction, EnemyTrajectoryType trajectoryType)
            : base(grid, hitbox)
        {
            this.Direction = direction;
            _trajectoryType = trajectoryType;
            Initialize();
        }

        private void Initialize()
        {
            InitializeTrajectories();
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            if (obj is DebugTile)
            {
                StopAtCollisionObject(obj);
            }
            else if (obj is CommanderKeen)
            {
                var keen = (CommanderKeen)obj;
                keen.Die();
                if (--_pierce < 0)
                {
                    StopAtCollisionObject(obj);
                }
            }
        }

        public int Damage
        {
            get { return _damage; }
        }

        public int Velocity
        {
            get { return _velocity; }
        }

        public int Pierce
        {
            get { return _pierce; }
        }

        public int Spread
        {
            get { return _spread; }
        }

        public int BlastRadius
        {
            get { return _blastRadius; }
        }

        public int RefireDelay
        {
            get { return _refireDelay; }
        }

        public bool KillsKeen
        {
            get { return true; }
        }

        public void Move()
        {
            int x = this.HitBox.X;
            int y = this.HitBox.Y;
            Rectangle newLocation = new Rectangle(x, y, this.HitBox.Width, this.HitBox.Height);

            switch (this.Direction)
            {
                case Enums.Direction.LEFT:
                    x -= _velocity;
                    newLocation = new Rectangle(x, y + _spreadOffset, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.RIGHT:
                    x += _velocity;
                    newLocation = new Rectangle(x, y + _spreadOffset, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.UP:
                    y -= _velocity;
                    newLocation = new Rectangle(x + _spreadOffset, y, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.DOWN:
                    y += _velocity;
                    newLocation = new Rectangle(x + _spreadOffset, y, this.HitBox.Width, this.HitBox.Height);
                    break;
            }

            this.HitBox = newLocation;
            if (_sprite == null)
            {
                _sprite = new PictureBox();

                _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            }
            _sprite.Location = this.HitBox.Location;
            UpdateSprite();
            this.UpdateCollisionNodes(this.Direction);
        }

        public virtual void Stop()
        {
            _shotComplete = true;
            UpdateSprite();
        }

        protected virtual void InitializeTrajectories()
        {
            _sprite = new PictureBox();


            switch (_trajectoryType)
            {
                case EnemyTrajectoryType.KEEN4_SPRITE_SHOT:
                    UPDATE_SPRITE_DELAY = 0;
                    _velocity = 70;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 0;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen4_sprite_shot1,
                        Properties.Resources.keen4_sprite_shot2,
                        Properties.Resources.keen4_sprite_shot3,
                        Properties.Resources.keen4_sprite_shot4
                    };

                    _shotCompleteSprites = new Image[]
                    {
                        Properties.Resources.keen4_sprite_shot1
                    };
                    break;
                case EnemyTrajectoryType.KEEN5_LASER_TURRET_SHOT:
                    UPDATE_SPRITE_DELAY = 0;
                    _velocity = 70;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 0;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen5_turret_laser1,
                        Properties.Resources.keen5_turret_laser2,
                        Properties.Resources.keen5_turret_laser3,
                        Properties.Resources.keen5_turret_laser4
                    };

                    _shotCompleteSprites = new Image[]
                    {
                        Properties.Resources.keen5_turret_laser_hit1,
                        Properties.Resources.keen5_turret_laser_hit2
                    };
                    break;
                case EnemyTrajectoryType.KEEN5_ROBO_RED_SHOT:
                     UPDATE_SPRITE_DELAY = 0;
                    _velocity = 70;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 3;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen5_robo_red_shot1,
                        Properties.Resources.keen5_robo_red_shot2
                    };

                    _shotCompleteSprites = new Image[]
                    {
                        Properties.Resources.keen5_robo_red_shot_hit1,
                        Properties.Resources.keen5_robo_red_shot_hit2
                    };
                    break;
                case EnemyTrajectoryType.KEEN5_SHOCKSHUND_SHOT:
                      UPDATE_SPRITE_DELAY = 0;
                    _velocity = 60;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 0;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen5_shockshund_shot1,
                        Properties.Resources.keen5_shockshund_shot2
                    };

                    _shotCompleteSprites = new Image[]
                    {
                        Properties.Resources.keen5_shockshund_shot_hit1,
                        Properties.Resources.keen5_shockshund_shot_hit2
                    };
                    break;
                case EnemyTrajectoryType.KEEN5_SHIKADI_SHOCK:
                    UPDATE_SPRITE_DELAY = 0;
                    _velocity = 14;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 0;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen5_standard_shikadi_electricity1
                    };

                    _shotCompleteSprites = new Image[]
                    {
                        Properties.Resources.keen5_standard_shikadi_electricity2
                    };
                    break;
                case EnemyTrajectoryType.KEEN6_BOBBA_SHOT:
                    UPDATE_SPRITE_DELAY = 0;
                    _velocity = 35;
                    _pierce = 0;
                    _damage = int.MaxValue;
                    _refireDelay = -1;
                    _spread = 0;
                    _blastRadius = 0;
                    _shotSprites = new Image[] 
                    {
                        Properties.Resources.keen6_bobba_fireball1,
                        Properties.Resources.keen6_bobba_fireball2,
                        Properties.Resources.keen6_bobba_fireball3,
                        Properties.Resources.keen6_bobba_fireball4
                    };

                    _shotCompleteSprites = new Image[]
                    {
                         Properties.Resources.keen6_bobba_fireball1
                    };
                    break;
            }

            _sprite.Image = _shotSprites[_currentShootSprite];
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
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
                _direction = value;
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }
        protected void GetSpreadOffset()
        {
            Random random = new Random();
            _spreadOffset = random.Next(_spread * -1, _spread + 1);
        }

        protected Rectangle GetAreaToCheckForCollision()
        {
            int x = this.HitBox.X;
            int y = this.HitBox.Y;
            Rectangle areaToCheck = new Rectangle(x, y, this.HitBox.Width, this.HitBox.Height);

            switch (this.Direction)
            {
                case Enums.Direction.LEFT:
                    areaToCheck = new Rectangle(x - _velocity, y + _spreadOffset, this.HitBox.Width + _velocity, this.HitBox.Height);
                    break;
                case Enums.Direction.RIGHT:
                    areaToCheck = new Rectangle(x, y + _spreadOffset, this.HitBox.Width + _velocity, this.HitBox.Height);
                    break;
                case Enums.Direction.UP:
                    areaToCheck = new Rectangle(x + _spreadOffset, y - _velocity, this.HitBox.Width, this.HitBox.Height + _velocity);
                    break;
                case Enums.Direction.DOWN:
                    areaToCheck = new Rectangle(x + _spreadOffset, y, this.HitBox.Width, this.HitBox.Height + _velocity);
                    break;
            }

            return areaToCheck;
        }

        public virtual void Update()
        {
            if (!_shotComplete)
            {
                GetSpreadOffset();
                var areaToCheck = GetAreaToCheckForCollision();
                var collisionObjects = this.CheckCollision(areaToCheck);
                var debugTiles = collisionObjects.OfType<DebugTile>();
                var keens = collisionObjects.OfType<CommanderKeen>().ToList();
                var itemsToCheck = new List<CollisionObject>();
                itemsToCheck.AddRange(debugTiles);
                foreach (var keen in keens)
                {
                    if (keen is CollisionObject)
                    {
                        var item = keen as CollisionObject;
                        if (item != null)
                            itemsToCheck.Add(item);
                    }
                }
                if (itemsToCheck.Any())
                {
                    HandleCollisionByDirection(collisionObjects);
                }
                else
                {
                    this.Move();
                }
            }
            else if (_shotCompleteSprites != null && _shotCompleteSprites.Any())
            {
                UpdateSprite();
            }
            else
            {
                this.UpdateCollisionNodes(this.Direction);
                CleanUpCollisionNodes();
                OnObjectComplete();
            }
        }

        protected virtual void UpdateSprite()
        {
            if (_currentSpriteDelay == UPDATE_SPRITE_DELAY)
            {
                if (!_shotComplete)
                {
                    if (_currentShootSprite < _shotSprites.Length - 1)
                    {
                        _currentShootSprite++;
                    }
                    else
                    {
                        _currentShootSprite = 0;
                    }
                    this.Sprite.Image = _shotSprites[_currentShootSprite];
                }
                else if (_currentCompleteSprite <= _shotCompleteSprites.Length - 1)
                {
                    this.Sprite.Image = _shotCompleteSprites[_currentCompleteSprite++];
                }
                else
                {
                    this.Sprite.Image = null;
                    CleanUpCollisionNodes();
                    OnObjectComplete();
                }

                _currentSpriteDelay = 0;
            }
            else
            {
                _currentSpriteDelay++;
            }
        }

        protected void CleanUpCollisionNodes()
        {
            if (_collidingNodes != null)
            {
                foreach (var node in _collidingNodes)
                {
                    node.Objects.Remove(this);
                }
            }
        }

        protected void OnObjectComplete()
        {
            if (Remove != null)
            {
                ObjectEventArgs e = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                foreach (var node in _collidingNodes)
                {
                    node.NonEnemies.Remove(this);
                    node.Objects.Remove(this);
                }
                Remove(this, e);
            }
        }

        protected void OnCreate()
        {
            if (this.Create != null)
            {
                ObjectEventArgs e = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                this.Create(this, e);
            }
        }

        protected void HandleCollisionByDirection(IEnumerable<CollisionObject> collisions)
        {
            switch (Direction)
            {
                case Enums.Direction.DOWN:
                    //int minY = collisions.Select(c => c.HitBox.Top).Min();
                    //var cDown = collisions.FirstOrDefault(c => c.HitBox.Top == minY);
                    //this.HitBox = new Rectangle(new Point(this.HitBox.X, cDown.HitBox.Top - this.HitBox.Height), this.HitBox.Size);
                    //this.HandleCollision(cDown);
                    collisions = collisions.OrderBy(c => c.HitBox.Top).ToList();
                    break;
                case Enums.Direction.UP:
                    //int maxY = collisions.Select(c => c.HitBox.Bottom).Max();
                    //var cUp = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxY);
                    //this.HitBox = new Rectangle(new Point(this.HitBox.X, cUp.HitBox.Bottom + 1), this.HitBox.Size);
                    //this.HandleCollision(cUp);
                    collisions = collisions.OrderByDescending(c => c.HitBox.Bottom).ToList();
                    break;
                case Enums.Direction.LEFT:
                    //int maxX = debugTiles.Select(c => c.HitBox.Right).Max();
                    //var cRight = debugTiles.FirstOrDefault(c => c.HitBox.Right == maxX);
                    //this.HitBox = new Rectangle(new Point(cRight.HitBox.Right + 1, this.HitBox.Y), this.HitBox.Size);
                    //this.HandleCollision(cRight);
                    collisions = collisions.OrderByDescending(c => c.HitBox.Right).ToList();
                    break;
                case Enums.Direction.RIGHT:
                    //int minX = collisions.Select(c => c.HitBox.Left).Min();
                    //var cLeft = collisions.FirstOrDefault(c => c.HitBox.Left == minX);
                    //this.HitBox = new Rectangle(new Point(cLeft.HitBox.Left - this.HitBox.Width, this.HitBox.Y), this.HitBox.Size);
                    //this.HandleCollision(cLeft);
                    collisions = collisions.OrderBy(c => c.HitBox.Left).ToList();
                    break;
            }
            bool handledDebugTileCollision = false;
            foreach (var collision in collisions)
            {
                bool handle = !handledDebugTileCollision;
                if (handle)
                {
                    if (collision is DebugTile)
                        handledDebugTileCollision = true;
                    this.HandleCollision(collision);
                }
            }
            UpdateCollisionNodes(this.Direction);
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
                if (this.Sprite != null)
                {
                    this.Sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(this.Direction);
                }
            }
        }

        protected void StopAtCollisionObject(CollisionObject obj)
        {
            SetLocationByCollision(obj);
            this.Stop();
        }

        private void SetLocationByCollision(CollisionObject obj)
        {
            switch (Direction)
            {
                case Enums.Direction.LEFT:
                    this.HitBox = new Rectangle(obj.HitBox.Right, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.RIGHT:
                    this.HitBox = new Rectangle(obj.HitBox.Left - this.HitBox.Width, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.UP:
                    this.HitBox = new Rectangle(this.HitBox.X, obj.HitBox.Bottom, this.HitBox.Width, this.HitBox.Height);
                    break;
                case Enums.Direction.DOWN:
                    this.HitBox = new Rectangle(this.HitBox.X, obj.HitBox.Top - this.HitBox.Height, this.HitBox.Width, this.HitBox.Height);
                    break;
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
    }
}
