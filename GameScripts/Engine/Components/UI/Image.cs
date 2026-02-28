using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public class Image : Entity
    {
        public Texture2D Texture;
        public Color Color;
        public Rectangle Rect;

        public Image(Texture2D texture, Vector2 position, Vector2 scale, Color color, float rotation = 0f, bool isStatic = false)
            : base(position, scale, rotation, isStatic)
        {
            Texture = texture;
            Color = color;
            Rect = new Rectangle(GetPosition(), GetSize());
        }
        public Point GetSize() {
            return Transform.Scale.ToPoint() * new Point(Texture.Width, Texture.Height);
        }
        public Point GetPosition() {
            return Transform.Position.ToPoint();
        }
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(Texture, Transform.Position, null, Color,
                Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 0f);
        }
    }
}