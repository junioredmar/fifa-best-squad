using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifaBestSquadWeb.Models
{
    public class Squad
    {
        public Squad()
        {
            Cards = new List<Position>();
        }

        public string Permutation { get; set; }

        public string BasePlayer { get; set; }

        public int Rating { get; set; }

        public List<Position> Cards { get; set; }

    }
}