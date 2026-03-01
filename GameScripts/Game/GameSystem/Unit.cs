using GameEngine.Components.UI;
using LurkerCommand.MapSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.GameSystem
{
    public sealed class Unit : Text, IGrid {
        private int value;
        public const int maxValue = 9;
        public int Value {
            get => value;
            set {
                if(value > maxValue) {
                    this.value = 1;
                    return;
                }
                this.value = value;
            }
        }
        public Point gridPosition { get; set; }
        public Cell currentCell;
        public bool isVisible;
        public Unit(SpriteFont font, Point gridPosition, Color color, int Value) : base(font, "", Vector2.Zero, color) {
            this.Value = Value;
        }

        public void BindCell(Cell cell) {
            if (currentCell == cell) return;
            cell.BindUnit(this);
            Transform.Position = cell.Transform.Position;
            currentCell = cell;
        }
        public void UpdateText() {
            
        }
    }
}
