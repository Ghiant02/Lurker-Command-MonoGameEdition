namespace GameEngine.Systems
{
    public abstract class GameObject
    {
        public bool IsActive = true;
        public readonly Transform Transform = new();
        public abstract void Init();
    }
}
