using FifaBestSquad;
using FifaBestSquadMain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FifaBestSquadWeb.Service
{
    public class FormationService
    {
        public List<Formation> GetFormations()
        {
            FormationRepository repository = new FormationRepository();
            return repository.GetFormationsFromJson();
        }

        public Formation GetFormationByPattern(string pattern)
        {
            FormationRepository repository = new FormationRepository();
            return repository.GetFormationByPattern(pattern);
        }
    }
}