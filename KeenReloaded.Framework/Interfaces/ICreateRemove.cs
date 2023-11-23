using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.KeenEventArgs;

namespace KeenReloaded.Framework.Interfaces
{
    interface ICreateRemove
    {
        event EventHandler<ObjectEventArgs> Create;
        event EventHandler<ObjectEventArgs> Remove;
    }
}
