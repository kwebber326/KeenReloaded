using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Interfaces
{
    public interface ISquashable
    {
        void Squash();
        bool IsSquashed { get; }
        bool CanSquash { get; }
        event EventHandler<ObjectEventArgs> Squashed;
    }
}
