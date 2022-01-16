using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore
{
    public static class ScissorTesting
    {
        private static readonly RasterizerState ScissorTestEnabled = new RasterizerState() { ScissorTestEnable = true };

        private static Stack<Rectangle> _scissorStack = new Stack<Rectangle>();

        public static Rectangle CurrentScreenArea
        {
            get
            {
                if (Engine.Scenes.CurrentDrawingScene is ISubScene sub)
                {
                    return _scissorStack.Count == 0 ? sub.ScreenArea : _scissorStack.Peek();
                }
                return _scissorStack.Count == 0 ? Engine.WindowManager.ScreenSizeRectangle : _scissorStack.Peek();
            }
        }

        public static RectangleF CurrentScreenAreaFloat => new RectangleF(CurrentScreenArea);

        public static void PushScissor(Rectangle scissor) 
        {
            if (_scissorStack.Count == 0)
            {
                _scissorStack.Push(scissor);
                return;
            }

            Rectangle intersection = Rectangle.Intersect(_scissorStack.Peek(), scissor);
            _scissorStack.Push(intersection);
        }

        public static bool PopScissor() => _scissorStack.TryPop(out _);

        public static RasterizerState SetScissorAndGetRasterizer()
        {
            if (_scissorStack.Count == 0) 
            {
                Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = Rectangle.Empty;
                return RasterizerState.CullCounterClockwise;
            }

            // peek and return
            Engine.SpriteBatch.GraphicsDevice.ScissorRectangle = _scissorStack.Peek();
            return ScissorTestEnabled;
        } 
    }
}