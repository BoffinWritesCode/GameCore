using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Utility.DropTables.Droppers
{
    public interface IWeightModifier<T>
    {
        void ModifyWeight(T value, ref float weight);
    }

    public class FuncWeightModifier<T> : IWeightModifier<T>
    {
        public Func<T, float> Method { get; set; }

        public FuncWeightModifier(Func<T, float> method)
        {
            Method = method;
        }

        public void ModifyWeight(T value, ref float weight)
        {
            weight *= Method(value);
        }
    }
}
