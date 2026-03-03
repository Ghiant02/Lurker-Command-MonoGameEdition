using GameEngine.Components.Core;
using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.Scenes
{
    public sealed class GameScene : Scene
    {
        private readonly GraphicsDevice _device;

        public GameScene(GraphicsDevice device) => _device = device;

        public override void Load()
        {
            Field.SetMap(this);
            TeamManager.Init();

            Camera2D cm = new Camera2D(_device);
            SetCamera(cm);

            CameraMovement cmMovement = new CameraMovement(cm, new Vector2(Field.MapWidth / 2, Field.MapHeight / 2));
            Add(cmMovement);

            Unit unit = new Unit(AssetManager.GetFont("Arial"), new Point(4, 1), 3, TeamManager.CurrentTeam);
            TeamManager.AddUnitToTeam(0, unit);
            Add(unit);
        }

        public override void Update(GameTime gameTime)
        {
            TeamManager.Update(gameTime);
            base.Update(gameTime);
        }
    }
}