using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    interface IFireable
    {
        void Fire();

        bool IsFiring { get; }

        int Ammo { get; }
    }
}
