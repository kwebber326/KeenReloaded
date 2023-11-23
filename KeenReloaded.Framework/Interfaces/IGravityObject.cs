using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IGravityObject
    {
        void Jump();

        bool CanJump { get; }

        void Fall();
    }
}
