using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Hazards
{
    public class Keen5PlatformBurner : Hazard
    {
        public Keen5PlatformBurner(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox, Enums.HazardType.KEEN5_BURNER)
        {

        }

        public int SpinSequence
        {
            get;
            set;
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
                if (_collidingNodes != null)
                {
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_LEFT);
                    this.UpdateCollisionNodes(Enums.Direction.UP_RIGHT);
                }
            }
        }

        public void UpdateLocation(Rectangle newHitbox)
        {
            this.HitBox = newHitbox;
        }

        public override bool IsDeadly
        {
            get
            {
                return SpinSequence != 6 && SpinSequence != 7;
            }
        }
    }
}
