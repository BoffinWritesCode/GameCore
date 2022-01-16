using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore;
using GameCore.Maths;
using GameCore.Graphics;
using GameCore.Input;
using Microsoft.Xna.Framework.Input;

namespace GameCore.UI.Elements
{
    public class UIToggle : UIElement
    {
        public event Action OnToggleOn;
        public event Action OnToggleOff;

        private ToggleGroup _group;
        public ToggleGroup MyToggleGroup
        {
            get => _group;
            set 
            {
                if (_group != null)
                {
                    _group.RemoveToggle(this);
                }

                _group = value;
                _group.AddToggle(this);
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set 
            {
                if (value == _isSelected) return;

                _isSelected = value;
                if (_isSelected)
                {
                    if (MyToggleGroup != null)
                    {
                        MyToggleGroup.ToggleTo(this);
                    }
                    OnToggleOn?.Invoke();
                    if (!_canBeToggledOffByUser) DoesInputCheck = false;
                }
                else 
                {
                    OnToggleOff?.Invoke();

                    if (!_canBeToggledOffByUser)
                    {
                        DoesInputCheck = true;
                    }
                }
            }
        }

        private bool _canBeToggledOffByUser;

        public UIToggle(ToggleGroup group, bool canBeToggledOffByUser = false) : base()
        {
            MyToggleGroup = group;
            _canBeToggledOffByUser = canBeToggledOffByUser;

            OnMouseReleased += Toggle;
        }

        private void Toggle(UIElement element, MouseInput input)
        {
            if (input != MouseInput.Left) return;

            if (IsSelected && !_canBeToggledOffByUser) return;

            IsSelected = !IsSelected;
        }
    }
}