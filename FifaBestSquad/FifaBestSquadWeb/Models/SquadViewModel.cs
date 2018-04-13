using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifaBestSquadWeb.Models
{
    public class SquadViewModel
    {
        public SquadViewModel()
        {
            Cards = new List<PositionViewModel>();
        }

        public string Permutation { get; set; }

        public string BasePlayer { get; set; }

        public int Rating { get; set; }

        public List<PositionViewModel> Cards { get; set; }

    }
}