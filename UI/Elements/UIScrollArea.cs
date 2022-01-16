using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Input;

namespace GameCore.UI.Elements
{
    public class UIScrollArea : UIElement
    {
        public UILayoutElement ScrollElement { get; protected set; }

        protected UISlider _myScrollbar;

        /// <summary>
        /// The scroll value from 0 to 1 horizontally.
        /// </summary>
        private float _scrollX;
        public float ScrollValueX { get => _scrollX; set { _scrollX = Math.Max(0f, Math.Min(GetAvailableMovement().X, value)); UpdateScrollElementPosition(); } }
        /// <summary>
        /// The scroll value from 0 to 1 vertically.
        /// </summary>
        private float _scrollY;
        public float ScrollValueY { get => _scrollY; set { _scrollY = Math.Max(0f, Math.Min(GetAvailableMovement().Y, value)); UpdateScrollElementPosition(); } }

        public Axis ScrollType { get; set; }
        public float ScrollDistance { get; set; } = 16f;

        public UIScrollArea()
        {
            OnScroll += ScrollCheck;
            ForceInputUpdate = true;
        }
        
        private void ScrollCheck(UIElement element)
        {
            Vector2 movementAvailable = GetAvailableMovement();
            float sensitivity = ScrollDistance;
            bool up = GameInput.DeltaScroll > 0;
            if (up)
            {
                switch (ScrollType)
                {
                    case Axis.Vertical:
                        ScrollValueY -= sensitivity;
                        break;
                    case Axis.Horizontal:
                        ScrollValueX -= sensitivity;
                        break;
                }
            }
            else
            {
                switch (ScrollType)
                {
                    case Axis.Vertical:
                        ScrollValueY += sensitivity;
                        break;
                    case Axis.Horizontal:
                        ScrollValueX += sensitivity;
                        break;
                }
            }

            // update scroll bar position
            if (_myScrollbar == null) return;
            if (ScrollType == Axis.Horizontal && movementAvailable.X == 0f) return;
            if (ScrollType == Axis.Vertical && movementAvailable.Y == 0f) return;

            _myScrollbar.Value = ScrollType == Axis.Horizontal ? ScrollValueX / movementAvailable.X : ScrollValueY / movementAvailable.Y;
        }

        public override void Update()
        {
            var rect = CalculateRect().ToRectangle();
            ScissorTesting.PushScissor(rect);

            bool updateScrolls = _dirtyRect;

            base.Update();

            // clamp the scrolls
            if (updateScrolls)
            {
                if (ScrollType == Axis.Horizontal) ScrollValueX = ScrollValueX;
                if (ScrollType == Axis.Vertical) ScrollValueY = ScrollValueY;
            }

            ScissorTesting.PopScissor();
        }

        public override void Draw()
        {
            if (ScrollElement == null || ScrollElement.Children.Count == 0) return;

            var rect = CalculateRect().ToRectangle();

            Engine.SpriteBatch.End();
            ScissorTesting.PushScissor(rect);
            Engine.SpriteBatch.Begin(rasterizerState: ScissorTesting.SetScissorAndGetRasterizer(), samplerState: Settings.SamplerState);

            var screenRect = new RectangleF(ScissorTesting.CurrentScreenArea);

            foreach (var child in ScrollElement.Children)
            {
                RectangleF area = child.CalculateRect();

                // perhaps this would be better if it broke? can we assume that once the first child isn't visible, the rest wont be?
                if (!area.Intersects(screenRect)) continue;

                child.Draw();
            }

            Engine.SpriteBatch.End();
            ScissorTesting.PopScissor();
            Engine.SpriteBatch.Begin(rasterizerState: ScissorTesting.SetScissorAndGetRasterizer(), samplerState: Settings.SamplerState);
        }

        public void SetScrollbar(UISlider scrollbar)
        {
            if (_myScrollbar != null) _myScrollbar.OnValueChanged -= ScrollToValue;
            _myScrollbar = scrollbar;
            scrollbar.OnValueChanged += ScrollToValue;
            UpdateScrollBarSize();
        }

        private void UpdateScrollBarSize()
        {
            RectangleF area = CalculateRect();
            Vector2 childArea = ScrollElement.ChildArea;
            if (childArea == Vector2.Zero) return;

            float percentageAvailable = ScrollType == Axis.Vertical ? area.Height / childArea.Y : area.Width / childArea.X;
            percentageAvailable = MathHelper.Clamp(percentageAvailable, 0f, 1f);
            _myScrollbar.SliderSize = _myScrollbar.SlideSpace * percentageAvailable;
        }
        
        private void ScrollToValue(float value)
        {
            if (ScrollType == Axis.Horizontal)
            {
                ScrollValueX = GetAvailableMovement().X * value;
                return;
            }

            ScrollValueY = GetAvailableMovement().Y * value;
        }

        public void SetScrollLayoutElement(UILayoutElement element) 
        { 
            if (ScrollElement != null)
            {
                ScrollElement.OnLayoutUpdate -= UpdateScrollElementPosition;
            }

            ScrollElement = element;
            ScrollElement.SetParent(this);
            ScrollElement.OnLayoutUpdate += UpdateScrollElementPosition;
        }

        public void UpdateScrollElementPosition()
        {
            // Vector2 availableMovement = GetAvailableMovement();
            if (ScrollType == Axis.Vertical)
            {
                ScrollElement.Rect.AnchorTopStretchHorizontally(ScrollElement.ChildArea.Y, (float)(-ScrollValueY), 0, 0);
                UpdateScrollBarSize();
                return;
            }
            ScrollElement.Rect.AnchorLeftStretchVertically(ScrollElement.ChildArea.X, (float)(-ScrollValueX), 0, 0);
            UpdateScrollBarSize();
            return;
        }

        private Vector2 GetAvailableMovement()
        {
            RectangleF area = CalculateRect();
            Vector2 childArea = ScrollElement.ChildArea;
            float availableXMovement = Math.Max(0, childArea.X - area.Width);
            float availableYMovement = Math.Max(0, childArea.Y - area.Height);
            return new Vector2(availableXMovement, availableYMovement);
        }
    }
}