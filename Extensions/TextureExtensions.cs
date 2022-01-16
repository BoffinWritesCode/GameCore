using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore
{
    public static class TextureExtensions
    {
        public static float TexelWidth(this Texture2D texture)
        {
            return 1f / texture.Width;
        }

        public static float TexelHeight(this Texture2D texture)
        {
            return 1f / texture.Height;
        }
    }
}
