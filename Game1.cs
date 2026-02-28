using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.Scenes;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LurkerCommand
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Lurker Command";
            IsMouseVisible = true;

            ConfigManager.Initialize();

            string widthStr = ConfigManager.Get("Width", "1440");
            string heightStr = ConfigManager.Get("Height", "1080");
            string fsStr = ConfigManager.Get("FullScreen", "false");

            _graphics.PreferredBackBufferWidth = int.Parse(widthStr);
            _graphics.PreferredBackBufferHeight = int.Parse(heightStr);

            if (bool.TryParse(fsStr, out bool isFull))
            {
                _graphics.IsFullScreen = isFull;
            }
            else
            {
                _graphics.IsFullScreen = false;
            }

            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            AssetManager.Init(Content);

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