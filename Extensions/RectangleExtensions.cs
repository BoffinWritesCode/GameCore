using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameCore
{
    public static class RectangleExtensions
    {
        public static Vector2 Size(this Rectangle rectangle) => new Vector2(rectangle.Width, rectangle.Height);
    }
}
