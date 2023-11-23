using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IActivateable
    {
        void Activate();
        void Deactivate();
        bool IsActive { get; }

        Guid ActivationID { get; }
    }
}
