using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.HelperClasses
{
    public class HighScore
    {
        public string Name { get; set; }

        public long Score { get; set; }

        public TimeSpan Time { get; set; }
    }
}
