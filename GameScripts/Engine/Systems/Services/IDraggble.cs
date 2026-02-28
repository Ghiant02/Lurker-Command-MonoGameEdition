using Microsoft.Xna.Framework;

namespace GameEngine.Services {
    public interface IDraggable
    {
        void OnDragStart();
        void OnDragUpdate(Vector2 position);
        void OnDragEnd();
    }
}