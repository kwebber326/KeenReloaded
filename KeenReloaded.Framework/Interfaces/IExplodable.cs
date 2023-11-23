using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IExplodable
    {
        void Explode();

        ExplosionState ExplosionState { get; /*set;*/ }
    }
}
