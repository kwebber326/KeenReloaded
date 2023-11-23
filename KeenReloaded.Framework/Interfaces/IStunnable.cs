using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    interface IStunnable
    {
        void Stun();

        bool IsStunned { get; }
    }
}
