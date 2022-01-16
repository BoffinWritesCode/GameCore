using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    public class DynamicModifier<T>
    {
        public Predicate<T> Rule;
        public float Modifier;

        public bool CompletesRule(T value)
        {
            return Rule(value);
        }
    }
}
