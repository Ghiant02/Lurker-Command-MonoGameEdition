namespace GameEngine.Systems {
    public interface IPoolable
    {
        bool IsInPool { get; set; }
        void OnSpawn();
        void OnDespawn();
    }
}