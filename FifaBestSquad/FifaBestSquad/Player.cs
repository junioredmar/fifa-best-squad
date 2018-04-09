using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifaBestSquad
{
    public class Player
    {
        public int BaseId { get; set; }

        public string Name { get; set; }

        public int Rating { get; set; }

        public string Nation { get; set; }

        public string Club { get; set; }

        public string League { get; set; }

        public PositionEnum Position { get; set; }

        public bool IsSpecialType { get; set; }
    }
}
