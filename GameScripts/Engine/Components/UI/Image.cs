using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public class Image : Entity
    {
        public Texture2D Texture;
        public Color Color;

        public Image(Texture2D texture, Vector2 position, Vector2 scale, Color color)
            : base(position, scale)
        {
            Texture = texture;
            Color = color;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.Draw(Texture, Transform.Position, null, Color,
                Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 0f);
        }
    }
}