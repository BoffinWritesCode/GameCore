using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    public interface IDropper<T>
    {
        IEnumerable<DropResult<T>> GetDropResults(Random random, IEnumerable<DynamicModifier<T>> modifiers = null);
        IEnumerable<DropChance<T>> GetDropChances(IEnumerable<DynamicModifier<T>> modifiers = null);
    }
}
