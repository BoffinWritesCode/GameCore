using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.BitmapFonts;

using GameCore.Localisation;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public enum TextAnchor
    {
        TopLeft,
        TopMiddle,
        TopRight,
        MiddleLeft,
        Center,
        MiddleRight,
        BottomLeft,
        BottomMiddle,
        BottomRight
    }

    public class UIText : UIElement, IColorable
    {
        private string _text;
        private LocalisationText _localisationText;
        public virtual LocalisationText LocalisedText 
        {
            get => _localisationText;
            set => _localisationText = value;
        }
        public virtual string Text
        {
            get => LocalisedText == null ? _text : LocalisedText.Value;
            set { _text = value; LocalisedText = null; }
        }

        public Color Color { get; set; }
        public Vector4 ColorMultiplier { get; set; } = Vector4.One;
        public BitmapFont Font { get; set; }
        public float Scale { get; set; }
        public bool AutoScale { get; set; }
        public float MaxAutoScale { get; set; }
        public Padding Padding { get; set; }
        public TextAnchor Anchor { get; set; }
        public Vector2 TextOffset { get; set; }
        public Vector2 RotationOrigin { get; set; } = new Vector2(0.5f);
        public Vector2 DropShadowOffset { get; set; }
        public float DropShadowSmoothing { get; set; }
        public Color DropShadowColor { get; set; }
        public Effect Effect { get; set; }
        public float Rotation { get; set; }

        public UIText(BitmapFont font, string text) : this(font, text, Color.White) { }
        public UIText(BitmapFont font, string text, Color color) : this(font, text, color, null) { }
        public UIText(BitmapFont font, string text, Color color, Effect effect) : base()
        {
            Scale = 1f;
            Font = font;
            Text = text;
            Color = color;
            Anchor = TextAnchor.Center;
            Effect = effect;

            DropShadowOffset = new Vector2(0f, 0.001f);
            DropShadowSmoothing = 0.2f;
            DropShadowColor = Color.Transparent;
        }

        public UIText(BitmapFont font, LocalisationText text) : this(font, text, Color.White) { }
        public UIText(BitmapFont font, LocalisationText text, Color color) : this(font, text, color, null) { }
        public UIText(BitmapFont font, LocalisationText text, Color color, Effect effect) : base()
        {
            Scale = 1f;
            Font = font;
            LocalisedText = text;
            Color = color;
            Anchor = TextAnchor.Center;
            Effect = effect;

            DropShadowOffset = new Vector2(0f, 0.001f);
            DropShadowSmoothing = 0.2f;
            DropShadowColor = Color.Transparent;
        }

        public override RectangleF CalculateRect()
        {
            if (_dirtyRect)
            {
                var newRect = base.CalculateRect();
                if (AutoScale)
                {
                    FitTextToRect();
                }
                return newRect;
            }
            return base.CalculateRect();
        }

        protected void FitTextToRect()
        {
            RectangleF myRect = CalculateRect();
            RectangleF measureText = new RectangleF(Vector2.Zero, Font.MeasureString(Text));

            float newScale =  MathExtras.ScaleToFitIntoFactor(measureText, myRect);

            // enforce scale
            if (MaxAutoScale <= 0f) MaxAutoScale = newScale;
            else newScale = MathF.Min(newScale, MaxAutoScale);
            
            Scale = newScale;
        }

        public override void Draw()
        {
            if (Effect != null)
            {
                Engine.SpriteBatch.End();
            
                var raster = ScissorTesting.SetScissorAndGetRasterizer();

                // TODO: Allow this to be customised. The effect could be anything, not specifically the SDF shader, so ensure that's okay.
                Effect.Parameters["shadowOffset"].SetValue(DropShadowOffset);
                Effect.Parameters["shadowSmoothing"].SetValue(DropShadowSmoothing);
                Effect.Parameters["shadowColor"].SetValue(DropShadowColor.ToVector4() * ColorMultiplier);

                Engine.SpriteBatch.Begin(rasterizerState: raster, samplerState: Settings.SamplerState, effect: Effect);

                DrawMyText();

                Engine.SpriteBatch.End();

                Engine.SpriteBatch.Begin(rasterizerState: ScissorTesting.SetScissorAndGetRasterizer(), samplerState: Settings.SamplerState);
            }
            else
            {
                DrawMyText();
            }

            base.Draw();
        }

        protected virtual void DrawMyText()
        {
            // get padding modified area
            Vector2 size = Font.MeasureString(Text);
            Vector2 pos = GetDrawPosition();

            Engine.SpriteBatch.DrawString(Font, Text, pos, Color.MultipliedBy(ColorMultiplier), Rotation, size * RotationOrigin, Scale, SpriteEffects.None, 0f);
        }

        protected Vector2 GetDrawPosition()
        {
            // get padding modified area
            RectangleF area = CalculateRect();
            Padding.ModifyWith(ref area, Padding);
            Vector2 size = Font.MeasureString(Text);
            Vector2 origin = GetOriginFromAnchor();
            return (area.TopLeft + area.Size * origin) + TextOffset + size * (RotationOrigin - origin) * Scale;
        }

        protected Vector2 GetOriginFromAnchor()
        {
            //return Vector2.Zero;

            switch (Anchor)
            {
                default:
                case TextAnchor.TopLeft:
                    return Vector2.Zero;
                case TextAnchor.TopMiddle:
                    return new Vector2(0.5f, 0f);
                case TextAnchor.TopRight:
                    return new Vector2(1f, 0f);
                case TextAnchor.MiddleLeft:
                    return new Vector2(0f, 0.5f);
                case TextAnchor.Center:
                    return new Vector2(0.5f, 0.5f);
                case TextAnchor.MiddleRight:
                    return new Vector2(1f, 0.5f);
                case TextAnchor.BottomLeft:
                    return new Vector2(0f, 1f);
                case TextAnchor.BottomMiddle:
                    return new Vector2(0.5f, 1f);
                case TextAnchor.BottomRight:
                    return new Vector2(1f, 1f);
            }
        }
    }
}