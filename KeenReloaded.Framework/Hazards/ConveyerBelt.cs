using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class ConveyerBelt : Hazard, IUpdatable
    {

        private readonly Direction _direction;
        private readonly int _addedLengths;
        private CommanderKeen _keen;
        private List<ConveyerBeltPart> _parts = new List<ConveyerBeltPart>();
        

        public ConveyerBelt(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, Direction conveyerBeltDirection, int addedLengths = 0) 
            : base(grid, hitbox, HazardType.KEEN6_CONVEYER_BELT)
        {
            if (keen == null)
                throw new ArgumentNullException("keen cannot be null");

            _keen = keen;
            _direction = conveyerBeltDirection == Direction.LEFT ? Direction.LEFT : Direction.RIGHT;
            _addedLengths = addedLengths < 0 ? 0 : addedLengths;
            Initialize();
        }

        public List<ISprite> Sprites
        {
            get
            {
                return _parts.OfType<ISprite>().ToList();
            }
        }

        private void Initialize()
        {
            int currentX = this.HitBox.X;
            int currentY = this.HitBox.Y;
            
            Rectangle leftPartRect = new Rectangle(currentX, currentY, ConveyerBeltPart.LEFT_EDGE_WIDTH, ConveyerBeltPart.PART_HEIGHT);
            var leftPart = new ConveyerBeltPart(_collisionGrid, leftPartRect, ConveyerBeltPartType.LEFT, _direction, _keen);
            _parts.Add(leftPart);

            for (int i = 0; i < _addedLengths; i++)
            {
                currentX += ConveyerBeltPart.MIDDLE_WIDTH;
                Rectangle middlePartRect = new Rectangle(currentX, currentY, ConveyerBeltPart.MIDDLE_WIDTH, ConveyerBeltPart.PART_HEIGHT);
                ConveyerBeltPart part = new ConveyerBeltPart(_collisionGrid, middlePartRect, ConveyerBeltPartType.MIDDLE, _direction, _keen);
                _parts.Add(part);
            }
            currentX += ConveyerBeltPart.MIDDLE_WIDTH;
            Rectangle rightPartRect = new Rectangle(currentX, currentY, ConveyerBeltPart.RIGHT_EDGE_WIDTH, ConveyerBeltPart.PART_HEIGHT);
            var rightPart = new ConveyerBeltPart(_collisionGrid, rightPartRect, ConveyerBeltPartType.RIGHT, _direction, _keen);
            _parts.Add(rightPart);
        }

        public override bool IsDeadly => false;

        protected override void SetSpriteFromType(HazardType type)
        {
             
        }

        public void Update()
        {
            foreach (var part in _parts)
            {
                part.Update();
            }
        }
    }

    class ConveyerBeltPart : DebugTile, IUpdatable
    {
        private const int VERTICAL_LANDING_OFFSET = 32;
        private const int VERTICAL_COLLISION_LENGTH = 64;
        private const int SPRITE_CHANGE_DELAY = 1;
        private const int PUSH_STRENGTH = 2;
        private int _currentSpriteChangeDelayTick;
        private int _currentSpriteIndex;

        private readonly ConveyerBeltPartType _partType;
        private readonly Direction _direction;
        private Image[] _sprites;

        public const int LEFT_EDGE_WIDTH = 128;
        public const int RIGHT_EDGE_WIDTH = 126;
        public const int MIDDLE_WIDTH = 64;
        public const int PART_HEIGHT = 96;

        private CommanderKeen _keen;

        public ConveyerBeltPart(SpaceHashGrid grid, Rectangle hitbox, ConveyerBeltPartType partType, Direction direction, CommanderKeen keen) : base(grid, hitbox)
        {
            _partType = partType;
            _direction = direction;
            _keen = keen;
            Initialize();
        }

        private void Initialize()
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            _sprite.Location = this.HitBox.Location;
            switch (_partType)
            {
                case ConveyerBeltPartType.LEFT:
                    _sprites = SpriteSheet.Keen6ConveyerBeltLeftSprites;
                    break;
                case ConveyerBeltPartType.RIGHT:
                    _sprites = SpriteSheet.Keen6ConveyerBeltRightSprites;
                    break;
                case ConveyerBeltPartType.MIDDLE:
                    _sprites = SpriteSheet.Keen6ConveyerBeltMiddleSprites;
                    break;
            }
            _currentSpriteIndex = _direction == Direction.RIGHT ? 0 : _sprites.Length - 1;
            _sprite.Image = _sprites[_currentSpriteIndex];

            this.HitBox = new Rectangle(_sprite.Location.X, _sprite.Location.Y + VERTICAL_LANDING_OFFSET, _sprite.Width, VERTICAL_COLLISION_LENGTH);
            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Direction.UP_RIGHT);
        }

        public  ConveyerBeltPartType PartType
        {
            get => _partType;
        }

        public void Update()
        {
            if (_currentSpriteChangeDelayTick++ >= SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                if (_direction == Direction.RIGHT)
                    RotateClockwise();
                else
                    RotateCounterClockwise();

                if (this.IsKeenStandingOnThis())
                {
                    this.UpdateKeenHorizontalPosition();
                }
            }
        }

        private void RotateClockwise()
        {
            if (++_currentSpriteIndex >= _sprites.Length)
            {
                _currentSpriteIndex = 0;
            }
            _sprite.Image = _sprites[_currentSpriteIndex];
        }

        private void RotateCounterClockwise()
        {
            if (--_currentSpriteIndex < 0)
            {
                _currentSpriteIndex = _sprites.Length - 1;
            }
            _sprite.Image = _sprites[_currentSpriteIndex];
        }

        private void UpdateKeenHorizontalPosition()
        {
            if (!_keen.IsDead())
                _keen.GetMovedHorizontally(this, _direction, PUSH_STRENGTH);
        }

        private bool IsKeenStandingOnThis()
        {
            bool isStanding = _keen.HitBox.Bottom == this.HitBox.Top - 1 || _keen.HitBox.Bottom == this.HitBox.Top;
            return isStanding;
        }
    }

    enum ConveyerBeltPartType
    {
        LEFT,
        RIGHT,
        MIDDLE
    }
}
