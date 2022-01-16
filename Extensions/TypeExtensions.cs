using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public static class TypeExtensions
    {
        public static bool HasDefaultConstructor(this Type t)
        {
            return t.IsValueType || t.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}
