using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace KeenReloaded.Framework.Assets
{
    public class Keen6SetPathPlatform : SetPathPlatform
    {
        public Keen6SetPathPlatform(SpaceHashGrid grid, Rectangle hitbox, CommanderKeen keen, List<Point> locations, Guid activationId, bool initiallyActive = false)
            : base(grid, hitbox, Enums.PlatformType.KEEN6, keen, locations, activationId, initiallyActive)
        {
            BipOperator = new PictureBox();
            BipOperator.SizeMode = PictureBoxSizeMode.AutoSize;
            BipOperator.Location = new Point(this.HitBox.X + this.HitBox.Width / 2 - 11, this.HitBox.Bottom);
            UpdateBipOperatorSprite();
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
                if (this.Sprite != null && value != null)
                {
                    this.Sprite.Location = new Point(this.HitBox.X, this.HitBox.Y - (this.Sprite.Height / 2));
                    this.UpdateCollisionNodes(_direction);
                    if (BipOperator != null)
                        BipOperator.Location = new Point(this.HitBox.X + this.HitBox.Width / 2 - 11, this.HitBox.Bottom);
                }
            }
        }

        public PictureBox BipOperator { get; private set; }

        public override void Update()
        {
            base.Update();
            UpdateBipOperatorSprite();
        }

        private void UpdateBipOperatorSprite()
        {
            switch (_direction)
            {
                case Enums.Direction.DOWN:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_down;
                    break;
                case Enums.Direction.DOWN_LEFT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_down_left;
                    break;
                case Enums.Direction.DOWN_RIGHT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_down_right;
                    break;
                case Enums.Direction.LEFT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_left;
                    break;
                case Enums.Direction.RIGHT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_right;
                    break;
                case Enums.Direction.UP:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_up;
                    break;
                case Enums.Direction.UP_LEFT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_up_left;
                    break;
                case Enums.Direction.UP_RIGHT:
                    BipOperator.Image = Properties.Resources.keen6_bip_platform_up_right;
                    break;
            }
        }
    }
}
