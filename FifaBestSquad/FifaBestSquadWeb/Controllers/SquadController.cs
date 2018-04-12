using FifaBestSquadWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FifaBestSquadWeb.Controllers
{
    public class SquadController : Controller
    {
        public ActionResult Index()
        {
            Formation formation = new Formation();
            formation.Patterns.Add(new SelectListItem
            {
                Text = "4-3-3(4)",
                Value = "4-3-3(4)"
            });
            formation.Patterns.Add(new SelectListItem
            {
                Text = "4-3-3(3)",
                Value = "4-3-3(3)",
                Selected = true
            });
            formation.Patterns.Add(new SelectListItem
            {
                Text = "4-3-3(2)",
                Value = "4-3-3(2)"
            });

            return View(formation);
        }        
        
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var value = collection["Pattern"];
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
