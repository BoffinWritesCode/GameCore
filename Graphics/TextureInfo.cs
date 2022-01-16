using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public struct TextureInfo
    {
        public Texture2D Texture { get; private set; }
        public Rectangle? Source { get; private set; }

        public TextureInfo(Texture2D texture, Rectangle? source)
        {
            Texture = texture;
            Source = source;
        }

        public Rectangle GetSourceNoNull()
        {
            return Source ?? new Rectangle(0, 0, Texture.Width, Texture.Height);
        }
    }
}