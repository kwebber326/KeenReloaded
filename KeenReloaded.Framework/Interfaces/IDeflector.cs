using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IDeflector
    {
        bool DeflectsHorizontally { get; }
        bool DeflectsVertically { get; }
    }
}
