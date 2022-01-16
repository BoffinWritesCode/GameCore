using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UIKeepAspectSprite : UIGraphic
    {
        public float Scale { get; set; } = 1f;
        public Vector2 SourceOffset { get; set; }

        public UIKeepAspectSprite(ISprite sprite) : base(sprite) { }
        public UIKeepAspectSprite(ISprite sprite, Color color) : base(sprite, color, Vector4.One) { }

        public override void Draw()
        {
            RectangleF rect = CalculateRect();

            // we want to keep the texture's aspect so we'll expand the source as needed
            TextureInfo info = Sprite.GetTextureInfo();
            Rectangle source = info.GetSourceNoNull();
            float scale = 1f / Scale;
            source.Width = (int)(rect.Width * scale);
            source.Height = (int)(rect.Height * scale);
            source.Offset((int)SourceOffset.X, (int)SourceOffset.Y);
            Engine.SpriteBatch.Draw(info.Texture, rect, source, Color.MultipliedBy(ColorMultiplier));

            base.Draw();
        }

        public override UIGraphic Clone()
        {
            return new UIKeepAspectSprite(Sprite, Color) { Scale = this.Scale, ColorMultiplier = this.ColorMultiplier };
        }
    }
}