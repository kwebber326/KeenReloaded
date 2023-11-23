using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Tiles;

namespace KeenReloaded.Framework.Assets
{
    public class Keen6EyeBallPole : Pole
    {
        public const int EYE_BALL_HORIZONTAL_OFFSET = 24;
        public Keen6EyeBallPole(SpaceHashGrid grid, Rectangle hitbox, int addedLengths = 0) 
            : base(grid, hitbox, PoleType.MIDDLE, BiomeType.KEEN6_FINAL, addedLengths)
        {
            var lastPoleBottom = this.Sprites.LastOrDefault()?.Sprite?.Bottom ?? this.HitBox.Bottom;
            EyeBallTileSprite = new Keen6EyeBallTile(grid, new Rectangle(this.HitBox.X - EYE_BALL_HORIZONTAL_OFFSET, lastPoleBottom, 62, 60));
        }

        public ISprite EyeBallTileSprite
        {
            get;private set;
        }

        public override void ChangeBiome(BiomeType newBiome)
        {
        }

        public override string ToString()
        {
            return base.ToString() + "|" + _addedLengths;
        }
    }
}
