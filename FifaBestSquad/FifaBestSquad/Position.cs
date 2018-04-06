using System.Collections.Generic;

namespace FifaBestSquad
{
    public class Position
    {
        public Position()
        {
            this.TiedPositions = new List<Position>();
            this.Paths = new List<List<char>>();
        }

        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }

        public List<Position> TiedPositions { get; set; }



        // Building properties of queuinggrap
        public char Index { get; set; }

        public bool Visited { get; set; }
        
        public List<List<char>> Paths { get; set; }

    }
}