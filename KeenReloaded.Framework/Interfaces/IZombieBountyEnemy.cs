using KeenReloaded.Framework.Enums;
using KeenReloaded.Framework.KeenEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IZombieBountyEnemy
    {
        PointItemType PointItem { get; }

        event EventHandler<ObjectEventArgs> Killed;
    }
}
