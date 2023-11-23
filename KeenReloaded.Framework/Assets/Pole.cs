using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using System.Windows.Forms;
using KeenReloaded.Framework.KeenEventArgs;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Assets
{
    public class Pole : CollisionObject, ICreateRemove, IBiomeTile
    {
        private const int POLE_WIDTH = 12;
        public Pole(SpaceHashGrid grid, Rectangle hitbox, PoleType poleType, BiomeType biomeType, int addedLengths = 0)
            : base(grid, hitbox)
        {
            _biomeType = biomeType;
            _poleType = poleType;
            _addedLengths = addedLengths;
            Initialize();
        }

        public PoleSprite Manhole { get; private set; }

        public PoleSprite ManholeFloor { get; private set; }

        private void Initialize()
        {
            this.Sprites = new List<ISprite>();
            this.HitBox = new Rectangle(this.HitBox.X, this.HitBox.Y, POLE_WIDTH, 59 + (32 * _addedLengths));
            if (_poleType == PoleType.MANHOLE || _poleType == PoleType.MANHOLE_FLOOR)
            {
                int widthDiff = PoleSprite.MANHOLE_WIDTH - this.HitBox.Width;

                PoleSprite pTop = new PoleSprite(PoleType.TOP, _biomeType, new Point(this.HitBox.X, this.HitBox.Y));
                this.Sprites.Add(pTop);

                PoleSprite pManhole = new PoleSprite(PoleType.MANHOLE, _biomeType, new Point(this.HitBox.X - (widthDiff / 2), pTop.Sprite.Bottom));
                this.Manhole = pManhole;
                PoleSprite pManholeFloor = new PoleSprite(PoleType.MANHOLE_FLOOR, _biomeType,
                    new Point(this.HitBox.X - (widthDiff / 2), pManhole.Sprite.Bottom));
                this.ManholeFloor = pManholeFloor;

                pManholeFloor.CollisionTile = new PoleTile(_collisionGrid, new Rectangle(pManholeFloor.Sprite.Location, pManholeFloor.Sprite.Size), pManholeFloor.Sprite);
                pManholeFloor.Create += new EventHandler<ObjectEventArgs>(pManholeFloor_Create);
                pManholeFloor.Remove += new EventHandler<ObjectEventArgs>(pManholeFloor_Remove);

                OnCreate(new ObjectEventArgs() { ObjectSprite = pManholeFloor.CollisionTile });

                this.Sprites.Add(pManhole);
                this.Sprites.Add(pManholeFloor);
                pManholeFloor.Sprite.BringToFront();
                ManHoleFloorSprite = pManholeFloor;
            }
            else
            {
                PoleSprite pTop = new PoleSprite(PoleType.TOP, _biomeType, new Point(this.HitBox.X, this.HitBox.Y));
                this.Sprites.Add(pTop);
            }
            for (int i = 1; i <= _addedLengths; i++)
            {
                PoleSprite Pmiddle = new PoleSprite(PoleType.MIDDLE, _biomeType, new Point(this.HitBox.X, this.HitBox.Y + (32 * i)));
                this.Sprites.Add(Pmiddle);
            }
            PoleSprite pBottom = new PoleSprite(PoleType.BOTTOM, _biomeType, new Point(this.HitBox.X, this.HitBox.Bottom - 28));
            this.Sprites.Add(pBottom);
            this.UpdateCollisionNodes(Direction.DOWN);
            ResetCollidingNodes();
        }

        void pManholeFloor_Remove(object sender, ObjectEventArgs e)
        {
            OnRemove(e);
        }

        void pManholeFloor_Create(object sender, ObjectEventArgs e)
        {
            OnCreate(e);
        }

        private void ResetCollidingNodes()
        {
            _collidingNodes = _collisionGrid.GetCurrentHashes(this);
            foreach (SpaceHashGridNode node in _collidingNodes)
            {
                node.Objects.Remove(this);
                node.Objects.Add(this);
            }
        }
        protected int _addedLengths;
        private PoleType _poleType;
        private BiomeType _biomeType;

        public List<ISprite> Sprites
        {
            get;
            private set;
        }
        protected override void HandleCollision(CollisionObject obj)
        {

        }

        public PoleSprite ManHoleFloorSprite { get; private set; }

        public BiomeType Biome => _biomeType;

        public Point SpriteLocation => this.HitBox.Location;

        public event EventHandler<ObjectEventArgs> Create;

        public event EventHandler<ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (this.Remove != null)
            {
                this.Remove(this, args);
            }
        }

        public virtual void ChangeBiome(BiomeType newBiome)
        {
            _biomeType = newBiome;
            var sprites = this.Sprites.OfType<IBiomeTile>();
            foreach (var sprite in sprites)
            {
                sprite.ChangeBiome(_biomeType);
            }
        }
    }

    public class PoleSprite : ISprite, ICreateRemove, IBiomeTile
    {
        public const int MANHOLE_WIDTH = 96;
        internal PoleSprite(PoleType type, BiomeType biomeType, Point p)
        {
            _biome = biomeType;
            _poleType = type;
            Initialize(type, biomeType, p);
        }

        public PoleTile CollisionTile { get; set; }

        private void Initialize(PoleType poleType, BiomeType biomeType, Point p)
        {
            _sprite = new PictureBox();

            SetSprite(poleType, biomeType);
            _sprite.Location = p;
            _sprite.SizeMode = PictureBoxSizeMode.AutoSize;
        }

        private void SetSprite(PoleType poleType, BiomeType biomeType)
        {
            switch (poleType)
            {
                case PoleType.BOTTOM:
                    SetBottomByBiome(biomeType);
                    break;
                case PoleType.MIDDLE:
                    SetMiddleByBiome(biomeType);
                    break;
                case PoleType.TOP:
                    SetTopByBiome(biomeType);
                    break;
                case PoleType.MANHOLE:
                    SetManholeByBiome(biomeType);
                    break;
                case PoleType.MANHOLE_FLOOR:
                    SetManholeFloorByBiome(biomeType);
                    break;
            }
        }

        private void SetManholeFloorByBiome(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.KEEN4_GREEN:
                    _sprite.Image = Properties.Resources.keen4_forest_pole_tile;
                    break;
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.keen4_pyramid_pole_manhole_floor;
                    break;
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_pole_manhole_floor;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_pole_manhole_floor;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_pole_manhole_floor;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_pole_manhole_floor;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_green_pole_manhole_floor;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_pole_manhole_floor;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_pole_manhole_floor;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_pole_manhole_floor;
                    break;
            }

        }

        private void SetManholeByBiome(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.KEEN4_GREEN:
                    _sprite.Image = Properties.Resources.keen4_forest_pole_manhole;
                    break;
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.keen4_pyramid_pole_manhole;
                    break;
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_pole_manhole;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_pole_manhole;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_pole_manhole;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_pole_manhole;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_green_pole_manhole;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_pole_manhole;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_pole_manhole;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_pole_manhole;
                    break;
            }
        }

        private void SetTopByBiome(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.KEEN4_GREEN:
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.pole_top;
                    break;
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_pole_top;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_pole_top;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_pole_top;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_pole_top;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_green_pole_top;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_pole_top;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_pole_top;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_pole_top;
                    break;
                case BiomeType.KEEN6_FINAL:
                    _sprite.Image = Properties.Resources.keen6_eyeball_pole;
                    break;
            }
        }

        private void SetMiddleByBiome(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.KEEN4_GREEN:
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.pole_middle;
                    break;
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_pole_middle;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_pole_middle;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_pole_middle;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_pole_middle;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_green_pole_middle;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_pole_middle;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_pole_middle;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_pole_middle;
                    break;
                case BiomeType.KEEN6_FINAL:
                    _sprite.Image = Properties.Resources.keen6_eyeball_pole;
                    break;
            }
        }

        private void SetBottomByBiome(BiomeType biomeType)
        {
            switch (biomeType)
            {
                case BiomeType.KEEN4_GREEN:
                case BiomeType.KEEN4_PYRAMID:
                    _sprite.Image = Properties.Resources.pole_bottom;
                    break;
                case BiomeType.KEEN4_CAVE:
                    _sprite.Image = Properties.Resources.keen4_cave_pole_bottom;
                    break;
                case BiomeType.KEEN4_MIRAGE:
                    _sprite.Image = Properties.Resources.keen4_mirage_pole_bottom;
                    break;
                case BiomeType.KEEN5_BLACK:
                    _sprite.Image = Properties.Resources.keen5_black_pole_bottom;
                    break;
                case BiomeType.KEEN5_RED:
                    _sprite.Image = Properties.Resources.keen5_red_pole_bottom;
                    break;
                case BiomeType.KEEN5_GREEN:
                    _sprite.Image = Properties.Resources.keen5_green_pole_bottom;
                    break;
                case BiomeType.KEEN6_FOREST:
                    _sprite.Image = Properties.Resources.keen6_forest_pole_bottom;
                    break;
                case BiomeType.KEEN6_INDUSTRIAL:
                    _sprite.Image = Properties.Resources.keen6_industrial_pole_bottom;
                    break;
                case BiomeType.KEEN6_DOME:
                    _sprite.Image = Properties.Resources.keen6_dome_pole_bottom;
                    break;
                case BiomeType.KEEN6_FINAL:
                    _sprite.Image = Properties.Resources.keen6_eyeball_pole;
                    break;
            }
        }



        private PictureBox _sprite;
        private BiomeType _biome;
        private PoleType _poleType;

        public PictureBox Sprite
        {
            get { return _sprite; }
        }

        public BiomeType Biome => _biome;

        public Point SpriteLocation => _sprite != null ? _sprite.Location : default(Point);

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Create;

        public event EventHandler<KeenEventArgs.ObjectEventArgs> Remove;

        protected void OnCreate(ObjectEventArgs args)
        {
            if (Create != null)
            {
                Create(this, args);
            }
        }

        protected void OnRemove(ObjectEventArgs args)
        {
            if (this.Remove != null)
            {
                this.Remove(this, args);
            }
        }

        public void ChangeBiome(BiomeType newBiome)
        {
            _biome = newBiome;
            SetSprite(_poleType, _biome);
        }
    }
}
