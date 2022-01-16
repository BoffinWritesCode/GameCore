using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// Drops one from the set
    /// </summary>
    public class OneFromSetDrop<T> : IDropper<T>
    {
        private List<IDropper<T>> _droppers;

        public OneFromSetDrop()
        {
            _droppers = new List<IDropper<T>>();
        }

        public OneFromSetDrop<T> AddDrop(T val, int min, int max)
        {
            _droppers.Add(new NormalDrop<T>(val, min, max));
            return this;
        }

        public OneFromSetDrop<T> AddDrop(T val, int min, int max, float chance)
        {
            _droppers.Add(new NormalDrop<T>(val, min, max, chance));
            return this;
        }

        public OneFromSetDrop<T> AddDropper(IDropper<T> dropper)
        {
            _droppers.Add(dropper);
            return this;
        }

        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            int index = random.Next(_droppers.Count);
            return _droppers[index].GetDropResults(random, modifiers);
        }

        private List<DropChance<T>> _chances;
        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            _chances = _chances ?? new List<DropChance<T>>();
            _chances.Clear();

            float multiplier = 1f / _droppers.Count;
            foreach(IDropper<T> dropper in _droppers)
            {
                IEnumerable<DropChance<T>> dropChances = dropper.GetDropChances(modifiers);
                foreach(DropChance<T> dropChance in dropChances)
                {
                    _chances.Add(new DropChance<T>(dropChance.Drop, dropChance.ChanceToDrop * multiplier));
                }
            }
            return _chances;
        }
    }
}
