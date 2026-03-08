using GameEngine.Components.Core;
using GameEngine.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        private readonly List<IDraw> _uiDrawables = new(128);
        private readonly List<IRect> _uiInputTargets = new(128);

        private readonly List<GameObject> _toAdd = new(128);
        private readonly List<GameObject> _uiToAdd = new(64);
        private readonly List<GameObject> _toRemove = new(128);

        public Camera2D camera { get; private set; }
        private bool _needsSort;
        private bool _needsUISort;

        private IDraggable _currentDraggedLBM;
        private Entity _draggedEntityLBM;
        private Vector2 _dragOffsetLBM;
        private bool _isDraggingUI_LBM;

        private IDraggable _currentDraggedRBM;
        private Entity _draggedEntityRBM;
        private Vector2 _dragOffsetRBM;
        private bool _isDraggingUI_RBM;

        public void Add(GameObject obj) => _toAdd.Add(obj);
        public void AddUI(GameObject obj) => _uiToAdd.Add(obj);
        public void Remove(GameObject obj) => _toRemove.Add(obj);

        public virtual void Update(GameTime gameTime)
        {
            HandleInput();
            SyncCollections();

            var updateSpan = CollectionsMarshal.AsSpan(_updatables);
            for (int i = 0; i < updateSpan.Length; i++)
                updateSpan[i].Update(gameTime);
        }

        private void HandleInput()
        {
            if (camera == null) return;

            Vector2 mouseScreen = InputManager.MousePosition;
            Vector2 mouseWorld = camera.ScreenToWorld(InputManager.MousePosition);

            if (InputManager.IsMouseButtonPressed(MouseButton.Left))
            {
                if (!ProcessInputGroup(_uiInputTargets, mouseScreen, out _currentDraggedLBM, out _draggedEntityLBM, true))
                {
                    ProcessInputGroup(_inputTargets, mouseWorld, out _currentDraggedLBM, out _draggedEntityLBM, false);
                }

                if (_currentDraggedLBM != null && _draggedEntityLBM != null)
                {
                    Vector2 origin = _isDraggingUI_LBM ? mouseScreen : mouseWorld;
                    _dragOffsetLBM = _draggedEntityLBM.Transform.LocalPosition - origin;
                    _currentDraggedLBM.OnDragStartLBM();
                }
            }
            else if (_draggedEntityLBM != null && InputManager.IsMouseButtonDown(MouseButton.Left))
            {
                Vector2 currentPos = _isDraggingUI_LBM ? mouseScreen : mouseWorld;
                _currentDraggedLBM.OnDragUpdateLBM(currentPos + _dragOffsetLBM);
            }
            else if (_draggedEntityLBM != null)
            {
                _currentDraggedLBM.OnDragEndLBM();
                _draggedEntityLBM = null;
                _currentDraggedLBM = null;
            }

            if (InputManager.IsMouseButtonPressed(MouseButton.Right))
            {
                if (!ProcessInputGroup(_uiInputTargets, mouseScreen, out _currentDraggedRBM, out _draggedEntityRBM, true, true))
                {
                    ProcessInputGroup(_inputTargets, mouseWorld, out _currentDraggedRBM, out _draggedEntityRBM, false, true);
                }

                if (_currentDraggedRBM != null && _draggedEntityRBM != null)
                {
                    Vector2 origin = _isDraggingUI_RBM ? mouseScreen : mouseWorld;
                    _dragOffsetRBM = _draggedEntityRBM.Transform.LocalPosition - origin;
                    _currentDraggedRBM.OnDragStartRBM();
                }
            }
            else if (_draggedEntityRBM != null && InputManager.IsMouseButtonDown(MouseButton.Right))
            {
                Vector2 currentPos = _isDraggingUI_RBM ? mouseScreen : mouseWorld;
                _currentDraggedRBM.OnDragUpdateRBM(currentPos + _dragOffsetRBM);
            }
            else if (_draggedEntityRBM != null)
            {
                _currentDraggedRBM.OnDragEndRBM();
                _draggedEntityRBM = null;
                _currentDraggedRBM = null;
            }
        }

        private bool ProcessInputGroup(List<IRect> targets, Vector2 mousePos, out IDraggable drag, out Entity entity, bool isUI, bool isRBM = false)
        {
            drag = null;
            entity = null;
            var span = CollectionsMarshal.AsSpan(targets);
            for (int i = span.Length - 1; i >= 0; i--)
            {
                if (span[i].GetBounds().Contains(mousePos.ToPoint()))
                {
                    if (span[i] is IClickable c) c.OnPointerDown();
                    if (span[i] is IDraggable d)
                    {
                        drag = d;
                        entity = span[i] as Entity;
                        if (isRBM) _isDraggingUI_RBM = isUI; else _isDraggingUI_LBM = isUI;
                    }
                    return true;
                }
            }
            return false;
        }

        private void SyncCollections()
        {
            if (_toAdd.Count == 0 && _uiToAdd.Count == 0 && _toRemove.Count == 0) return;

            foreach (var obj in _toRemove)
            {
                if (obj is IUpdate u) _updatables.Remove(u);
                if (obj is IDraw d) { _drawables.Remove(d); _uiDrawables.Remove(d); }
                if (obj is IRect r) { _inputTargets.Remove(r); _uiInputTargets.Remove(r); }
            }
            _toRemove.Clear();

            foreach (var obj in _toAdd)
            {
                if (obj is IUpdate u) _updatables.Add(u);
                if (obj is IDraw d) _drawables.Add(d);
                if (obj is IRect r) _inputTargets.Add(r);
                _needsSort = true;
            }
            _toAdd.Clear();

            foreach (var obj in _uiToAdd)
            {
                if (obj is IUpdate u) _updatables.Add(u);
                if (obj is IDraw d) _uiDrawables.Add(d);
                if (obj is IRect r) _uiInputTargets.Add(r);
                _needsUISort = true;
            }
            _uiToAdd.Clear();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (camera == null) return;

            if (_needsSort) { _drawables.Sort((a, b) => a.OrderInLayer.CompareTo(b.OrderInLayer)); _needsSort = false; }
            if (_needsUISort) { _uiDrawables.Sort((a, b) => a.OrderInLayer.CompareTo(b.OrderInLayer)); _needsUISort = false; }

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetViewMatrix());
            var worldSpan = CollectionsMarshal.AsSpan(_drawables);
            for (int i = 0; i < worldSpan.Length; i++) worldSpan[i].Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            var uiSpan = CollectionsMarshal.AsSpan(_uiDrawables);
            for (int i = 0; i < uiSpan.Length; i++) uiSpan[i].Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        public virtual void Dispose()
        {
            _drawables.AddRange(_uiDrawables);
            foreach (var d in _drawables) if (d is IDisposable disp) disp.Dispose();

            _updatables.Clear();
            _drawables.Clear();
            _uiDrawables.Clear();
            _inputTargets.Clear();
            _uiInputTargets.Clear();
        }

        public abstract void Load();
        public void SetCamera(Camera2D cam) => camera = cam;
    }
}