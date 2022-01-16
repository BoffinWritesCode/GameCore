using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GameCore.Input;
using GameCore.Localisation;
using GameCore.Scenes;
using Microsoft.Xna.Framework.Content;
using GameCore.UI;
using GameCore.Graphics;

namespace GameCore
{
    /// <summary>
    /// The base engine class for the game.
    /// </summary>
    public class Engine : Game
    {
        public static Engine Instance { get; protected set; }
        public static WindowManager WindowManager { get; protected set;}
        public static SpriteBatch SpriteBatch { get; protected set; }
        public static Random Random { get; protected set; }
        public static LocalisationSet Localisation { get; protected set; }
        public static SceneManager Scenes { get; protected set; }
        public static ContentManager ContentManager => Instance.Content;
        public static GraphicsDeviceManager GraphicsDeviceManager => WindowManager.GraphicsDeviceManager;

        public Engine(bool mouseVisible = true)
        {
            Random = new Random();
            Instance = this;
            
            Scenes = new SceneManager();
            IsMouseVisible = mouseVisible;

            Content.RootDirectory = "Content";
        
            WindowManager = new WindowManager(this, GraphicsProfile.HiDef);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            GameInput.UpdateInput();

            Scenes.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            Time.UpdateDraw(gameTime);

            GraphicsDevice.Clear(Color.Black);

            Scenes.Draw();
        }
    }
}