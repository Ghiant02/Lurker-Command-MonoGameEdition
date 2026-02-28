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

        public Game1() {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "Lurker Command";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            AssetManager.Init(Content);
            ConfigManager.Initialize();

            _graphics.PreferredBackBufferWidth = int.Parse(ConfigManager.Get("Width"));
            _graphics.PreferredBackBufferHeight = int.Parse(ConfigManager.Get("Height"));

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