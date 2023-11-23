using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IAcquirable
    {
        event EventHandler Acquired;
        bool IsAcquired { get; set; }
    }
}
