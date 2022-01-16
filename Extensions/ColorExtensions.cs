using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore
{
    public static class ColorExtensions
    {
        public static Color MultipliedBy(this Color color, Vector4 multiplier)
        {
            return new Color(color.ToVector4() * multiplier);
        }
    }
}