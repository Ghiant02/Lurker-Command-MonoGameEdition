using GameEngine.Components.UI;
using LurkerCommand.MapSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.GameSystem
{
    public sealed class Unit : Text, IGrid
    {
        private int value;
        public const int maxValue = 9;
        public int Value
        {
            get => value;
            set
            {
                this.value = value > maxValue ? 1 : value;
                UpdateText();
            }
        }
        public Point gridPosition { get; set; }
        public Cell currentCell;
        public bool isVisible;

        public Unit(SpriteFont font, Point gridPosition, Color color, int Value) : base(font, "", Vector2.Zero, color)
        {
            this.Value = Value;
            BindCell(Field.GetCell(gridPosition));
        }

        public void BindCell(Cell cell)
        {
            if (currentCell == cell && !cell.IsEmpty) return;

            cell.BindUnit(this);
            Transform.Position = cell.Transform.Position + cell.GetSize().ToVector2() * 0.5f;

            currentCell?.Unbind();
            currentCell = cell;
            gridPosition = cell.gridPosition;

            Field.UpdateVisibility(this);
        }

        public void UpdateText()
        {
            text = value.ToString();
            CenterOrigin();
        }
    }
}