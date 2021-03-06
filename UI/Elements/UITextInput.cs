using System;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

using GameCore.Localisation;
using GameCore.Maths;
using GameCore.Graphics;
using GameCore.Input;

using TextCopy;

namespace GameCore.UI.Elements
{
    public class UITextInput : UIText
    {
        protected LocalisationText _placerholderLocalised;
        protected string _placeholderText;
        protected int _cursorLocation;
        protected GameWindow _window;
        protected StringBuilder _builder;
        protected bool _isSelected;
        protected int _forceCursorDraw;
        protected int _selectionEnd1;
        protected int _selectionEnd2;

        public Color PlaceholderColor { get; set; }

        public override string Text 
        {
            get => _builder.ToString();
            set 
            {
                if (_builder == null) _builder = new StringBuilder();
                _builder.Clear();
                _builder.Append(value);
                OnTextUpdate();
            }
        }

        public string PlaceholderText
        {
            get => _placerholderLocalised == null ? _placeholderText : _placerholderLocalised.Value;
            set { _placerholderLocalised = null; _placeholderText = value; }
        }

        protected string DrawText => (_builder.Length == 0 && !_isSelected) ? PlaceholderText : _builder.ToString();

        public event Action<string> OnValueChanged;

        public bool Multiline { get; set; } = false;
        public int MaximumLength { get; set; } = 40;

        public UITextInput(BitmapFont font, string placeholderText, bool doEnableEvents = true) : base(font, "")
        {
            _placeholderText = placeholderText;
            Construct(font, doEnableEvents);
        }

        public UITextInput(BitmapFont font, LocalisationText placeholderText, bool doEnableEvents = true) : base(font, "")
        {
            _placerholderLocalised = placeholderText;
            Construct(font, doEnableEvents);
        }

        private void Construct(BitmapFont font, bool doEnableEvents)
        {
            _window = Engine.Instance.Window;

            if (_builder == null)
                _builder = new StringBuilder();

            _selectionEnd1 = _selectionEnd2 = -1;

            if (doEnableEvents)
            {
                OnMousePressed += (UIElement e, MouseInput _) => EnableInput();
                OnLeftClickElsewhere += (UIElement _) => DisableInput();
            }

            UIUtils.SetupCursorOnHover(this, MouseCursor.IBeam);
        }

        protected override void DrawMyText()
        {
            string text = DrawText;

            // get padding modified area
            RectangleF area = CalculateRect();
            Padding.ModifyWith(ref area, Padding);
            Vector2 size = Font.MeasureString(text);
            Vector2 origin = GetOriginFromAnchor();
            Vector2 pos = (area.TopLeft + area.Size * origin) + TextOffset + size * (RotationOrigin - origin) * Scale;

            Engine.SpriteBatch.DrawString(Font, text, pos, (_builder.Length == 0 && !_isSelected) ? PlaceholderColor.MultipliedBy(ColorMultiplier) : Color.MultipliedBy(ColorMultiplier), Rotation, size * RotationOrigin, Scale, SpriteEffects.None, 0f);
        }

        public override void Draw()
        {
            base.Draw();

            if (!_isSelected) return;

            RectangleF area = CalculateRect();
            Padding.ModifyWith(ref area, Padding);
            Vector2 size = Font.MeasureString(DrawText);
            size.Y = Font.LineHeight;
            Vector2 origin = GetOriginFromAnchor();
            Vector2 pos = (area.TopLeft + area.Size * origin) + TextOffset + size * (RotationOrigin - origin) * Scale;

            // draw selection
            if (_selectionEnd1 != -1)
            {
                int min = Math.Min(_selectionEnd1, _selectionEnd2);
                int max = Math.Max(_selectionEnd1, _selectionEnd2);

                string start = _builder.ToString(0, min);
                Vector2 startSize = Font.MeasureString(start);
                startSize.Y = Font.LineHeight * 0.5f;
                string end = _builder.ToString(0, max);
                Vector2 endSize = Font.MeasureString(end);
                endSize.Y = Font.LineHeight * 0.5f;
                
                Vector2 selectionCenter = (area.TopLeft + area.Size * origin) + TextOffset + (startSize + (endSize - startSize) * 0.5f) * Scale;
                selectionCenter = selectionCenter.RotateMeAround(pos, Rotation);

                Vector2 scale = new Vector2(endSize.X - startSize.X, Font.LineHeight) * Scale;

                Engine.SpriteBatch.Draw(GraphicsExtras.PixelSprite, selectionCenter, Color * 0.2f, Rotation, new Vector2(0.5f), scale);
            }

            // draw cursor
            bool drawBlink = _forceCursorDraw > 0 || (Time.TotalTime % 1f) > 0.5f;
            if (drawBlink)
            {
                _forceCursorDraw--;
                string before = _builder.ToString(0, _cursorLocation);
                Vector2 cursorMeasure = Font.MeasureString(before);
                cursorMeasure.Y = Font.LineHeight * 0.5f;
                Vector2 cursorCenter = (area.TopLeft + area.Size * origin) + TextOffset + cursorMeasure * Scale;
                cursorCenter = cursorCenter.RotateMeAround(pos, Rotation);
                Engine.SpriteBatch.Draw(GraphicsExtras.PixelSprite, cursorCenter, Color, Rotation, new Vector2(0.5f), new Vector2(2f, Font.LineHeight * Scale * 0.8f));
            }
        }

        public override void Update()
        {
            if (_isSelected)
            {
                if (GameInput.IsTextInputPressed(Keys.Home))
                {
                    HandleControlCharacter(Keys.Home);
                }

                if (GameInput.IsTextInputPressed(Keys.End))
                {
                    HandleControlCharacter(Keys.End);
                }

                if (GameInput.IsTextInputPressed(Keys.Left))
                {
                    HandleControlCharacter(Keys.Left);
                }
                else if (GameInput.IsTextInputPressed(Keys.Right))
                {
                    HandleControlCharacter(Keys.Right);
                }
                
                bool ctrlDown = GameInput.IsDown(Keys.LeftControl) || GameInput.IsDown(Keys.RightControl);
                bool shiftDown = GameInput.IsDown(Keys.LeftShift) || GameInput.IsDown(Keys.RightShift);

                if (ctrlDown && !shiftDown)
                {
                    if (GameInput.IsJustPressed(Keys.C)) HandleControlShortcuts(Keys.C);
                    else if (GameInput.IsJustPressed(Keys.X)) HandleControlShortcuts(Keys.X);
                    else if (GameInput.IsJustPressed(Keys.V)) HandleControlShortcuts(Keys.V);
                    else if (GameInput.IsJustPressed(Keys.A)) HandleControlShortcuts(Keys.A);
                }
            }

            base.Update();
        }

        public void EnableInput()
        {
            if (_isSelected) return;

            _isSelected = true;
            _window.TextInput += TextInput;
        }

        public void DisableInput()
        {
            if (!_isSelected) return;

            _isSelected = false;
            _window.TextInput -= TextInput;
        }

        protected void TextInput(object sender, TextInputEventArgs e)
        {
            if (char.IsControl(e.Character))
            {
                HandleControlCharacter(e.Key);
                return;
            }
            AddToInput(e.Character.ToString());
        }

        protected void HandleControlShortcuts(Keys key)
        {
            switch (key)
            {
                case Keys.C:
                    // if there's a selection and ctrl+C
                    if (_selectionEnd1 != -1)
                    {
                        string copy = _builder.ToString(GetSelectionBeginning(), GetSelectionLength());
                        ClipboardService.SetText(copy);
                    }
                    return;
                case Keys.X:
                    // if there's a selection and ctrl+X
                    if (_selectionEnd1 != -1)
                    {
                        string copy = _builder.ToString(GetSelectionBeginning(), GetSelectionLength());
                        ClipboardService.SetText(copy);

                        DeleteSelection();
                    }
                    return;
                case Keys.V:
                    // ctrl+V
                    string paste = ClipboardService.GetText();
                    if (!string.IsNullOrEmpty(paste))
                    {
                        AddToInput(paste);
                    }
                    return;
                case Keys.A:
                    // ctrl+A
                    _selectionEnd1 = 0;
                    _selectionEnd2 = _builder.Length;
                    return;
            }
        }

        protected virtual void AddToInput(string value)
        {
            if (_selectionEnd1 != -1)
            {
                DeleteSelection();
            }

            _builder.Insert(_cursorLocation, value);
            if (_builder.Length > MaximumLength)
            {
                _builder.Remove(MaximumLength, _builder.Length - MaximumLength);
            }
            _cursorLocation += value.Length;
            ValidateCursorLocation(_builder);

            OnTextUpdate();
            
            EndSelection();
        }

        protected void HandleControlCharacter(Keys key)
        {
            bool ctrlDown = GameInput.IsDown(Keys.LeftControl) || GameInput.IsDown(Keys.RightControl);
            bool shiftDown = GameInput.IsDown(Keys.LeftShift) || GameInput.IsDown(Keys.RightShift);

            switch(key)
            {
                case Keys.Back:
                    if (_selectionEnd1 != -1)
                    {
                        DeleteSelection();
                        break;
                    }
                    if (_cursorLocation <= 0) break;
                    _builder.Remove(--_cursorLocation, 1);
                    
                    OnTextUpdate();
                    break;
                case Keys.Left:
                    _forceCursorDraw = 31;

                    if (shiftDown) TryStartSelection();
                    else EndSelection();

                    if (ctrlDown)
                    {
                        MoveUntilNextNonChar(-1);
                    }
                    else
                    {
                        _cursorLocation--;
                    }

                    ValidateCursorLocation(_builder);

                    if (shiftDown) UpdateSelection();
                    break;
                case Keys.Right:
                    _forceCursorDraw = 31;

                    if (shiftDown) TryStartSelection();
                    else EndSelection();

                    if (ctrlDown)
                    {
                        MoveUntilNextNonChar(1);
                    }
                    else
                    {
                        _cursorLocation++;
                    }

                    ValidateCursorLocation(_builder);

                    if (shiftDown) UpdateSelection();
                    break;
                case Keys.Delete:
                    if (_selectionEnd1 != -1)
                    {
                        DeleteSelection();
                        break;
                    }

                    if (shiftDown)
                    {
                        _builder.Length = 0;
                        OnTextUpdate();
                        break;
                    }

                    if (_cursorLocation >= _builder.Length) break;

                    _builder.Remove(_cursorLocation, 1);
                    OnTextUpdate();
                    break;
                case Keys.Home:
                    if (shiftDown)
                    {
                        _selectionEnd2 = _cursorLocation;
                        _cursorLocation = 0;
                        _selectionEnd1 = 0;
                        break;
                    }
                    _cursorLocation = 0;
                    EndSelection();
                    break;
                case Keys.End:
                    if (shiftDown)
                    {
                        _selectionEnd2 = _cursorLocation;
                        _cursorLocation = _builder.Length;
                        _selectionEnd1 = _cursorLocation;
                        break;
                    }
                    _cursorLocation = _builder.Length;
                    EndSelection();
                    break;
                case Keys.Enter:
                    if (Multiline)
                    {
                        // add new line here
                    }
                    break;
            }
        }

        protected void MoveUntilNextNonChar(int moveSpeed)
        {
            int curIndex = _cursorLocation;
            if (moveSpeed < 0) curIndex += moveSpeed;

            if (!WithinString(curIndex)) return;


            // move until we find our first letter or digit
            if (!char.IsLetterOrDigit(_builder[curIndex]))
            {
                for (; WithinString(curIndex); curIndex += moveSpeed) if (char.IsLetterOrDigit(_builder[curIndex])) break;
            }

            // skip all letters / digits
            for (; WithinString(curIndex); curIndex += moveSpeed) if (!char.IsLetterOrDigit(_builder[curIndex])) break;

            if (!WithinString(curIndex))
            {
                _cursorLocation = curIndex < 0 ? 0 : _builder.Length;
                return;
            }

            if (!char.IsLetterOrDigit(_builder[curIndex])) curIndex++;

            _cursorLocation = curIndex;
        }

        protected void DeleteSelection()
        {
            int startIndex = GetSelectionBeginning();
            int len = GetSelectionLength();

            _builder.Remove(startIndex, len);

            OnTextUpdate();
            EndSelection();

            if (_cursorLocation <= startIndex) return;
            _cursorLocation -= len;
            ValidateCursorLocation(_builder);
        }

        protected void TryStartSelection()
        {
            if (_selectionEnd1 == -1) 
            {
                _selectionEnd1 = _selectionEnd2 = _cursorLocation;
            }
        }

        protected void EndSelection() => _selectionEnd1 = -1;

        protected void UpdateSelection() => _selectionEnd2 = _cursorLocation;

        protected void OnTextUpdate()
        {
            if (AutoScale) FitTextToRect();
            OnValueChanged?.Invoke(_builder.ToString());
        }

        protected bool WithinString(int index)
        {
            return index >= 0 && index < _builder.Length;
        }

        protected int GetSelectionBeginning() => Math.Min(_selectionEnd1, _selectionEnd2);

        protected int GetSelectionLength() => Math.Max(_selectionEnd1, _selectionEnd2) - GetSelectionBeginning();

        protected void ValidateCursorLocation(StringBuilder text)
        {
            _cursorLocation = MathHelper.Clamp(_cursorLocation, 0, text.Length);
        }

        public override void Dispose()
        {
            if (_isSelected)
            {
                _window.TextInput -= TextInput;
            }
            base.Dispose();
        }
    }
}