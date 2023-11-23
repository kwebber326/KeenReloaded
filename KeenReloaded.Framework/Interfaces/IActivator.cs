using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IActivator
    {
        List<IActivateable> ToggleObjects { get; }

        bool IsActive { get; }

        void Toggle();

        event EventHandler<ToggleEventArgs> Toggled;
    }
}
