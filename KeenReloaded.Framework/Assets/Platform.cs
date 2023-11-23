﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Assets
{
    public abstract class Platform : CollisionObject, IUpdatable, IActivateable, ISprite
    {
        protected PlatformType _type;
        private System.Windows.Forms.PictureBox _sprite;
        protected CommanderKeen _keen;
        protected int _moveVelocity = 5;
        protected int _acceleration = 10;
        protected Direction _direction;
        protected int MAX_GRAVITY_SPEED = 50;
        protected int _currentVerticalVelocity;

        protected Image[] _images;
        protected readonly Guid _activationId;

        public Platform(SpaceHashGrid grid, Rectangle hitbox, PlatformType type, CommanderKeen keen, Guid activationId)
            : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("Keen was not properly set");

            _keen = keen;
            _type = type;
            _activationId = activationId;
            Initialize();
        }

        protected virtual void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            SetInitialSprite();
            _sprite.Location = this.HitBox.Location;
            this.HitBox = new Rectangle(_sprite.Location.X, _sprite.Location.Y + (_sprite.Height / 2), _sprite.Width, _sprite.Height / 2);
        }

        protected void SetInitialSprite()
        {
            switch (_type)
            {
                case PlatformType.KEEN4:
                    _sprite.Image = Properties.Resources.keen4_platorm_stationary;
                    _images = new Image[]
                    {
                        Properties.Resources.keen4_platorm_stationary
                    };
                    break;
                case PlatformType.KEEN5_PINK:
                     _sprite.Image = Properties.Resources.keen5_pink_platform;
                    _images = new Image[]
                    {
                        Properties.Resources.keen5_pink_platform
                    };
                    break;
                case PlatformType.KEEN5_ORANGE:
                    _sprite.Image = Properties.Resources.keen5_orange_platform;
                    _images = new Image[]
                    {
                        Properties.Resources.keen5_orange_platform
                    };
                    break;
                case PlatformType.KEEN6:
                     _sprite.Image = Properties.Resources.keen6_bip_platform;
                    _images = new Image[]
                    {
                        Properties.Resources.keen6_bip_platform
                    };
                    break;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        protected virtual bool IsKeenStandingOnPlatform()
        {
            bool intersectsX = _keen.HitBox.Right > this.HitBox.Left && _keen.HitBox.Left < this.HitBox.Right;//_keen.HitBox.Left > this.HitBox.Left && _keen.HitBox.Left < this.HitBox.Right;
            if (_keen.IsLookingDown && _direction == Direction.DOWN)
            {
                bool standingOnPlatformLookingDown = intersectsX && _keen.HitBox.Bottom >= this.HitBox.Top - 21 && _keen.HitBox.Top <= this.HitBox.Top;
                if (standingOnPlatformLookingDown)
                {
                    this.UpdateKeenVerticalPosition();
                }
                return standingOnPlatformLookingDown;
            }
            return _keen.HitBox.Bottom == this.HitBox.Top - 1 && intersectsX;
        }

        protected virtual void UpdateKeenVerticalPosition()
        {
            if (!_keen.IsDead())
                _keen.GetMovedVertically(this);
        }

        protected virtual void UpdateKeenHorizontalPosition()
        {
            if (!_keen.IsDead())
                _keen.GetMovedHorizontally(this, _direction, _moveVelocity);
        }

        public abstract void Activate();

        public abstract void Deactivate();

        public virtual bool IsActive
        {
            get { return false; }
        }

        public abstract void Update();

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
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
                if (_sprite != null && value != null)
                {
                    _sprite.Location = new Point(this.HitBox.X, this.HitBox.Y - (_sprite.Height / 2));
                    this.UpdateCollisionNodes(_direction);
                }
            }
        }

        public Guid ActivationID => _activationId;
    }
}