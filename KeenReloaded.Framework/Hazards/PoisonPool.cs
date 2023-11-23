using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Hazards
{
    public class PoisonPool : Hazard, IUpdatable
    {
        private int _floorCollisionVerticalOffset;
        private int _poisonCollisionVerticalOffset;
        private const int FLOOR_LEFT_WIDTH = 32, FLOOR_RIGHT_WIDTH = 38;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private int _currentSprite;
        private int _lengths;

        private CollisionObject _rightFloor, _leftFloor, _middleFloor;

        private Image[] _leftSprites = SpriteSheet.Keen4PoisonPoolLeftImages;
        private Image[] _rightSprites = SpriteSheet.Keen4PoisonPoolRightImages;
        private Image[] _middleSprites = SpriteSheet.Keen4PoisonPoolMiddleImages;

        private List<PictureBox> _sprites;
        public PoisonPool(SpaceHashGrid grid, Rectangle hitbox, int lengths = 1) : base(grid, hitbox, HazardType.KEEN4_POISON_POOL)
        {
            if (lengths < 0)
                throw new ArgumentException("Lengths cannot be less than 1");

            _lengths = lengths;

            Initialize();
        }

        public IEnumerable<PictureBox> Sprites
        {
            get
            {
                return _sprites;
            }
            private set
            {
                _sprites = value as List<PictureBox>;
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
                if (value != null && this.Sprite != null)
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }

        private void Initialize()
        {
            _floorCollisionVerticalOffset = this.HitBox.Height / 2;
            _poisonCollisionVerticalOffset = _floorCollisionVerticalOffset + 2;

            List<PictureBox> sprites = new List<PictureBox>();

            PictureBox pLeft = new PictureBox();
            pLeft.SizeMode = PictureBoxSizeMode.AutoSize;
            pLeft.Location = new Point(this.HitBox.X, this.HitBox.Y);
            pLeft.Image = Properties.Resources.keen4_poison_pool1_edge_left;

           sprites.Add(pLeft);

            _leftFloor = new DebugTile(_collisionGrid, new Rectangle(
              this.HitBox.Left,
              this.HitBox.Y + _floorCollisionVerticalOffset,
              FLOOR_LEFT_WIDTH,
              this.HitBox.Height - _floorCollisionVerticalOffset));

            int newRight = pLeft.Right;
            int middleWidth = 0;
            for (int i = 0; i < _lengths; i++)
            {
                PictureBox pMiddle = new PictureBox();
                pMiddle = new PictureBox();
                pMiddle.SizeMode = PictureBoxSizeMode.AutoSize;
                pMiddle.Location = new Point(newRight, pLeft.Top);
                pMiddle.Image = Properties.Resources.keen4_poison_pool1_middle;
                middleWidth = pMiddle.Width;
                sprites.Add(pMiddle);
                newRight += pMiddle.Width;
            }

            PictureBox pRight = new PictureBox();
            pRight.SizeMode = PictureBoxSizeMode.AutoSize;
            pRight.Location = new Point(newRight, this.HitBox.Y);
            pRight.Image = Properties.Resources.keen4_poison_pool1_edge_right;

            sprites.Add(pRight);

            _middleFloor = new DebugTile(_collisionGrid, new Rectangle(
                 _leftFloor.HitBox.Right
               , this.HitBox.Y + _poisonCollisionVerticalOffset + 1
               , middleWidth * _lengths
               , this.HitBox.Height - _floorCollisionVerticalOffset - 1));

            _rightFloor = new DebugTile(_collisionGrid, new Rectangle(
                 newRight
               , this.HitBox.Y + _floorCollisionVerticalOffset
               , FLOOR_RIGHT_WIDTH
               , this.HitBox.Height - _floorCollisionVerticalOffset));
            this.HitBox = new Rectangle(
               _leftFloor.HitBox.Right
               , this.HitBox.Y + _poisonCollisionVerticalOffset
               , middleWidth * _lengths
               , this.HitBox.Height - _floorCollisionVerticalOffset);

            this.Sprites = sprites;
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(ref _currentSpriteChangeDelayTick, ref _currentSprite, SPRITE_CHANGE_DELAY, UpdateSprite);
        }

        private void UpdateSprite()
        {
            if (_currentSprite >= _middleSprites.Length)
            {
                _currentSprite = 0;
            }
            _sprites[0].Image = _leftSprites[_currentSprite];
            for (int i = 1; i < _sprites.Count - 1; i++)
            {
                _sprites[i].Image = _middleSprites[_currentSprite];
            }
            _sprites[_sprites.Count - 1].Image = _rightSprites[_currentSprite];
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_lengths}";
        }
    }
}
