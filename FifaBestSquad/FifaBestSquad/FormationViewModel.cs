using System;
using System.Collections.Generic;
using System.Text;

namespace FifaBestSquad
{
    public class FormationViewModel
    {
        public FormationViewModel()
        {
            Squads = new List<Squad>();
        }

        public List<Squad> Squads { get; set; }
    }

    public class Squad
    {
        public Squad()
        {
            Cards = new List<FifaBestSquad.Card>();
        }

        public int Rating { get; set; }

        public List<Card> Cards { get; set; }
    }

    public class Card
    {
        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }
    }
}
