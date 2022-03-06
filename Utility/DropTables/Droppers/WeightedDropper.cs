using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables.Droppers
{
    public class WeightedDropper<T> : IDropper<T>
    {
        private float _totalWeight;
        private List<WeightedDrop> _droppers;

        public List<IWeightModifier<T>> WeightModifiers { get; private set; }

        public WeightedDropper()
        {
            _droppers = new List<WeightedDrop>();
            WeightModifiers = new List<IWeightModifier<T>>();
        }

        public WeightedDropper<T> AddDropper(IDropper<T> dropper, float weight)
        {
            _droppers.Add(new WeightedDrop(dropper, weight));
            return this;
        }

        public WeightedDropper<T> AddSimpleDrop(T value, float weight)
        {
            _droppers.Add(new WeightedDrop(new SimpleDropper<T>(value), value, weight));
            return this;
        }

        public WeightedDropper<T> AddSimpleDrop(T value, float chance, float weight)
        {
            _droppers.Add(new WeightedDrop(new SimpleDropper<T>(value, chance), value, weight));
            return this;
        }

        public WeightedDropper<T> AddSimpleDrop(T value, int minInclusive, int maxInclusive, float chance, float weight)
        {
            _droppers.Add(new WeightedDrop(new SimpleDropper<T>(value, minInclusive, maxInclusive, chance), value, weight));
            return this;
        }

        public void Drop(Random random, Dictionary<T, int> resultDictionary)
        {
            CalculateTotal();
            float randomValue = random.NextFloat(_totalWeight);
            float accumulated = 0f;
            for (int i = 0; i < _droppers.Count; i++)
            {
                float weight = _droppers[i].Weight;
                if (_droppers[i].HasValue) ModifyWeight(_droppers[i].Value, ref weight);
                float newAccumulation = accumulated + weight;
                if (newAccumulation > randomValue)
                {
                    _droppers[i].Dropper.Drop(random, resultDictionary);
                    break;
                }
                accumulated = newAccumulation;
            }
        }

        public void Chances(Dictionary<T, float> chanceDictionary, float cumulativeMultiplier)
        {
            CalculateTotal();
            for (int i = 0; i < _droppers.Count; i++)
            {
                float weight = _droppers[i].Weight;
                if (_droppers[i].HasValue) ModifyWeight(_droppers[i].Value, ref weight);
                _droppers[i].Dropper.Chances(chanceDictionary, cumulativeMultiplier * (weight / _totalWeight));
            }
        }
        
        private void CalculateTotal()
        {
            _totalWeight = 0f;
            foreach (var drop in _droppers)
            {
                float weight = drop.Weight;
                if (drop.HasValue) ModifyWeight(drop.Value, ref weight);
                _totalWeight += weight;
            }
        }
        
        protected virtual void ModifyWeight(T value, ref float weight)
        {
            foreach (var mod in WeightModifiers)
            {
                mod.ModifyWeight(value, ref weight);
            }
        }

        private class WeightedDrop
        {
            public IDropper<T> Dropper { get; private set; }
            public bool HasValue { get; private set; }
            public T Value { get; private set; }
            public float Weight { get; private set; }

            public WeightedDrop(IDropper<T> dropper, float weight)
            {
                Dropper = dropper;
                Weight = weight;
            }

            public WeightedDrop(IDropper<T> dropper, T value, float weight)
            {
                HasValue = true;
                Value = value;
                Dropper = dropper;
                Weight = weight;
            }
        }
    }
}
