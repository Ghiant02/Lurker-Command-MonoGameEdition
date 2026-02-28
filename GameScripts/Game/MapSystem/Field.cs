using GameEngine.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameEngine.Systems;

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
    }
}
