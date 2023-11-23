using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Tiles
{
    public class Keen6ClawTile : SingleMaskedPlatformTile
    {
        private const int EDGE_HORIZONTAL_OFFSET_LEFT = 32;
        private const int VERTICAL_OFFSET = 64;
        private const int EDGE_HORIZONTAL_OFFSET_RIGHT = 32;
        private const int ARM_LENGTH = 62;
        private const int ARM_WIDTH = 64;
        private const int ARM_HORIZONTAL_OFFSET = 28;

        private readonly int _addedLengths;

        public Keen6ClawTile(SpaceHashGrid grid, Rectangle hitbox, int addedLengths = 0) 
            : base(grid, hitbox, BiomeType.KEEN6_FINAL)
        {
            _addedLengths = addedLengths;
            this.AddArmLengths();
        }

        public List<PictureBox> ArmLengths
        {
            get; private set;
        }

        public override void ChangeBiome(BiomeType newBiome)
        {
           
        }

        private void AddArmLengths()
        {
            int baseYPos = this.HitBox.Bottom;
            this.ArmLengths = new List<PictureBox>();
            for (int i = 0; i < _addedLengths; i++)
            {
                int offsetY = i * ARM_LENGTH;
                PictureBox p = new PictureBox();
                p.SizeMode = PictureBoxSizeMode.AutoSize;
                p.Image = Properties.Resources.keen6_claw_platform_arm;
                int x = this.Sprite.Location.X + ARM_HORIZONTAL_OFFSET;
                int y = baseYPos + offsetY;
                p.Location = new Point(x, y);
                this.ArmLengths.Add(p);
            }
        }

        protected override void SetSprite()
        {
            this.Sprite.Image = Properties.Resources.keen6_claw_platform;
        }

        protected override void Initialize()
        {
            SetSprite();

            this.HitBox = new Rectangle(
                this.HitBox.X + EDGE_HORIZONTAL_OFFSET_LEFT //x
              , this.HitBox.Y + VERTICAL_OFFSET   //y
              , this.HitBox.Width - EDGE_HORIZONTAL_OFFSET_LEFT - EDGE_HORIZONTAL_OFFSET_RIGHT //width
              , this.HitBox.Height - VERTICAL_OFFSET); //height

            this.Sprite.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Sprite.BackColor = Color.Transparent;
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}|{this.SpriteLocation.X}|{this.SpriteLocation.Y}|{this.ArmLengths?.Count}";
        }
    }
}
