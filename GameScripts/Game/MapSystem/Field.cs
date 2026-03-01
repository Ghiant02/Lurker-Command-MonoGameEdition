using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LurkerCommand.MapSystem
{
    public static class Field {
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
        public static List<Cell> GetAvailableCells(Cell startCell, int moves)
        {
            List<Cell> output = new List<Cell>();
            Point pos = startCell.gridPosition;

            for (int i = -moves; i <= moves; i++)
            {
                if (i == 0) continue;
                TryAdd(pos.X + i, pos.Y);

                TryAdd(pos.X, pos.Y + i);
            }

            void TryAdd(int x, int y)
            {
                if (CellInField(x, y))
                {
                    Cell targetCell = cells[x, y];
                    if (targetCell != null && targetCell.IsEmpty)
                    {
                        output.Add(targetCell);
                    }
                }
            }

            return output;
        }
        public static Cell GetCellByWorldPos(Vector2 worldPosition)
        {
            int x = (int)(worldPosition.X / CellWidth);
            int y = (int)(worldPosition.Y / CellHeight);

            if (CellInField(x, y))
            {
                return cells[x, y];
            }
            return null;
        }
        public static void ToggleMoveNotes(Cell cell, bool toggle, int value) {
            Span<Cell> available = CollectionsMarshal.AsSpan(GetAvailableCells(cell, value));
            for(byte i = 0; i < available.Length; i++) {
                available[i].Toggle(toggle);
            }
        }
        public static bool CellInField(int x, int y) => x >= 0 && y >= 0 && x < MapWidth && y < MapHeight;

        public static bool CellInField(Cell cell) => cell != null && CellInField(cell.gridPosition.X, cell.gridPosition.Y);
        public static Cell GetCell(Point gridPosition) => cells[gridPosition.X, gridPosition.Y];
        public static Cell GetCell(int X, int Y) => cells[X, Y];
    }
}
