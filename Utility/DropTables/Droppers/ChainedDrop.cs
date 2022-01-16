using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// Drop that drops, or if it fails, moves to the next drop
    /// </summary>
    public class ChainedDrop<T> : IDropper<T>
    {
        private IDropper<T> _dropper;
        private ChainedDrop<T> _next;

        public ChainedDrop(T val, int min, int max, float chance)
        {
            _dropper = new NormalDrop<T>(val, min, max, chance);
        }

        public ChainedDrop(IDropper<T> dropper)
        {
            _dropper = dropper;
        }

        public ChainedDrop<T> AddNext(T val, int min, int max, float chance)
        {
            _next = new ChainedDrop<T>(val, min, max, chance);
            return _next;
        }

        public ChainedDrop<T> AddNext(IDropper<T> next)
        {
            if (_next == null)
            {
                _next = new ChainedDrop<T>(next);
                return this;
            }
            _next.AddNext(next);
            return this;
        }

        private List<DropResult<T>> _results;
        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            _results = _dropper.GetDropResults(random, modifiers).ToList();
            if (_results.Count != 0 || _next == null) return _results;
            return _next.GetDropResults(random, modifiers);
        }

        private List<DropChance<T>> _newChances;
        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            IEnumerable<DropChance<T>> myChances = _dropper.GetDropChances(modifiers);
            if (_next == null)
            {
                return myChances;
            }
            float myTotal = 0f;
            
            _newChances = _newChances ?? new List<DropChance<T>>();
            _newChances.Clear();

            foreach (DropChance<T> chance in myChances)
            {
                myTotal += chance.ChanceToDrop;
                _newChances.Add(chance);
            }
            myTotal = 1f - myTotal;
            IEnumerable<DropChance<T>> chances = _next.GetDropChances(modifiers);
            foreach(DropChance<T> dropChance in chances)
            {
                dropChance.ChanceToDrop *= myTotal;
                _newChances.Add(dropChance);
            }
            return _newChances;
        }
    }
}
