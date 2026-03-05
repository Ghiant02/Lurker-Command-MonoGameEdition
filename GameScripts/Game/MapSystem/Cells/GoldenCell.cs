using LurkerCommand.GameScripts.Game.MapSystem.Cells;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LurkerCommand.MapSystem
{
    public sealed class GoldenCell : Cell
    {
        public const byte bonusValue = 1;
        public override bool canCaptured { get; set; } = true;
        public override CellType cellType { get; set; } = CellType.GoldenCell;
        public GoldenCell(Texture2D texture, Vector2 position, Vector2 scale) : base(texture, position, scale)
        {

        }
    }
}