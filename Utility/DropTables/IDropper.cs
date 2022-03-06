using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables
{
    /// <summary>
    /// Dropper interface.
    /// </summary>
    /// <typeparam name="T">The type to "drop"</typeparam>
    public interface IDropper<T>
    {
        void Drop(Random random, Dictionary<T, int> resultDictionary);
        void Chances(Dictionary<T, float> chanceDictionary, float cumulativeMultiplier);
    }
}
