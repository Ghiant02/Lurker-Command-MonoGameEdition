using System;
using System.Collections.Generic;

namespace GameEngine.Systems
{
    public static class PoolManager
    {
        private static readonly Dictionary<Type, Queue<object>> _pools = new();

        public static T Get<T>() where T : class, IPoolable, new()
        {
            var type = typeof(T);

            if (!_pools.ContainsKey(type))
                _pools[type] = new Queue<object>();

            T obj;
            if (_pools[type].Count > 0)
            {
                obj = (T)_pools[type].Dequeue();
            }
            else
            {
                obj = new T();
            }

            obj.IsInPool = false;
            obj.OnSpawn();
            return obj;
        }

        public static void Return<T>(T obj) where T : class, IPoolable
        {
            if (obj.IsInPool) return;

            obj.OnDespawn();
            obj.IsInPool = true;
            _pools[typeof(T)].Enqueue(obj);
        }
    }
}