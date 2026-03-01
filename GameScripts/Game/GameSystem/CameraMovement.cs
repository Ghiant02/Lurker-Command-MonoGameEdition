using GameEngine.Components.Core;
using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.MapSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public sealed class CameraMovement : Entity
{
    public Camera2D camera;
    public const int speed = 5;
    public const float zoomFactor = 0.1f;

    private Vector2 velocity;
    private readonly Dictionary<Keys, Vector2> _directions = new() {
    { Keys.A, new Vector2(-1, 0) },
    { Keys.Left, new Vector2(-1, 0) },
    { Keys.D, new Vector2(1, 0) },
    { Keys.Right, new Vector2(1, 0) },
    { Keys.W, new Vector2(0, -1) },
    { Keys.Up, new Vector2(0, -1) },
    { Keys.S, new Vector2(0, 1) },
    { Keys.Down, new Vector2(0, 1) },
    };
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
        Vector2 direction = Vector2.Zero;

        foreach (var (key, vector) in _directions)
        {
            if (InputManager.IsKeyDown(key)) direction += vector;
        }

        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            velocity = direction * speed;
        }
        else
        {
            velocity = Vector2.Zero;
        }

        int scroll = InputManager.ScrollDelta;
        if (scroll != 0)
        {
            camera.Zoom = MathHelper.Clamp(camera.Zoom + (scroll > 0 ? zoomFactor : -zoomFactor), 0.1f, 5f);
        }

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