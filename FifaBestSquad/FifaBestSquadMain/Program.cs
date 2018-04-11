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
            Console.WriteLine("STARTED");

            //var formation = new Formation("4-3-3");

            //var uniquePathCreator = new UniquePathCreator();
            //var permutations = uniquePathCreator.CreateUniquePath(formation);

            //var analyzer = new SquadCreator();
            //analyzer.BuildPerfectSquad(formation, permutations);

            var resultChecker = new ResultChecker();
            var result = resultChecker.GetResults();
            PrintResults(result);

            Console.WriteLine("FINISHED");
            Console.ReadLine();
        }

        private static void PrintResults(FormationViewModel result)
        {
            var allSquads = result.Squads;
            if (allSquads == null || !allSquads.Any())
            {
                Console.WriteLine("No results found");
                return;
            }

            allSquads = allSquads.OrderByDescending(s => s.Rating).ToList();

            foreach (var squad in allSquads)
            {
                var bestPlayers = squad.Cards.Where(c => c.Player.Rating > 88).ToList();
                var terriblePlayers = squad.Cards.Where(c => c.Player.Rating < 80).ToList();
                
                if (!terriblePlayers.Any())
                {
                    Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "]");
                    foreach (var card in squad.Cards)
                    {
                        Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
                    }
                }
            }


            //allSquads = allSquads.Where(s => s.Rating == 84).ToList();
            //Console.WriteLine("------------------------- Printing results got from json [" + allSquads.Count() + "] -------------------------");
            //for (int i = 0; i < allSquads.Count(); i++)
            //{
            //    var squad = allSquads.ElementAt(i);
            //    Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "]");
            //    foreach (var card in squad.Cards)
            //    {
            //        Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
            //    }
            //}

        }
    }
}
