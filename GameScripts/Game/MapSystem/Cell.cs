using GameEngine.Components.UI;
using LurkerCommand.GameSystem;
using LurkerCommand.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.MapSystem
{
    public sealed class Cell : Image, IGrid {
        public Point gridPosition { get; set; }
        public readonly Color defaultColor = Color.White;
        public readonly Color hiddenColor = new Color(175, 175, 175, 255);
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
            set => isEmpty = value;
        }
        private Text moveNote;
        public Cell(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position, scale, new Color(175, 175, 175, 255), 0f, true) {
            
        }
        public void BindUnit(Unit unit) {
            if (currentUnit == unit && !IsEmpty) return;
            currentUnit = unit;
        }
        public void Unbind() {
            currentUnit = null;
            IsEmpty = false;
        }
        public void Toggle(bool toggle) {
            moveNote.IsActive = toggle;
        }
    }
}
