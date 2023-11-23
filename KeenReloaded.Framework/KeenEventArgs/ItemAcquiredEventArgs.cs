using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.KeenEventArgs
{
    public class ItemAcquiredEventArgs : EventArgs
    {
        public Item Item { get; set; }
    }
}
