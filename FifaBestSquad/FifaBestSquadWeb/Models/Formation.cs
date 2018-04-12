﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FifaBestSquadWeb.Models
{
    public class Formation
    {
        public Formation()
        {
            Patterns = new List<SelectListItem>();
        }

        public List<SelectListItem> Patterns { get; set; }

        public string Pattern { get; set; }
    }
}