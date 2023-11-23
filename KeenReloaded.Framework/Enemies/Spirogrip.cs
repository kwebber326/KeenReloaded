using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class Spirogrip : CollisionObject, IEnemy, IUpdatable, ISprite
    {
        private Direction _direction;

        private int _currentRotateSprite;
        private const int ROTATE_SPRITE_CHANGE_DELAY = 1;
        private int _rotateSpriteChangeDelayTick;
        private Point _originalPos;
        private const int ATTACK_CHANCE = 80;
        private const int ATTACK_SPEED = 45;
        private const int WAIT_TIME = 25;
        private int _currentWaitTime;
        private const int BACK_UP_LENGTH = 100;
        private const int BACK_UP_SPEED = 10;
        private readonly int _backupTime = BACK_UP_LENGTH / BACK_UP_SPEED;
        private int _currentBackupTime;
        private const int WAIT_TIME_BEFORE_ROTATE = 15;
        private int _currentRotateWaitTimeTick;

        private Random _random = new Random();
        private Image[] _rotateSprites = new Image[]
        {
            Properties.Resources.keen5_spirogrip_rotate1,
            Properties.Resources.keen5_spirogrip_rotate2,
            Properties.Resources.keen5_spirogrip_rotate3,
            Properties.Resources.keen5_spirogrip_rotate4,
            Properties.Resources.keen5_spirogrip_rotate5,
            Properties.Resources.keen5_spirogrip_rotate6,
            Properties.Resources.keen5_spirogrip_rotate7,
            Properties.Resources.keen5_spirogrip_rotate8
        };

        private Point[] _rotateOffsets = new Point[]
        {
            new Point(-28, 0),
            new Point(-4, 0),
            new Point(4, -26),
            new Point(26, -6),
            new Point(2, 4),
            new Point(0, 26),
            new Point(0, 2),
            new Point(0, 0)
        };

        public Spirogrip(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
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
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = _rotateSprites[_currentRotateSprite];
            _random = new Random();
            _originalPos = new Point(this.HitBox.X, this.HitBox.Y);
            this.Rotate();
        }

        private System.Windows.Forms.PictureBox _sprite;
        private CommanderKeen _keen;
        private SpiroGripState _state;
        protected override void HandleCollision(CollisionObject obj)
        {

        }
        Direction Direction
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

        SpiroGripState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            switch (_state)
            {
                case SpiroGripState.ROTATING:
                    SetLocationOffsetsFromRotationValue();
                    if (++_currentRotateSprite >= _rotateSprites.Length)
                    {
                        _currentRotateSprite = 0;
                    }
                    _sprite.Image = _rotateSprites[_currentRotateSprite];
                    break;
                case SpiroGripState.ATTACKING:
                    SetRandomDirection();
                    break;
            }
            this.HitBox = new Rectangle(_sprite.Location, _sprite.Size);
        }

        private void SetLocationOffsetsFromRotationValue()
        {
            var location = _sprite.Location;
            location.Offset(_rotateOffsets[_currentRotateSprite]);
            _sprite.Location = location;
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
                    //this.UpdateCollisionNodes(this.Direction);
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }


        public bool DeadlyTouch
        {
            get { return true; }
        }

        public void HandleHit(ITrajectory trajectory)
        {

        }

        public bool IsActive
        {
            get { return true; }
        }

        public void Update()
        {
            switch (_state)
            {
                case SpiroGripState.ROTATING:
                    this.Rotate();
                    break;
                case SpiroGripState.ATTACKING:
                    this.Attack();
                    break;
                case SpiroGripState.STOPPED:
                    this.Wait();
                    break;
                case SpiroGripState.BACKING_UP:
                    this.BackUp();
                    break;
                case SpiroGripState.WAIT_BEFORE_ROTATE:
                    this.WaitBeforeRotate();
                    break;
            }
        }

        private void WaitBeforeRotate()
        {
            if (this.State != SpiroGripState.WAIT_BEFORE_ROTATE)
            {
                this.State = SpiroGripState.WAIT_BEFORE_ROTATE;
            }

            if (_currentRotateWaitTimeTick++ == WAIT_TIME_BEFORE_ROTATE)
            {
                _currentRotateWaitTimeTick = 0;
                this.Rotate();
            }
        }

        private void BackUp()
        {
            if (this.State != SpiroGripState.BACKING_UP)
            {
                this.State = SpiroGripState.BACKING_UP;
                ReverseDirection();
            }

            int xOffset = 0, yOffset = 0;
            switch (_direction)
            {
                case Enums.Direction.LEFT:
                    xOffset = BACK_UP_SPEED * -1;
                    break;
                case Enums.Direction.RIGHT:
                    xOffset = BACK_UP_SPEED;
                    break;
                case Enums.Direction.UP:
                    yOffset = BACK_UP_SPEED * -1;
                    break;
                case Enums.Direction.DOWN:
                    yOffset = BACK_UP_SPEED;
                    break;
            }
            int xPos = this.HitBox.X, yPos = this.HitBox.Y;
            int xWidth = this.HitBox.Width, yHeight = this.HitBox.Height;
            if (this.Direction == Enums.Direction.LEFT)
            {
                xPos += xOffset;
            }
            else if (this.Direction == Enums.Direction.UP)
            {
                yPos += yOffset;
            }
            if (IsHorizontalDirection(this.Direction))
            {
                xWidth += BACK_UP_SPEED;
            }
            else if (IsVerticalDirection(this.Direction))
            {
                yHeight += BACK_UP_SPEED;
            }

            Rectangle areaToCheck = new Rectangle(xPos, yPos, xWidth, yHeight);
            var collisions = this.CheckCollision(areaToCheck, true);
            CollisionObject tile = null;
            if (IsVerticalDirection(this.Direction))
            {
                tile = this.Direction == Enums.Direction.UP ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
                if (tile != null)
                {
                    int yCollidePos = this.Direction == Enums.Direction.UP ? tile.HitBox.Bottom + 1 : tile.HitBox.Top - this.HitBox.Height - 1;
                    this.HitBox = new Rectangle(this.HitBox.X, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
                    }
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(areaToCheck))
                    {
                        _keen.Die();
                    }
                }
            }
            else if (IsHorizontalDirection(this.Direction))
            {
                tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
                if (tile != null)
                {
                    int xCollidePos = this.Direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                    this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
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
            if (_currentBackupTime++ == _backupTime)
            {
                _currentBackupTime = 0;
                this.WaitBeforeRotate();
            }
        }

        private void ReverseDirection()
        {
            if (IsHorizontalDirection(this.Direction))
            {
                this.Direction = ChangeHorizontalDirection(_direction);
            }
            else if (IsVerticalDirection(this.Direction))
            {
                this.Direction = ChangeVerticalDirection(_direction);
            }
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => c is DebugTile && c.HitBox.Top < this.HitBox.Top && c.HitBox.Left <= this.HitBox.Right && c.HitBox.Right >= this.HitBox.Left ).ToList();
            if (debugTiles.Any())
            {
                int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
                CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
                return obj;
            }
            return null;
        }

        private void Wait()
        {
            if (this.State != SpiroGripState.STOPPED)
            {
                this.State = SpiroGripState.STOPPED;
            }

            if (_currentWaitTime++ == WAIT_TIME)
            {
                _currentWaitTime = 0;
                this.BackUp();
            }
        }

        private void Attack()
        {
            if (this.State != SpiroGripState.ATTACKING)
            {
                this.HitBox = new Rectangle(_originalPos, this.HitBox.Size);
                this.State = SpiroGripState.ATTACKING;
            }

            int xOffset = 0, yOffset = 0;
            switch (_direction)
            {
                case Enums.Direction.LEFT:
                    xOffset = ATTACK_SPEED * -1;
                    break;
                case Enums.Direction.RIGHT:
                    xOffset = ATTACK_SPEED;
                    break;
                case Enums.Direction.UP:
                    yOffset = ATTACK_SPEED * -1;
                    break;
                case Enums.Direction.DOWN:
                    yOffset = ATTACK_SPEED;
                    break;
            }
            int xPos = this.HitBox.X, yPos = this.HitBox.Y;
            int xWidth = this.HitBox.Width, yHeight = this.HitBox.Height;
            if (this.Direction == Enums.Direction.LEFT)
            {
                xPos += xOffset;
            }
            else if (this.Direction == Enums.Direction.UP)
            {
                yPos += yOffset;
            }
            if (IsHorizontalDirection(this.Direction))
            {
                xWidth += ATTACK_SPEED;
            }
            else if (IsVerticalDirection(this.Direction))
            {
                yHeight += ATTACK_SPEED;
            }

            Rectangle areaToCheck = new Rectangle(xPos, yPos, xWidth, yHeight);
            var collisions = this.CheckCollision(areaToCheck, true);

            CollisionObject tile = null;

            if (IsVerticalDirection(this.Direction))
            {
                tile = this.Direction == Enums.Direction.UP ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);
                if (tile != null)
                {
                    int yCollidePos = this.Direction == Enums.Direction.UP ? tile.HitBox.Bottom + 1 : tile.HitBox.Top - this.HitBox.Height - 1;
                    this.HitBox = new Rectangle(this.HitBox.X, yCollidePos, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
                    }
                    this.Wait();
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(areaToCheck))
                    {
                        _keen.Die();
                    }
                }
            }
            else if (IsHorizontalDirection(this.Direction))
            {
                tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
                if (tile != null)
                {
                    int xCollidePos = this.Direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                    this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    if (_keen.HitBox.IntersectsWith(this.HitBox))
                    {
                        _keen.Die();
                    }
                    this.Wait();
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
        }

        private void SetRandomDirection()
        {
            int directionVal = _random.Next(1, 5);
            switch (directionVal)
            {
                case 1:
                    this.Direction = Enums.Direction.LEFT;
                    _sprite.Image = Properties.Resources.keen5_spirogrip_left;
                    break;
                case 2:
                    this.Direction = Enums.Direction.RIGHT;
                    _sprite.Image = Properties.Resources.keen5_spirogrip_right;
                    break;
                case 3:
                    this.Direction = Enums.Direction.UP;
                    _sprite.Image = Properties.Resources.keen5_spirogrip_up;
                    break;
                case 4:
                    this.Direction = Enums.Direction.DOWN;
                    _sprite.Image = Properties.Resources.keen5_spirogrip_down;
                    break;
            }
        }

        private void SetCurrentRotateSpriteFromDirection()
        {
            switch (_direction)
            {
                case Direction.UP:
                    _currentRotateSprite = 4;
                    break;
                case Direction.DOWN:
                    _currentRotateSprite = 0;
                    break;
                case Direction.LEFT:
                    _currentRotateSprite = 2;
                    break;
                case Direction.RIGHT:
                    _currentRotateSprite = 6;
                    break;
            }
        }

        private void Rotate()
        {
            if (this.State != SpiroGripState.ROTATING)
            {
                _originalPos = new Point(this.HitBox.X, this.HitBox.Y);
                ReverseDirection();
                SetCurrentRotateSpriteFromDirection();
                this.State = SpiroGripState.ROTATING;
                _rotateSpriteChangeDelayTick = 0;
            }

            if (_rotateSpriteChangeDelayTick++ == ROTATE_SPRITE_CHANGE_DELAY)
            {
                _rotateSpriteChangeDelayTick = 0;
                UpdateSprite();
                _random = new Random();
                int attackVal = _random.Next(1, ATTACK_CHANCE + 1);
                if (attackVal == ATTACK_CHANCE)
                {
                    this.Attack();
                }
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum SpiroGripState
    {
        ROTATING,
        ATTACKING,
        STOPPED,
        BACKING_UP,
        WAIT_BEFORE_ROTATE
    }
}
