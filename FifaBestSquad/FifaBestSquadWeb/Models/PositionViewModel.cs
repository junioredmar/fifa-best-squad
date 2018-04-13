using FifaBestSquad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifaBestSquadWeb.Models
{
    public class PositionViewModel
    {
        public PositionEnum PositionEnum { get; set; }

        public PlayerViewModel Player { get; set; }
    }
}