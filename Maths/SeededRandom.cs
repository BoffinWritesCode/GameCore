using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Maths
{
    public class SeededRandom
    {
        public int Seed { get; private set; }
        public Random Random { get; private set; }

        public SeededRandom(int seed)
        {
            Random = new Random(seed);
            Seed = seed;
        }

        public int Next() => Random.Next();
        public int Next(int maxValue) => Random.Next(maxValue);
        public int Next(int minValue, int maxValue) => Random.Next(minValue, maxValue);
        public double NextDouble() => Random.NextDouble();
        public float NextFloat(float max) => Random.NextFloat(max);
        public float NextFloat(float min, float max) => Random.NextFloat(min, max);
        public bool NextBool() => Random.Next(2) == 0;

        public static implicit operator Random(SeededRandom random)
        {
            return random.Random;
        }
    }
}
