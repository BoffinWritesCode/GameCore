using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Graphics
{
    public class Sprite : ISprite
    {
        public Texture2D Texture { get; protected set; }
        public Rectangle? Source { get; set; }

        public Sprite(Texture2D image, Rectangle? source = null)
        {
            Texture = image;
            Source = source;
        }
        
        public TextureInfo GetTextureInfo()
        {
            return new TextureInfo(Texture, Source);
        }
    }
}