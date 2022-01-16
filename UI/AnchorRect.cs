using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI
{
    public class AnchorRect
    {
        private AnchorPoint _left;
        public AnchorPoint Left { get => _left; set { _left = value; OnModifyAnchorPoint?.Invoke(); } }
        private AnchorPoint _top;
        public AnchorPoint Top { get => _top; set { _top = value; OnModifyAnchorPoint?.Invoke(); } }
        private AnchorPoint _right;
        public AnchorPoint Right { get => _right; set { _right = value; OnModifyAnchorPoint?.Invoke(); } }
        private AnchorPoint _bottom;
        public AnchorPoint Bottom { get => _bottom; set { _bottom = value; OnModifyAnchorPoint?.Invoke(); } }

        public event Action OnModifyAnchorPoint;

        public AnchorRect() { }

        public AnchorRect(AnchorRect copy)
        {
            _left = new AnchorPoint(copy._left.AxisInterpolation, copy._left.PixelOffset);
            _top = new AnchorPoint(copy._top.AxisInterpolation, copy._top.PixelOffset);
            _right = new AnchorPoint(copy._right.AxisInterpolation, copy._right.PixelOffset);
            _bottom = new AnchorPoint(copy._bottom.AxisInterpolation, copy._bottom.PixelOffset);
        }

        public void Lerp(AnchorRect a1, AnchorRect a2, float interpolation)
        {
            _left = AnchorPoint.Lerp(a1._left, a2._left, interpolation);
            _right = AnchorPoint.Lerp(a1._right, a2._right, interpolation);
            _top = AnchorPoint.Lerp(a1._top, a2._top, interpolation);
            Bottom = AnchorPoint.Lerp(a1._bottom, a2._bottom, interpolation);
        }

        public AnchorRect FillParent(float leftPadding = 0f, float topPadding = 0f, float rightPadding = 0f, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorTopLeft(float width, float height, float leftPadding = 0f, float topPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(0f, leftPadding + width);
            Bottom = new AnchorPoint(0f, topPadding + height);

            return this;
        }

        public AnchorRect AnchorTopMiddle(float width, float height, float topPadding = 0f)
        {
            Left = new AnchorPoint(0.5f, width * -0.5f);
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(0.5f, width * 0.5f);
            Bottom = new AnchorPoint(0f, topPadding + height);

            return this;
        }

        public AnchorRect AnchorTopRight(float width, float height, float rightPadding = 0f, float topPadding = 0f)
        {
            Left = new AnchorPoint(1f, -(rightPadding + width));
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(0f, topPadding + height);

            return this;
        }

        public AnchorRect AnchorMiddleLeft(float width, float height, float leftPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(0.5f, height * -0.5f);
            Right = new AnchorPoint(0f, leftPadding + width);
            Bottom = new AnchorPoint(0.5f, height * 0.5f);

            return this;
        }

        public AnchorRect AnchorMiddleRight(float width, float height, float rightPadding = 0f)
        {
            Left = new AnchorPoint(1f, -(rightPadding + width));
            Top = new AnchorPoint(0.5f, height * -0.5f);
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(0.5f, height * 0.5f);

            return this;
        }

        public AnchorRect AnchorMiddle(float width, float height, float xOffset = 0f, float yOffset = 0f)
        {
            Left = new AnchorPoint(0.5f, width * -0.5f + xOffset);
            Top = new AnchorPoint(0.5f, height * -0.5f + yOffset);
            Right = new AnchorPoint(0.5f, width * 0.5f + xOffset);
            Bottom = new AnchorPoint(0.5f, height * 0.5f + yOffset);

            return this;
        }

        public AnchorRect AnchorBottomLeft(float width, float height, float leftPadding = 0f, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(1f, -(bottomPadding + height));
            Right = new AnchorPoint(0f, leftPadding + width);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorBottomMiddle(float width, float height, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(0.5f, width * -0.5f);
            Top = new AnchorPoint(1f, -(bottomPadding + height));
            Right = new AnchorPoint(0.5f, width * 0.5f);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorBottomRight(float width, float height, float rightPadding = 0f, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(1f, -(rightPadding + width));
            Top = new AnchorPoint(1f, -(bottomPadding + height));
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorTopStretchHorizontally(float height, float topPadding = 0f, float leftPadding = 0f, float rightPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(0f, topPadding + height);

            return this;
        }

        public AnchorRect AnchorBottomStretchHorizontally(float height, float bottomPadding = 0f, float leftPadding = 0f, float rightPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(1f, -(bottomPadding + height));
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorLeftStretchVertically(float width, float leftPadding = 0f, float topPadding = 0f, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(0f, leftPadding);
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(0f, leftPadding + width);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }

        public AnchorRect AnchorRightStretchVertically(float width, float rightPadding = 0f, float topPadding = 0f, float bottomPadding = 0f)
        {
            Left = new AnchorPoint(1f, -(rightPadding + width));
            Top = new AnchorPoint(0f, topPadding);
            Right = new AnchorPoint(1f, -rightPadding);
            Bottom = new AnchorPoint(1f, -bottomPadding);

            return this;
        }
    }
}