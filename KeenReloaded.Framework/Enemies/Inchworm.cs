using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Enemies
{
    public class Inchworm : CollisionObject, IUpdatable, ISprite
    {
        private readonly CommanderKeen _keen;
        private readonly Image[] _moveLeftSprites = SpriteSheet.InchwormLeftImages;
        private readonly Image[] _moveRightSprites = SpriteSheet.InchwormRightImages;
        private PictureBox _sprite;
        private InchWormMoveState _state;
        private int _currentSpriteIndex;
        private int _currentSpriteChangeDelayTick;
        private int _bounceOffSteps = 0;
        private int _currentBounceOffStep = 0;
        private Direction _direction;

        private const int SPRITE_CHANGE_DELAY = 15;
        private const int MIN_BOUNCE_OFF_STEPS = 1, MAX_BOUNCE_OFF_STEPS = 5;
        protected const int FALL_VELOCITY = 20;
        protected const int MOVE_VELOCITY = 10;
        public Inchworm(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen) : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("keen was not properly set");

            _keen = keen;

            Initialize();
        }

        public override Rectangle HitBox
        {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (this.Sprite != null && base.HitBox != null)
                {
                    //align the sprite with the edge of the hitbox in order to represent current position visually
                    UpdateSpritePositionByHitbox();
                    var directionToUpdate = _state == InchWormMoveState.FALLING ? Direction.DOWN : _direction;
                    //update collision detection algorithm to show which space hashes currently intersect with this object
                    this.UpdateCollisionNodes(directionToUpdate);
                }
            }
        }

        private void UpdateSpritePositionByHitbox()
        {
            var bottomAlignedPointY = this.HitBox.Bottom - this.Sprite.Height;
            var edgeAlignedPointX = _direction == Direction.LEFT ?
                this.HitBox.Left : this.HitBox.Right - this.Sprite.Width;
            this.Sprite.Location = new Point(edgeAlignedPointX, bottomAlignedPointY);
        }

        private void Initialize()
        {
            _state = InchWormMoveState.FALLING;
            SetHorizontalDirectionFromKeenLocation(_keen, ref _direction);
            _sprite = new PictureBox();
            _sprite.Image = _direction == Direction.LEFT ? _moveLeftSprites.FirstOrDefault() : _moveRightSprites.FirstOrDefault();
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
            this.UpdateSpritePositionByHitbox();
            _random = new Random();
        }

        public PictureBox Sprite => _sprite;



        public void Update()
        {
            if (_state == InchWormMoveState.FALLING)
            {
                var tile = this.BasicFallReturnTile(FALL_VELOCITY);
                if (tile != null)
                {
                    this.HitBox = new Rectangle(this.HitBox.X, tile.HitBox.Top - this.HitBox.Height - 1, this.HitBox.Width, this.HitBox.Height);
                    _state = InchWormMoveState.FOLLOWING_KEEN;
                }
            }
            else
            {
                //check if the floor is not underneath this object (disappearing platforms can trigger this)
                if (this.IsNothingBeneath())
                {
                    _state = InchWormMoveState.FALLING;
                    return;
                }

                //update move state on delay
                if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
                {
                    _currentSpriteChangeDelayTick = 0;
                    _currentSpriteIndex++;
                    //follow keen
                    if (_state == InchWormMoveState.FOLLOWING_KEEN)
                    {
                        TryFollowKeen();
                    }
                    else
                    {
                        ExecuteDirectionChangeSequence();
                    }
                }
            }
        }

        private void ExecuteDirectionChangeSequence()
        {
            //checks if this is the first step in the sequence
            if (_bounceOffSteps == 0)
            {
                _random = new Random();
                _bounceOffSteps = _random.Next(MIN_BOUNCE_OFF_STEPS, MAX_BOUNCE_OFF_STEPS + 1);
                _currentBounceOffStep = 0;
            }
            else if (++_currentBounceOffStep <= _bounceOffSteps)
            {
                this.UpdateSprite();
                this.UpdateMovement();
            }
            else
            {
                _currentBounceOffStep = 0;
                _bounceOffSteps = 0;
                this.TurnAround();
                _state = InchWormMoveState.FOLLOWING_KEEN;
            }
        }

        private void TurnAround()
        {
            _direction = this.ChangeHorizontalDirection(_direction);
            this.UpdateSprite();
        }

        private void TryFollowKeen()
        {
            _direction = SetDirectionFromObjectHorizontal(_keen, true);
            this.UpdateSprite();
            this.UpdateMovement();
        }

        private void UpdateMovement()
        {
            int xPos = _direction == Direction.LEFT
                ? this.HitBox.Left - MOVE_VELOCITY
                : this.HitBox.Right;

            Rectangle areaToCheck = new Rectangle(xPos, this.HitBox.Y, this.HitBox.Width + MOVE_VELOCITY, this.HitBox.Height);
            var collisions = this.CheckCollision(areaToCheck, true);
            bool isOnEdge = this.IsOnEdge(_direction);
            if (collisions.Any() || isOnEdge)
            {
                TurnAround();
                _state = InchWormMoveState.BOUNCING_OFF_BARRIER;
            }
            else
            {
                int xChange = _direction == Direction.LEFT ? MOVE_VELOCITY * -1 : MOVE_VELOCITY;
                this.HitBox = new Rectangle(this.HitBox.X + xChange, this.HitBox.Y, this.HitBox.Width, this.HitBox.Height);
            }
        }

        public void UpdateSprite()
        {
            var sprites = _direction == Direction.LEFT ? _moveLeftSprites : _moveRightSprites;

            if (_currentSpriteIndex >= sprites.Length)
            {
                _currentSpriteIndex = 0;
            }

            _sprite.Image = sprites[_currentSpriteIndex];
            this.UpdateSpritePositionByHitbox();
        }

        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }

    enum InchWormMoveState
    {
        FOLLOWING_KEEN,
        BOUNCING_OFF_BARRIER,
        FALLING
    }
}
