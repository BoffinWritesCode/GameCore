using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public static class GraphicsExtras
    {
        static GraphicsExtras()
        {
            Pixel = new Texture2D(Engine.WindowManager.GraphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });

            PixelSprite = new Sprite(Pixel);
        }

        public static Texture2D Pixel;
        public static Sprite PixelSprite;
    }
}