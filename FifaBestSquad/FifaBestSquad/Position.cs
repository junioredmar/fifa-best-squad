using Newtonsoft.Json;
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

        [JsonIgnore]
        public Player Player { get; set; }

        [JsonIgnore]
        public List<Position> TiedPositions { get; set; }

        public char Index { get; set; }

        //edmar - remove it
        //public override bool Equals(object obj)
        //{
        //    Position other = obj as Position;

        //    if (other == null)
        //    {
        //        return false;
        //    }

        //    if (this.Player == null || other.Player == null)
        //    {
        //        return false;
        //    }

        //    if (this.Player.Id == null || other.Player.Id == null)
        //    {
        //        return false;
        //    }

        //    return this.PositionEnum == other.PositionEnum || this.Player.Id.Equals(other.Player.Id);
        //}

        //public override int GetHashCode()
        //{
        //    return this.Player.GetHashCode();
        //}

    }
}