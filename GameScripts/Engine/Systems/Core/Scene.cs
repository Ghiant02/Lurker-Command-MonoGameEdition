using GameEngine.Components.Core;
using GameEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace GameEngine.Systems
{
    public abstract class Scene : IDisposable
    {
        private readonly List<IUpdate> _updatables = new(1024);
        private readonly List<IDraw> _drawables = new(1024);

        private readonly List<IRect> _inputTargets = new(512);

        private readonly List<GameObject> _toAdd = new(128);
        private readonly List<GameObject> _toRemove = new(128);

        private Camera2D camera;
        private bool _needsSort;

        private IDraggable _currentDragged;
        private Entity _draggedEntity;
        private Vector2 _dragOffset;

        public void Add(GameObject obj) => _toAdd.Add(obj);
        public void Remove(GameObject obj) => _toRemove.Add(obj);

        private void HandleInput()
        {
            Vector2 mouseWorld = camera.ScreenToWorld(InputManager.MousePosition);

            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
            {
                for (int i = _inputTargets.Count - 1; i >= 0; i--)
                {
                    var rectable = _inputTargets[i];
                    if (rectable.GetBounds().Contains(mouseWorld.ToPoint()))
                    {
                        if (rectable is IClickable clickable) clickable.OnPointerDown();

                        if (rectable is IDraggable draggable)
                        {
                            _currentDragged = draggable;
                            _draggedEntity = rectable as Entity;

                            if (_draggedEntity != null)
                            {
                                _dragOffset = _draggedEntity.Transform.LocalPosition - mouseWorld;
                                _currentDragged.OnDragStart();
                            }
                        }
                        break;
                    }
                }
            }
            else if (_draggedEntity != null && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _currentDragged.OnDragUpdate(mouseWorld + _dragOffset);
            }
            else if (_draggedEntity != null)
            {
                _currentDragged.OnDragEnd();
                _draggedEntity = null;
                _currentDragged = null;
            }
        }

        public void Update(GameTime gameTime)
        {
            HandleInput();
            SyncCollections();

            for (int i = 0; i < _updatables.Count; i++)
            {
                _updatables[i].Update(gameTime);
            }
        }

        private void SyncCollections()
        {
            if (_toAdd.Count == 0 && _toRemove.Count == 0) return;

            foreach (var obj in _toRemove)
            {
                if (obj is IUpdate u) _updatables.Remove(u);
                if (obj is IDraw d) _drawables.Remove(d);
                if (obj is IRect r) _inputTargets.Remove(r);
            }
            _toRemove.Clear();

            foreach (var obj in _toAdd)
            {
                if (obj is IUpdate u && !_updatables.Contains(u)) _updatables.Add(u);
                if (obj is IDraw d && !_drawables.Contains(d)) _drawables.Add(d);
                if (obj is IRect r && !_inputTargets.Contains(r)) _inputTargets.Add(r);
            }
            _toAdd.Clear();
            _needsSort = true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_needsSort)
            {
                _drawables.Sort((a, b) => a.OrderInLayer.CompareTo(b.OrderInLayer));
                _needsSort = false;
            }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());

            var drawSpan = CollectionsMarshal.AsSpan(_drawables);
            for (int i = 0; i < drawSpan.Length; i++)
            {
                drawSpan[i].Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
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
        public abstract void Load();
        public void SetCamera(Camera2D cam) => camera = cam;
    }
}