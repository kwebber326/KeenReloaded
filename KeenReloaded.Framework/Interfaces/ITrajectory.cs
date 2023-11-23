using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface ITrajectory : IMoveable
    {
        int Damage { get; }

        int Velocity { get; }

        int Pierce { get; }

        int Spread { get; }

        int BlastRadius { get; }

        int RefireDelay { get; }

        bool KillsKeen { get; }
    }
}
