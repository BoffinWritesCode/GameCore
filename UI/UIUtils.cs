using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Animation;
using GameCore.Input;
using GameCore.Graphics;
using Microsoft.Xna.Framework.Input;
using GameCore.Maths;

namespace GameCore.UI
{
    public static class UIUtils
    {
        public static void SetupMouseInteractionColor(UIElement element, MouseInput button, float highlightedMult, float pressedMult, float animTime, Easing easing, params IColorable[] colourables)
        {
            for (int i = 0; i < colourables.Length; i++)
            {
                // normal state
                ColorableDriver clrDriver = new ColorableDriver(colourables[i], true);
                float a = colourables[i].Color.ToVector4().W;
                if (a >= 1f)
                {
                    Vector3 color = colourables[i].Color.ToVector3();
                    // highlighted
                    clrDriver.AddState(new Color(color * highlightedMult));
                    // pressed
                    clrDriver.AddState(new Color(color * pressedMult));
                }
                else
                {
                    Vector4 color = colourables[i].Color.ToVector4();
                    // highlighted
                    clrDriver.AddState(new Color(color * highlightedMult));
                    // pressed
                    clrDriver.AddState(new Color(color * pressedMult));
                }

                element.Animator.AddDriver(clrDriver);
            }
            element.Animator.CanTransitionWhenTransitioning = true;

            element.OnMouseEnter += (UIElement e) => e.Animator.TransitionToState(1, animTime, easing);
            element.OnMouseExit += (UIElement e) => e.Animator.TransitionToState(0, animTime, easing);
            element.OnMousePressed += (UIElement e, MouseInput b) => { if (b == button) e.Animator.TransitionToState(2, animTime, easing); };
            element.OnMouseReleased += (UIElement e, MouseInput _) => 
            {
                if (e.CalculateRect().Contains(GameInput.MousePosition))
                    e.Animator.TransitionToState(1, animTime, easing);
                else
                    e.Animator.TransitionToState(0, animTime, easing);
            };
        }

        public static void SetupMouseInteractionColorIfCondition(UIElement element, MouseInput button, float highlightedMult, float pressedMult, float animTime, Easing easing, Func<bool> condition, params IColorable[] colourables)
        {
            for (int i = 0; i < colourables.Length; i++)
            {
                // normal state
                ColorableDriver clrDriver = new ColorableDriver(colourables[i], true);
                
                float a = colourables[i].Color.ToVector4().W;
                if (a >= 1f)
                {
                    Vector3 color = colourables[i].Color.ToVector3();
                    // highlighted
                    clrDriver.AddState(new Color(color * highlightedMult));
                    // pressed
                    clrDriver.AddState(new Color(color * pressedMult));
                }
                else
                {
                    Vector4 color = colourables[i].Color.ToVector4();
                    // highlighted
                    clrDriver.AddState(new Color(color * highlightedMult));
                    // pressed
                    clrDriver.AddState(new Color(color * pressedMult));
                }

                element.Animator.AddDriver(clrDriver);
            }
            element.Animator.CanTransitionWhenTransitioning = true;

            element.OnMouseEnter += (UIElement e) => { if (condition()) e.Animator.TransitionToState(1, animTime, easing); };
            element.OnMouseExit += (UIElement e) => { if (condition()) e.Animator.TransitionToState(0, animTime, easing); };
            element.OnMousePressed += (UIElement e, MouseInput b) => { if (condition() && b == button) e.Animator.TransitionToState(2, animTime, easing); };
            element.OnMouseReleased += (UIElement e, MouseInput _) => 
            {
                if (!condition()) return;
                if (e.CalculateRect().Contains(GameInput.MousePosition))
                    e.Animator.TransitionToState(1, animTime, easing);
                else
                    e.Animator.TransitionToState(0, animTime, easing);
            };
        }

        public static void SetupCursorOnHover(UIElement element, MouseCursor hoverCursor)
        {
            element.OnMouseEnter += (UIElement e) => Mouse.SetCursor(hoverCursor);
            element.OnMouseExit += (UIElement e) => Mouse.SetCursor(MouseCursor.Arrow);
        }

        public static void SetupCursorOnHoverWithCondition(UIElement element, MouseCursor hoverCursor, Func<bool> condition)
        {
            element.OnMouseEnter += (UIElement e) => { if (condition()) Mouse.SetCursor(hoverCursor); else Mouse.SetCursor(MouseCursor.Arrow); };
            element.OnMouseExit += (UIElement e) => { Mouse.SetCursor(MouseCursor.Arrow); };
        }

        public static void DrawNineSlicePanelStretched(RectangleF rect, ISprite sprite, Color color, int borderTop, int borderRight, int borderBottom, int borderLeft, bool drawMiddle = true)
        {
            TextureInfo info = sprite.GetTextureInfo();
            Rectangle source = info.GetSourceNoNull();

            DrawStretched(rect, info.Texture, color, source, borderTop, borderRight, borderBottom, borderLeft, drawMiddle);
            //else DrawRepeated(rect, info.Texture, source);

            // top left
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(rect.TopLeft, new Vector2(borderLeft, borderTop)), new Rectangle(source.X, source.Y, borderLeft, borderTop), color);
            // top right
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Right - borderRight, rect.Top), new Vector2(borderRight, borderTop)), new Rectangle(source.Right - borderRight, source.Y, borderRight, borderTop), color);
            // bottom left
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Left, rect.Bottom - borderBottom), new Vector2(borderLeft, borderBottom)), new Rectangle(source.X, source.Bottom - borderBottom, borderLeft, borderBottom), color);
            // bottom right
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Right - borderRight, rect.Bottom - borderBottom), new Vector2(borderRight, borderBottom)), new Rectangle(source.Right - borderRight, source.Bottom - borderBottom, borderRight, borderBottom), color);
        }

        public static void DrawNineSlicePanelRepeated(RectangleF rect, ISprite sprite, Color color, int borderTop, int borderRight, int borderBottom, int borderLeft, bool drawMiddle = true)
        {
            TextureInfo info = sprite.GetTextureInfo();
            Rectangle source = info.GetSourceNoNull();

            DrawRepeated(rect, info.Texture, color, source, borderTop, borderRight, borderBottom, borderLeft, drawMiddle);

            // top left
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(rect.TopLeft, new Vector2(borderLeft, borderTop)), new Rectangle(source.X, source.Y, borderLeft, borderTop), color);
            // top right
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Right - borderRight, rect.Top), new Vector2(borderRight, borderTop)), new Rectangle(source.Right - borderRight, source.Y, borderRight, borderTop), color);
            // bottom left
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Left, rect.Bottom - borderBottom), new Vector2(borderLeft, borderBottom)), new Rectangle(source.X, source.Bottom - borderBottom, borderLeft, borderBottom), color);
            // bottom right
            Engine.SpriteBatch.Draw(info.Texture, new RectangleF(new Vector2(rect.Right - borderRight, rect.Bottom - borderBottom), new Vector2(borderRight, borderBottom)), new Rectangle(source.Right - borderRight, source.Bottom - borderBottom, borderRight, borderBottom), color);
        }

        private static void DrawStretched(RectangleF rect, Texture2D texture, Color color, Rectangle source, int borderTop, int borderRight, int borderBottom, int borderLeft, bool doFill)
        {
            // middle
            if (doFill)
                Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Left + borderLeft, rect.Top + borderTop, rect.Width - borderLeft - borderRight, rect.Height - borderTop - borderBottom), new Rectangle(source.X + borderLeft, source.Y + borderTop, source.Width - borderLeft - borderRight, source.Height - borderTop - borderBottom), color);

            // top
            Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Left + borderLeft, rect.Top, rect.Width - borderLeft - borderRight, borderTop), new Rectangle(source.X + borderLeft, source.Y, source.Width - borderLeft - borderRight, borderTop), color);
            // bottom
            Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Left + borderLeft, rect.Bottom - borderBottom, rect.Width - borderLeft -  borderRight, borderBottom), new Rectangle(source.X + borderLeft, source.Bottom - borderBottom, source.Width - borderLeft - borderRight, borderBottom), color);
            // left
            Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Left, rect.Top + borderTop, borderLeft, rect.Height - borderTop - borderBottom), new Rectangle(source.X, source.Y + borderTop, borderLeft, source.Height - borderTop - borderBottom), color);
            // right
            Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Right - borderRight, rect.Top + borderTop, borderRight, rect.Height - borderTop - borderBottom), new Rectangle(source.Right - borderRight, source.Y + borderTop, borderRight, source.Height - borderTop - borderBottom), color);
        }

        private static void DrawRepeated(RectangleF rect, Texture2D texture, Color color, Rectangle source, int borderTop, int borderRight, int borderBottom, int borderLeft, bool doFill)
        {
            Rectangle centerSrc = new Rectangle(source.Left + borderLeft, source.Top + borderTop, source.Width - borderLeft - borderRight, source.Height - borderTop - borderBottom);

            if (doFill)
            {
                Vector2 available = rect.Size - new Vector2(borderLeft + borderRight, borderTop + borderBottom);
                Vector2 current = available;
                for (float x = rect.Left + borderLeft; x < rect.Right - borderRight; x += centerSrc.Width)
                {
                    for (float y = rect.Top + borderTop; y < rect.Bottom - borderBottom; y += centerSrc.Height)
                    {
                        Vector2 size = new Vector2(Math.Min(centerSrc.Width, current.X), Math.Min(centerSrc.Height, current.Y));
                        Engine.SpriteBatch.Draw(texture, new RectangleF(x, y, size.X, size.Y), new Rectangle(centerSrc.X, centerSrc.Y, (int)size.X, (int)size.Y), color);
                        current.Y -= centerSrc.Height;
                    }
                    current.X -= centerSrc.Width;
                    current.Y = available.Y;
                }
            }

            float availableX = rect.Width - borderLeft - borderRight;
            for (float x = rect.Left + borderLeft; x < rect.Right - borderRight; x += centerSrc.Width)
            {
                float w = MathF.Min(centerSrc.Width, availableX);

                // top
                Engine.SpriteBatch.Draw(texture, new RectangleF(x, rect.Top, w, borderTop), new Rectangle(centerSrc.X, source.Y, (int)w, borderTop), color);
                // bottom
                Engine.SpriteBatch.Draw(texture, new RectangleF(x, rect.Bottom - borderBottom, w, borderBottom), new Rectangle(centerSrc.X, source.Bottom - borderBottom, (int)w, borderBottom), color);

                availableX -= centerSrc.Width;
            }

            float availableY = rect.Height - borderTop - borderBottom;
            for (float y = rect.Top + borderTop; y < rect.Bottom - borderBottom; y += centerSrc.Height)
            {
                float h = MathF.Min(centerSrc.Height, availableY);

                // left
                Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Left, y, borderLeft, h), new Rectangle(source.X, centerSrc.Y, borderLeft, (int)h), color);
                // right
                Engine.SpriteBatch.Draw(texture, new RectangleF(rect.Right - borderRight, y, borderRight, h), new Rectangle(source.Right - borderRight, centerSrc.Y, borderRight, (int)h), color);

                availableY -= centerSrc.Height;
            }
        }
    }
}