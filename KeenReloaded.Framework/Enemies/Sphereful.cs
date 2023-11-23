using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Enemies
{
    public class Sphereful : CollisionObject, IUpdatable, ISprite, IEnemy
    {
        private CommanderKeen _keen;
        private System.Windows.Forms.PictureBox _sprite;

        private const int INITIAL_MOVE_VELOCITY = 5;
        private int _moveVelocity = INITIAL_MOVE_VELOCITY;

        private const int INITIAL_VERTICAL_VELOCITY = 8;
        private int _verticalVelocity = INITIAL_VERTICAL_VELOCITY;

        private const int MIN_MOVE_VELOCITY = 5;
        private const int MIN_FALL_VELOCITY = 2;
        private const int MIN_JUMP_VELOCITY = -2;

        private const int VELOCITY_CHANGE_DELAY = 4;
        private int _velocityChangeDelayTick;
        private const int ORB_LOCATION_OFFSET = 10;
        private const int ORB_VELOCITY = 4;
        private int _orb1Offset, _orb2Offset, _orb3Offset, _orb4Offset;
        private const int ORB_MOVE_TIMES = 5;
        private const int ORB_SPRITE_CHANGE_DELAY = 1;
        private int _orbSpriteChangeDelayTick;
        private bool _topTwo = true;

        private const int SPRITE_CHANGE_DELAY = 1;
        private int _spriteChangeDelayTick;
        private int _currentSprite;
        private Image[] _sprites = new Image[] {
            Properties.Resources.keen5_sphereful1,
            Properties.Resources.keen5_sphereful2,
            Properties.Resources.keen5_sphereful3,
            Properties.Resources.keen5_sphereful4
        };

        private List<PictureBox> _orbs = new List<PictureBox>()
        {
            new PictureBox() { Image = Properties.Resources.keen5_sphereful_orb1, SizeMode = PictureBoxSizeMode.AutoSize },
            new PictureBox() { Image = Properties.Resources.keen5_sphereful_orb2, SizeMode = PictureBoxSizeMode.AutoSize },
            new PictureBox() { Image = Properties.Resources.keen5_sphereful_orb3, SizeMode = PictureBoxSizeMode.AutoSize },
            new PictureBox() { Image = Properties.Resources.keen5_sphereful_orb4, SizeMode = PictureBoxSizeMode.AutoSize }
        };

        public List<PictureBox> Orbs
        {
            get
            {
                return _orbs;
            }
        }

        private bool _bouncingAway;

        public Sphereful(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            if (keen == null)
            {
                throw new ArgumentNullException("Keen was not properly set");
            }
            _keen = keen;
            Initialize();
        }

        protected Direction Direction
        {
            get;
            set;
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            _sprite.Image = _sprites[_currentSprite];
            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            SetTopTwoOrbLocation();
            _orbs[2].Visible = false;
            _orbs[3].Visible = false;
        }

      

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        protected override CollisionObject GetTopMostLandingTile(List<CollisionObject> collisions)
        {
            CollisionObject topMostTile;
            var landingTiles = collisions.Where(h =>
                   h.HitBox.Top >= this.HitBox.Top
                && h.HitBox.Right >= this.HitBox.Left
                && h.HitBox.Left <= this.HitBox.Right);

            if (!landingTiles.Any())
                return null;

            int minY = landingTiles.Select(c => c.HitBox.Top).Min();
            topMostTile = landingTiles.FirstOrDefault(t => t.HitBox.Top == minY);

            return topMostTile;
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.Where(c => !(c is PlatformTile)
                && c.HitBox.Bottom <= this.HitBox.Top
                && c.HitBox.Right >= this.HitBox.Left
                && c.HitBox.Left <= this.HitBox.Right).ToList();
            if (debugTiles.Any())
            {
                int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
                CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
                return obj;
            }
            return null;
        }

        protected override Direction ChangeHorizontalDirection(Direction direction)
        {
            var retVal = base.ChangeHorizontalDirection(direction);
            _moveVelocity = _moveVelocity *= -1;
            return retVal;
        }

        protected override Direction ChangeVerticalDirection(Direction direction)
        {
            var retVal = base.ChangeVerticalDirection(direction);
            _verticalVelocity = _verticalVelocity < 0 ? INITIAL_VERTICAL_VELOCITY / 2 : INITIAL_VERTICAL_VELOCITY * -1;
            //_moveVelocity = retVal == Enums.Direction.LEFT ? INITIAL_MOVE_VELOCITY * -1 : INITIAL_MOVE_VELOCITY;
            return retVal;
        }

        protected override Direction SetDirectionFromObjectHorizontal(CollisionObject obj, bool chase)
        {
            if (obj == null)
                throw new ArgumentNullException("object to chase cannot be null");

            if (this.HitBox.X < obj.HitBox.X)
            {
                var direction = chase ? Direction.RIGHT : Direction.LEFT;
                AdjustVelocityByDirection(direction);
                return direction;
            }
            var dir = chase ? Direction.LEFT : Direction.RIGHT;
            AdjustVelocityByDirection(dir);
            return dir;
        }

        private void AdjustVelocityByDirection(Enums.Direction direction)
        {
            if (_velocityChangeDelayTick == VELOCITY_CHANGE_DELAY)
            {
                _velocityChangeDelayTick = 0;
                if (direction == Enums.Direction.LEFT && _moveVelocity > MIN_MOVE_VELOCITY * -1)
                    _moveVelocity--;
                else if (direction == Enums.Direction.RIGHT && _moveVelocity < MIN_MOVE_VELOCITY)
                    _moveVelocity++;
            }
        }

        public void Update()
        {
            UpdateSprite();
            UpdateOrbLocations();

            if (!_bouncingAway)
                this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            else if (_velocityChangeDelayTick == VELOCITY_CHANGE_DELAY)
            {
                _velocityChangeDelayTick = 0;
                if (this.Direction == Enums.Direction.RIGHT)
                {
                    if (_moveVelocity-- <= 0)
                    {
                        _bouncingAway = false;
                    }
                }
                else if (_moveVelocity++ >= 0)
                {
                    _bouncingAway = false;
                }
            }

            int xPosCheck = _moveVelocity < 0 ? this.HitBox.X + _moveVelocity : this.HitBox.X;
            int yPosCheck = _verticalVelocity < 0 ? this.HitBox.Y + _verticalVelocity : this.HitBox.Y;
            int checkDistanceX = Math.Abs(_moveVelocity);
            int checkDistanceY = Math.Abs(_verticalVelocity);

            Rectangle areaToCheck = new Rectangle(xPosCheck, yPosCheck, this.HitBox.Width + checkDistanceX, this.HitBox.Height + checkDistanceY);
            var collisions = this.CheckCollision(areaToCheck, true);
            var horizontalTile = _moveVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = _verticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);

            int xPos = this.HitBox.X + _moveVelocity;
            int yPos = this.HitBox.Y + _verticalVelocity;

            if (horizontalTile != null)
            {
                xPos = _moveVelocity < 0 ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.Direction = ChangeHorizontalDirection(this.Direction);
                _bouncingAway = true;
            }

            if (verticalTile != null)
            {
                yPos = _verticalVelocity < 0 ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                var direction = ChangeVerticalDirection(_verticalVelocity <= 0 ? Enums.Direction.UP : Enums.Direction.DOWN);
                //_bouncingAway = false;
            }

            this.HitBox = new Rectangle(xPos, yPos, this.HitBox.Width, this.HitBox.Height);
            if (this.HitBox.IntersectsWith(_keen.HitBox))
            {
                _keen.Die();
            }

            if (_verticalVelocity < 0)
            {
                if (Math.Abs(_verticalVelocity) > 0)
                {
                    _verticalVelocity++;
                }
            }
            else if (_verticalVelocity > MIN_FALL_VELOCITY)
            {
                _verticalVelocity--;
            }
            else if (_verticalVelocity < MIN_FALL_VELOCITY)
            {
                _verticalVelocity = MIN_FALL_VELOCITY;
            }

            if (this.Direction == Enums.Direction.LEFT)
            {
                if (Math.Abs(_moveVelocity) > MIN_MOVE_VELOCITY)
                {
                    _moveVelocity++;
                }
            }
            else if (_moveVelocity > MIN_MOVE_VELOCITY)
            {
                _moveVelocity--;
            }
            _velocityChangeDelayTick++;
        }

        private void UpdateOrbLocations()
        {
            if (_orbSpriteChangeDelayTick++ == ORB_SPRITE_CHANGE_DELAY)
            {
                _orbSpriteChangeDelayTick = 0;
                int currentIteration = 0;
                if (_topTwo)
                {
                    currentIteration = _orb1Offset / ORB_VELOCITY;
                    if (currentIteration < ORB_MOVE_TIMES)
                    {
                        _orb1Offset += ORB_VELOCITY;
                        _orb2Offset -= ORB_VELOCITY;
                        if (currentIteration == ORB_MOVE_TIMES - 1)
                        {
                            _orbs[2].Visible = true;
                            _orbs[3].Visible = true;
                            SetBottomTwoOrbLocation();
                        }
                    }
                    else
                    {
                        _orbs[0].Visible = false;
                        _orbs[1].Visible = false;
                        _orbs[2].Visible = true;
                        _orbs[3].Visible = true;
                        _orb1Offset = 0;
                        _orb2Offset = 0;
                        _topTwo = false;
                    }
                }
                else
                {
                    currentIteration = _orb3Offset / ORB_VELOCITY;
                    if (currentIteration < ORB_MOVE_TIMES)
                    {
                        _orb3Offset += ORB_VELOCITY;
                        _orb4Offset -= ORB_VELOCITY;
                        if (currentIteration == ORB_MOVE_TIMES - 1)
                        {
                            _orbs[0].Visible = true;
                            _orbs[1].Visible = true;
                            SetTopTwoOrbLocation();
                        }
                    }
                    else
                    {
                        _orbs[0].Visible = true;
                        _orbs[1].Visible = true;
                        _orbs[2].Visible = false;
                        _orbs[3].Visible = false;
                        _topTwo = true;
                        _orb3Offset = 0;
                        _orb4Offset = 0;
                    }
                }
                if (currentIteration < ORB_MOVE_TIMES - 1)
                {
                    _orbs[0].Location = new Point(_orbs[0].Location.X + _orb1Offset, _orbs[0].Location.Y + _orb1Offset);
                    _orbs[1].Location = new Point(_orbs[1].Location.X + _orb2Offset, _orbs[1].Location.Y - _orb2Offset);
                    _orbs[2].Location = new Point(_orbs[2].Location.X + _orb3Offset, _orbs[2].Location.Y + _orb3Offset);
                    _orbs[3].Location = new Point(_orbs[3].Location.X + _orb4Offset, _orbs[3].Location.Y - _orb4Offset);
                }
            }
        }

        private void SetTopTwoOrbLocation()
        {
            _orbs[0].Location = new Point(this.HitBox.X + ORB_LOCATION_OFFSET / 2 - 2, this.HitBox.Y + ORB_LOCATION_OFFSET / 2);
            _orbs[1].Location = new Point(this.HitBox.Right - ORB_LOCATION_OFFSET - 4, this.HitBox.Y + ORB_LOCATION_OFFSET / 2);
            _orbs[2].Location = new Point(this.HitBox.X + ORB_LOCATION_OFFSET / 2 - 2, this.HitBox.Bottom - ORB_LOCATION_OFFSET - 2);
            _orbs[3].Location = new Point(this.HitBox.Right - ORB_LOCATION_OFFSET - 4, this.HitBox.Bottom - ORB_LOCATION_OFFSET - 2);
        }

        private void SetBottomTwoOrbLocation()
        {
            _orbs[2].Location = new Point(this.HitBox.X + ORB_LOCATION_OFFSET / 2 - 2, this.HitBox.Y + ORB_LOCATION_OFFSET / 2);
            _orbs[3].Location = new Point(this.HitBox.Right - ORB_LOCATION_OFFSET - 4, this.HitBox.Y + ORB_LOCATION_OFFSET / 2);
            _orbs[0].Location = new Point(this.HitBox.X + ORB_LOCATION_OFFSET / 2 - 2, this.HitBox.Bottom - ORB_LOCATION_OFFSET);
            _orbs[1].Location = new Point(this.HitBox.Right - ORB_LOCATION_OFFSET - 4, this.HitBox.Bottom - ORB_LOCATION_OFFSET);
        }

        public override Rectangle HitBox
        {
            get
            {
                return base.HitBox;
            }
            protected set
            {
                int xDif = this.HitBox.X - value.X, yDif = this.HitBox.Y - value.Y;
                base.HitBox = value;
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    this.UpdateCollisionNodes(this.Direction);
                    if (_verticalVelocity < 0)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.UP);
                    }
                    else if (_verticalVelocity > 0)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }

                    _orbs[0].Location = new Point(_orbs[0].Location.X - xDif, _orbs[0].Location.Y - yDif);
                    _orbs[1].Location = new Point(_orbs[1].Location.X - xDif, _orbs[1].Location.Y - yDif);
                    _orbs[2].Location = new Point(_orbs[2].Location.X - xDif, _orbs[2].Location.Y - yDif);
                    _orbs[3].Location = new Point(_orbs[3].Location.X - xDif, _orbs[3].Location.Y - yDif);
                }
            }
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _sprites.Length)
            {
                _currentSprite = 0;
            }
            _sprite.Image = _sprites[_currentSprite++];
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

        }

        public bool IsActive
        {
            get { return true; }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
