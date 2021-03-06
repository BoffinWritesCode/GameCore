using System;
using System.Collections.Generic;
using System.Text;
using GameCore.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameCore.Input
{
    /// <summary>
    /// Handles all the main input
    /// <br>Register controls for specific actions using this object, access via string key.</br>
    /// <br>TODO: Change to int key? Is that necessary? Return index from <see cref="RegisterControl"/>?</br>
    /// </summary>
    public static class GameInput
    {
        private const float KEY_REPEAT_DELAY = 0.4f; 
        private const float KEY_REPEAT_TIME = 0.03f; 

        public static Dictionary<string, InputBinding> Controls;
        private static Dictionary<Keys, float> _keyTimes;

        public static KeyboardState PreviousKeyState { get; private set; }
        public static KeyboardState CurrentKeyState { get; private set; }
        public static MouseState PreviousMouseState { get; private set; }
        public static MouseState CurrentMouseState { get; private set; }

        public static int DeltaScroll => CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
        public static int DeltaScrollHorizontal => CurrentMouseState.HorizontalScrollWheelValue - PreviousMouseState.HorizontalScrollWheelValue;

        public static bool KeyboardEatenByTextInput { get; set; }
        public static Vector2 MousePosition => new Vector2(CurrentMouseState.X, CurrentMouseState.Y);

        static GameInput()
        {
            Controls = new Dictionary<string, InputBinding>();
            _keyTimes = new Dictionary<Keys, float>();
        }

        /// <summary>
        /// Make sure this is called as early as possible in the Game's main Update method.
        /// </summary>
        public static void UpdateInput()
        {
            // before everything else, clear whether mouse is over UI
            UIInputManager.MouseOverUIElement = false;

            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();
        }

        public static InputBinding RegisterControl(string name, Keys defaultKeyboard)
        {
            Controls[name] = new InputBinding(defaultKeyboard);
            return Controls[name];
        }

        public static InputBinding RegisterControl(string name, MouseInput defaultMouse)
        {
            Controls[name] = new InputBinding(defaultMouse);
            return Controls[name];
        }

        public static void UnregisterControl(string name)
        {
            Controls.Remove(name);
        }

        public static bool IsDown(Keys key)
        {
            if (KeyboardEatenByTextInput) return false;
            return CurrentKeyState.IsKeyDown(key);
        }

        public static bool IsUp(Keys key)
        {
            if (KeyboardEatenByTextInput) return true;
            return CurrentKeyState.IsKeyUp(key);
        }

        public static bool IsJustPressed(Keys key)
        {
            if (KeyboardEatenByTextInput) return false;
            return PreviousKeyState.IsKeyUp(key) && CurrentKeyState.IsKeyDown(key);
        }

        public static bool IsJustReleased(Keys key)
        {
            if (KeyboardEatenByTextInput) return false;
            return CurrentKeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key);
        }

        public static bool IsTextInputPressed(Keys key)
        {
            if (_keyTimes.TryGetValue(key, out float result))
            {
                if (!IsDown(key))
                {
                    _keyTimes.Remove(key);
                    return false;
                }

                _keyTimes[key] = result + Time.RawDeltaTime;
                if (_keyTimes[key] > KEY_REPEAT_DELAY + KEY_REPEAT_TIME)
                {
                    _keyTimes[key] = _keyTimes[key] - KEY_REPEAT_TIME;
                    return true;
                }
            }

            if (IsDown(key))
            {
                _keyTimes.Add(key, 1);
                return true;
            }

            return false;
        }

        public static bool IsDown(MouseInput button, bool ignoreUI = false)
        {
            if (!ignoreUI && UIInputManager.MouseOverUIElement) return false;
            return MouseIsDownInState(button, CurrentMouseState);
        }

        public static bool IsUp(MouseInput button, bool ignoreUI = false)
        {
            if (!ignoreUI && UIInputManager.MouseOverUIElement) return false;
            return !MouseIsDownInState(button, CurrentMouseState);
        }

        public static bool IsJustPressed(MouseInput button, bool ignoreUI = false)
        {
            if (!ignoreUI && UIInputManager.MouseOverUIElement) return false;
            return MouseIsDownInState(button, CurrentMouseState) && !MouseIsDownInState(button, PreviousMouseState);
        }

        public static bool IsJustReleased(MouseInput button, bool ignoreUI = false)
        {
            if (!ignoreUI && UIInputManager.MouseOverUIElement) return false;
            return !MouseIsDownInState(button, CurrentMouseState) && MouseIsDownInState(button, PreviousMouseState);
        }

        /// <summary>
        /// Use IsDown(MouseInput button) for normal checks.
        /// </summary>
        public static bool MouseIsDownInState(MouseInput button, MouseState state)
        {
            switch (button)
            {
                default:
                case MouseInput.Left:
                    return state.LeftButton == ButtonState.Pressed;
                case MouseInput.Middle:
                    return state.MiddleButton == ButtonState.Pressed;
                case MouseInput.Right:
                    return state.RightButton == ButtonState.Pressed;
                case MouseInput.MouseButton1:
                    return state.XButton1 == ButtonState.Pressed;
                case MouseInput.MouseButton2:
                    return state.XButton2 == ButtonState.Pressed;
                case MouseInput.ScrollDown:
                    return DeltaScroll < 0;
                case MouseInput.ScrollUp:
                    return DeltaScroll > 0;
            }
        }
    }
}
