using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.Hazards
{
    public class TarPit : Hazard, IUpdatable
    {
        private List<PictureBox> _sprites;
        private List<PictureBox> _depthSprites;
        private int _lengths;
        private int _depths;
        private const int SPRITE_WIDTH = 64;
        private const int SPRITE_HEIGHT = 64;
        private const int HITBOX_VERTICAL_OFFSET = 32;
        private const int SPRITE_CHANGE_DELAY = 1;
        private int _currentSpriteChangeDelayTick;
        private Dictionary<int, int> _currentSpriteIndeces;

        public TarPit(SpaceHashGrid grid, Rectangle hitbox, int lengths = 1, int depths = 0) : base(grid, hitbox, HazardType.KEEN4_TAR)
        {
            if (lengths < 1)
                throw new ArgumentException("lengths cannot be less than 1");

            if (depths < 0)
                throw new ArgumentException("depths cannot be less than 1");

            _lengths = lengths;
            _depths = depths;
            Initialize();
        }

        public override Rectangle HitBox
        {
            get => base.HitBox;
            protected set
            {
                base.HitBox = value;
                if (value != null && _collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Direction.UP_RIGHT);
                }
            }
        }

        public IEnumerable<PictureBox> Sprites
        {
            get
            {
                return _sprites;
            }
        }

        public IEnumerable<PictureBox> DepthSprites
        {
            get
            {
                return _depthSprites;
            }
        }

        private void Initialize()
        {
            _sprites = new List<PictureBox>();
            _depthSprites = new List<PictureBox>();
            _currentSpriteIndeces = new Dictionary<int, int>();
            int currentX = this.HitBox.X;
            var images = SpriteSheet.Keen4TarPitImages;
            for (int i = 0; i < _lengths; i++)
            {
                PictureBox p = new PictureBox();
                p.SizeMode = PictureBoxSizeMode.Normal;
                p.Width = SPRITE_WIDTH;
                p.Height = SPRITE_HEIGHT;
                p.Location = new Point(currentX, this.HitBox.Y);
                int randVal = _random.Next(0, images.Length);
                p.Image = images[randVal];

                _sprites.Add(p);
                _currentSpriteIndeces.Add(i, randVal);

                for (int j = 0; j < _depths; j++)
                {
                    PictureBox pDepth = new PictureBox();
                    pDepth.SizeMode = PictureBoxSizeMode.AutoSize;
                    pDepth.Location = new Point(currentX, this.HitBox.Y + (SPRITE_HEIGHT * (j + 1)) - 2);
                    pDepth.Image = Properties.Resources.keen4_tar_depth;
                    _depthSprites.Add(pDepth);
                }
                currentX += SPRITE_WIDTH;
            }

            this.HitBox = new Rectangle(
                  this.HitBox.X//x
                , this.HitBox.Y + HITBOX_VERTICAL_OFFSET//y
                , SPRITE_WIDTH * _lengths//width
                , this.HitBox.Height - HITBOX_VERTICAL_OFFSET + (SPRITE_HEIGHT * _depths));//height
        }

        public void Update()
        {
            if (_currentSpriteChangeDelayTick++ == SPRITE_CHANGE_DELAY)
            {
                _currentSpriteChangeDelayTick = 0;
                for (int i = 0; i < _currentSpriteIndeces.Count; i++)
                {
                    int spriteIndex = _currentSpriteIndeces[i];
                    if (++spriteIndex >= SpriteSheet.Keen4TarPitImages.Length)
                    {
                        spriteIndex = 0;
                    }
                    _currentSpriteIndeces[i] = spriteIndex;
                    _sprites[i].Image = SpriteSheet.Keen4TarPitImages[spriteIndex];
                }
            }
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.Sprite.Location.X}|{this.Sprite.Location.Y}|{_lengths}|{_depths}";
        }
    }
}
