﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Enums;
using System.Drawing;
using KeenReloaded.Framework.Hazards;

namespace KeenReloaded.Framework.Enemies
{
    public class KorathInhabitant : DestructibleObject, IUpdatable, ISprite, IEnemy, IZombieBountyEnemy
    {
        private KorathInhabitantState _state;
        private System.Windows.Forms.PictureBox _sprite;
        private Enums.Direction _direction;
        private CommanderKeen _keen;

        private const int KILL_HITBOX_WIDTH = 10;
        private const int KILL_HITBOX_HEIGHT = 10;

        private int _currentWalkSprite;
        private const int WALK_SPRITE_CHANGE_DELAY = 2;
        private int _currentWalkSpriteChangeDelayTick;

        private int _currentStunnedSprite;
        private const int STUNNED_SPRITE_CHANGE_DELAY = 1;
        private int _currentStunnedSpriteChangeDelaTick;

        private const int BASIC_FALL_VELOCITY = 40;
        private const int WALK_VELOCITY = 10;

        private const int LOOK_CHANCE = 50;
        private const int LOOK_TIME = 10;
        private int _currentLookTimeTick;

        public KorathInhabitant(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
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
            this.Direction = SetDirectionFromObjectHorizontal(_keen, true);
            this.Fall();
            _random = new Random();
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
                    if (_state == KorathInhabitantState.FALLING)
                    {
                        this.UpdateCollisionNodes(Enums.Direction.DOWN);
                    }
                }
            }
        }

        public override void Die()
        {
            this.UpdateStunnedState();
        }

        public void Update()
        {
            switch (_state)
            {
                case KorathInhabitantState.LOOKING:
                    this.Look();
                    break;
                case KorathInhabitantState.STUNNED:
                    this.UpdateStunnedState();
                    break;
                case KorathInhabitantState.WALKING:
                    this.Walk();
                    break;
                case KorathInhabitantState.FALLING:
                    this.Fall();
                    break;
            }
        }

        private void Fall()
        {
            if (this.State != KorathInhabitantState.FALLING)
            {
                this.State = KorathInhabitantState.FALLING;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            }

            var tile = this.BasicFallReturnTile(BASIC_FALL_VELOCITY);
            if (tile != null)
            {
                this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                this.Walk();
            }
        }

        private void Walk()
        {
            if (this.State != KorathInhabitantState.WALKING)
            {
                this.State = KorathInhabitantState.WALKING;
                if (_keen.HitBox.IntersectsWith(this.HitBox) && !IsKeenInFrontOfThis())
                {
                    _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), true, this);
                }
                else
                {
                    _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), false, this);
                }
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (IsOnEdge(this.Direction, 2))
            {
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                return;
            }

            int lookVal = _random.Next(1, LOOK_CHANCE + 1);
            if (lookVal == LOOK_CHANCE)
            {
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
                this.Look();
                return;
            }

            int xOffset = _direction == Enums.Direction.LEFT ? WALK_VELOCITY * -1 : WALK_VELOCITY;
            int xPosCheck = _direction == Enums.Direction.LEFT ? this.HitBox.X + xOffset : this.HitBox.X;
            Rectangle areaToCheck = new Rectangle(xPosCheck, this.HitBox.Y, this.HitBox.Width + WALK_VELOCITY, this.HitBox.Height);

            var collisions = this.CheckCollision(areaToCheck, true);
            var tile = _direction == Enums.Direction.LEFT ? GetRightMostLeftTile(collisions) : GetLeftMostRightTile(collisions);

            if (tile != null)
            {
                int xCollidePos = _direction == Enums.Direction.LEFT ? tile.HitBox.Right + 1 : tile.HitBox.Left - this.HitBox.Width - 1;
                this.HitBox = new Rectangle(xCollidePos, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
                this.Direction = this.ChangeHorizontalDirection(this.Direction);
                ExecuteFreeMoveLogic(xOffset, areaToCheck);
            }
            else
            {
                ExecuteFreeMoveLogic(xOffset, areaToCheck);
            }

            if (_currentWalkSpriteChangeDelayTick++ == WALK_SPRITE_CHANGE_DELAY)
            {
                _currentWalkSpriteChangeDelayTick = 0;
                _currentWalkSprite++;
                UpdateSprite();
            }
        }


        private void ExecuteFreeMoveLogic(int xOffset, Rectangle areaToCheck)
        {
            _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
            _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);

            Rectangle pushAreaToCheck = this.Direction == Enums.Direction.LEFT
                ? new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, xOffset * -1, this.HitBox.Height)
                : new Rectangle(this.HitBox.Right, this.HitBox.Y, xOffset, this.HitBox.Height);
            this.HitBox = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);

            if (!(_keen.MoveState == MoveState.ON_POLE || _keen.IsDead()))
            {

                if (_keen.HitBox.IntersectsWith(pushAreaToCheck))
                {
                    _keen.SetKeenPushState(this.Direction, true, this);
                    _keen.GetMovedHorizontally(this, this.Direction, WALK_VELOCITY);
                }
                else
                {
                    _keen.SetKeenPushState(this.Direction, false, this);
                }
            }
        }

        private void UpdateStunnedState()
        {
            if (this.State != KorathInhabitantState.STUNNED)
            {
                this.State = KorathInhabitantState.STUNNED;
                _keen.SetKeenPushState(Enums.Direction.LEFT, false, this);
                _keen.SetKeenPushState(Enums.Direction.RIGHT, false, this);
            }

            if (IsNothingBeneath())
            {
                this.BasicFall(BASIC_FALL_VELOCITY);
            }

            var spriteIndex = _currentStunnedSprite;
            this.UpdateSpriteByDelay(ref _currentStunnedSpriteChangeDelaTick, ref _currentStunnedSprite, STUNNED_SPRITE_CHANGE_DELAY);
            if (_currentStunnedSprite != spriteIndex)
            {
                var image = SpriteSheet.KorathIStunnedImages[_currentStunnedSprite];
                this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y - (image.Size.Height - this.HitBox.Height), image.Width, image.Height);
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


        private void Look()
        {
            if (this.State != KorathInhabitantState.LOOKING)
            {
                this.State = KorathInhabitantState.LOOKING;
                _currentLookTimeTick = 0;
            }

            if (IsNothingBeneath())
            {
                this.Fall();
                return;
            }

            if (_currentLookTimeTick++ == LOOK_TIME)
            {
                this.Walk();
            }
            else if (_keen.HitBox.IntersectsWith(this.HitBox) && !IsKeenInFrontOfThis())
            {
                _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), true, this);
            }
            else
            {
                _keen.SetKeenPushState(ChangeHorizontalDirection(_keen.Direction), false, this);
            }
        }

        private bool IsKeenInFrontOfThis()
        {
            if (!_keen.HitBox.IntersectsWith(this.HitBox))
                return false;
            if (_keen.Direction == Enums.Direction.LEFT)
            {
                if (_keen.HitBox.Left <= this.HitBox.Right - 15)
                    return true;
            }
            else
            {
                if (_keen.HitBox.Right >= this.HitBox.Left + 15)
                    return true;
            }

            return false;
        }

        KorathInhabitantState State
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

        Direction Direction
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
            switch (_state)
            {
                case KorathInhabitantState.FALLING:
                case KorathInhabitantState.WALKING:
                    var spriteSet = _direction == Enums.Direction.LEFT ? SpriteSheet.KorathIWalkLeftImages : SpriteSheet.KorathIWalkRightImages;
                    if (_currentWalkSprite >= spriteSet.Length)
                    {
                        _currentWalkSprite = 0;
                    }
                    _sprite.Image = spriteSet[_currentWalkSprite];
                    break;
                case KorathInhabitantState.STUNNED:
                    spriteSet = SpriteSheet.KorathIStunnedImages;
                    if (_currentStunnedSprite >= spriteSet.Length)
                    {
                        _currentStunnedSprite = 1;
                    }
                    _sprite.Image = spriteSet[_currentStunnedSprite];
                    break;
                case KorathInhabitantState.LOOKING:
                    _sprite.Image = Properties.Resources.keen5_korath_inhabitant_look;
                    break;
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public bool DeadlyTouch
        {
            get { return false; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            this.TakeDamage(trajectory);
        }

        public bool IsActive
        {
            get { return this.State != KorathInhabitantState.STUNNED; }
        }

        public PointItemType PointItem => PointItemType.KEEN5_SHIKADI_GUM;

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum KorathInhabitantState
    {
        LOOKING,
        WALKING,
        FALLING,
        STUNNED
    }
}
