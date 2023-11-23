using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework
{
    public class Hazard : CollisionObject, ISprite
    {
        private HazardType _type;
        protected System.Windows.Forms.PictureBox _sprite;
        public Hazard(SpaceHashGrid grid, Rectangle hitbox, HazardType hazardType)
            : base(grid, hitbox)
        {
            _type = hazardType;
            SetSpriteFromType(_type);  
        }

        protected virtual void SetSpriteFromType(HazardType type)
        {
            _sprite = new System.Windows.Forms.PictureBox();
            _sprite.Location = this.HitBox.Location;
            _sprite.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            switch (type)
            {
                case HazardType.KEEN4_SPIKE:
                    _sprite.Image = Properties.Resources.keen_4_spikes;
                    break;
                case  HazardType.KEEN4_MINE:
                    _sprite.Image = Properties.Resources.keen4_mine;
                    break;
                case HazardType.KEEN4_LIGHTNING_BOLT:
                    _sprite.Image = Properties.Resources.keen4_lightning_bolt1;
                    break;
                case HazardType.KEEN4_ROCKET_PROPELLED_PLATFORM:
                    _sprite.Image = Properties.Resources.keen4_rocket_propelled_platform1;
                    break;
                case HazardType.KEEN4_FIRE:
                    _sprite.Image = Properties.Resources.keen4_fire_left1;
                    break;
                case HazardType.KEEN4_SLUG_POOP:
                    _sprite.Image = Properties.Resources.keen4_slug_poop_active;
                    break;
                case HazardType.KEEN5_SPINNING_FIRE:
                    _sprite.Image = Properties.Resources.keen5_spinning_fire_hazard1;
                    break;
                case HazardType.KEEN5_SPINNING_BURN_PLATFORM:
                    _sprite.Image = Properties.Resources.keen5_spinning_burn_platform7;
                    break;
                case HazardType.KEEN6_BURN_HAZARD:
                    _sprite.Image = Properties.Resources.keen6_burn_hazard1;
                    break;
                case HazardType.KEEN6_SPIKE:
                    _sprite.Image = Properties.Resources.keen6_dome_spikes;
                    break;
                case HazardType.KEEN6_DRILL:
                    _sprite.Image = Properties.Resources.keen6_drill1;
                    break;
                case HazardType.KEEN6_ELECTRIC_RODS:
                    _sprite.Image = Properties.Resources.keen6_electric_rods1;
                    break;
            }
        }

        public virtual bool IsDeadly
        {
            get
            {
                return true;
            }
        }

        public void Kill(DestructibleObject obj)
        {
            obj.Die();
        }

        protected override void HandleCollision(CollisionObject obj)
        {
            DestructibleObject destructoObj = obj as DestructibleObject;
            if (destructoObj != null)
            {
                this.Kill(destructoObj);
            }
        }

        public System.Windows.Forms.PictureBox Sprite
        {
            get { return _sprite; }
        }

        public override string ToString()
        {
            return $"{base.ToString()}|{this.HitBox.Width}|{this.HitBox.Height}|{this._type.ToString()}";

        }
    }
}
