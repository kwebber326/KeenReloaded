﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KeenReloaded.Framework.Enums;

namespace KeenReloaded.Framework.Interfaces
{
    public interface IMoveable
    {
        void Move();

        void Stop();

        MoveState MoveState { get; set; }

        Direction Direction { get; set; }

    }
}
