using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Maths;

namespace GameCore
{
    public struct ScreenEventArgs
    {
        public int NewWidth { get; private set; }
        public int NewHeight { get; private set; }
        public ScreenEventArgs(int w, int h)
        {
            NewWidth = w;
            NewHeight = h;
        }
    }

    /// <summary>
    /// Manages the GraphicsDeviceManager, screen size and other various graphics/device/window utilities.
    /// </summary>
    public class WindowManager
    {
        protected Game _game;

        private Point? _sizeBeforeFullscreen;

        public event EventHandler<ScreenEventArgs> OnScreenSizeChanged;

        public Point MinimumScreenSize { get; set; }
        public GraphicsDeviceManager GraphicsDeviceManager { get; protected set; }
        public bool BordlessFullScreen { get => !GraphicsDeviceManager.HardwareModeSwitch; set => GraphicsDeviceManager.HardwareModeSwitch = !value; }
        
        // bunch of values you can get
        public GraphicsDevice GraphicsDevice => GraphicsDeviceManager.GraphicsDevice;
        public bool IsFullScreen => GraphicsDeviceManager.IsFullScreen;
        public int ScreenWidth => GraphicsDeviceManager.PreferredBackBufferWidth;
        public int ScreenHeight => GraphicsDeviceManager.PreferredBackBufferHeight;
        public Vector2 ScreenSizeVector => new Vector2(ScreenWidth, ScreenHeight);
        public Rectangle ScreenSizeRectangle => new Rectangle(0, 0, ScreenWidth, ScreenHeight);
        public RectangleF ScreenSizeRectangleF => new RectangleF(0, 0, ScreenWidth, ScreenHeight);
        public int ViewportWidth => GraphicsDeviceManager.GraphicsDevice.Viewport.Width;
        public int ViewportHeight => GraphicsDeviceManager.GraphicsDevice.Viewport.Height;
        public Viewport Viewport => GraphicsDeviceManager.GraphicsDevice.Viewport;

        public WindowManager(Game game, GraphicsProfile profile = GraphicsProfile.HiDef, int screenWidth = 1280, int screenHeight = 720, bool defaultFullscreen = false)
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(game);
            if (GraphicsAdapter.DefaultAdapter.IsProfileSupported(profile))
            {
                GraphicsDeviceManager.GraphicsProfile = profile;
            }
            else
            {
                GraphicsDeviceManager.GraphicsProfile = GraphicsProfile.Reach;
            }
            MinimumScreenSize = new Point(1280, 720);

            if (defaultFullscreen)
            {
                SetFullScreen(defaultFullscreen);
            }
            else
            {
                SetSize(screenWidth, screenHeight, true);
            }

            if (!defaultFullscreen)
            {
                _sizeBeforeFullscreen = new Point(screenWidth, screenHeight);
            }

            _game = game;
            _game.Window.AllowUserResizing = true;
            _game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }

        public void SetFullScreen(bool fullscreen)
        {
            if (IsFullScreen == fullscreen) return;

            GraphicsDeviceManager.IsFullScreen = fullscreen;

            if (fullscreen)
            {
                _sizeBeforeFullscreen = _game.Window.ClientBounds.Size;

                SetSize(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
            }
            else if (_sizeBeforeFullscreen.HasValue)
            {
                SetSize(_sizeBeforeFullscreen.Value.X, _sizeBeforeFullscreen.Value.Y);
            }
            else
            {
                // there was no pre-defined size for what to set the size to so set it to 75%.
                SetSize((int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.75), (int)(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.75));
            }

            GraphicsDeviceManager.ApplyChanges();
        }

        public void SetSize(int x, int y, bool applyChanges = true)
        {
            GraphicsDeviceManager.PreferredBackBufferWidth = Math.Max(x, MinimumScreenSize.X);
            GraphicsDeviceManager.PreferredBackBufferHeight = Math.Max(y, MinimumScreenSize.Y);
            if (applyChanges) GraphicsDeviceManager.ApplyChanges();
        }

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            _game.Window.ClientSizeChanged -= Window_ClientSizeChanged;

            SetSize(_game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height);
            GraphicsDeviceManager.ApplyChanges();

            OnScreenSizeChanged?.Invoke(null, new ScreenEventArgs(_game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height));

            _game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        }
    }
}