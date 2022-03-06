using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public static class Debugging
    {
        public static void WriteLine(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }
    }
}
