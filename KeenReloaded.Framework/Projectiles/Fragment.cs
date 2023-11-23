using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.KeenEventArgs;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Assets;

namespace KeenReloaded.Framework.Trajectories
{
    public class Fragment : CollisionObject, IUpdatable, ISprite, ICreateRemove
    {
        public Fragment(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, Direction direction, FragmentType fragmentType, int initialHorizontalSpeed, int initialVerticalSpeed, bool explodeVertically = false)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _direction = direction;
            _fragmentType = fragmentType;
            _explodeVertically = explodeVertically;
            INITIAL_MOVE_VELOCITY = initialHorizontalSpeed;
            INITIAL_VERTICAL_VELOCITY = initialVerticalSpeed;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            if (_fragmentType == FragmentType.KEEN5_SHELLEY)
            {
                _sprite.Image = _direction == Direction.LEFT ? Properties.Resources.keen5_shelley_shard1
                    : Properties.Resources.keen5_shelley_shard2;
            }
            else if (_fragmentType == FragmentType.KEEN5_SHIKADI_MINE)
            {
                _sprite.Image = Properties.Resources.keen5_shikadi_mine_fragment;
            }

            if (!_explodeVertically)
            {
                _currentHorizontalVelocity = _direction == Direction.LEFT ? INITIAL_MOVE_VELOCITY * -1 : INITIAL_MOVE_VELOCITY;
                _currentVerticalVelocity = 0;
            }
            else
            {
                _currentHorizontalVelocity = _direction == Direction.LEFT ? AIR_RESISTANCE * -1 : AIR_RESISTANCE;
                _currentVerticalVelocity = INITIAL_VERTICAL_VELOCITY;
            }

            _lastVerticalImpact = int.MaxValue;
            this.Update();
        }

        protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.OfType<DebugTile>();
            if (collisions.Any() && debugTiles.Any())
            {
                var rightTiles = debugTiles.Where(c => c.HitBox.Left >= this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (rightTiles.Any())
                {
                    int minX = rightTiles.Select(t => t.HitBox.Left).Min();
                    CollisionObject obj = rightTiles.FirstOrDefault(x => x.HitBox.Left == minX);
                    return obj;
                }
            }
            return null;
        }

        protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        {
            var debugTiles = collisions.OfType<DebugTile>();
            if (collisions.Any() && debugTiles.Any())
            {
                var leftTiles = debugTiles.Where(c => c.HitBox.Left <= this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
                if (leftTiles.Any())
                {
                    int maxX = leftTiles.Select(t => t.HitBox.Right).Max();
                    CollisionObject obj = leftTiles.FirstOrDefault(x => x.HitBox.Right == maxX);
                    return obj;
                }
            }
            return null;
        }

        protected override CollisionObject GetCeilingTile(List<CollisionObject> collisions)
        {

            var debugTiles = collisions.Where(c => c is DebugTile && this.HitBox.Bottom >= c.HitBox.Top && (this.HitBox.IntersectsWith(c.HitBox) || c.HitBox.Top < this.HitBox.Top)).ToList();
            if (debugTiles.Any())
            {
                int maxBottom = debugTiles.Select(c => c.HitBox.Bottom).Max();
                CollisionObject obj = collisions.FirstOrDefault(c => c.HitBox.Bottom == maxBottom);
                return obj;
            }
            return null;
        }

        //protected override CollisionObject GetTopMostLandingTile(List<CollisionObject> collisions)
        //{
        //    CollisionObject topMostTile;
        //    var landingTiles = collisions.Where(h => (h is DebugTile || h is Platform || h is PlatformTile || h is PoleTile || h is Keen6Switch)
        //        && (this.HitBox.Top <= h.HitBox.Top));

        //    if (!landingTiles.Any())
        //        return null;

        //    int minY = landingTiles.Select(c => c.HitBox.Top).Min();
        //    topMostTile = landingTiles.FirstOrDefault(t => t.HitBox.Top == minY);

        //    return topMostTile;
        //}



        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
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
                    this.UpdateCollisionNodes(this._direction);
                    if (_currentVerticalVelocity > 0)
                    {
                        this.UpdateCollisionNodes(Direction.DOWN);
                    }
                    else if (_currentVerticalVelocity < 0)
                    {
                        this.UpdateCollisionNodes(Direction.UP);
                    }
                }
            }
        }

        public void Update()
        {
            if (_stoppedHorizontalMovement && _stoppedVerticalMovement)
            {
                OnRemove(new ObjectEventArgs() { ObjectSprite = this });
                return;
            }

            Rectangle areaToCheck = new Rectangle(
                  _currentHorizontalVelocity < 0 ? this.HitBox.X + _currentHorizontalVelocity : this.HitBox.X //X
                , _currentVerticalVelocity < 0 ? this.HitBox.Y + _currentVerticalVelocity : this.HitBox.Y //Y
                , this.HitBox.Width + Math.Abs(_currentHorizontalVelocity)//width
                , this.HitBox.Height + Math.Abs(_currentVerticalVelocity));//height

            var collisions = this.CheckCollision(areaToCheck, true);

            var horizontalTile = _currentHorizontalVelocity < 0 ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);
            var verticalTile = _currentVerticalVelocity < 0 ? GetCeilingTile(collisions) : GetTopMostLandingTile(collisions);

            if (horizontalTile != null)
            {
                int collisionXPos = _currentHorizontalVelocity < 0 ? horizontalTile.HitBox.Right + 1 : horizontalTile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(collisionXPos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                GetHorizontalImpact();
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
                DecelerateHorizontalMovement();
            }
            else
            {
                Rectangle areaToCheckToKillKeen = new Rectangle(
                    _currentHorizontalVelocity < 0 ? this.HitBox.X + _currentHorizontalVelocity : this.HitBox.X,
                    this.HitBox.Y, this.HitBox.Width + Math.Abs(_currentHorizontalVelocity), this.HitBox.Height);
                if (_keen.HitBox.IntersectsWith(areaToCheckToKillKeen))
                {
                    _keen.Die();
                }
                this.HitBox = new Rectangle(this.HitBox.X + _currentHorizontalVelocity, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                DecelerateHorizontalMovement();
            }

            if (verticalTile != null)
            {
                int _collisionYPos = _currentVerticalVelocity < 0 ? verticalTile.HitBox.Bottom + 1 : verticalTile.HitBox.Top - this.HitBox.Height - 1;
                this.HitBox = new Rectangle(this.HitBox.X, _collisionYPos, this.HitBox.Width, this.HitBox.Height);
                GetVerticalImpact();
                if (_keen.HitBox.IntersectsWith(this.HitBox))
                {
                    _keen.Die();
                }
            }
            else
            {
                Rectangle areaToCheckToKillKeen = new Rectangle(
                   this.HitBox.X,
                   _currentVerticalVelocity < 0 ? this.HitBox.Y + _currentVerticalVelocity : this.HitBox.Y
                   , this.HitBox.Width, this.HitBox.Height + Math.Abs(_currentVerticalVelocity));

                if (_keen.HitBox.IntersectsWith(areaToCheckToKillKeen))
                {
                    _keen.Die();
                }
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                AccelerateVerticalMovement();
                if (_currentHorizontalVelocity == 0 && Math.Abs(_lastVerticalImpact) <= MIN_VERTICAL_VELOCITY)
                {
                    _stoppedVerticalMovement = true;
                    _stoppedHorizontalMovement = true;
                }
            }
        }

        private void GetVerticalImpact()
        {
            if (Math.Abs(_currentVerticalVelocity) <= MIN_HORIZONTAL_VELOCITY && _currentHorizontalVelocity == 0)
            {
                _currentVerticalVelocity = 0;
                _stoppedVerticalMovement = true;
            }
            else
            {
                _currentVerticalVelocity = GetImpactVelocityVertical(_currentVerticalVelocity);
                _lastVerticalImpact = _currentVerticalVelocity;
                _currentHorizontalVelocity = GetImpactVelocityHorizontal(_currentHorizontalVelocity, false);
            }
        }

        private void GetHorizontalImpact()
        {
            if (Math.Abs(_currentHorizontalVelocity) <= MIN_HORIZONTAL_VELOCITY)
            {
                _currentHorizontalVelocity = 0;
                _stoppedHorizontalMovement = true;
            }
            else
            {
                _currentHorizontalVelocity = GetImpactVelocityHorizontal(_currentHorizontalVelocity, true);
            }
        }

        private void AccelerateVerticalMovement()
        {
            if (_currentVerticalVelocity + VERTICAL_ACCELERATION <= MAX_VERTICAL_VELOCITY)
            {
                _currentVerticalVelocity += VERTICAL_ACCELERATION;
            }
            else
            {
                _currentVerticalVelocity = MAX_VERTICAL_VELOCITY;
            }
        }

        private void DecelerateHorizontalMovement()
        {
            if (_currentHorizontalVelocity < 0)
            {
                if (_currentHorizontalVelocity + AIR_RESISTANCE <= 0)
                {
                    _currentHorizontalVelocity += AIR_RESISTANCE;
                }
                else
                {
                    _currentHorizontalVelocity = 0;
                    //end horizontal movement
                    _stoppedHorizontalMovement = true;
                }
            }
            else if (_currentHorizontalVelocity > 0)
            {
                if (_currentHorizontalVelocity - AIR_RESISTANCE >= 0)
                {
                    _currentHorizontalVelocity -= AIR_RESISTANCE;
                }
                else
                {
                    _currentHorizontalVelocity = 0;
                    //end horizontalMovement;
                    _stoppedHorizontalMovement = true;
                }
            }
            else if (_currentHorizontalVelocity == 0)
            {
                _stoppedHorizontalMovement = true;
            }
        }

        private int GetImpactVelocityHorizontal(int velocity, bool switchDirection)
        {
            double kineticLoss = velocity / KINETIC_IMPACT_DENOMINATOR; /*(INITIAL_MOVE_VELOCITY - Math.Abs(velocity)) / INITIAL_MOVE_VELOCITY;*/ // velocity / VELOCITY_DECREASE;  //((double)velocity) / ((double)INITIAL_MOVE_VELOCITY) * 100.0;
            int kLossInt = Convert.ToInt32(kineticLoss);
            // if (kLossInt < velocity)
            velocity -= kLossInt;
            //else 
            //  velocity = 0;
            if (switchDirection)
                velocity *= -1;
            return velocity;
        }

        private int GetImpactVelocityVertical(int velocity)
        {
            int kineticLoss = velocity / KINETIC_IMPACT_DENOMINATOR; //(((double)velocity + VELOCITY_DECREASE) - (double)velocity) / ((double)velocity) * 100.0;
                                                                     // int kLossInt = Convert.ToInt32(kineticLoss);
            if (Math.Abs(kineticLoss) < Math.Abs(velocity))
            {
                velocity -= kineticLoss;
            }
            else
            {
                velocity = 0;
                //endverticalMovement

                _stoppedVerticalMovement = true;
            }
            velocity *= -1;
            return velocity;
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;
        private System.Windows.Forms.PictureBox _sprite;
        private Direction _direction;
        private CommanderKeen _keen;
        private FragmentType _fragmentType;

        private readonly int INITIAL_MOVE_VELOCITY = 40;
        private readonly int INITIAL_VERTICAL_VELOCITY = 50;
        private const int VERTICAL_ACCELERATION = 5;
        private const int MAX_VERTICAL_VELOCITY = 70;
        private const int KINETIC_IMPACT_DENOMINATOR = 2;
        private const int MIN_VERTICAL_VELOCITY = 5;
        private const int MIN_HORIZONTAL_VELOCITY = 5;
        private const int AIR_RESISTANCE = 5;
        private int _currentHorizontalVelocity;
        private int _currentVerticalVelocity;
        private bool _stoppedVerticalMovement, _stoppedHorizontalMovement;
        private int _lastVerticalImpact;
        private bool _explodeVertically;

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
                if (args.ObjectSprite == this)
                {
                    foreach (var node in _collidingNodes)
                    {
                        node.Objects.Remove(this);
                    }
                }
                this.Remove(this, args);
            }
        }
    }
}
