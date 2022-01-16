using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF position, Rectangle? source, Color color)
        {
            spriteBatch.Draw(texture, position, source, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF position, Rectangle? source, Color color, float rotation, Vector2 origin)
        {
            spriteBatch.Draw(texture, position, source, color, rotation, origin, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, Texture2D texture, RectangleF position, Rectangle? source, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            float width = source.HasValue ? source.Value.Width : texture.Width;
            float height = source.HasValue ? source.Value.Height : texture.Height;
            Vector2 rectSize = position.Size;
            Vector2 scale = new Vector2(rectSize.X / width, rectSize.Y / height);

            spriteBatch.Draw(texture, position.TopLeft, source, color, rotation, origin, scale, effects, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Vector2 position, Color color)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Vector2 position, Color color, float rotation, Vector2 origin, float scale)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, scale, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, scale, effects, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Rectangle position, Color color)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Rectangle position, Color color, float rotation, Vector2 origin)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, Rectangle position, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, effects, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, RectangleF position, Color color)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, 0f, Vector2.Zero,  SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, RectangleF position, Color color, float rotation, Vector2 origin)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, SpriteEffects.None, 0f);
        }

        public static void Draw(this SpriteBatch spriteBatch, TextureInfo textureInfo, RectangleF position, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
        {
            spriteBatch.Draw(textureInfo.Texture, position, textureInfo.Source, color, rotation, origin, effects, layerDepth);
        }

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, float scale) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin, scale);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin, scale);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin, scale, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Rectangle position, Color color) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Rectangle position, Color color, float rotation, Vector2 origin) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, Rectangle position, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin, effects, layerDepth);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, RectangleF position, Color color) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, RectangleF position, Color color, float rotation, Vector2 origin) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin);

        public static void Draw(this SpriteBatch spriteBatch, Sprite sprite, RectangleF position, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth) => spriteBatch.Draw(sprite.GetTextureInfo(), position, color, rotation, origin, effects, layerDepth);
    }
}