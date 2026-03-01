using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LurkerCommand.GameSystem
{
    public class Team {
        private List<Unit> units = new List<Unit>();
        public readonly Color teamColor;
        private int moves;
        public int Moves {
            get => moves;
            set => moves = MathHelper.Clamp(value, 0, int.MaxValue);
        }
        public void AddUnit(Unit unit) {
            unit.SetTeam(this);
            units.Add(unit);
        }
        public void RemoveUnit(Unit unit) {
            unit.SetTeam(null);
            units.Remove(unit);
        }
        public void SumMoves() {
            Span<Unit> span = CollectionsMarshal.AsSpan(units);
            for(int i = 0; i < span.Length; i++) {
                Moves += span[i].Moves;
            }
        }
        public Team(Color teamColor) {
            this.teamColor = teamColor;
        }
    }
}
