using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.Assets;

namespace KeenReloaded.Framework.Trajectories
{
    public class ShikadiShock : StraightShotTrajectory
    {
        private Pole _pole;
        public ShikadiShock(SpaceHashGrid grid, Rectangle hitbox, Direction direction, Pole pole)
            : base(grid, hitbox, direction, EnemyTrajectoryType.KEEN5_SHIKADI_SHOCK)
        {
            if (pole == null)
            {
                throw new ArgumentNullException("Shikadi shock must have a pole to ride");
            }
            _pole = pole;
        }

        public override void Update()
        {
            base.Update();
            if (this.HitBox.Top <= _pole.HitBox.Top)
            {
                _shotComplete = true;
            }
        }
    }
}
