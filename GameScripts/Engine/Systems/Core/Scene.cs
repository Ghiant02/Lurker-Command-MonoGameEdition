using GameEngine.Components.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameEngine.Systems
{
    public abstract class Scene : IDisposable
    {
        private readonly List<IUpdate> _updatables = new(1024);
        private readonly List<IDraw> _drawables = new(1024);

        private readonly List<object> _toAdd = new(128);
        private readonly List<object> _toRemove = new(128);

        public Camera2D camera;

        private bool _needsSort;

        public abstract void SetCamera();
        public abstract void Load();

        public void Add(object obj)
        {
            _toAdd.Add(obj);
            _needsSort = true;
        }

        public void Remove(object obj) => _toRemove.Add(obj);

        public void Update(GameTime gameTime)
        {
            SyncCollections();

            int count = _updatables.Count;
            for (int i = 0; i < count; i++)
            {
                _updatables[i].Update(gameTime);
            }
        }
        public virtual void Dispose()
        {
            foreach (var obj in _drawables)
            {
                if (obj is IDisposable d) d.Dispose();
            }

            _updatables.Clear();
            _drawables.Clear();
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_needsSort)
            {
                _drawables.Sort((a, b) => a.OrderInLayer.CompareTo(b.OrderInLayer));
                _needsSort = false;
            }

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null, null, null,
                camera.GetViewMatrix()
            );

            int count = _drawables.Count;
            for (int i = 0; i < count; i++)
            {
                _drawables[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void SyncCollections()
        {
            if (_toAdd.Count == 0 && _toRemove.Count == 0) return;

            if (_toRemove.Count > 0)
            {
                for (int i = 0; i < _toRemove.Count; i++)
                {
                    var obj = _toRemove[i];
                    if (obj is IUpdate u) _updatables.Remove(u);
                    if (obj is IDraw d) _drawables.Remove(d);
                }
                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                for (int i = 0; i < _toAdd.Count; i++)
                {
                    var obj = _toAdd[i];
                    if (obj is IUpdate u) _updatables.Add(u);
                    if (obj is IDraw d) _drawables.Add(d);
                }
                _toAdd.Clear();
            }
        }
    }
}