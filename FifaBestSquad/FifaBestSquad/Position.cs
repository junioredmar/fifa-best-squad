using System.Collections.Generic;

namespace FifaBestSquad
{
    public class Position
    {
        public Position()
        {
            Ligations = new List<Ligation>();
        }

        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }

        public List<Ligation> Ligations { get; set; }

    }
}