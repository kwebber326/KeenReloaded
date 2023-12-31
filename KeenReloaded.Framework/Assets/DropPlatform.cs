﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Assets
{
    public class DropPlatform : Platform
    {
        protected int _fallDistanceLimit;
        protected int _currentFallDistance;
        protected Rectangle _originalLocation;

        public DropPlatform(SpaceHashGrid grid, Rectangle hitbox, PlatformType type, CommanderKeen keen, int maxDrop, Guid activationId) 
            : base (grid, hitbox, type, keen, activationId)
        {
            _fallDistanceLimit = maxDrop;
            _originalLocation = new Rectangle(this.HitBox.Location, this.HitBox.Size);
        }

        protected virtual void Fall()
        {
            if (_currentFallDistance < _fallDistanceLimit)
            {
                _direction = Direction.DOWN;
                var landingTile = GetTopMostLandingTile(_currentVerticalVelocity);
                if (landingTile != null)
                {
                    _currentFallDistance += landingTile.HitBox.Top - this.HitBox.Bottom - 1;
                    this.HitBox = new Rectangle(this.HitBox.X, landingTile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                    _currentVerticalVelocity = 0;
                    UpdateKeenVerticalPosition();
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                    _currentFallDistance += _currentVerticalVelocity;
                    if (_currentVerticalVelocity + _acceleration <= MAX_GRAVITY_SPEED)
                    {
                        _currentVerticalVelocity += _acceleration;
                    }
                    UpdateKeenVerticalPosition();
                }
               
            }
        }

        public override void Activate()
        {
           
        }

        public override void Deactivate()
        {
           
        }

        public override void Update()
        {
            bool keenStandingOnPlatform = IsKeenStandingOnPlatform();
            if (keenStandingOnPlatform && _currentFallDistance < _fallDistanceLimit)
            {
                if (_currentVerticalVelocity < 0)
                    _currentVerticalVelocity = 0;

                this.Fall();
            }
            else if (!keenStandingOnPlatform && _currentFallDistance > 0)
            {
                if (_currentVerticalVelocity > 0)
                {
                    _currentVerticalVelocity = 0;
                }
                this.Rise();
            }
            this.UpdateCollisionNodes(Direction.UP);
            this.UpdateCollisionNodes(Direction.DOWN);
        }

        protected virtual void Rise()
        {
            _direction = Direction.UP;
            Rectangle areaToCheck = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck);
            var ceilingTile = GetCeilingTile(collisions);
            if (ceilingTile != null)
            {
                _currentVerticalVelocity = 0;
                _currentFallDistance -= ceilingTile.HitBox.Bottom - 1 - this.HitBox.Top;
                this.HitBox = new Rectangle(this.HitBox.X, ceilingTile.HitBox.Bottom + 1, this.HitBox.Width, this.HitBox.Height);
            }
            else
            {
                if (_currentFallDistance + _currentVerticalVelocity < 0)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, _originalLocation.Y, this.HitBox.Width, this.HitBox.Height);
                }
                else
                {
                    this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + _currentVerticalVelocity, this.HitBox.Width, this.HitBox.Height);
                }
                _currentFallDistance += _currentVerticalVelocity;
                if (_currentVerticalVelocity - _acceleration >= MAX_GRAVITY_SPEED * -1)
                {
                    _currentVerticalVelocity -= _acceleration;
                }
                if (_currentFallDistance < 0)
                {
                    _currentFallDistance = 0;
                }
            }
            if (this.CollidesWith(_keen) && !IsKeenStandingOnPlatform() && _currentVerticalVelocity <= (MAX_GRAVITY_SPEED / 2) * -1)
            {
                UpdateKeenVerticalPosition();
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{this.HitBox.Width}|{this.HitBox.Height}|{_type.ToString()}|{_fallDistanceLimit}|{_activationId.ToString()}";
        }
    }
}
