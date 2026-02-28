using GameEngine.Components.UI;
using LurkerCommand.MapSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.GameSystem
{
    public sealed class Unit : Text, IGrid {
        public Point gridPosition { get; set; }
        public Cell currentCell;
        public Unit(SpriteFont font, string text, Color color, Vector2 position) : base(font, text, position, color) {
            
        }

        public void BindCell(Cell cell) {
            if (currentCell == cell) return;
            currentCell = cell;
        }
    }
}
