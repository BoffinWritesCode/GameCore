using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables.Droppers
{
    /// <summary>
    /// Drop that must meet a condition for it to drop
    /// </summary>
    public class ConditionalDrop<T> : IDropper<T>
    {
        public IDropCondition Condition { get; set; }

        private IDropper<T> _drop;

        public ConditionalDrop(T val, int min, int max, IDropCondition condition = null)
        {
            _drop = new NormalDrop<T>(val, min, max);
            Condition = condition;
        }

        public ConditionalDrop(T val, int min, int max, float chance, IDropCondition condition = null)
        {
            _drop = new NormalDrop<T>(val, min, max, chance);
            Condition = condition;
        }

        public ConditionalDrop(IDropper<T> dropper, IDropCondition condition = null)
        {
            _drop = dropper;
            Condition = condition;
        }

        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            if (Condition == null || Condition.CanDrop())
            {
                return _drop.GetDropResults(random, modifiers);
            }

            return new List<DropResult<T>>();
        }

        private List<DropChance<T>> _emptyList;
        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            if (Condition != null && !Condition.CanDrop()) return _emptyList ?? (_emptyList = new List<DropChance<T>>());

            return _drop.GetDropChances(modifiers);
        }
    }
}
