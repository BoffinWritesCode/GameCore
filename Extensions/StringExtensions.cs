using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameCore
{
    public static class StringExtensions
    {
        public static int GetByteLength(this string s)
        {
            int len = 1;

            // get the base encoding length of the string
            if (s.Length >= 128) len++;
            if (s.Length >= 16384) len++;

            return len + s.Length;
        }
    }
}
