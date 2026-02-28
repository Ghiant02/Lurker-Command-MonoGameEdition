using GameEngine.Components.Core;
using GameEngine.Systems;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.Scenes
{
    public sealed class Game : Scene {
        private GraphicsDevice device;
        public Game(GraphicsDevice device) {
            this.device = device;
        }
        public override void Load() {
            SetCamera(new Camera2D(device));

        }
    }
}
