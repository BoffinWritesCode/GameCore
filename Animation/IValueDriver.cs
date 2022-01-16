using System;
using System.Collections.Generic;

namespace GameCore.Animation
{
    public interface IValueDriver
    {
        void Drive(int from, int to, float interpolation);
    }
}