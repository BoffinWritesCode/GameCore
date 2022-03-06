using System;
using System.Collections.Generic;
using System.Text;
using GameCore.Maths;
using GameCore.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameCore.Utility.Camera
{
    public class Camera2D
    {
        /// <summary>
        /// The area on the screen that the camera displays to. Will be set automatically by the camera if UseFullWindow is true.
        /// </summary>
        public Rectangle ScreenArea { get; set; }
        /// <summary>
        /// Whether the camera displays using the full size of the screen. This is true by default.
        /// </summary>
        public bool UseFullWindow { get; set; } = true;

        public bool PixelPerfectPosition { get; set; }

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; } = 1f;

        /// <summary>
        /// Camera modifiers modify the camera itself, and where it is and it's values.
        /// </summary>
        public ObjectRegister<IClassModifier<Camera2D>> CameraModifiers { get; }

        /// <summary>
        /// 
        /// Value modifiers modify the values output by the camera, and can be used for temporary changes like camera shake.
        /// </summary>
        public ObjectRegister<IValueModifier<CameraValues>> ValueModifiers { get; }

        public Camera2D()
        {
            CameraModifiers = new ObjectRegister<IClassModifier<Camera2D>>();
            ValueModifiers = new ObjectRegister<IValueModifier<CameraValues>>();
        }

        public virtual void Update()
        {
            if (UseFullWindow) ScreenArea = Engine.WindowManager.ScreenSizeRectangle;

            foreach (var mod in CameraModifiers)
            {
                mod.Modify(this);
            }
        }

        /// <summary>
        /// Gets a RectangleF representing what the camera is looking at in world space.
        /// </summary>
        public RectangleF GetWorldArea()
        {
            // Camera relies on screen size
            Vector2 topLeft = GetScreenTopLeft();
            Vector2 size = ScreenArea.Size.ToVector2() / Scale;

            // screen area returned is pixels
            return new RectangleF(topLeft, size);
        }

        private Vector2 GetScreenTopLeft()
        {
            // Camera relies on screen size
            Vector2 screenOver2 = ScreenArea.Size.ToVector2() * 0.5f;
            return Position - (screenOver2 / Scale);
        }

        public Vector2 MouseInWorld() => ScreenPointToWorld(GameInput.MousePosition - ScreenArea.Location.ToVector2());

        public Vector2 ScreenPointToWorld(Vector2 screenPoint)
        {
            RectangleF worldArea = GetWorldArea();
            Vector2 posRelative = screenPoint / ScreenArea.Size();
            return worldArea.TopLeft + worldArea.Size * posRelative;
        }

        public Matrix GetMatrix()
        {
            Vector2 halfScreen = ScreenArea.Size.ToVector2() * 0.5f;

            CameraValues values = new CameraValues(Position, Rotation, Scale);
            foreach (var mod in ValueModifiers) mod.Modify(ref values);

            values.Position -= ScreenArea.Location.ToVector2() / Scale;

            if (PixelPerfectPosition) values.Position = new Vector2((int)values.Position.X, (int)values.Position.Y);

            return
                Matrix.CreateTranslation(-values.Position.X, -values.Position.Y, 0f) *
                Matrix.CreateRotationZ(values.Rotation) *
                Matrix.CreateScale(values.Scale, values.Scale, 1f) *
                Matrix.CreateTranslation(halfScreen.X, halfScreen.Y, 0f);
        }
    }

    public struct CameraValues
    {
        public Vector2 Position;
        public float Rotation;
        public float Scale;

        public CameraValues(Vector2 pos, float rot, float scale)
        {
            Position = pos;
            Rotation = rot;
            Scale = scale;
        }
    }
}
