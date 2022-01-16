using System;
using System.Collections.Generic;
using System.Text;

using GameCore.Utility.DropTables.Droppers;

namespace GameCore.Utility.DropTables
{
    /// <summary>
    /// A drop table. Based on how it is configured, calling GetDropResults will return a list of drops or values based on <T>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DropTable<T> : IDropper<T>
    {
        private List<IDropper<T>> _droppers;

        public DropTable()
        {
            _droppers = new List<IDropper<T>>();
        }

        public DropTable<T> AddDrop(T drop, int min, int max, IDropCondition condition = null)
        {
            IDropper<T> dropper = new NormalDrop<T>(drop, min, max);
            return AddDropper(dropper, condition);
        }

        public DropTable<T> AddDrop(T drop, int min, int max, float chance, IDropCondition condition = null)
        {
            IDropper<T> dropper = new NormalDrop<T>(drop, min, max, chance);
            return AddDropper(dropper, condition);
        }

        public DropTable<T> AddDropper(IDropper<T> dropper, IDropCondition condition = null)
        {
            if (condition != null)
            {
                ConditionalDrop<T> drop = new ConditionalDrop<T>(dropper, condition);
                _droppers.Add(drop);
                return this;
            }
            _droppers.Add(dropper);
            return this;
        }

        public IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            List<DropResult<T>> drops = new List<DropResult<T>>();

            //loop through all drop pools, add drops to list
            foreach (IDropper<T> dropper in _droppers)
            {
                var dropResults = dropper.GetDropResults(random, modifiers);
                foreach (DropResult<T> drop in dropResults)
                {
                    drops.Add(drop);
                }
            }

            return drops;
        }

        public IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null)
        {
            List<DropChance<T>> chances = new List<DropChance<T>>();

            //loop through all drop pools, add chances to list
            foreach (IDropper<T> dropper in _droppers)
            {
                IEnumerable<DropChance<T>> dropChances = dropper.GetDropChances(modifiers);
                foreach(DropChance<T> chance in dropChances)
                {
                    chances.Add(chance);
                }
            }

            return chances;
        }
    }
}
