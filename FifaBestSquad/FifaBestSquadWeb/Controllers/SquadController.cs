using FifaBestSquad;
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

            FifaBestSquadWeb.Models.FormationViewModel formationViewModel = new FifaBestSquadWeb.Models.FormationViewModel();

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
                string value = collection["Pattern"];

                FormationService service = new FormationService();
                Formation formation = service.GetFormationByPattern(value);
                


                //var uniquePathCreator = new UniquePathCreator();
                //var permutations = uniquePathCreator.CreateUniquePath(formation);

                var analyzer = new SquadCreator();
                analyzer.BuildPerfectSquad(formation, formation.Permutations);


                return RedirectToAction("SquadResult");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult SquadResult()
        {
            var resultChecker = new ResultChecker();
            var result = resultChecker.GetResults();

            return View(result.Squads);
        }

        public ActionResult SquadDetail(string permutation)
        {
            var resultChecker = new ResultChecker();
            var result = resultChecker.GetResults();
            var squad = result.Squads.FirstOrDefault(s => s.Permutation == permutation);

            return View(squad.Positions);
        }

    }
}
