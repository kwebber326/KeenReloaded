using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Enemies
{
    public class Schoolfish : CollisionObject, IUpdatable, ISprite
    {
        private readonly CommanderKeen _keen;
        private PictureBox _sprite;
        private Direction _direction;
        private int _currentSpriteIndex;
        private int _curentSpriteIndexChangeDelayTick;
        private const int SPRITE_CHANGE_DELAY = 5;

        private const int VELOCITY = 10;

        private const int FOLLOW_KEEN_DELAY_MIN = 3;
        private const int FOLLOW_KEEN_DELAY_MAX = 5;
        private int _followKeenDelay = 4;
        private int _followKeenDelayTick;

        public Schoolfish(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen) : base(grid, hitbox)
        {
            if (keen == null)
                throw new ArgumentNullException("keen was not properly set");

            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            SetFullDirectionFromKeenLocation(_keen, ref _direction);
            RefreshCollisionNodes();

            _sprite = new PictureBox();
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;

            UpdateSprite();

            _random = new Random();
        }

        private void UpdateSprite()
        {
           
            var sprites = IsLeftDirection(_direction)
                ? SpriteSheet.SchoolFishLeftImages : SpriteSheet.SchoolFishRightImages;

            if (_currentSpriteIndex >= sprites.Length)
                _currentSpriteIndex = 0;

            _sprite.Image = sprites[_currentSpriteIndex];
        }

        public override Rectangle HitBox {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (_sprite != null && this.HitBox != null)
                {
                    _sprite.Location = this.HitBox.Location;
                    RefreshCollisionNodes();
                }
            }
        }

        private void RefreshCollisionNodes()
        {
            this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
        }

        public PictureBox Sprite => _sprite;

        protected override void SetVerticalDirectionFromKeenLocation(CommanderKeen keen, ref Direction direction)
        {
            if (keen == null)
                throw new ArgumentNullException("keen cannot be null");

            if (keen.HitBox.Top > this.HitBox.Top)
            {
                direction = Direction.DOWN;
            }
            else
            {
                direction = Direction.UP;
            }
        }

        public void Update()
        {
            //set direction based on keen location using a delay that is randomized each time
            //to make the movement pattern more natural looking
            UpdateFollowingDelay();

            //initial collision detection
            MoveBasedOnCollisionDetection();

            //update sprite by delay
            this.UpdateSpriteByDelayBase(ref _curentSpriteIndexChangeDelayTick, ref _currentSpriteIndex, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void MoveBasedOnCollisionDetection()
        {
            Rectangle areaToCheck;
            int xOffset = 0, yOffset = 0, x, y, width, height;
            SetCollisionCheckOffsetsByDirection(ref xOffset, ref yOffset);
            x = this.HitBox.X + xOffset;
            y = this.HitBox.Y + yOffset;
            width = this.HitBox.Width + Math.Abs(xOffset);
            height = this.HitBox.Height + Math.Abs(yOffset);
            areaToCheck = new Rectangle(x, y, width, height);
            var collisions = this.CheckCollision(areaToCheck, true);
            if (!collisions.Any(c => !(c is PlatformTile || c is PoleTile)))
            {
                //move
                Move();
            }
        }

        private void UpdateFollowingDelay()
        {
            if (++_followKeenDelayTick >= _followKeenDelay)
            {
                this.SetFullDirectionFromKeenLocation(_keen, ref _direction);
                _followKeenDelayTick = 0;
                _random = new Random();
                _followKeenDelay = _random.Next(FOLLOW_KEEN_DELAY_MIN, FOLLOW_KEEN_DELAY_MAX + 1);
            }
        }

        private void Move()
        {
            int x = this.HitBox.X, y = this.HitBox.Y;
            if (IsLeftDirection(_direction))
            {
                x -= VELOCITY;
            }
            else
            {
                x += VELOCITY;
            }

            if (IsUpDirection(_direction))
            {
                y -= VELOCITY;
            }
            else
            {
                y += VELOCITY;
            }

            this.HitBox = new Rectangle(x, y, this.HitBox.Width, this.HitBox.Height);
        }

        private void SetCollisionCheckOffsetsByDirection(ref int xOffset, ref int yOffset)
        {
            switch (_direction)
            {
                case Direction.DOWN:
                    xOffset = 0;
                    yOffset = VELOCITY;
                    break;
                case Direction.UP:
                    xOffset = 0;
                    yOffset = VELOCITY * -1;
                    break;
                case Direction.LEFT:
                    xOffset = VELOCITY * -1;
                    yOffset = 0;
                    break;
                case Direction.RIGHT:
                    xOffset = VELOCITY;
                    yOffset = 0;
                    break;
                case Direction.DOWN_LEFT:
                    xOffset = VELOCITY * -1;
                    yOffset = VELOCITY;
                    break;
                case Direction.UP_LEFT:
                    xOffset = VELOCITY * -1;
                    yOffset = VELOCITY * -1;
                    break;
                case Direction.DOWN_RIGHT:
                    xOffset = VELOCITY;
                    yOffset = VELOCITY;
                    break;
                case Direction.UP_RIGHT:
                    xOffset = VELOCITY;
                    yOffset = VELOCITY * -1;
                    break;
            }
        }

        protected override void HandleCollision(CollisionObject obj)
        {
         
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}";
        }
    }
}
