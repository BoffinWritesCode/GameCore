using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;
using GameCore.Input;

namespace GameCore.UI
{
    public static class UIInputManager
    {
        private static Dictionary<UIElement, InputData> _parentsChecked = new Dictionary<UIElement, InputData>();
        private static List<InputData> _dataUpdated = new List<InputData>();
        private static List<UIElement> _forcedElements = new List<UIElement>();
        private static InputData _currentData;
        private static UIElement _currentClicked;
        private static MouseInput _currentButton;    
        private static UIElement _foremostElement;

        /// <summary>
        /// Is the mouse over a UI element?
        /// </summary>
        public static bool MouseOverUIElement { get; internal set; }

        public static void SetCurrentInputParent(UIElement element)
        {
            if (_parentsChecked.TryGetValue(element, out InputData data))
            {
                _currentData = data;
                return;
            }

            _currentData = new InputData();
            _parentsChecked.Add(element, _currentData);
        }

        public static void SetForemostElement(UIElement element)
        {
            _foremostElement = element;
        }

        public static void SetJustPressed(UIElement element, MouseInput button)
        {
            _currentButton = button;
            _currentClicked = element;

            _currentClicked.HandleMousePressed(button);
        }

        public static bool IsSomethingElseClicked(UIElement me, out UIElement clicked)
        {
            clicked = _currentClicked;
            bool isJustPressed = GameInput.IsJustPressed(MouseInput.Left, true);
            return isJustPressed && _currentClicked != me;
        }

        public static void ForceInputCheck(UIElement element)
        {
            _forcedElements.Add(element);
        }

        public static void UpdateInput()
        {
            // nope
            if (_currentData == null) return;

            RunInputCheck();
            _dataUpdated.Add(_currentData);
            _currentData = null;

            foreach (UIElement el in _forcedElements)
            {
                SetCurrentInputParent(el);
                SetForemostElement(el);
                RunInputCheck();
                _dataUpdated.Add(_currentData);
                _currentData = null;
            }

            _forcedElements.Clear();
        }

        private static void RunInputCheck()
        {
            bool changed = _foremostElement != _currentData.LastForemost;
            if (changed) 
                _currentData.LastForemost?.HandleMouseNotOver(changed);
            _foremostElement?.HandleMouseOver(changed);

            if (_currentClicked != null && GameInput.IsJustReleased(_currentButton, true))
            {
                _currentData.LastClicked = _currentClicked;
                _currentClicked = null;
                _currentData.LastClicked?.HandleMouseUp(_currentButton);
            }

            _currentClicked?.HandleMouseDown(_currentButton);
            _currentData.LastForemost = _foremostElement;

            // if foremost isnt null, mouse is over a ui element.
            MouseOverUIElement |= _foremostElement != null;
            
            _foremostElement = null;
        }

        public static void ClearUnupdatedParents()
        {
            foreach (var kvp in _parentsChecked)
            {
                if (!_dataUpdated.Remove(kvp.Value))
                {
                    _parentsChecked.Remove(kvp.Key);
                }
            }
            _dataUpdated.Clear();
        }

        private class InputData
        {
            public UIElement LastForemost { get; set; }
            public UIElement LastClicked { get; set; }
            public MouseInput CurrentButton { get; set; }
        }
    }
}