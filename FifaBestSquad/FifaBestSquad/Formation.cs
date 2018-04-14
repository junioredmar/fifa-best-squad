using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FifaBestSquad
{
    public class Formation
    {
        public Formation(string formationPattern)
        {
            this.Pattern = formationPattern;
            this.Positions = new List<Position>();
        }

        public string Pattern { get; set; }

        public List<Position> Positions { get; set; }

        public List<string> Permutations { get; set; }
    }
}