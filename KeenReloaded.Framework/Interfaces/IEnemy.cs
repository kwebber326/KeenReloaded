using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IEnemy
    {
        bool DeadlyTouch { get; }

        void HandleHit(ITrajectory trajectory);

        bool IsActive { get; }
    }
}
