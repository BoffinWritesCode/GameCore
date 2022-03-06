using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UIIcon : UIGraphic
    {
        public Vector2 Origin { get; set; } = Vector2.Zero;
        public Vector2 ImageScale { get; set; } = Vector2.One;
        public float Rotation { get; set; }

        public UIIcon(ISprite sprite) : base(sprite) { }
        public UIIcon(ISprite sprite, Color color) : base(sprite, color, Vector4.One) { }
        public UIIcon(ISprite sprite, Color color, Vector4 mult) : base(sprite, color, mult) { }

        public void CenterOrigin()
        {
            if (Sprite == null) return;

            Origin = Sprite.GetTextureInfo().GetSourceNoNull().Size.ToVector2() * 0.5f;
        }

        public override void Draw()
        {
            TextureInfo info = Sprite.GetTextureInfo();
            RectangleF src = new RectangleF(info.GetSourceNoNull());

            // get containing area to position icon
            RectangleF rect = CalculateRect();

            Engine.SpriteBatch.Draw(info.Texture, rect.Center, info.Source, Color.MultipliedBy(ColorMultiplier), Rotation, Origin, ImageScale, SpriteEffects.None, 0f);

            base.Draw();
        }

        public override UIGraphic Clone()
        {
            return new UIIcon(Sprite, Color, ColorMultiplier) { Rotation = this.Rotation, ImageScale = this.ImageScale };
        }
    }
}