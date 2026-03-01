using GameEngine.Services;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public class Image : Entity, IRect
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
            return Transform.LocalScale.ToPoint() * new Point(Texture.Width, Texture.Height);
        }
        public Point GetPosition() {
            return Transform.LocalPosition.ToPoint();
        }
        public Rectangle GetBounds() => Rect;
        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(Texture, Transform.LocalPosition, null, Color,
                Transform.LocalRotation, Vector2.Zero, Transform.LocalRotation, SpriteEffects.None, 0f);
        }
    }
}