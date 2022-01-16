using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameCore
{
    public static class FloatExtensions
    {
        public static Vector2 ToDirection(this float angle)
        {
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }
    }
}
