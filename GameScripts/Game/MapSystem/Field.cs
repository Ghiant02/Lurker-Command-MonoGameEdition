using GameEngine.Services;
using GameEngine.Systems;
using LurkerCommand.GameSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace LurkerCommand.MapSystem
{
    public static class Field {
        private const byte sizeX = 32;
        private const byte sizeY = 32;
        private const byte cellScale = 2;
        public static Cell[,] cells = new Cell[sizeX, sizeY];
        public static int MapWidth { get; private set; }
        public static int MapHeight { get; private set; }

        public static void SetMap(Scene scene) {
            Texture2D cellTexture = AssetManager.GetTexture("square");

            int cellWidth = cellTexture.Width * cellScale;
            int cellHeight = cellTexture.Height * cellScale;
            MapWidth = sizeX * cellTexture.Width * cellScale;
            MapHeight = sizeY * cellTexture.Height * cellScale;

            for (byte x = 0; x < sizeX; x++) {
                for (byte y = 0; y < sizeY; y++) {
                    Vector2 worldPosition = new Vector2(x * cellWidth, y * cellHeight);

                    cells[x, y] = new Cell(cellTexture, worldPosition, new Vector2(cellScale, cellScale));
                    cells[x, y].gridPosition = new Point(x, y);
                    scene.Add(cells[x, y]);
                }
            }
        }
        public static void UpdateVisibility(Unit unit, Point unitPos)
        {
            if (unit == null) return;

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
                if (cell != null) cell.IsVisible = false;
            }
        }
        public static bool CellInField(int x, int y) => x >= 0 && y >= 0 && x < MapWidth && y < MapHeight;

        public static bool CellInField(Cell cell) => cell != null && CellInField(cell.gridPosition.X, cell.gridPosition.Y);
        public static Cell GetCell(Point gridPosition) => cells[gridPosition.X, gridPosition.Y];
        public static Cell GetCell(int X, int Y) => cells[X, Y];
    }
}
