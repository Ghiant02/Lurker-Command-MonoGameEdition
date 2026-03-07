using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Systems {
    public interface IDraw
    {
        int OrderInLayer { get; set; }
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
