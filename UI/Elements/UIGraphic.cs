using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public abstract class UIGraphic : UIElement, IColorable
    {
        public ISprite Sprite { get; set; }
        public Color Color { get; set; }
        public Vector4 ColorMultiplier { get; set; }

        public UIGraphic(ISprite sprite) : this(sprite, Color.White, Vector4.One) { }
        public UIGraphic(ISprite sprite, Color color, Vector4 mult) : base()
        {
            Sprite = sprite;
            Color = color;
            ColorMultiplier = mult;
        }
        public UIGraphic(UIGraphic other) : this(other.Sprite, other.Color, other.ColorMultiplier) { }

        public virtual UIGraphic Clone() { throw new NotImplementedException(); }
    }
}