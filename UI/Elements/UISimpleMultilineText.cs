using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MonoGame.Extended.BitmapFonts;

using GameCore.Localisation;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    /// <summary>
    /// UIText that can handle multiple lines. Can not rotate.
    /// </summary>
    public class UISimpleMultilineText : UIText
    {
        private bool _needsWrap;
        private string _wrapped;
        public override LocalisationText LocalisedText { get => base.LocalisedText; set { base.LocalisedText = value; WrapText(); } }
        public override string Text { get => base.Text; set { base.Text = value; WrapText(); } }

        public UISimpleMultilineText(BitmapFont font, string text) : base(font, text, Color.White) { }
        public UISimpleMultilineText(BitmapFont font, string text, Color color) : base(font, text, color, null) { }
        public UISimpleMultilineText(BitmapFont font, string text, Color color, Effect effect) : base(font, text, color, effect) { }
        public UISimpleMultilineText(BitmapFont font, LocalisationText text) : base(font, text, Color.White) { text.OnChange += OnLanguageChange; }
        public UISimpleMultilineText(BitmapFont font, LocalisationText text, Color color) : base(font, text, color, null) { text.OnChange += OnLanguageChange; }
        public UISimpleMultilineText(BitmapFont font, LocalisationText text, Color color, Effect effect) : base(font, text, color, effect) { text.OnChange += OnLanguageChange; }

        void OnLanguageChange()
        {
            _needsWrap = true;
        }

        public override void SetDirtyRect()
        {
            base.SetDirtyRect();
            _needsWrap = true;
        }

        private void WrapText()
        {
            float width = CalculateRect().Width;
            _wrapped = Font.WrapText(Text, width / Scale);
        }

        public override void Dispose()
        {
            LocalisedText.OnChange -= OnLanguageChange;
            base.Dispose();
        }

        protected override void DrawMyText()
        {
            if (_needsWrap) WrapText();

            string[] lines = _wrapped.Split('\n');
            
            // get the total size of all the text
            Vector2 totalSize = Vector2.Zero;
            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 size = Font.MeasureString(lines[i]);
                totalSize.X = MathF.Max(totalSize.X, size.X);
                totalSize.Y += size.Y;
            }

            // find the correct top left corner based on the anchor
            Vector2 origin = GetOriginFromAnchor();
            RectangleF area = CalculateRect();
            Vector2 topLeft = area.TopLeft + area.Size * origin - totalSize * origin * Scale;

            for (int i = 0; i < lines.Length; i++)
            {
                // draw each line, based on the anchor.
                Vector2 size = Font.MeasureString(lines[i]);
                Vector2 pos = topLeft + Vector2.UnitX * (totalSize.X - size.X) * origin.X * Scale;
                Engine.SpriteBatch.DrawString(Font, lines[i], pos, Color.MultipliedBy(ColorMultiplier), 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                topLeft.Y += Font.LineHeight * Scale;
            }
        }
    }
}