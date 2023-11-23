using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace KeenReloaded.Framework.Hazards
{
    public class KorathInhabitantSpike : Hazard
    {
        public KorathInhabitantSpike(SpaceHashGrid grid, Rectangle hitbox)
            : base(grid, hitbox, Enums.HazardType.KEEN5_KORATH_INHABITANT_SPIKE)
        {

        }

        public void UpdateHitbox(Rectangle hitbox)
        {
            this.HitBox = hitbox;
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
                    this.UpdateCollisionNodes(Enums.Direction.DOWN_RIGHT);
                }
            }
        }
    }
}
