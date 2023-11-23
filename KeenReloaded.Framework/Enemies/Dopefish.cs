﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Tiles;
using KeenReloaded.Framework.Trajectories;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Enemies
{
    public class Dopefish : CollisionObject, IUpdatable, IMoveable, IEnemy, ISprite, ICreateRemove
    {
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;
        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        private CommanderKeen _keen;
        private Direction _direction;
        private PictureBox _sprite;

        private const int MOVE_VELOCITY = 5;
        private const int ATTACK_VELOCITY = 30;
        private const int ATTACK_TIME = 5;
        private int _currentAttackTime;

        private const int PAUSE_TIME = 20;
        private int _currentPauseTime;

        private const int LOOK_TIME = 20;
        private int _currentLookTime;

        private const int FART_TIME = 25;
        private int _currentFartTime;

        private const int SPRITE_CHANGE_DELAY = 10;
        private int _currentSpriteChangeDelayTick;
        private int _currentMoveImage;

        Rectangle _forwardVision;
        Size _visionSize;

        private Image[] _moveRightImages = new Image[] 
        {
            Properties.Resources.keen4_dopefish_move_right1,
            Properties.Resources.keen4_dopefish_move_right2
        };
        private Image[] _moveLeftImages = new Image[] 
        {
            Properties.Resources.keen4_dopefish_move_left1,
            Properties.Resources.keen4_dopefish_move_left2
        };
        private DopefishMoveState _dopefishState;
        private bool _ateKeen;

        public Dopefish(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen)
            : base(grid, hitbox)
        {
            _keen = keen;
            _ateKeen = false;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new PictureBox();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            UpdateSprite();
            SetDirectionFromKeenLocation();
            _visionSize = new Size(100, this.HitBox.Height);
            SetAttackVision();
        }

        private void SetAttackVision()
        {
            if (IsRightDirection())
            {
                if (IsUpDirection())
                {
                    _forwardVision = new Rectangle(this.HitBox.X, this.HitBox.Y - _visionSize.Height, this.HitBox.Width + _visionSize.Width, _visionSize.Height + this.HitBox.Height);
                }
                else if (IsDownDirection())
                {
                    _forwardVision = new Rectangle(this.HitBox.X, this.HitBox.Y, this.HitBox.Width + _visionSize.Width, _visionSize.Height + this.HitBox.Height);
                }
                else
                {
                    _forwardVision = new Rectangle(this.HitBox.Right, this.HitBox.Y, _visionSize.Width, _visionSize.Height);
                }
            }
            else
            {
                if (IsUpDirection())
                {
                    _forwardVision = new Rectangle(this.HitBox.X - _visionSize.Width, this.HitBox.Y - _visionSize.Height, this.HitBox.Width + _visionSize.Width, _visionSize.Height + this.HitBox.Height);
                }
                else if (IsDownDirection())
                {
                    _forwardVision = new Rectangle(this.HitBox.X - _visionSize.Width, this.HitBox.Y, this.HitBox.Width + _visionSize.Width, _visionSize.Height + this.HitBox.Height);
                }
                else
                {
                    _forwardVision = new Rectangle(this.HitBox.Left - _visionSize.Width, this.HitBox.Y, _visionSize.Width, _visionSize.Height);
                }
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
                    SetAttackVision();
                }
            }
        }

        private void SetDirectionFromKeenLocation()
        {
            var direction = this.Direction;
            SetFullDirectionFromKeenLocation(_keen, ref direction);
            this.Direction = direction;
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            switch (_dopefishState)
            {
                case DopefishMoveState.CHASING:
                    this.Chase();
                    break;
                case DopefishMoveState.ATTACKING:
                    this.Attack();
                    break;
                case DopefishMoveState.PAUSING:
                    this.Pause();
                    break;
                case DopefishMoveState.LOOKING:
                    this.Look();
                    break;
                case DopefishMoveState.FARTING:
                    this.Fart();
                    break;
            }
        }

        private void Pause()
        {
            if (this.State != DopefishMoveState.PAUSING)
            {
                this.State = DopefishMoveState.PAUSING;
                _currentPauseTime = 0;
            }

            if (_currentPauseTime++ == PAUSE_TIME)
            {
                _currentPauseTime = 0;
                this.Look();
            }
        }

        public void Look()
        {
            if (this.State != DopefishMoveState.LOOKING)
            {
                this.State = DopefishMoveState.LOOKING;
                _currentLookTime = 0;
            }

            if (_currentLookTime++ == LOOK_TIME)
            {
                _currentLookTime = 0;
           
                this.Fart();
            }
        }

        public void Attack()
        {
            if (this.State != DopefishMoveState.ATTACKING)
            {
                this.State = DopefishMoveState.ATTACKING;
                _currentAttackTime = 0;
            }
            this.Move();
            _currentAttackTime++;
            if (this.HitBox.IntersectsWith(_keen.HitBox))
            {
                _keen.DisappearDeath();
                _ateKeen = true;
                this.Pause();
            }
            else if (_currentAttackTime == ATTACK_TIME)
            {
                this.Pause();
            }
        }

        public void Chase()
        {
            if (this.State != DopefishMoveState.CHASING)
            {
                this.State = DopefishMoveState.CHASING;
            }
            this.Move();
            if (_forwardVision.IntersectsWith(_keen.HitBox))
            {
                var collisions = this.CheckCollision(_forwardVision);
                if (!AreBlocksImpedingAttack(collisions) && !_ateKeen)
                    Attack();
            }
        }

        private bool AreBlocksImpedingAttack(List<CollisionObject> collisions)
        {
            if (this.HitBox.IntersectsWith(_keen.HitBox))
                return false;

            if (collisions.Any())
            {
                if (IsLeftDirection())
                {
                    var leftImpedingBlocks = collisions.Any(b => b.HitBox.Right > _keen.HitBox.Right
                        && b.HitBox.Right < this.HitBox.Left
                        && b.HitBox.Top < this.HitBox.Bottom
                        && b.HitBox.Bottom > this.HitBox.Top);
                    if (leftImpedingBlocks)
                        return true;
                }
                else
                {
                    var rightImpedingBlocks = collisions.Any(b => b.HitBox.Left < _keen.HitBox.Left
                       && b.HitBox.Left > this.HitBox.Left
                       && b.HitBox.Top < this.HitBox.Bottom
                       && b.HitBox.Bottom > this.HitBox.Top);
                    if (rightImpedingBlocks)
                        return true;
                }

                if (IsUpDirection())
                {
                    var upImpedingBlocks = collisions.Any(b => b.HitBox.Bottom > _keen.HitBox.Bottom
                        && b.HitBox.Bottom < this.HitBox.Top
                        && b.HitBox.Right > this.HitBox.Left
                        && b.HitBox.Left < this.HitBox.Right);
                    if (upImpedingBlocks)
                        return true;
                }
                else
                {
                    var downImpedingBlocks = collisions.Any(b => b.HitBox.Top < _keen.HitBox.Top
                        && b.HitBox.Top > this.HitBox.Bottom
                        && b.HitBox.Right > this.HitBox.Left
                        && b.HitBox.Left < this.HitBox.Right);
                    if (downImpedingBlocks)
                        return true;
                }
            }
            return false;
        }

        public void Move()
        {
            int velocity = this.State == DopefishMoveState.CHASING
              ? MOVE_VELOCITY
              : this.State == DopefishMoveState.ATTACKING ? ATTACK_VELOCITY : 0;

            SetDirectionFromKeenLocation();
            int xOffset = GetXOffsetForCollisionDetection(velocity), yOffset = GetYOffsetForCollisionDetection(velocity);

            Rectangle areaToCheck = new Rectangle(this.HitBox.X + xOffset, this.HitBox.Y + yOffset, this.HitBox.Width + yOffset, this.HitBox.Height + velocity);
            var collisions = this.CheckCollision(areaToCheck, true);

            bool isUpDirection = IsUpDirection();
            bool isDownDirection = IsDownDirection();
            bool isLeftDirection = IsLeftDirection();
            bool isRightDirection = IsRightDirection();

            Rectangle newLocation = this.HitBox;

            if (isLeftDirection)
            {
                var leftTile = GetRightMostLeftTile(collisions);
                if (leftTile != null)
                {
                    newLocation = new Rectangle(leftTile.HitBox.Right + 1, newLocation.Y, newLocation.Width, newLocation.Height);
                }
                else
                {
                    newLocation = new Rectangle(newLocation.X - velocity, newLocation.Y, newLocation.Width, newLocation.Height);
                }
            }
            else if (isRightDirection)
            {
                var rightTile = GetLeftMostRightTile(collisions);
                if (rightTile != null)
                {
                    newLocation = new Rectangle(rightTile.HitBox.Left - newLocation.Width - 1, newLocation.Y, newLocation.Width, newLocation.Height);
                }
                else
                {
                    newLocation = new Rectangle(newLocation.X + velocity, newLocation.Y, newLocation.Width, newLocation.Height);
                }
            }

            if (isUpDirection)
            {
                var ceilingTile = GetCeilingTile(collisions);
                if (ceilingTile != null)
                {
                    newLocation = new Rectangle(newLocation.X, ceilingTile.HitBox.Bottom + 1, newLocation.Width, newLocation.Height);
                }
                else
                {
                    newLocation = new Rectangle(newLocation.X, newLocation.Y - velocity, newLocation.Width, newLocation.Height);
                }
            }
            else if (isDownDirection)
            {
                var landingTile = GetTopMostLandingTile(velocity);
                if (landingTile != null)
                {
                    newLocation = new Rectangle(newLocation.X, landingTile.HitBox.Top - newLocation.Height - 1, newLocation.Width, newLocation.Height);
                }
                else
                {
                    newLocation = new Rectangle(newLocation.X, newLocation.Y + velocity, newLocation.Width, newLocation.Height);
                }
            }

            this.HitBox = newLocation;
            UpdateSpriteByDelay();
        }

        private int GetXOffsetForCollisionDetection(int velocity)
        {
            if (_keen.HitBox.Left < this.HitBox.Left)
            {
                return velocity * -1;
            }
            else if (_keen.HitBox.Right > this.HitBox.Right)
            {
                return velocity;
            }

            return 0;
        }

        private int GetYOffsetForCollisionDetection(int velocity)
        {
            if (_keen.HitBox.Top < this.HitBox.Top)
            {
                return velocity * -1;
            }
            else if (_keen.HitBox.Bottom > this.HitBox.Bottom)
            {
                return velocity;
            }

            return 0;
        }

        //protected override CollisionObject GetLeftMostRightTile(List<CollisionObject> collisions)
        //{
        //    if (collisions.Any() && collisions.OfType<DebugTile>().Any())
        //    {
        //        var rightTiles = collisions.OfType<DebugTile>().Where(c => c.HitBox.Left > this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
        //        if (rightTiles.Any())
        //        {
        //            int minX = rightTiles.Select(t => t.HitBox.Left).Min();
        //            CollisionObject obj = rightTiles.FirstOrDefault(x => x.HitBox.Left == minX);
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        //protected override CollisionObject GetRightMostLeftTile(List<CollisionObject> collisions)
        //{
        //    if (collisions.Any() && collisions.OfType<DebugTile>().Any())
        //    {
        //        var leftTiles = collisions.OfType<DebugTile>().Where(c => c.HitBox.Left < this.HitBox.Left && c.HitBox.Top < this.HitBox.Bottom && c.HitBox.Bottom > this.HitBox.Top).ToList();
        //        if (leftTiles.Any())
        //        {
        //            int maxX = leftTiles.Select(t => t.HitBox.Right).Max();
        //            CollisionObject obj = leftTiles.FirstOrDefault(x => x.HitBox.Right == maxX);
        //            return obj;
        //        }
        //    }
        //    return null;
        //}

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Fart()
        {
            if (this.State != DopefishMoveState.FARTING)
            {
                this.State = DopefishMoveState.FARTING;
                _currentFartTime = 0;
            }

            if (_currentFartTime == FART_TIME * 3 / 4)
            {
                MakeFartBubble();
            }

            if (_currentFartTime++ == FART_TIME)
            {
                _currentFartTime = 0;
                _currentSpriteChangeDelayTick = SPRITE_CHANGE_DELAY;
                
                this.Chase();
            }
        }

        private void MakeFartBubble()
        {
            FartBubble bubble = new FartBubble(_collisionGrid, new Rectangle(this.HitBox.Right - 30, this.HitBox.Y + 30, 30, 30));
            bubble.Create += new EventHandler<KeenEventArgs.ObjectEventArgs>(bubble_Create);
            bubble.Remove += new EventHandler<ObjectEventArgs>(bubble_Remove);
            ObjectEventArgs e = new ObjectEventArgs()
            {
                ObjectSprite = bubble
            };
            OnCreate(e);
        }

        void bubble_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void bubble_Create(object sender, KeenEventArgs.ObjectEventArgs e)
        {
            OnCreate(e);
        }

        protected void OnCreate(ObjectEventArgs e)
        {
            if (this.Create != null)
                this.Create(this, e);
        }

        protected void OnRemove(ObjectEventArgs e)
        {
            if (this.Remove != null)
                this.Remove(this, e);
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

        DopefishMoveState State
        {
            get
            {
                return _dopefishState;
            }
            set
            {
                _dopefishState = value;
                UpdateSpriteByState();
            }
        }

        private void UpdateSpriteByState()
        {
            if (_dopefishState == DopefishMoveState.CHASING)
            {
                UpdateSpriteByDelay();
            }
            else
            {
                UpdateSprite();
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
                UpdateSpriteByState();
                SetAttackVision();
            }
        }

        private void UpdateSpriteByDelay()
        {
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                var spriteSet = IsLeftDirection() ? _moveLeftImages : _moveRightImages;
                if (_currentMoveImage >= spriteSet.Length)
                {
                    _currentMoveImage = 0;
                }
                _currentSpriteChangeDelayTick = 0;
                UpdateSprite();
                _currentMoveImage++;
            }
        }

        private bool IsUpDirection()
        {
            return Direction == Enums.Direction.UP || Direction == Enums.Direction.UP_LEFT || Direction == Enums.Direction.UP_RIGHT;
        }

        private bool IsDownDirection()
        {
            return Direction == Enums.Direction.DOWN || Direction == Enums.Direction.DOWN_LEFT || Direction == Enums.Direction.DOWN_RIGHT;
        }

        private bool IsLeftDirection()
        {
            return Direction == Enums.Direction.LEFT || Direction == Enums.Direction.UP_LEFT || Direction == Enums.Direction.DOWN_LEFT;
        }

        private bool IsRightDirection()
        {
            return Direction == Enums.Direction.RIGHT || Direction == Enums.Direction.UP_RIGHT || Direction == Enums.Direction.DOWN_RIGHT;
        }

        private void UpdateSprite()
        {
            switch (_dopefishState)
            {
                case DopefishMoveState.CHASING:
                case DopefishMoveState.PAUSING:
                    var spriteSet = IsLeftDirection() ? _moveLeftImages : _moveRightImages;
                    if (_currentMoveImage >= spriteSet.Length)
                        _currentMoveImage = 0;
                    _sprite.Image = spriteSet[_currentMoveImage];
                    break;
                case DopefishMoveState.ATTACKING:
                    _sprite.Image = IsLeftDirection()
                        ? Properties.Resources.keen4_dopefish_attack_left
                        : Properties.Resources.keen4_dopefish_attack_right;
                    break;
                case DopefishMoveState.LOOKING:
                    _sprite.Image = Properties.Resources.keen4_dopefish_look;
                    break;
                case DopefishMoveState.FARTING:
                    _sprite.Image = Properties.Resources.keen4_dopefish_fart;
                    break;
            }
        }

        public bool DeadlyTouch
        {
            get { return false; }
        }

        public void HandleHit(ITrajectory trajectory)
        {
            throw new NotImplementedException();
        }

        public bool IsActive
        {
            get { return false; }
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
    enum DopefishMoveState
    {
        CHASING,
        ATTACKING,
        PAUSING,
        LOOKING,
        FARTING
    }
}
