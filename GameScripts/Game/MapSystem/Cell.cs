using GameEngine.Components.UI;
using LurkerCommand.GameSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.MapSystem
{
    public sealed class Cell : Image, IGrid {
        public Point gridPosition { get; set; }
        public readonly Color defaultColor = new Color(0,0,0,255);
        public readonly Color hiddenColor = new Color(0,0,0,100);
        private Unit currentUnit;
        private bool isVisible = false;
        private bool isEmpty = true;
        public bool IsVisible {
            get => isVisible;
            set {
                if (isVisible == value) return;
                isVisible = value;

                Color = value ? defaultColor : hiddenColor;
            }
        }
        public bool IsEmpty {
            get => isEmpty;
            set {
                if (isEmpty == value) return;
                isEmpty = value;
            }
        }
        public Cell(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position, scale, Color.White, 0f, true) {
            
        }
        public void BindUnit(Unit unit) {
            if (currentUnit == unit) return;
            currentUnit = unit;
            IsEmpty = true;
        }
        public void Unbind() {
            currentUnit = null;
            IsEmpty = false;
        }
    }
}
