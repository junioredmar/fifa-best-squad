using FifaBestSquad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifaBestSquadMain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");

            UniquePathCreator uniquePathCreator = new UniquePathCreator();
            var permutations = uniquePathCreator.CreateUniquePath();

            SquadCreator analyzer = new SquadCreator();
            analyzer.BuildPerfectSquad(permutations);

            //ResultChecker resultChecker = new ResultChecker();
            //var result = resultChecker.GetResults();
            //PrintResults(result);

            Console.ReadLine();
        }

        private static void PrintResults(FormationViewModel result)
        {
            var allSquads = result.Squads;
            allSquads = allSquads.OrderByDescending(s => s.Rating).ToList();
            Console.WriteLine("------------------------- TOP SQUADS -------------------------");
            foreach (var squad in allSquads.Where(s => s.Rating >= 85))
            {
                Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "]");
                foreach (var card in squad.Cards)
                {
                    Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
                }
            }

            var premierSquads = allSquads.Where(s => s.Cards.Any(c => c.Player.League.ToLower().StartsWith("premier")));
            Console.WriteLine(
                "------------------------- Premier League Squads [" + premierSquads.Count() + "]-------------------------");
            foreach (var squad in premierSquads)
            {
                if (squad.Rating < 83)
                {
                    continue;
                }

                Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "]");
                foreach (var card in squad.Cards)
                {
                    Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
                }
            }
        }
    }
}
