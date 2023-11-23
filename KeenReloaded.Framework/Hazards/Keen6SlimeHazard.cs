using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen6SlimeHazard : DebugTile, IUpdatable
    {
        private readonly int _extraLengths;
        private const int SPRITE_HEIGHT = 64;
        private const int EDGE_WIDTH = 32;
        private const int MID_WIDTH = 64;
        private const int COLLISION_VERTICAL_OFFSET = 48;

        private List<IUpdatable> _parts;

        public Keen6SlimeHazard(SpaceHashGrid grid, Rectangle hitbox, int extraLengths = 0)
            : base(grid, hitbox)
        {
            _extraLengths = extraLengths < 0 ? 0 : extraLengths;
            Initialize();
        }

        private void Initialize()
        {
            int currentX = this.HitBox.X;
            int currentY = this.HitBox.Y;
            _parts = new List<IUpdatable>();
            Keen6SlimeHazardPart leftPart = new Keen6SlimeHazardPart(
                _collisionGrid
                , new Rectangle(currentX, currentY, EDGE_WIDTH, SPRITE_HEIGHT)
                , Keen6SlimePartType.LEFT);

            _parts.Add(leftPart);
            currentX += EDGE_WIDTH;
            for (int i = 0; i < _extraLengths; i++)
            {
                Keen6SlimeHazardPart part = new Keen6SlimeHazardPart(
                    _collisionGrid
                    , new Rectangle(currentX, currentY, MID_WIDTH, SPRITE_HEIGHT)
                    , Keen6SlimePartType.MIDDLE);
                _parts.Add(part);
                currentX += MID_WIDTH;
            }
            Keen6SlimeHazardPart rightPart = new Keen6SlimeHazardPart(
             _collisionGrid
             , new Rectangle(currentX, currentY, EDGE_WIDTH, SPRITE_HEIGHT)
             , Keen6SlimePartType.RIGHT);
            _parts.Add(rightPart);

            int width = (EDGE_WIDTH * 2) + (MID_WIDTH * _extraLengths);
           
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y + COLLISION_VERTICAL_OFFSET, width, SPRITE_HEIGHT - COLLISION_VERTICAL_OFFSET);
            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Direction.UP_RIGHT);
        }

        public void Update()
        {
            foreach (var part in _parts)
            {
                part.Update();
            }
        }

        public List<PictureBox> Sprites
        {
            get
            {
                return _parts?.OfType<ISprite>().Select(p => p.Sprite).ToList();
            }

        }

        public override string ToString()
        {
            return base.ToString() + "|" + _extraLengths;
        }
    }

    class Keen6SlimeHazardPart : Hazard, IUpdatable
    {
        private readonly Keen6SlimePartType _partType;
        private Image[] _sprites;
        private const int VERTICAL_OFFSET = 16;
        private const int DEATH_RANGE_HEIGHT = 4;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _spriteChangeDelayTick;
        private int _currentSprite;
        public Keen6SlimeHazardPart(SpaceHashGrid grid, Rectangle hitbox, Keen6SlimePartType partType)
            : base(grid, hitbox, HazardType.KEEN6_SLIME_FLOOR)
        {
            _partType = partType;
            SetSpriteFromType(HazardType.KEEN6_SLIME_FLOOR);
        }

        protected override void SetSpriteFromType(HazardType type)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            switch (_partType)
            {
                case Keen6SlimePartType.LEFT:
                    _sprites = SpriteSheet.Keen6SlimeHazardLeftImages;
                    break;
                case Keen6SlimePartType.RIGHT:
                    _sprites = SpriteSheet.Keen6SlimeHazardRightImages;
                    break;
                case Keen6SlimePartType.MIDDLE:
                    _sprites = SpriteSheet.Keen6SlimeHazardMiddleImages;
                    break;
            }
            _sprite.Image = _sprites[_currentSprite];
            this.HitBox = new Rectangle(this.HitBox.X, this.Sprite.Location.Y + VERTICAL_OFFSET, this.HitBox.Width, DEATH_RANGE_HEIGHT);
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(
                ref _spriteChangeDelayTick
              , ref _currentSprite
              , SPRITE_CHANGE_DELAY
              , () =>
                {
                    if (_currentSprite >= _sprites.Length)
                    {
                        _currentSprite = 0;
                    }
                    this.Sprite.Image = _sprites[_currentSprite];
                });
        }
    }

    enum Keen6SlimePartType
    {
        LEFT,
        RIGHT,
        MIDDLE
    }
}
