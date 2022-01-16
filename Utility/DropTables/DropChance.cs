using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    public class DropChance<T>
    {
        public T Drop { get; private set; }
        public float ChanceToDrop { get; set; }

        public DropChance(T drop, float chance)
        {
            Drop = drop;
            ChanceToDrop = Math.Min(1f, chance);
        }
    }
}
