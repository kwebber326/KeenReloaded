using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen6ElectricRodHazard : Hazard, IUpdatable
    {
        private const int LEFT_TILE_WIDTH = 22, LEFT_TILE_HEIGHT = 82;
        private const int RIGHT_TILE_WIDTH = 8, RIGHT_TILE_HEIGHT = 82;
        private const int DEATH_ZONE_WIDTH = 98, DEATH_ZONE_HEIGHT = 82;
        private const int BOTTOM_TILE_WIDTH = 128, BOTTOM_TILE_HEIGHT = 14;

        private const int SPRITE_CHANGE_DELAY = 2;
        private int _currentSpriteChangeDelayTick;
        private int _currentSpriteIndex;

        private DebugTile _leftTile;
        private DebugTile _rightTile;
        private DebugTile _bottomTile;

        private readonly Image[] _sprites = SpriteSheet.Keen6ElectricRodSprites;
        public Keen6ElectricRodHazard(SpaceHashGrid grid, Rectangle hitbox) 
            : base(grid, hitbox, HazardType.KEEN6_ELECTRIC_RODS)
        {
            InitializeCollisionTiles();
        }

        private void InitializeCollisionTiles()
        {
            _leftTile = new DebugTile(_collisionGrid, new Rectangle(this.Sprite.Location.X, this.Sprite.Location.Y, LEFT_TILE_WIDTH, LEFT_TILE_HEIGHT));
            _rightTile = new DebugTile(_collisionGrid, new Rectangle(this.Sprite.Right - RIGHT_TILE_WIDTH, this.Sprite.Location.Y, RIGHT_TILE_WIDTH, RIGHT_TILE_HEIGHT));
            _bottomTile = new DebugTile(_collisionGrid, new Rectangle(this.Sprite.Location.X, this.Sprite.Bottom - BOTTOM_TILE_HEIGHT, BOTTOM_TILE_WIDTH, BOTTOM_TILE_HEIGHT));
            //death zone
            this.HitBox = new Rectangle(this.Sprite.Location.X + LEFT_TILE_WIDTH + 1, this.Sprite.Location.Y, DEATH_ZONE_WIDTH, DEATH_ZONE_HEIGHT);
            this.UpdateCollisionNodes(Direction.DOWN_LEFT);
            this.UpdateCollisionNodes(Direction.UP_RIGHT);
        }

        public void Update()
        {
            this.UpdateSpriteByDelayBase(
                ref _currentSpriteChangeDelayTick
              , ref _currentSpriteIndex
              , SPRITE_CHANGE_DELAY
              , UpdateSprite);
        }

        private void UpdateSprite()
        {
            if (_currentSpriteIndex >= _sprites.Length)
                _currentSpriteIndex = 0;

            this.Sprite.Image = _sprites[_currentSpriteIndex];
        }
    }
}
