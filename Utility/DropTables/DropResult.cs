using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    public class DropResult<T>
    {
        public T Value { get; private set; }
        public int Amount { get; private set; }

        public DropResult(T val, int am)
        {
            Value = val;
            Amount = am;
        }
    }
}
