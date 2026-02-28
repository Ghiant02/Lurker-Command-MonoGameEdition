using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public class Text : Entity
    {
        private string _content;
        private Vector2 _size;
        private Vector2 _origin;

        public Color Color;
        public SpriteFont Font;

        public string Content
        {
            get => _content;
            set
            {
                if (_content == value) return;
                _content = value;
                UpdateSize();
            }
        }

        public Vector2 Origin { get => _origin; set => _origin = value; }

        public Text(SpriteFont font, string text, Vector2 position, Color color, bool isStatic = false)
            : base(position, Vector2.One, 0f, isStatic)
        {
            Font = font;
            Color = color;
            Content = text;
            _origin = Vector2.Zero;
        }

        private void UpdateSize()
        {
            if (string.IsNullOrEmpty(_content))
            {
                _size = Vector2.Zero;
                return;
            }
            _size = Font.MeasureString(_content);
        }

        public void CenterOrigin() => _origin = _size * 0.5f;

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)(Transform.Position.X - _origin.X * Transform.Scale.X),
                (int)(Transform.Position.Y - _origin.Y * Transform.Scale.Y),
                (int)(_size.X * Transform.Scale.X),
                (int)(_size.Y * Transform.Scale.Y)
            );
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.DrawString(Font, _content, Transform.Position, Color,
                Transform.Rotation, _origin, Transform.Scale, SpriteEffects.None, 0f);
        }
    }
}