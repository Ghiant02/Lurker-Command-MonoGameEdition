using LurkerCommand.GameScripts.Game.MapSystem.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.MapSystem
{
    public sealed class ImmortalCell : Cell
    {
        public override bool canCaptured { get; set; } = false;
        public override CellType cellType { get; set; } = CellType.ImmortalCell;
        public ImmortalCell(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position, scale) {

        }
    }
}