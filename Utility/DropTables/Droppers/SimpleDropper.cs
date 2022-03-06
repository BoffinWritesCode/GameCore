using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// A simple dropper. Drops a value, with a random amount, at a set chance.
    /// </summary>
    public class SimpleDropper<T> : IDropper<T>
    {
        private T _val;
        private int _min;
        private int _max;
        private float _dropChance;

        public SimpleDropper(T value) : this(value, 1, 1, 1f) { }
        public SimpleDropper(T value, float chance) : this(value, 1, 1, chance) { }
        public SimpleDropper(T value, int minInclusive, int maxInclusive) : this(value, minInclusive, maxInclusive, 1f) { }
        public SimpleDropper(T value, int minInclusive, int maxInclusive, float chance)
        {
            _val = value;
            _min = minInclusive;
            _max = maxInclusive + 1;
            _dropChance = chance;
        }

        public void Drop(Random random, Dictionary<T, int> resultDictionary)
        {
            if (random.NextFloat(1f) <= _dropChance)
            {
                int current = resultDictionary.ContainsKey(_val) ? resultDictionary[_val] : 0;
                resultDictionary[_val] = current + random.Next(_min, _max);
            }
        }

        public void Chances(Dictionary<T, float> chanceDictionary, float cumulativeMultiplier)
        {
            float current = chanceDictionary.ContainsKey(_val) ? chanceDictionary[_val] : 0f;
            chanceDictionary[_val] = current + (1f - current) * _dropChance * cumulativeMultiplier;
        }
    }
}
