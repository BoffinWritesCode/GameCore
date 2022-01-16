using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Graphics;
using System.Collections.Generic;

namespace GameCore
{
    public static class RandomExtensions
    {
        public static float NextFloat(this Random random, float min, float max)
        {
            return (float)random.NextDouble() * (max - min) + min;
        }

        public static float NextFloat(this Random random,float max)
        {
            return (float)random.NextDouble() * max;
        }

        public static float NextRotation(this Random random) => random.NextFloat(-MathHelper.Pi, MathHelper.Pi);
        public static Vector2 NextDirection(this Random random) => random.NextFloat(-MathHelper.Pi, MathHelper.Pi).ToDirection();

        public static void Shuffle<T>(this Random random, ref T[] input)
        {
            for (int i = input.Length - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);

                T value = input[index];
                input[index] = input[i];
                input[i] = value;
            }
        }

        public static void Shuffle<T>(this Random random, ref IList<T> input)
        {
            for (int i = input.Count - 1; i > 0; i--)
            {
                int index = random.Next(i + 1);

                T value = input[index];
                input[index] = input[i];
                input[i] = value;
            }
        }
    }
}