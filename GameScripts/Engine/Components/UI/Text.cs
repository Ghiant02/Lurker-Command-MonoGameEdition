using GameEngine.Services;
using GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Components.UI
{
    public class Text : Entity, IRect
    {
        private string _text;
        private Vector2 _size;
        private Vector2 _origin;

        public Color Color;
        public SpriteFont Font;

        public string text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                UpdateSize();
            }
        }

        public Vector2 Origin { get => _origin; set => _origin = value; }

        public Text(SpriteFont font, string text, Vector2 position, Color color = default, bool isStatic = false)
            : base(position, Vector2.One, 0f, isStatic)
        {
            Font = font;
            Color = color;
            this.text = text;
            _origin = Vector2.Zero;
        }

        private void UpdateSize()
        {
            if (string.IsNullOrEmpty(_text))
            {
                _size = Vector2.Zero;
                return;
            }
            _size = Font.MeasureString(_text);
        }

        public void CenterOrigin() => _origin = _size * 0.5f;

        public Rectangle GetBounds()
        {
            return new Rectangle(
                (int)(Transform.LocalPosition.X - _origin.X * Transform.LocalScale.X),
                (int)(Transform.LocalPosition.Y - _origin.Y * Transform.LocalScale.Y),
                (int)(_size.X * Transform.LocalScale.X),
                (int)(_size.Y * Transform.LocalScale.Y)
            );
        }

        public override void Draw(GameTime gameTime, SpriteBatch sb)
        {
            sb.DrawString(Font, _text, Transform.LocalPosition, Color,
                Transform.LocalRotation, _origin, Transform.LocalScale, SpriteEffects.None, 0f);
        }
    }
}