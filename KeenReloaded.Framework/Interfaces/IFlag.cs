using KeenReloaded.Framework.KeenEventArgs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IFlag
    {
        int PointsDegradedPerSecond { get; }

        bool IsCaptured { get; }

        Point LocationOfOrigin { get; }

        event EventHandler<FlagCapturedEventArgs> FlagCaptured;
        event EventHandler<FlagCapturedEventArgs> FlagPointsChanged;

        void Capture();
    }
}
