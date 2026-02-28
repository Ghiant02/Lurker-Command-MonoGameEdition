using GameEngine.Components.UI;
using LurkerCommand.GameSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.MapSystem
{
    public sealed class Cell : Image, IGrid {
        public Point gridPosition { get; set; }
        private Unit currentUnit;
        public Cell(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position, scale, Color.White, 0f, true) {
            
        }
        public void BindUnit(Unit unit) {
            if (currentUnit == unit) return;
            currentUnit = unit;
        }
    }
}
