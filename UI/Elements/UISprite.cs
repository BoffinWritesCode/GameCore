using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UISprite : UIGraphic
    {
        public UISprite(ISprite sprite) : base(sprite) { }
        public UISprite(ISprite sprite, Color color) : base(sprite, color, Vector4.One) { }
        public UISprite(ISprite sprite, Color color, Vector4 mult) : base(sprite, color, mult) { }

        public override void Draw()
        {
            RectangleF rect = CalculateRect();
            TextureInfo info = Sprite.GetTextureInfo();

            Engine.SpriteBatch.Draw(info, rect, Color.MultipliedBy(ColorMultiplier));

            base.Draw();
        }

        public override UIGraphic Clone()
        {
            return new UISprite(Sprite, Color, ColorMultiplier);
        }
    }
}