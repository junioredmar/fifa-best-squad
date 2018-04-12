using FifaBestSquad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifaBestSquadWeb.Models
{
    public class Position
    {
        public PositionEnum PositionEnum { get; set; }

        public Player Player { get; set; }
    }
}