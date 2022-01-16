using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;
using GameCore.Input;

namespace GameCore.UI.Elements
{
    public class UISlider : UIElement
    {
        public UIGraphic SliderElement { get; protected set; }
        public Axis SliderAxis { get; set; }
        public float Value 
        {
            get 
            {
                RectangleF area = CalculateRect();
                RectangleF slider = SliderElement.CalculateRect();

                float divider = SliderAxis == Axis.Horizontal ? (area.Width - slider.Width) : (area.Height - slider.Height);
                if (divider == 0f) return 0f;

                if (SliderAxis == Axis.Horizontal) return (slider.Left - area.Left) / divider;
                return (slider.Top - area.Top) / divider;
            }
            set
            {
                float valueToUse = MathHelper.Clamp(value, 0f, 1f);
                RectangleF area = CalculateRect();

                if (SliderAxis == Axis.Horizontal) SliderElement.Rect.AnchorTopLeft(SliderSize, area.Height, (area.Width - SliderSize) * valueToUse, 0f);
                else SliderElement.Rect.AnchorTopLeft(area.Width, SliderSize, 0f, (area.Height - SliderSize) * valueToUse);

                ValidateSlider();
            }
        }
        public event Action<float> OnValueChanged;

        private float _sliderSize;
        public float SliderSize { get => _sliderSize; set { _sliderSize = value; ValidateSlider(); } }
        public bool BeingDragged { get; protected set; }

        /// <summary>
        /// The amount of space that the slider can slide in.
        /// </summary>
        public float SlideSpace 
        {
            get
            {
                RectangleF area = CalculateRect();
                if (SliderAxis == Axis.Horizontal) return area.Width;
                return area.Height;
            }
        }

        private Vector2 _mouseOffset;

        public UISlider(UIGraphic slider) : base()
        {
            SetSliderElement(slider);
        }

        public void SetSliderElement(UIGraphic element)
        {
            float prevValue = 0f;
            if (SliderElement != null)
            {
                prevValue = Value;
                SliderElement.OnMousePressed -= StartDrag;
                SliderElement.OnMouseDown -= Drag;
                //SliderElement.OnReleaseDrag -= EndDrag;
                RemoveChild(SliderElement);
            }
            
            SliderElement = element;

            AddChild(SliderElement);
            SliderElement.OnMousePressed += StartDrag;
            SliderElement.OnMouseDown += Drag;
            SliderElement.OnMouseReleased += EndDrag;

            Value = prevValue;
        }

        private void StartDrag(UIElement element, MouseInput obj)
        {
            if (obj != MouseInput.Left) return;

            RectangleF rect = SliderElement.CalculateRect();
            Vector2 mousePos = GameInput.MousePosition;
            _mouseOffset = mousePos - rect.TopLeft;
            BeingDragged = true;
        }

        private void Drag(UIElement element, MouseInput obj)
        {
            if (obj != MouseInput.Left) return;

            BeingDragged = true;
            RectangleF rect = CalculateRect();
            Vector2 mousePos = GameInput.MousePosition;
            if (SliderAxis == Axis.Horizontal) SliderElement.Rect.AnchorTopLeft(SliderSize, rect.Height, mousePos.X - _mouseOffset.X - rect.Left, 0f);
            else SliderElement.Rect.AnchorTopLeft(rect.Width, SliderSize, 0f, mousePos.Y - _mouseOffset.Y - rect.Top);

            float currentValue = Value;
            ValidateSlider();
            if (currentValue == Value) OnValueChanged?.Invoke(currentValue);
        }

        private void EndDrag(UIElement element, MouseInput obj)
        {
            if (obj != MouseInput.Left) return;

            BeingDragged = false;
        }

        public override RectangleF CalculateRect()
        {
            if (_dirtyRect)
            {
                RectangleF result = base.CalculateRect();

                ValidateSlider();

                return result;
            }

            return base.CalculateRect();
        }

        /// <summary>
        /// Ensures the slider element can be in the position it's been told to be.
        /// </summary>
        private void ValidateSlider()
        {
            RectangleF rect = CalculateRect();
            RectangleF sliderRect = SliderElement.CalculateRect();
            switch (SliderAxis)
            {
                case Axis.Horizontal:
                    
                    _sliderSize = MathHelper.Clamp(_sliderSize, 0.0001f, rect.Width);
                    if (sliderRect.Width != SliderSize || sliderRect.Height != rect.Height)
                    {
                        SliderElement.Rect.AnchorTopLeft(SliderSize, rect.Width, sliderRect.Left - rect.Left, 0f);
                    }

                    // clamp the slider
                    if (sliderRect.Left < rect.Left) { SliderElement.Rect.AnchorTopLeft(SliderSize, rect.Height, 0f, 0f); OnValueChanged?.Invoke(Value); }
                    else if (sliderRect.Right > rect.Right) { SliderElement.Rect.AnchorTopLeft(SliderSize, rect.Height, rect.Width - SliderSize, 0f); OnValueChanged?.Invoke(Value); }

                    break;
                case Axis.Vertical:
                    
                    _sliderSize = MathHelper.Clamp(_sliderSize, 0.0001f, rect.Height);
                    if (sliderRect.Width != rect.Width || sliderRect.Height != SliderSize)
                    {
                        SliderElement.Rect.AnchorTopLeft(rect.Width, SliderSize, 0f, sliderRect.Top - rect.Top);
                    }

                    // clamp the slider
                    if (sliderRect.Top < rect.Top) { SliderElement.Rect.AnchorTopLeft(rect.Width, SliderSize, 0f, 0f); OnValueChanged?.Invoke(Value); }
                    else if (sliderRect.Bottom > rect.Bottom) { SliderElement.Rect.AnchorTopLeft(rect.Width, SliderSize, 0f, rect.Height - SliderSize); OnValueChanged?.Invoke(Value); }

                    break;
            }
        }
    }
}