using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// Drops one from the set
    /// </summary>
    public class WeightedDrop<T> : IDropper<T>
    {
        private List<DropperWeighted<T>> _droppers;
        private float _randomValue;
        private float _total;

        public WeightedDrop(float randVal = 1000f)
        {
            _droppers = new List<DropperWeighted<T>>();
            _randomValue = randVal;
        }

        public WeightedDrop<T> AddDrop(T val, int min, int max, float weight = 1f, float chance = 1f, bool recalc = true)
        {
            _droppers.Add(new DropperWeighted<T>(new NormalDrop<T>(val, min, max, chance), weight));
            if (recalc) Recalculate();
            return this;
        }

        public WeightedDrop<T> AddDropper(IDropper<T> dropper, float weight = 1f, bool recalc = true)
        {
            _droppers.Add(new DropperWeighted<T>(dropper, weight));
            if (recalc) Recalculate();
            return this;
        }

        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            float value = random.NextFloat(_randomValue);
            float accumulated = 0f;
            foreach (DropperWeighted<T> weighted in _droppers)
            {
                if (value <= accumulated + weighted.Value)
                {
                    return weighted.Dropper.GetDropResults(random, modifiers);
                }
                accumulated += weighted.Value;
            }

            return new List<DropResult<T>>();
        }

        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            List<DropChance<T>> chances = new List<DropChance<T>>();
            foreach (DropperWeighted<T> dropper in _droppers)
            {
                IEnumerable<DropChance<T>> dropChances = dropper.Dropper.GetDropChances(modifiers);
                foreach(DropChance<T> dropChance in dropChances)
                {
                    chances.Add(new DropChance<T>(dropChance.Drop, dropChance.ChanceToDrop * (dropper.Weight / _total)));
                }
            }
            return chances;
        }

        public void Recalculate()
        {
            foreach (DropperWeighted<T> weighted in _droppers)
            {
                _total += weighted.Weight;
            }

            foreach (DropperWeighted<T> weighted in _droppers)
            {
                weighted.Value = (weighted.Weight / _total) * _randomValue;
            }
        }

        private class DropperWeighted<T>
        {
            public IDropper<T> Dropper { get; private set; }
            public float Weight { get; private set; }
            public float Value { get; set; }
            public DropperWeighted(IDropper<T> dropper, float weight)
            {
                Dropper = dropper;
                Weight = weight;
            }
        }
    }
}
