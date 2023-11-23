using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Interfaces;

namespace KeenReloaded.Framework.KeenEventArgs
{
    public class ObjectEventArgs : EventArgs
    {
        public ISprite ObjectSprite { get; set; }
    }
}
