using GameEngine.Components.Core;
using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.Scenes
{
    public sealed class GameScene : Scene {
        private GraphicsDevice device;
        public GameScene(GraphicsDevice device) {
            this.device = device;
        }
        public override void Load() {
            Field.SetMap(this);
            Camera2D cm = new Camera2D(device);

            SetCamera(cm);

            CameraMovement cmMovement = new CameraMovement(cm, new Vector2(-100f, -100f));
            Unit unit = new Unit(AssetManager.GetFont("Arial"), new Point(4, 1), 3, new Team(Color.Red));

            Add(cmMovement);
            Add(unit);
        }
    }
}