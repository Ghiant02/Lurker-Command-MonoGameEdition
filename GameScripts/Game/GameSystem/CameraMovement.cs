using GameEngine.Components.Core;
using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

public sealed class CameraMovement : Entity
{
    public Camera2D camera;
    public const int speed = 4;

    private Vector2 velocity;
    private readonly Vector2 left = new Vector2(-speed, 0);
    private readonly Vector2 right = new Vector2(speed, 0);
    private readonly Vector2 up = new Vector2(0, speed);
    private readonly Vector2 down = new Vector2(0, -speed);

    public CameraMovement(Camera2D camera, Vector2 startPosition) : base(Vector2.Zero, Vector2.One, 0f, false)
    {
        this.camera = camera;
        MoveCamera(startPosition);
    }

    public override void Update(GameTime gameTime)
    {
        HandleInput();
    }

    public void HandleInput()
    {
        velocity = Vector2.Zero;

        if (InputManager.IsKeyDown(Keys.A)) velocity += left;
        else if (InputManager.IsKeyDown(Keys.D)) velocity += right;

        if (InputManager.IsKeyDown(Keys.W)) velocity += down;
        else if (InputManager.IsKeyDown(Keys.S)) velocity += up;

        if (InputManager.IsKeyDown(Keys.OemPlus)) camera.Zoom += 0.01f;
        else if (InputManager.IsKeyDown(Keys.OemMinus)) camera.Zoom -= 0.01f;

        MoveCamera(camera.Position + velocity);
    }

    public void MoveCamera(Vector2 targetPosition)
    {
        float halfViewWidth = (camera.graphics.Viewport.Width * 0.5f) / camera.Zoom;
        float halfViewHeight = (camera.graphics.Viewport.Height * 0.5f) / camera.Zoom;

        float minX = halfViewWidth;
        float maxX = Field.MapWidth - halfViewWidth;

        float minY = halfViewHeight;
        float maxY = Field.MapHeight - halfViewHeight;

        float finalX = (maxX > minX) ? MathHelper.Clamp(targetPosition.X, minX, maxX) : Field.MapWidth * 0.5f;
        float finalY = (maxY > minY) ? MathHelper.Clamp(targetPosition.Y, minY, maxY) : Field.MapHeight * 0.5f;

        camera.Position = new Vector2(finalX, finalY);
    }
}