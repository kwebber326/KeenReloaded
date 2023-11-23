﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Assets;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class MovingPlatformTile : PlatformTile
    {
        private CommanderKeen _keen;
        public MovingPlatformTile(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox)
        {

        }

        public CollisionObject AssociatedObject { get; set; }

        public void AssignKeen(CommanderKeen keen)
        {
            _keen = keen;
            _keen.KeenMoved += new EventHandler(_keen_KeenMoved);
        }

        void _keen_KeenMoved(object sender, EventArgs e)
        {
            if (_keen != null && (_keen.IsDead() || _keen.MoveState == Enums.MoveState.ON_POLE || !KeenIsStandingOnThis()))
            {
                UnassignKeen();
            }
        }

        private void UnassignKeen()
        {
            _keen.KeenMoved -= _keen_KeenMoved;
            _keen = null;
        }

        private bool KeenIsStandingOnThis()
        {
            if (_keen != null)
            {
                bool xIntersect = _keen.HitBox.Left < this.HitBox.Right && _keen.HitBox.Right > this.HitBox.Left;
                var platform = this.AssociatedObject as SetPathPlatform;
                bool yStanding = (_keen.HitBox.Bottom == this.HitBox.Top - 1);
                bool isStanding = xIntersect && yStanding;

                return isStanding;
            }
            return false;
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
                if (this.Sprite != null && value != null)
                {
                    this.Sprite.Location = this.HitBox.Location;
                }
            }
        }

        public CommanderKeen Keen
        {
            get
            {
                return _keen;
            }
        }

        public void Move(Point p)
        {
            Point previous = this.HitBox.Location;
            this.HitBox = new Rectangle(p, this.HitBox.Size);
            UpdateCollisionNodesByMoveDirection(previous);
            if (_keen != null && !_keen.IsDead() && _keen.MoveState != Enums.MoveState.ON_POLE)
            {
                Point newPos = new Point(_keen.HitBox.X + (this.HitBox.X - previous.X), this.HitBox.Top - _keen.HitBox.Height - 1);
                _keen.MoveKeenToPosition(newPos, this);
            }
        }

        private void UpdateCollisionNodesByMoveDirection(Point previous)
        {
            if (previous.X < this.HitBox.X)
            {
                this.UpdateCollisionNodes(Enums.Direction.LEFT);
            }
            else if (previous.X > this.HitBox.X)
            {
                this.UpdateCollisionNodes(Enums.Direction.RIGHT);
            }

            if (previous.Y < this.HitBox.Y)
            {
                this.UpdateCollisionNodes(Enums.Direction.UP);
            }
            else if (previous.Y > this.HitBox.Y)
            {
                this.UpdateCollisionNodes(Enums.Direction.DOWN);
            }
        }
    }
}
