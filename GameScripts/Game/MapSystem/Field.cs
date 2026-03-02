using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LurkerCommand.MapSystem
{
    public static class Field {
        private static readonly Queue<(Cell cell, int dist)> _bfsQueue = new(256);
        private static readonly HashSet<Cell> _visited = new(256);
        private static readonly List<Cell> _result = new(256);
        private const byte sizeX = 32;
        private const byte sizeY = 32;
        private const byte cellScale = 2;
        public static Cell[,] cells = new Cell[sizeX, sizeY];
        public static int MapWidth { get; private set; }
        public static int MapHeight { get; private set; }

        public static int CellWidth { get; private set; }
        public static int CellHeight { get; private set; }

        public static void SetMap(Scene scene) {
            Texture2D cellTexture = AssetManager.GetTexture("square");

            CellWidth = cellTexture.Width * cellScale;
            CellHeight = cellTexture.Height * cellScale;

            MapWidth = sizeX * CellWidth;
            MapHeight = sizeY * CellHeight;

            for (byte x = 0; x < sizeX; x++) {
                for (byte y = 0; y < sizeY; y++) {
                    Vector2 worldPosition = new Vector2(x * CellWidth, y * CellHeight);

                    cells[x, y] = new Cell(cellTexture, worldPosition, new Vector2(cellScale, cellScale));
                    cells[x, y].gridPosition = new Point(x, y);
                    scene.Add(cells[x, y]);
                }
            }
        }
        public static void UpdateVisibility(Unit unit)
        {
            if (unit == null) return;
            Point unitPos = unit.gridPosition;
            ClearVisibility();
            int radius = unit.Value;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (Math.Abs(x) + Math.Abs(y) > radius) continue;

                    int cx = unitPos.X + x;
                    int cy = unitPos.Y + y;

                    if (CellInField(cx, cy))
                    {
                        cells[cx, cy].IsVisible = true;
                    }
                }
            }
        }
        public static void ClearVisibility()
        {
            foreach (var cell in cells)
            {
                cell?.IsVisible = false;
            }
        }

        public static ReadOnlySpan<Cell> GetAvailableCells(Cell startCell, int maxRange)
        {
            _bfsQueue.Clear();
            _visited.Clear();
            _result.Clear();

            _bfsQueue.Enqueue((startCell, 0));
            _visited.Add(startCell);

            while (_bfsQueue.Count > 0)
            {
                var (current, dist) = _bfsQueue.Dequeue();
                if (dist > 0) _result.Add(current);
                if (dist >= maxRange) continue;

                Point p = current.gridPosition;
                CheckNeighbor(p.X + 1, p.Y, dist + 1);
                CheckNeighbor(p.X - 1, p.Y, dist + 1);
                CheckNeighbor(p.X, p.Y + 1, dist + 1);
                CheckNeighbor(p.X, p.Y - 1, dist + 1);
            }

            return CollectionsMarshal.AsSpan(_result);
        }

        private static void CheckNeighbor(int x, int y, int newDist)
        {
            Cell neighbor = GetCell(new Point(x, y));
            if (neighbor != null && neighbor.IsEmpty && _visited.Add(neighbor))
            {
                _bfsQueue.Enqueue((neighbor, newDist));
            }
        }
        public static Cell GetCellByWorldPos(Vector2 worldPosition) {
            int x = (int)Math.Floor(worldPosition.X / CellWidth);
            int y = (int)Math.Floor(worldPosition.Y / CellHeight);

            return GetCell(x, y);
        }
        public static void ToggleMoveNotes(Cell cell, bool toggle, int value) {
            ReadOnlySpan<Cell> available = GetAvailableCells(cell, value);
            for (byte i = 0; i < available.Length; i++) {
                available[i].Toggle(toggle);
            }
        }
        public static bool CellInField(int x, int y) => x >= 0 && y >= 0 && x < sizeX && y < sizeY;
        public static bool CellInField(Cell cell) => cell != null && CellInField(cell.gridPosition.X, cell.gridPosition.Y);
        public static Cell GetCell(Point gridPosition) => GetCell(gridPosition.X, gridPosition.Y);
        public static Cell GetCell(int x, int y)
        {
            if (CellInField(x, y)) return cells[x, y];
            return null;
        }
    }
}
