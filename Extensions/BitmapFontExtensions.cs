using System;

using MonoGame.Extended.BitmapFonts;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore
{
    public static class BitmapFontExtensions
    {
        public static float GetScaleFromLineHeight(this BitmapFont font, float targetLineHeight)
        {
            return targetLineHeight / font.LineHeight;
        }
    }
}