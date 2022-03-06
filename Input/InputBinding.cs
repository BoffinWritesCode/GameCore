using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameCore.Input
{
    public enum MouseInput
    {
        Left,
        Middle,
        Right,
        ScrollUp,
        ScrollDown,
        MouseButton1,
        MouseButton2
    }
    
    public class InputBinding
    {
        private bool _isMouse;
        private int _assignedValueKeyboard;

        public InputBinding(Keys key)
        {
            _assignedValueKeyboard = (int)key;
            _isMouse = false;
        }

        public InputBinding(MouseInput mouse)
        {
            _assignedValueKeyboard = (int)mouse;
            _isMouse = true;
        }

        public bool IsDown()
        {
            return _isMouse ? IsDown(GameInput.CurrentMouseState) : IsDown(GameInput.CurrentKeyState);
        }

        internal bool IsDown(KeyboardState keyboard)
        {
            return keyboard.IsKeyDown((Keys)_assignedValueKeyboard);
        }

        internal bool IsDown(MouseState mouse)
        {
            MouseInput button = (MouseInput)_assignedValueKeyboard;
            switch (button)
            {
                default:
                case MouseInput.Left:
                    return mouse.LeftButton == ButtonState.Pressed;
                case MouseInput.Middle:
                    return mouse.MiddleButton == ButtonState.Pressed;
                case MouseInput.Right:
                    return mouse.RightButton == ButtonState.Pressed;
                case MouseInput.MouseButton1:
                    return mouse.XButton1 == ButtonState.Pressed;
                case MouseInput.MouseButton2:
                    return mouse.XButton2 == ButtonState.Pressed;
                case MouseInput.ScrollDown:
                    return GameInput.DeltaScroll < 0;
                case MouseInput.ScrollUp:
                    return GameInput.DeltaScroll > 0;
            }
        }

        public bool IsUp()
        {
            return _isMouse ? IsUp(GameInput.CurrentMouseState) : IsUp(GameInput.CurrentKeyState);
        }

        internal bool IsUp(KeyboardState keyboard)
        {
            return keyboard.IsKeyUp((Keys)_assignedValueKeyboard);
        }

        internal bool IsUp(MouseState mouse)
        {
            MouseInput button = (MouseInput)_assignedValueKeyboard;
            switch (button)
            {
                default:
                case MouseInput.Left:
                    return mouse.LeftButton == ButtonState.Released;
                case MouseInput.Middle:
                    return mouse.MiddleButton == ButtonState.Released;
                case MouseInput.Right:
                    return mouse.RightButton == ButtonState.Released;
                case MouseInput.MouseButton1:
                    return mouse.XButton1 == ButtonState.Released;
                case MouseInput.MouseButton2:
                    return mouse.XButton2 == ButtonState.Released;
                case MouseInput.ScrollDown:
                    return GameInput.DeltaScroll > 0;
                case MouseInput.ScrollUp:
                    return GameInput.DeltaScroll < 0;
            }
        }

        public bool IsJustPressed()
        {
            return _isMouse ? IsJustPressed(GameInput.PreviousMouseState, GameInput.CurrentMouseState) : IsJustPressed(GameInput.PreviousKeyState, GameInput.CurrentKeyState);
        }

        internal bool IsJustPressed(KeyboardState old, KeyboardState current)
        {
            return IsUp(old) && IsDown(current);
        }

        internal bool IsJustPressed(MouseState old, MouseState current)
        {
            return IsUp(old) && IsDown(current);
        }

        public bool IsJustReleased()
        {
            return _isMouse ? IsJustReleased(GameInput.PreviousMouseState, GameInput.CurrentMouseState) : IsJustReleased(GameInput.PreviousKeyState, GameInput.CurrentKeyState);
        }

        internal bool IsJustReleased(KeyboardState old, KeyboardState current)
        {
            return IsDown(old) && IsUp(current);
        }

        internal bool IsJustReleased(MouseState old, MouseState current)
        {
            return IsDown(old) && IsUp(current);
        }

        public float GetPressValue()
        {
            return GetPressValue(GameInput.CurrentKeyState);
        }

        internal float GetPressValue(KeyboardState keyboard)
        {
            return IsDown(keyboard) ? 1f : 0f;
        }

        internal float GetPressValue(MouseState mouse)
        {
            MouseInput myValue = (MouseInput)_assignedValueKeyboard;
            switch (myValue)
            {
                case MouseInput.ScrollDown:
                case MouseInput.ScrollUp:
                    return GameInput.DeltaScroll;
                default:
                    return IsDown(mouse) ? 1f : 0f;
            }
        }
    }
}
