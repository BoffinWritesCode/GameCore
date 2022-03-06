using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore.UI.Elements
{
    /// <summary>
    /// The base for some UI. Handles the spritebatch begin/end methods within it, applies settings etc.
    /// <br>To be used in IScene objects to correctly determine the area their screen parent takes up.</br>
    /// </summary>
    public class UISceneBase : UIElement, IDisposable
    {
        public ISubScene Scene { get; set; }
        public bool UsesMouseSystem { get; set; } = true;

        private Rectangle _prevScreenArea;

        public UISceneBase(UITreeSettings settings, IScene scene) : base()
        {
            if (scene != null && scene is ISubScene subScene) Scene = subScene;

            Settings = settings;
            Rect.FillParent();
            ForceUpdateSettings();

            Engine.WindowManager.OnScreenSizeChanged += ScreenSizeChanged;
        }

        private void ScreenSizeChanged(object sender, ScreenEventArgs args)
        {
            SetDirtyRect();
        }

        public override void Update()
        {
            if (!Active) return;

            UIInputManager.SetCurrentInputParent(this);

            if (Scene != null && Scene.ScreenArea != _prevScreenArea)
            {
                _prevScreenArea = Scene.ScreenArea;
                SetDirtyRect();
            }

            base.Update();

            UIInputManager.UpdateInput();
        }
        
        public override RectangleF CalculateRect()
        {
            if (!_dirtyRect) return _calculatedRect;

            _dirtyRect = false;

            RectangleF rect = Rect.Calculate(Parent == null ? Scene == null ? Engine.WindowManager.ScreenSizeRectangleF : new RectangleF(Scene.ScreenArea) : Parent.CalculateRect());

            if (Settings != null && Settings.PixelPerfect)
            {
                return _calculatedRect = rect.RoundToInt();
            }
            
            return _calculatedRect = rect;
        }

        public override void Draw()
        {
            if (!Active) return;

            Engine.SpriteBatch.Begin(rasterizerState: ScissorTesting.SetScissorAndGetRasterizer(), samplerState: Settings.SamplerState);

            base.Draw();

            Engine.SpriteBatch.End();
        }

        public override void Dispose()
        {
            Engine.WindowManager.OnScreenSizeChanged -= ScreenSizeChanged;
            base.Dispose();
        }
    }
}