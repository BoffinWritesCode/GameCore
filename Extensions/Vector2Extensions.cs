using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public static class Vector2Extensions
    {
        public static float ToAngle(this Vector2 vector)
        {
            return MathF.Atan2(vector.Y, vector.X);
        }

        public static Vector2 RotateMeAround(this Vector2 point, Vector2 around, float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);

            Vector2 between = point - around;

            return new Vector2(around.X + between.X * cos - between.Y * sin, around.Y + between.X * sin + between.Y * cos);
        }

        public static Vector2 Clockwise90(this Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }

        public static Vector2 CounterClockwise90(this Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }
    }
}
