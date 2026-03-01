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

        private readonly List<GameObject> _toAdd = new(128);
        private readonly List<GameObject> _toRemove = new(128);

        private Camera2D camera;

        private bool _needsSort;

        public uint ID;
        public void SetCamera(Camera2D camera) {
            this.camera = camera;
        }
        public abstract void Load();

        public void Add(GameObject obj)
        {
            _toAdd.Add(obj);
            _needsSort = true;
        }
        /*private IDraggable _currentDragged;
        private Vector2 _dragOffset;

        private Entity _draggedEntity;

        private void HandleInput()
        {
            Vector2 mouseWorld = camera.ScreenToWorld(InputManager.MousePosition);

            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
            {
                for (int i = _drawables.Count - 1; i >= 0; i--)
                {
                    if (_drawables[i] is Entity ent && ent.Bounds.Contains(mouseWorld))
                    {
                        if (ent is IClickable p) p.OnPointerDown();

                        if (ent is IDraggable d)
                        {
                            _draggedEntity = ent;
                            _dragOffset = ent.Transform.Position - mouseWorld;
                            d.OnDragStart();
                        }

                        break;
                    }
                }
            }
            else if (_draggedEntity != null && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (_draggedEntity is IDraggable d)
                {
                    d.OnDragUpdate(mouseWorld + _dragOffset);
                }
            }
            else if (_draggedEntity != null)
            {
                if (_draggedEntity is IDraggable d) d.OnDragEnd();
                _draggedEntity = null;
            }
        }*/
        public void Remove(GameObject obj) => _toRemove.Add(obj);

        public void Update(GameTime gameTime)
        {
            SyncCollections();

            Span<IUpdate> updatables = CollectionsMarshal.AsSpan(_updatables);
            for (int i = 0; i < updatables.Length; i++)
            {
                updatables[i].Update(gameTime);
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

            Span<IDraw> drawables = CollectionsMarshal.AsSpan(_drawables);
            for (int i = 0; i < drawables.Length; i++)
            {
                drawables[i].Draw(gameTime, spriteBatch);
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

                    if (obj is Entity ent)
                    {
                        _updatables.Remove(ent);
                        _drawables.Remove(ent);
                    }
                    else
                    {
                        if (obj is IUpdate u) _updatables.Remove(u);
                        if (obj is IDraw d) _drawables.Remove(d);
                    }
                }
                _toRemove.Clear();
            }

            if (_toAdd.Count > 0)
            {
                for (int i = 0; i < _toAdd.Count; i++)
                {
                    var obj = _toAdd[i];

                    if (obj is Entity ent)
                    {
                        if (!_updatables.Contains(ent)) _updatables.Add(ent);
                        if (!_drawables.Contains(ent)) _drawables.Add(ent);
                    }
                    else
                    {
                        if (obj is IUpdate u && !_updatables.Contains(u)) _updatables.Add(u);
                        if (obj is IDraw d && !_drawables.Contains(d)) _drawables.Add(d);
                    }
                }
                _toAdd.Clear();
            }
        }
    }
}