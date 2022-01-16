using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;

namespace GameCore.UI.Elements
{
    public class UI9SlicePanel : UIGraphic
    {
        public int BorderWidth 
        {
            set 
            {
                BorderTop = value;
                BorderLeft = value;
                BorderRight = value;
                BorderBottom = value;
            }
        }
        public int BorderTop { get; set; }
        public int BorderLeft { get; set; }
        public int BorderRight { get; set; }
        public int BorderBottom { get; set; }
        public bool DoFill { get; set; }
        public bool StretchToFill { get; set; }
        public override bool EatsMouse => true;

        public UI9SlicePanel(ISprite sprite, int borderSize) : this(sprite, borderSize, Color.White) { }
        public UI9SlicePanel(ISprite sprite, int borderSize, Color color) : base(sprite, color, Vector4.One)
        {
            BorderWidth = borderSize;
            StretchToFill = true;
            DoFill = true;
        }

        public override void Draw()
        {
            RectangleF rect = CalculateRect();
            
            if (StretchToFill) UIUtils.DrawNineSlicePanelStretched(rect, Sprite, Color.MultipliedBy(ColorMultiplier), BorderTop, BorderRight, BorderBottom, BorderLeft, DoFill);
            else UIUtils.DrawNineSlicePanelRepeated(rect, Sprite, Color.MultipliedBy(ColorMultiplier), BorderTop, BorderRight, BorderBottom, BorderLeft, DoFill);

            base.Draw();
        }

        public override UIGraphic Clone()
        {
            return new UI9SlicePanel(Sprite, 0, Color)
            {
                BorderTop = this.BorderTop,
                BorderLeft = this.BorderLeft,
                BorderRight = this.BorderRight,
                BorderBottom = this.BorderBottom,
                ColorMultiplier = this.ColorMultiplier
            };
        }
    }
}