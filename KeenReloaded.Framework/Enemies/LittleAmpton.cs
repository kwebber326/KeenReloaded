using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Enemies
{
    public class LittleAmpton : DestructibleObject, IUpdatable, ISprite, IEnemy, IZombieBountyEnemy
    {
        private System.Windows.Forms.PictureBox _sprite;
        private LittleAmptonState _state;
        private Pole _currentPole;
        private Keen5ControlPanel _currentControlPanel;
        private Direction _poleClimbDirection = Direction.UP;

        private const int FALL_VELOCITY = 30;
        private const int MOVE_VELOCITY = 7;
        private const int POLE_CLIMB_VELOCITY = 20;
        private const int POLE_CLIMB_VERTICAL_OFFSET = 32;

        private int _currentCalibrateSprite;
        private Dictionary<int, Image> _calibrateStateSprites = SpriteSheet.Keen5LittleAmptonCalibrationImages;

        private int _currentMoveSprite;
        private Image[] _moveLeftSprites = new Image[]{
            Properties.Resources.keen5_little_ampton_left1,
            Properties.Resources.keen5_little_ampton_left2,
            Properties.Resources.keen5_little_ampton_left3,
            Properties.Resources.keen5_little_ampton_left4
        };

        private Image[] _moveRightSprites = new Image[]{
            Properties.Resources.keen5_little_ampton_right1,
            Properties.Resources.keen5_little_ampton_right2,
            Properties.Resources.keen5_little_ampton_right3,
            Properties.Resources.keen5_little_ampton_right4
        };

        private int _currentStunnedSprite;
        private Image[] _stunnedSprites = new Image[]{
            Properties.Resources.keen5_little_ampton_stunned1,
            Properties.Resources.keen5_little_ampton_stunned2,
            Properties.Resources.keen5_little_ampton_stunned3,
            Properties.Resources.keen5_little_ampton_stunned4
        };
        private Enums.Direction _direction;
        private CommanderKeen _keen;

        private int _currentWalkSpriteChangeDelayTick;
        private const int WALK_SPRITE_CHANGE_DELAY = 2;

        private int _currentChargeSpriteChangeDelayTick;
        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;

        private bool _crossedPoleTile;

        public LittleAmpton(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("Keen was not properly set");
            }
            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.Move(MOVE_VELOCITY);
            _sprite.Location = this.HitBox.Location;
            this.Health = 1;
            int rand = _random.Next(0, 2);
            this.Direction = rand == 0 ? Enums.Direction.LEFT : Enums.Direction.RIGHT;
        }
        public override void Die()
        {
            this.UpdateStunnedState();
        }

        public void Update()
        {
            switch (_state)
            {
                case LittleAmptonState.ON_POLE:
                    this.MoveOnPole(_currentPole);
                    break;
                case LittleAmptonState.MOVING:
                    this.Move(MOVE_VELOCITY);
                    break;
                case LittleAmptonState.LOOKING:
                    this.Look();
                    break;
                case LittleAmptonState.CALIBRATING:
                    this.CalibrateMachine();
                    break;
                case LittleAmptonState.STUNNED:
                    this.UpdateStunnedState();
                    break;
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != LittleAmptonState.STUNNED)
            {
                this.State = LittleAmptonState.STUNNED;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                return;
            }

            var spriteIndex = _currentStunnedSprite;
            this.UpdateSpriteByDelay(ref _currentChargeSpriteChangeDelayTick, ref _currentStunnedSprite, STUNNED_SPRITE_CHANGE_DELAY);
            if (_currentStunnedSprite != spriteIndex)
            {
                var image = _stunnedSprites[_currentStunnedSprite];
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - (image.Size.Height - this.HitBox.Height), image.Width, image.Height);
            }

            if (IsNothingBeneath())
            {
                BasicFall(FALL_VELOCITY);
            }
        }

        private void CalibrateMachine()
        {
            if (this.State != LittleAmptonState.CALIBRATING)
            {
                this.State = LittleAmptonState.CALIBRATING;
                _currentCalibrateSprite = 0;
            }

            if (++_currentCalibrateSprite >= _calibrateStateSprites.Count)
            {
                _currentCalibrateSprite = 0;
                this.Move(MOVE_VELOCITY);
                return;
            }

            _sprite.Image = _calibrateStateSprites[_currentCalibrateSprite];
        }

        private void Look()
        {
            if (this.State != LittleAmptonState.LOOKING)
            {
                this.State = LittleAmptonState.LOOKING;
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
                    this.UpdateCollisionNodes(this.Direction);
                }
            }
        }

        protected override Direction ChangeHorizontalDirection(Direction direction)
        {
            _currentPole = null;
            _currentControlPanel = null;
            _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
            return base.ChangeHorizontalDirection(direction);
        }

        private void Move(int velocity)
        {
            if (this.State != LittleAmptonState.MOVING)
            {
                this.State = LittleAmptonState.MOVING;
                _crossedPoleTile = false;
            }



            bool nothingBeneath = IsNothingBeneath();
            if (nothingBeneath)
            {
                this.BasicFall(FALL_VELOCITY);
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
            }
            else
            {
                if (IsOnEdge(this.Direction, 3))
                {
                    this.Direction = this.ChangeHorizontalDirection(this.Direction);
                    return;
                }

                int xOffset = this.Direction == Enums.Direction.LEFT ? velocity * -1 : velocity;
                int xPos = this.Direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
                Rectangle areaToCheck = new Rectangle(xPos, this.HitBox.Y, this.HitBox.Width + velocity, this.HitBox.Height);
                var collisions = this.CheckCollision(areaToCheck);
                //pole collisions
                var poles = collisions.OfType<Pole>();
                var panels = collisions.OfType<Keen5ControlPanel>();

                var tile = this.Direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

                if (tile != null)
                {
                    if (this.Direction == Enums.Direction.LEFT)
                    {
                        xOffset = this.HitBox.X - (tile.HitBox.Right + 1);
                        this.HitBox = new Rectangle(tile.HitBox.Right + 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                    else
                    {
                        xOffset = tile.HitBox.Left - this.HitBox.Width - 1 - this.HitBox.X;
                        this.HitBox = new Rectangle(tile.HitBox.Left - this.HitBox.Width - 1, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                    }
                    this.Direction = this.ChangeHorizontalDirection(this.Direction);
                }
                else if (poles.Any(p => p != _currentPole))
                {
                    int closestX = this.HitBox.X;
                    poles = poles.Where(p => (this.HitBox.Bottom - POLE_CLIMB_VERTICAL_OFFSET) <= p.HitBox.Bottom).ToList();
                    if (poles.Any())
                    {
                        if (this.Direction == Enums.Direction.LEFT)
                        {
                            closestX = poles.Select(p => p.HitBox.Right).Max();
                        }
                        else
                        {
                            closestX = poles.Select(p => p.HitBox.Right).Min();
                        }
                        this.HitBox = new Rectangle(closestX - this.HitBox.Width / 2, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        var pole = poles.FirstOrDefault(p => p.HitBox.Right == closestX);
                        if (pole != null)
                            this.MoveOnPole(pole);
                    }
                    else
                    {
                        ExecuteFreeMoveLogic(xOffset, areaToCheck);
                    }
                }
                else if (panels.Any(p => p != _currentControlPanel))
                {
                    int closestX = this.HitBox.X;
                    if (this.Direction == Enums.Direction.LEFT)
                    {
                        closestX = panels.Select(p => p.HitBox.Right).Max();
                    }
                    else
                    {
                        closestX = panels.Select(p => p.HitBox.Right).Min();
                    }

                    var panel = panels.FirstOrDefault(p => p.HitBox.Right == closestX);
                    if (panel != null)
                    {
                        this.HitBox = new Rectangle((panel.HitBox.X + panel.HitBox.Width / 2) - (this.HitBox.Width / 2), this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        this.BeingMachineCalibration(panel);
                        ExecuteFreeMoveLogic(xOffset, areaToCheck);
                    }
                }
                else
                {
                    ExecuteFreeMoveLogic(xOffset, areaToCheck);
                }
            }
        }

        private void ExecuteFreeMoveLogic(int xOffset, Rectangle areaToCheck)
        {
            Rectangle pushAreaToCheck = this.Direction == Enums.Direction.LEFT
                ? new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, xOffset * -1, this.HitBox.Height)
                : new Rectangle(this.HitBox.Right, this.HitBox.Y, xOffset, this.HitBox.Height);
            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            this.UpdateSpriteByDelay(ref _currentWalkSpriteChangeDelayTick, ref _currentMoveSprite, WALK_SPRITE_CHANGE_DELAY);

            if (_keen.HitBox.IntersectsWith(pushAreaToCheck))
            {
                _keen.SetKeenPushState(this.Direction, true, this);
                _keen.GetMovedHorizontally(this, this.Direction, MOVE_VELOCITY);
            }
            else
            {
                _keen.SetKeenPushState(this.Direction, false, this);
            }
        }


        private void UpdateSpriteByDelay(ref int delayTicker, ref int spriteIndex, int delayThreshold)
        {
            if (delayTicker++ == delayThreshold)
            {
                delayTicker = 0;
                spriteIndex++;
                UpdateSprite();
            }
        }

        private void KillKeenBasedOnYPosition(int yOffset)
        {
            if (!this.IsDead())
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.X, yOffset < 0 ? this.HitBox.Y + yOffset : this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + Math.Abs(yOffset));
                if (_keen.HitBox.IntersectsWith(areaToCheck))
                {
                    _keen.Die();
                }
            }
        }

        private void BeingMachineCalibration(Keen5ControlPanel panel)
        {
            _currentControlPanel = panel;
            if (_currentControlPanel == null)
            {
                this.Move(MOVE_VELOCITY);
                return;
            }
            this.CalibrateMachine();
        }

        private void MoveOnPole(Pole p)
        {
            _currentPole = p;
            if (_currentPole == null)
            {
                this.Move(MOVE_VELOCITY);
                return;
            }

            if (this.State != LittleAmptonState.ON_POLE)
            {
                this.State = LittleAmptonState.ON_POLE;
            }

            if (_poleClimbDirection == Enums.Direction.UP)
            {
                int yOffset = POLE_CLIMB_VELOCITY * -1;
                Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height + POLE_CLIMB_VELOCITY);
                var collisions = this.CheckCollision(areaToCheck, true);

                KillKeenBasedOnYPosition(yOffset);

                var ceilingTile = GetCeilingTile(collisions);

                if (collisions.OfType<PoleTile>().Any())
                    _crossedPoleTile = true;

                if (ceilingTile != null)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, ceilingTile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
                    _poleClimbDirection = Enums.Direction.DOWN;
                }
                else
                {
                    Rectangle newLocation = new Rectangle(this.HitBox.X, this.HitBox.Y + yOffset, this.HitBox.Width, this.HitBox.Height);
                    if (_currentPole.HitBox.Top > newLocation.Top)
                    {
                        this.HitBox = new Rectangle(this.HitBox.X, _currentPole.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                        _poleClimbDirection = Enums.Direction.DOWN;
                    }
                    else
                    {
                        this.HitBox = newLocation;
                    }
                }
                this.UpdateCollisionNodes(Enums.Direction.UP);
            }
            else
            {
                Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height + POLE_CLIMB_VELOCITY);
                Rectangle newLocation = new Rectangle(this.HitBox.X, this.HitBox.Y + POLE_CLIMB_VELOCITY, this.HitBox.Width, this.HitBox.Height);

                var landingTile = GetTopMostLandingTile(POLE_CLIMB_VELOCITY);
                if ((landingTile != null && !(landingTile is PoleTile))
                 || (_crossedPoleTile && landingTile is PoleTile))
                {
                    this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                    KillKeenBasedOnYPosition(0);
                    _poleClimbDirection = Enums.Direction.UP;
                    this.Move(MOVE_VELOCITY);
                }
                else if (newLocation.Bottom > _currentPole.HitBox.Bottom)
                {
                    if (landingTile != null && landingTile is PoleTile)
                        _crossedPoleTile = true;

                    KillKeenBasedOnYPosition(_currentPole.HitBox.Bottom - this.HitBox.Bottom);
                    this.HitBox = new Rectangle(this.HitBox.X, _currentPole.HitBox.Bottom - this.HitBox.Height, this.HitBox.Width, this.HitBox.Height);
                    landingTile = GetTopMostLandingTile(POLE_CLIMB_VERTICAL_OFFSET);
                    if (landingTile == null)
                    {
                        _poleClimbDirection = Enums.Direction.UP;
                    }
                    else
                    {
                        _poleClimbDirection = Enums.Direction.UP;
                        this.Move(MOVE_VELOCITY);
                    }
                }
                else
                {
                    KillKeenBasedOnYPosition(POLE_CLIMB_VELOCITY);
                    this.HitBox = newLocation;

                }
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
            }

        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        LittleAmptonState State
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
                case LittleAmptonState.ON_POLE:
                    this.Sprite.Image = Properties.Resources.keen5_little_ampton_pole;
                    break;
                case LittleAmptonState.MOVING:
                    var spriteSet = this.Direction == Enums.Direction.LEFT ? _moveLeftSprites : _moveRightSprites;
                    if (_currentMoveSprite >= spriteSet.Length)
                    {
                        _currentMoveSprite = 0;
                    }
                    this.Sprite.Image = spriteSet[_currentMoveSprite];
                    break;
                case LittleAmptonState.LOOKING:
                    this.Sprite.Image = Properties.Resources.keen5_little_ampton_look;
                    break;
                case LittleAmptonState.STUNNED:
                    if (_currentStunnedSprite >= _stunnedSprites.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    this.Sprite.Image = _stunnedSprites[_currentStunnedSprite];
                    break;
            }
        }

        public bool DeadlyTouch
        {
            get { return _state == LittleAmptonState.ON_POLE; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return !this.IsDead(); }
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
                UpdateSprite();
            }
        }

        public PointItemType PointItem => PointItemType.KEEN5_SHIKADI_GUM;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum LittleAmptonState
    {
        MOVING,
        LOOKING,
        CALIBRATING,
        ON_POLE,
        STUNNED
    }
}
