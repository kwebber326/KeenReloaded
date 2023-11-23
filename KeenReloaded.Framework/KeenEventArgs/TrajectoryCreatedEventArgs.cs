using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.KeenEventArgs
{
    public class TrajectoryEventArgs : System.EventArgs
    {
        public ITrajectory Trajectory { get; set; }
    }
}
