using GameEngine.Components.Core;
using GameEngine.Services;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LurkerCommand.GameSystem
{
    public sealed class CameraMovement : Entity {
        public Camera2D camera;
        public const int speed = 3;
        private readonly Vector2 left = new Vector2(-speed, 0);
        private readonly Vector2 right = new Vector2(speed, 0);
        private readonly Vector2 up = new Vector2(0, speed);
        private readonly Vector2 down = new Vector2(0, -speed);
        public CameraMovement(Camera2D camera) : base(Vector2.Zero, Vector2.One, 0f, false) {
            this.camera = camera;
        }
        public override void Update(GameTime gameTime)
        {
            Vector2 velocity = Vector2.Zero;

            if (InputManager.IsKeyDown(Keys.A)) velocity += left;
            else if (InputManager.IsKeyDown(Keys.D)) velocity += right;

            if (InputManager.IsKeyDown(Keys.W)) velocity += down;
            else if (InputManager.IsKeyDown(Keys.S)) velocity += up;

            camera.Position += velocity;
        }
    }
}
