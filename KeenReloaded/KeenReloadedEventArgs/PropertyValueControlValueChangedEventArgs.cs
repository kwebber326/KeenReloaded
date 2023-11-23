using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.KeenReloadedEventArgs
{
    public class PropertyValueControlValueChangedEventArgs : EventArgs
    {
        public object NewValue { get; set; }
    }
}
