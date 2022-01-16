using GameCore.Localisation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Xml.Linq;

namespace GameCore.UI.Elements
{
    public class UIModularText : UIText
    {
        // UIModularText modularText = new UIModularText(null, "<text>this text is <spin speed=\"0.5\">W A C K Y</spin></text>", Color.White);

        public UIModularText(BitmapFont font, string text) : base(font, text, Color.White) { EvaluateString(); }
        public UIModularText(BitmapFont font, string text, Color color) : base(font, text, color, null) { EvaluateString(); }
        public UIModularText(BitmapFont font, string text, Color color, Effect effect) : base(font, text, color, effect) { EvaluateString(); }
        public UIModularText(BitmapFont font, LocalisationText text) : base(font, text, Color.White) { EvaluateString(); }
        public UIModularText(BitmapFont font, LocalisationText text, Color color) : base(font, text, color, null) { EvaluateString(); }
        public UIModularText(BitmapFont font, LocalisationText text, Color color, Effect effect) : base(font, text, color, effect) { EvaluateString(); }

        protected void EvaluateString()
        {
            XElement root = XElement.Parse(Text);
            foreach (var child in root.Nodes())
            {

            }
        }

        protected void RecursiveTextParse(XElement element)
        {

        }

        protected override void DrawMyText()
        {
        }
    }
}
