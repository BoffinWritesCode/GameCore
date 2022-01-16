using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Graphics;
using Microsoft.Xna.Framework.Input;
using GameCore.Input;

namespace GameCore.UI.Elements
{
    public class UISash : UIElement, IColorable
    {
        protected enum SashMode
        {
            Element1Fixed,
            Element2Fixed,
            PercentageBased
        }

        private float sashThickness = 4f;
        public float SashThickness 
        { 
            get => sashThickness; 
            set 
            {
                sashThickness = value; 
                if (_sash != null) _sash.SliderSize = value;
            }
        }
        public Color Color { get; set; } = Color.White;
        public Vector4 ColorMultiplier { get; set; } = Vector4.One;
        public float VisibilityDelay { get; set; } = 0.2f;
        public float AlphaTransitionSpeed { get; set; } = 5f;
        public Axis Direction { get; set; }

        protected float _alpha;
        protected UISlider _sash;
        protected bool _hoveringSash;
        protected float _delayTimer;
        protected SashMode _mode;
        protected float _sashValue;
        protected UIElement _element1;
        protected UIElement _element2;

        public UISash(UIElement first, UIElement second, Axis dir)
        {
            AddChild(first);
            AddChild(second);

            _element1 = first;
            _element2 = second;

            _mode = SashMode.PercentageBased;
            Direction = dir;

            // create the slider
            _sash = new UISlider(new UISprite(GraphicsExtras.PixelSprite, Color));
            _sash.SliderAxis = dir;
            _sash.SliderSize = SashThickness;
            _sash.OnValueChanged += UpdateSashDisplay;
            _sash.SliderElement.OnMouseEnter += SashEnter;
            _sash.SliderElement.OnMouseExit += SashExit;
            _sash.SliderElement.OnMouseReleased += (UIElement element, MouseInput button) => { if (button == MouseInput.Left) SashExit(element); }; 
            _sash.Value = 0.5f;
            _sashValue = 0.5f;

            AddChild(_sash);

            UpdateChildrenSizes();
        }

        public void SetElement1FixedSize()
        {
            _mode = SashMode.Element1Fixed;
        }

        public void SetElement2FixedSize()
        {
            _mode = SashMode.Element2Fixed;
        }

        public override void Update()
        {
            base.Update();

            if (_hoveringSash || _sash.BeingDragged)
            {
                _delayTimer += Time.DeltaTime;
                if (_delayTimer > VisibilityDelay)
                {
                    _alpha += AlphaTransitionSpeed * Time.DeltaTime;
                    if (_alpha > 1f) _alpha = 1f;
                }
            }
            else
            {
                _alpha -= AlphaTransitionSpeed * Time.DeltaTime;
                if (_alpha <= 0f)
                {
                    _alpha = 0f;
                    _delayTimer = 0f;
                }
            }
        }

        public override void Draw()
        {
            _sash.SliderElement.Color = (Color * _alpha).MultipliedBy(ColorMultiplier);

            base.Draw();
        }

        public override RectangleF CalculateRect()
        {
            if (_dirtyRect)
            {
                // if we have a dirty rect, 
                RectangleF newRect = base.CalculateRect();
                _sash.CalculateRect();

                switch (_mode)
                {
                    case SashMode.PercentageBased:
                        _sash.Value = _sashValue;
                        UpdateSashDisplay(_sashValue);
                        break;
                }

                return newRect;
            }

            return base.CalculateRect();
        }

        private void SashExit(UIElement element)
        {
            _hoveringSash = false;
            if (!_sash.BeingDragged) Mouse.SetCursor(MouseCursor.Arrow);
        }

        private void SashEnter(UIElement element)
        {
            _hoveringSash = true;
            Mouse.SetCursor(Direction == Axis.Horizontal ? MouseCursor.SizeWE : MouseCursor.SizeNS);
        }

        private void UpdateSashDisplay(float value)
        {
            _sashValue = value;
            UpdateChildrenSizes();
        }

        private void UpdateChildrenSizes()
        {
            RectangleF area = CalculateRect();
            switch (_mode)
            {
                case SashMode.PercentageBased:
                    float firstPercent = _sashValue;
                    float secondPercent = 1f - _sashValue;
                    if (Direction == Axis.Horizontal)
                    {
                        float el1TargetWidth = area.Width * firstPercent;
                        float el2TargetWidth = area.Width * secondPercent;
                        _element1.Rect.AnchorLeftStretchVertically(el1TargetWidth, 0f, 0f, 0f);
                        _element2.Rect.AnchorRightStretchVertically(el2TargetWidth, 0f, 0f, 0f);

                        RectangleF newEl1 = _element1.CalculateRect();
                        RectangleF newEl2 = _element2.CalculateRect();
                        if (newEl1.Width != el1TargetWidth)
                        {
                            _element2.Rect.AnchorRightStretchVertically(area.Width - newEl1.Width, 0f, 0f, 0f);
                            _sash.Value = newEl1.Width / area.Width;
                        }
                        if (newEl2.Width != el2TargetWidth)
                        {
                            _element1.Rect.AnchorLeftStretchVertically(area.Width - newEl2.Width, 0f, 0f, 0f);
                            _sash.Value = 1f - (newEl2.Width / area.Width);
                        }
                    }
                    else
                    {
                        _element1.Rect.AnchorTopStretchHorizontally(area.Height * firstPercent, 0f, 0f, 0f);
                        _element2.Rect.AnchorBottomStretchHorizontally(area.Height * secondPercent, 0f, 0f, 0f);
                    }
                    break;
            }
        }
    }
}