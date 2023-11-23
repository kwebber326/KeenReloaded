using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IAnimation
    {
        void GetNextAnimationImage();

        void Reset();

        void StartAnimation();

        void StopAnimation();

        int AnimationDelay { get; }
    }
}
