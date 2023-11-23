using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Assets
{
    public class BottomOutPlatform : DropPlatform, ICreateRemove
    {
        private int _bottomOutDistance;
        private bool _bottomedOut;
        public BottomOutPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, PlatformType type, int bottomOutDistance)
            : base(grid, hitbox, type, keen, int.MaxValue, Guid.Empty)
        {
            _bottomOutDistance = bottomOutDistance;
        }

        protected override void Fall()
        {
            //Retain falling without vertical collision detection for self
            if (IsKeenStandingOnPlatform() || _bottomedOut)
            {
                _direction = Direction.DOWN;
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                bool hitGround = false;

                //since this falls through platforms, only update keen Y pos when keen hits floor tiles
                Rectangle areaToCheck = new Rectangle(_keen.HitBox.X, _keen.HitBox.Y, _keen.HitBox.Width, _keen.HitBox.Height + _currentVerticalVelocity);
                var collisions = _keen.CheckCollision(areaToCheck, true);
                if (!collisions.Any() && (_keen.MoveState == MoveState.STANDING || _keen.MoveState == MoveState.RUNNING))
                    UpdateKeenVerticalPosition();
                else if (collisions.Any() && !_bottomedOut)
                {
                    var collisionTile = collisions.OrderBy(c => c.HitBox.Top).FirstOrDefault();
                    if (collisionTile != null)
                    {
                        this.HitBox = new Rectangle(this.HitBox.X, collisionTile.HitBox.Y - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                        UpdateKeenVerticalPosition();
                        hitGround = true;
                    }
                }

                if (!hitGround)
                    _currentFallDistance += _currentVerticalVelocity;

                //update speed
                if (_currentVerticalVelocity + _acceleration <= MAX_GRAVITY_SPEED)
                {
                    _currentVerticalVelocity += _acceleration;
                }

                //know when to keep falling without rising
                if (_currentFallDistance >= _bottomOutDistance)
                {
                    _bottomedOut = true;
                }
                //know when to remove self from map
                if (_collidingNodes == null || !_collidingNodes.Any())
                {
                    OnRemove();
                }
            }
        }

        public override void Update()
        {
            bool keenStandingOnPlatform = IsKeenStandingOnPlatform();
            if (keenStandingOnPlatform || _bottomedOut)
            {
                if (_currentVerticalVelocity < 0)
                    _currentVerticalVelocity = 0;

                this.Fall();
            }
            else if (!keenStandingOnPlatform && _currentFallDistance > 0 && !_bottomedOut)
            {
                if (_currentVerticalVelocity > 0)
                {
                    _currentVerticalVelocity = 0;
                }
                this.Rise();
            }
        }

        protected void OnCreate()
        {
            if (Create != null)
                this.Create(this, null);
        }

        protected void OnRemove()
        {
            if (Remove != null)
            {
                ObjectEventArgs args = new ObjectEventArgs()
                {
                    ObjectSprite = this
                };
                DetachFromObjects();
                Remove(this, args);
            }
        }

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        public override string ToString()
        {
            return $"{typeof(BottomOutPlatform).Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.HitBox.Width}|{this.HitBox.Height}|{_type.ToString()}|{_bottomOutDistance}";
        }
    }
}
