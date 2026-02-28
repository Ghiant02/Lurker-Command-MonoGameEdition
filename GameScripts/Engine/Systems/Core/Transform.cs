using Microsoft.Xna.Framework;

namespace GameEngine.Systems
{
    public class Transform
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;

        public Transform()
        {
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Rotation = 0f;
        }
    }
}
