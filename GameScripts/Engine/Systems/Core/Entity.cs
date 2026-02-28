using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems
{
    public abstract class Entity : GameObject, IUpdate, IDraw
    {
        public Transform Transform { get; }
        public int OrderInLayer { get; set; }

        protected Entity(Vector2 position, Vector2 scale, float rotation = 0f)
        {
            Transform = new Transform
            {
                Position = position,
                Scale = scale,
                Rotation = rotation
            };
        }

        public virtual void Update(GameTime gameTime) { }
        public abstract void Draw(GameTime gameTime, SpriteBatch sb);

        public void Destroy() => SceneManager.CurrentScene?.Remove(this);
        public override void OnToggled(bool value) {
            if (value) SceneManager.CurrentScene?.Add(this);
            else SceneManager.CurrentScene?.Remove(this);
        }
    }
}