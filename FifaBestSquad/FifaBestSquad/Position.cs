using System.Collections.Generic;

namespace FifaBestSquad
{
    public class Position
    {
        public Position()
        {
            this.TiedPositions = new List<Position>();
        }

        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }

        public List<Position> TiedPositions { get; set; }



        // Building properties of queuing
        public char Index { get; set; }

        public bool Visited { get; set; }
        
        public List<Queue<char>> Queues { get; set; }

    }
}