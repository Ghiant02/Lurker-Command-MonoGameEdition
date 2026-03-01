using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.Scenes;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace LurkerCommand
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Keys FullScreenKey;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            ConfigManager.Initialize();
            AssetManager.Init(Content);

            _graphics.PreferredBackBufferWidth = int.Parse(ConfigManager.Get("Width", "1440"));
            _graphics.PreferredBackBufferHeight = int.Parse(ConfigManager.Get("Height", "1080"));
            _graphics.IsFullScreen = bool.Parse(ConfigManager.Get("FullScreen", "false"));
            Window.AllowAltF4 = bool.Parse(ConfigManager.Get("AltF4", "true"));
            Window.AllowUserResizing = bool.Parse(ConfigManager.Get("AllowResizing", "false"));
            Window.Title = ConfigManager.Get("WindowTitle", "Lurker Command");
            if (!Enum.TryParse(ConfigManager.Get("FullScreenKey", "F11"), out FullScreenKey))
            {
                FullScreenKey = Keys.F11;
            }
            _graphics.HardwareModeSwitch = false;

            _graphics.ApplyChanges();

            GameScene game = new GameScene(GraphicsDevice);
            SceneManager.SetScene(game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            if(InputManager.IsKeyPressed(FullScreenKey)) {
                _graphics.ToggleFullScreen();
            }
            SceneManager.CurrentScene?.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SceneManager.CurrentScene?.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}