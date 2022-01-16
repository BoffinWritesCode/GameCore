using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// Drop that has the specified chance to drop.
    /// </summary>
    public class NormalDrop<T> : IDropper<T>
    {
        public float Chance { get; private set; }

        private T _val;
        private int _min;
        private int _max;

        public NormalDrop(T val, int min, int max)
        {
            _val = val;
            _min = min;
            _max = max;
            Chance = 1f;
        }

        public NormalDrop(T val, int min, int max, float chance)
        {
            _val = val;
            _min = min;
            _max = max;
            Chance = chance;
        }

        private List<DropResult<T>> _resultsList;
        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            _resultsList = _resultsList ?? new List<DropResult<T>>();
            _resultsList.Clear();

            float modifiedChance = Chance;
            if (modifiers != null)
            {
                foreach(DynamicModifier<T> modifier in modifiers)
                {
                    if (modifier.CompletesRule(_val)) modifiedChance *= modifier.Modifier;
                }
            }

            if (random.NextFloat(1f) <= modifiedChance)
            {
                _resultsList.Add(new DropResult<T>(_val, random.Next(_min, _max + 1)));
            }

            return _resultsList;
        }

        private List<DropChance<T>> _dropChances = new List<DropChance<T>>();
        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            float modifiedChance = Chance;
            if (modifiers != null)
            {
                foreach (DynamicModifier<T> modifier in modifiers)
                {
                    if (modifier.CompletesRule(_val)) modifiedChance *= modifier.Modifier;
                }
            }

            _dropChances.Clear();
            _dropChances.Add(new DropChance<T>(_val, modifiedChance));

            return _dropChances;
        }
    }
}
