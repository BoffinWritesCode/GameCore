using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public interface IClassModifier<T> where T : class
    {
        void Modify(T obj);
    }

    public interface IValueModifier<T> where T : struct
    {
        void Modify(ref T obj);
    }
}
