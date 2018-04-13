using FifaBestSquadWeb.Models;
using FifaBestSquadWeb.Service;
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
            FormationService service = new FormationService();
            var formations = service.GetFormations();

            FormationViewModel formationViewModel = new FormationViewModel();

            foreach (var formation in formations)
            {
                formationViewModel.Patterns.Add(new SelectListItem
                {
                    Text = formation.Pattern,
                    Value = formation.Pattern
                });
            }

            return View(formationViewModel);
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
