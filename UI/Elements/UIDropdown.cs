using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;
using Microsoft.Xna.Framework.Input;
using GameCore.Input;
using MonoGame.Extended.BitmapFonts;

namespace GameCore.UI.Elements
{
    public class UIDropdown : UIElement, IColorable
    {
        private Dictionary<string, int> _dropdownIndex;
        protected List<DropdownData> _dropdownElements;
        protected bool _showDropdown;
        protected int _currentIndex;
        protected BitmapFont _font;
        protected Effect _sdf;
        protected UIGraphic _bg;

        private Color _color;
        public Color Color 
        { 
            get => _color;
            set
            {
                _color = value;
                foreach (DropdownData child in _dropdownElements) child.Background.Color = value;
            }
        }

        private Vector4 _clrMult;
        public Vector4 ColorMultiplier
        {
            get => _clrMult;
            set
            {
                _clrMult = value;
                foreach (DropdownData child in _dropdownElements) child.Background.ColorMultiplier = value;
            }
        }

        private Color _fontColor;
        public Color FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
                if (SelectedUIText != null) SelectedUIText.Color = value;
                foreach (DropdownData child in _dropdownElements) child.Text.Color = value;
            }
        }

        private Vector4 _fontClrMult;
        public Vector4 FontColorMultiplier
        {
            get => _fontClrMult;
            set
            {
                _fontClrMult = value;
                if (SelectedUIText != null) SelectedUIText.ColorMultiplier = value;
                foreach (DropdownData child in _dropdownElements) child.Text.ColorMultiplier = value;
            }
        }

        public string CurrentlySelected => _dropdownElements[_currentIndex].Text.Text;
        public float ChildHeight { get; set; } = 24f;
        public UIText SelectedUIText { get; protected set; }

        /// <summary>
        /// The graphic used for when a child is selected
        /// </summary>
        public UIGraphic SelectedGraphic { get; protected set; }
        
        /// <summary>
        /// The graphic used for dropdown arrows
        /// </summary>
        public UIGraphic DropdownArrowElement { get; protected set; }

        /// <summary>
        /// This graphic is used behind each dropdown option child.
        /// </summary>
        public UIGraphic ChildGraphic { get; protected set; }

        public UIDropdown(BitmapFont font, Effect sdfShader, UIGraphic dropdownArrowGraphic, UIGraphic backgroundGraphic, UIGraphic childGraphic, UIGraphic selectedGraphic, Color fontColor) : base()
        {
            UIUtils.SetupCursorOnHover(this, MouseCursor.Hand);

            _sdf = sdfShader;
            _font = font;
            _dropdownIndex = new Dictionary<string, int>();
            _dropdownElements = new List<DropdownData>();

            Color = backgroundGraphic.Color;
            ColorMultiplier = backgroundGraphic.ColorMultiplier;
            FontColor = fontColor;

            // default value
            SelectedGraphic = selectedGraphic;
            ChildGraphic = childGraphic;
            _bg = backgroundGraphic;

            AddChild(_bg);

            DropdownArrowElement = dropdownArrowGraphic;
            Point size = dropdownArrowGraphic.Sprite.GetTextureInfo().GetSourceNoNull().Size;
            DropdownArrowElement.Rect.AnchorMiddleRight(size.X, size.Y, 4f);
            AddChild(DropdownArrowElement);

            // create the selected text
            SelectedUIText = new UIText(font, "");
            SelectedUIText.Rect.FillParent(4f, 0f, 24f, 0f);
            SelectedUIText.Color = FontColor;
            SelectedUIText.AutoScale = true;
            SelectedUIText.Anchor = TextAnchor.MiddleLeft;
            SelectedUIText.MaxAutoScale = font.GetScaleFromLineHeight(24f);
            SelectedUIText.Scale = SelectedUIText.MaxAutoScale;
            SelectedUIText.Effect = sdfShader;
            SelectedUIText.Settings = Settings;
            AddChild(SelectedUIText);

            UIUtils.SetupMouseInteractionColor(this, MouseInput.Left, 0.85f, 0.7f, 0f, Easing.Linear, _bg);
            OnMousePressed += OnClick;
            OnLeftClickElsewhere += ClickOff;
        }

        private void OnClick(UIElement element, MouseInput input)
        {
            if (input != MouseInput.Left) return;

            _showDropdown = !_showDropdown;
        }

        private void ClickOff(UIElement clicked)
        {
            DropdownData click = _dropdownElements.Find(d => d.Background == clicked);
            if (click != null) return;
            _showDropdown = false;
        }

        public void AddOption(string value, Action onSet)
        {
            if (_dropdownIndex.ContainsKey(value)) throw new ArgumentException($"Dropdown of value '{value}' already exists!");

            // background
            UIGraphic dropChild = ChildGraphic.Clone();
            dropChild.Rect.AnchorBottomStretchHorizontally(ChildHeight, (_dropdownElements.Count + 1) * -ChildHeight, 0f, 0f);
            dropChild.Color = Color;
            UIUtils.SetupMouseInteractionColor(dropChild, MouseInput.Left, 0.85f, 0.7f,  0f, Easing.Linear, dropChild);
            UIUtils.SetupCursorOnHover(dropChild, MouseCursor.Hand);

            // selected sprite
            UIGraphic selectedClone = SelectedGraphic.Clone();
            selectedClone.ColorMultiplier = Vector4.Zero;
            Point size = selectedClone.Sprite.GetTextureInfo().GetSourceNoNull().Size;
            selectedClone.Rect.AnchorMiddleLeft(size.X, size.Y, 4f);
            dropChild.AddChild(selectedClone);

            // set up the text
            UIText dropText = new UIText(_font, value, FontColor);
            dropText.Anchor = TextAnchor.MiddleLeft;
            dropText.Text = value;
            dropText.Scale = _font.GetScaleFromLineHeight(ChildHeight);
            dropText.Rect.FillParent(4f + size.X + 4f, 0f, 0f, 0f);
            dropText.Effect = _sdf;
            dropText.Color = FontColor;
            dropChild.AddChild(dropText);
            
            // add to the various lists
            DropdownData data = new DropdownData(dropChild, dropText, selectedClone, onSet);
            _dropdownElements.Add(data);
            _dropdownIndex.Add(value, Children.Count);

            SelectOption(data);

            AddChild(dropChild);

            // if we have no current selection, select this one
            if (string.IsNullOrEmpty(SelectedUIText.Text))
            {
                Select(0);
            }
        }

        public void Select(int index)
        {
            _currentIndex = index;
            var data = _dropdownElements[index];

            data.OnSet?.Invoke();
            SelectedUIText.Text = data.Text.Text;

            // set the color for the newly selected to white, others to transparent
            foreach (DropdownData child in _dropdownElements) child.SelectedSprite.ColorMultiplier = Vector4.Zero;
            data.SelectedSprite.ColorMultiplier = Vector4.One;
            _showDropdown = false;
        }

        private void SelectOption(DropdownData data)
        {
            void Method(UIElement element, MouseInput button)
            {
                if (button != MouseInput.Left) return;

                _currentIndex = _dropdownElements.IndexOf(data);
                Select(_currentIndex);
            }

            data.Background.OnMousePressed += Method;
        }

        public void RemoveOption(string value)
        {
            if (!_dropdownIndex.TryGetValue(value, out int index)) return;

            _dropdownElements.RemoveAt(index);
        }

        public override void Update()
        {
            _animator?.Update();

            if (!IgnoreMouse && CalculateRect().Contains(GameInput.MousePosition))
            {
                UIInputManager.SetForemostElement(this);
            }

            _bg.Update();
            SelectedUIText.Update();
            DropdownArrowElement.Update();
            if (_showDropdown)
            {
                foreach (var child in _dropdownElements) child.Background.Update();
            }

            DoElsewhereCheck();
        }

        public override void ForceUpdateSettings()
        {
            foreach (var child in _dropdownElements)
            {
                child.Text.Settings = Settings;
            }
            base.ForceUpdateSettings();
        }
        
        public override void Draw()
        {
            _bg.Draw();
            SelectedUIText.Draw();
            DropdownArrowElement.Draw();
            if (_showDropdown)
            {
                foreach (var child in _dropdownElements) child.Background.Draw();
            }
        }

        protected class DropdownData
        {
            public UIGraphic Background { get; }
            public UIText Text { get; }
            public UIGraphic SelectedSprite { get; }
            public Action OnSet { get; }

            public DropdownData(UIGraphic bg, UIText text, UIGraphic ss, Action set)
            {
                Background = bg;
                Text = text;
                SelectedSprite = ss;
                OnSet = set;
            }
        }
    }
}