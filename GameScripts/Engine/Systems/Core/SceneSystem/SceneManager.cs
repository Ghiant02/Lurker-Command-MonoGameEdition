namespace GameEngine.Systems {
    public static class SceneManager
    {
        public static Scene CurrentScene { get; private set; }
        public static void SetScene(Scene scene)
        {
            CurrentScene?.Dispose();
            scene.Load();
            CurrentScene = scene;
        }
        public static void Add(GameObject gameObject) => CurrentScene?.Add(gameObject);
        public static void Remove(GameObject gameObject) => CurrentScene?.Remove(gameObject);
    }
}