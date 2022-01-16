using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI
{
    public struct Padding
    {
        public float Top { get; set; }
        public float Bottom { get; set; }
        public float Left { get; set; }
        public float Right { get; set; }

        public Padding(float t, float b, float l, float r)
        {
            Top = t;
            Bottom = b;
            Left = l;
            Right = r;
        }

        public static void ModifyWith(ref RectangleF rect, Padding padding)
        {
            rect.X += padding.Left;
            rect.Y += padding.Top;
            rect.Width -= padding.Left + padding.Right;
            rect.Height -= padding.Top + padding.Bottom;
        }
    }
}