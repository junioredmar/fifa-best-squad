using System;
using System.Collections.Generic;
using System.Text;

namespace FifaBestSquad
{
    using System.Linq;

    public class FormationResult
    {
        public FormationResult()
        {
            Squads = new List<SquadResult>();
        }

        public List<SquadResult> Squads { get; set; }
    }

    public class SquadResult
    {
        public SquadResult()
        {
            Positions = new List<FifaBestSquad.PositionResult>();
        }

        public string Permutation { get; set; }

        public string BasePlayer { get; set; }

        public int Rating { get; set; }

        public List<PositionResult> Positions { get; set; }

        public override bool Equals(object obj)
        {
            SquadResult other = obj as SquadResult;
            if (other == null)
            {
                return false;
            }

            var playersSum = this.Positions.Sum(c => c.Player.BaseId);
            var otherPlayersSum = other.Positions.Sum(c => c.Player.BaseId);
            if(playersSum != otherPlayersSum)
            {
                return false;
            }

            return true;
            //for (var i = 0; i < this.Cards.Count(); i++)
            //{
            //    var thisCard = this.Cards.ElementAt(i);
            //    var otherCard = other.Cards.ElementAt(i);
            //    if (otherCard == null || !otherCard.Equals(thisCard))
            //    {
            //        return false;
            //    }
            //}

            //return true;
        }

        public override int GetHashCode()
        {
            return this.Positions.GetHashCode();
        }
    }

    public class PositionResult
    {
        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }

        public override int GetHashCode()
        {
            return this.Player.GetHashCode();
        }
    }
}
