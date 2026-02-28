using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public sealed class Text : Entity
    {
        public string Content;
        public Color Color;
        public SpriteFont Font;

        public Text(SpriteFont font, string text, Vector2 position, Color color)
            : base(position, Vector2.One)
        {
            Font = font;
            Content = text;
            Color = color;
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            if (!IsActive) return;
            sb.DrawString(Font, Content, Transform.Position, Color,
                Transform.Rotation, Vector2.Zero, Transform.Scale, SpriteEffects.None, 0f);
        }
    }
}