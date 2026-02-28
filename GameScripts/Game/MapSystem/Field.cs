using GameEngine.Services;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GameEngine.Systems;

namespace LurkerCommand.MapSystem
{
    public static class Field {
        private const byte sizeX = 64;
        private const byte sizeY = 64;
        public static Cell[,] cells = new Cell[sizeX, sizeY];

        public static void SetMap(Scene scene) {
            Texture2D cellTexture = AssetManager.GetTexture("square");

            int cellWidth = cellTexture.Width;
            int cellHeight = cellTexture.Height;

            for (byte x = 0; x < sizeX; x++) {
                for (byte y = 0; y < sizeY; y++) {
                    Vector2 worldPosition = new Vector2(x * cellWidth, y * cellHeight);

                    cells[x, y] = new Cell(cellTexture, worldPosition, Vector2.One);
                    cells[x, y].gridPosition = new Point(x, y);
                    scene.Add(cells[x, y]);
                }
            }
        }
    }
}
