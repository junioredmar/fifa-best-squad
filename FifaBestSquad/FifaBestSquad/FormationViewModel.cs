using System;
using System.Collections.Generic;
using System.Text;

namespace FifaBestSquad
{
    using System.Linq;

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

        public string Permutation { get; set; }

        public string BasePlayer { get; set; }

        public int Rating { get; set; }

        public List<Card> Cards { get; set; }

        public override bool Equals(object obj)
        {
            Squad other = obj as Squad;
            if (other == null)
            {
                return false;
            }

            for (var i = 0; i < this.Cards.Count(); i++)
            {
                var thisCard = this.Cards.ElementAt(i);
                var otherCard = other.Cards.ElementAt(i);
                if (otherCard == null || !otherCard.Equals(thisCard))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return this.Cards.GetHashCode();
        }
    }

    public class Card
    {
        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }

        public override bool Equals(object obj)
        {
            Card other = obj as Card;
            if (other == null)
            {
                return false;
            }

            if (this.Player == null || other.Player == null)
            {
                return false;
            }

            if (this.Player.Id == null || other.Player.Id == null)
            {
                return false;
            }

            if (this.PositionEnum == other.PositionEnum && this.Player.Id.Equals(other.Player.Id))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Player.GetHashCode();
        }
    }
}
