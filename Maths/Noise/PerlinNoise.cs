using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace GameCore.Maths.Noise
{
    //based on https://github.com/keijiro/PerlinNoise
    public class PerlinNoise : INoise
    {
        private const int PSIZE = 512;
        private const int PMASK = 511;

        private int[] _permutation;
        private Random _random;

        public int Seed { get; }

        public PerlinNoise() : this((int)DateTime.Now.Ticks) { }
        public PerlinNoise(int seed)
        {
            _random = new Random(seed);
            Seed = seed;

            //shuffle to create permutation
            _permutation = new int[PSIZE + 1];
            for (int i = 0; i < PSIZE + 1; i++) _permutation[i] = i;
            _random.Shuffle(ref _permutation);
        }

        public float Noise1D(float x)
        {
            int X = (int)Math.Floor(x) & PMASK;
            x -= (float)Math.Floor(x);
            var u = Fade(x);
            return 0.5f + MathHelper.Lerp(u, Grad(_permutation[X], x), Grad(_permutation[X + 1], x - 1));
        }

        /// <returns>A value between 0 and 1</returns>
        public float Noise2D(float x, float y)
        {
            //get the indexes in the permutation based on the x and y values (& 255 ensures they are between 0 and 255)
            int permX = (int)Math.Floor(x) & PMASK;
            int permY = (int)Math.Floor(y) & PMASK;

            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);

            float u = Fade(x);
            float v = Fade(y);

            int permA = (_permutation[permX] + permY) & PMASK;
            int permB = (_permutation[permX + 1] + permY) & PMASK;

            //returns value from 0 to 1
            return (1f + MathHelper.Lerp(
                MathHelper.Lerp(Grad(_permutation[permA], x, y), Grad(_permutation[permB], x - 1, y), u),
                MathHelper.Lerp(Grad(_permutation[permA + 1], x, y - 1), Grad(_permutation[permB + 1], x - 1, y - 1), u),
                v)) * 0.5f;
        }

        public float Noise2DOctaves(float x, float y, int octaves, float lacunarity = 1.75f)
        {
            //initial values
            float value = 0f;
            float gain = 1f / lacunarity;

            for (int i = 0; i < octaves; i++)
            {
                value += Noise2D(x, y) * gain;
                //multiply the values to move them and get different data
                x *= lacunarity;
                y *= lacunarity;
                //each extra octave only effects the final value by a half.
                gain *= 0.5f;
            }

            return value;
        }

        public float Noise2DOctavesWithNoiseOffset(float x, float y, int octaves, float offsetMultiplier = 1f, float lacunarity = 1.75f)
        {
            //initial values
            float value = 0f;
            float gain = 1f / lacunarity;

            for (int i = 0; i < octaves; i++)
            {
                value += Noise2D(x, y) * gain;
                //multiply the values to move them and get different data

                x += (Noise2D(-x, -y) - 0.5f) * offsetMultiplier;
                y += (Noise2D(-y, -x) - 0.5f) * offsetMultiplier;

                x *= lacunarity;
                y *= lacunarity;
                //each extra octave only effects the final value by a half.
                gain *= 0.5f;
            }

            return value;
        }

        public float Noise3D(float x, float y, float z)
        {
            var X = (int)Math.Floor(x) & PMASK;
            var Y = (int)Math.Floor(y) & PMASK;
            var Z = (int)Math.Floor(z) & PMASK;
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            z -= (float)Math.Floor(z);
            var u = Fade(x);
            var v = Fade(y);
            var w = Fade(z);
            var A = (_permutation[X] + Y) & PMASK;
            var B = (_permutation[X + 1] + Y) & PMASK;
            var AA = (_permutation[A] + Z) & PMASK;
            var BA = (_permutation[B] + Z) & PMASK;
            var AB = (_permutation[A + 1] + Z) & PMASK;
            var BB = (_permutation[B + 1] + Z) & PMASK;
            return MathHelper.Lerp(MathHelper.Lerp(MathHelper.Lerp(Grad(_permutation[AA], x, y, z), Grad(_permutation[BA], x - 1, y, z), u),
                               MathHelper.Lerp(Grad(_permutation[AB], x, y - 1, z), Grad(_permutation[BB], x - 1, y - 1, z), u), v),
                       MathHelper.Lerp(MathHelper.Lerp(Grad(_permutation[AA + 1], x, y, z - 1), Grad(_permutation[BA + 1], x - 1, y, z - 1), u),
                               MathHelper.Lerp(Grad(_permutation[AB + 1], x, y - 1, z - 1), Grad(_permutation[BB + 1], x - 1, y - 1, z - 1), u), v), w);
        }

        public float Noise3DOctaves(float x, float y, float z, int octaves, float lacunarity = 1.75f)
        {
            //initial values
            float value = 0f;
            float gain = 1f / lacunarity;

            for (int i = 0; i < octaves; i++)
            {
                value += Noise3D(x, y, z) * gain;
                //multiply the values to move them and get different data
                x *= lacunarity;
                y *= lacunarity;
                //each extra octave only effects the final value by a half.
                gain *= 0.5f;
            }

            return value;
        }

        private static float Fade(float t)
        {
            //fade function pushes values closer to integers
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Grad(int hash, float x)
        {
            return (hash & 1) == 0 ? x : -x;
        }

        private static float Grad(int hash, float x, float y)
        {
            return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
        }

        private static float Grad(int hash, float x, float y, float z)
        {
            var h = hash & 15;
            var u = h < 8 ? x : y;
            var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}
