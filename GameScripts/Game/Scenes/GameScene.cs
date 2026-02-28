using GameEngine.Components.Core;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.Scenes
{
    public sealed class GameScene : Scene {
        private GraphicsDevice device;
        public GameScene(GraphicsDevice device) {
            this.device = device;
        }
        public override void Load() {
            Camera2D cm = new Camera2D(device);
            SetCamera(cm);
            CameraMovement cmMovement = new CameraMovement(cm);
            Add(cmMovement);
            Field.SetMap(this);
        }
    }
}