using KeenReloaded.Framework.Interfaces;
using KeenReloaded.Framework.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.KeenEventArgs
{
    public class FlagCapturedEventArgs : EventArgs
    {
        public Flag Flag { get; set; }

        public EnemyFlag EnemyFlag { get; set; }
    }
}
