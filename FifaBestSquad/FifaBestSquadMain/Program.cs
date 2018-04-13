using FifaBestSquad;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifaBestSquadMain
{
    class Program
    {
        private const string Path = "../../content/data/formation";

        static void Main(string[] args)
        {
            Console.WriteLine("STARTED");

            var formation = new Formation("4-3-3");

            //var toSave = JsonConvert.SerializeObject(formation);
            //Directory.CreateDirectory(Path);
            //File.WriteAllText(string.Format("{0}/{1}.json", Path, formation.Pattern), toSave);

            BringFormationsToMemory();

            //var uniquePathCreator = new UniquePathCreator();
            //var permutations = uniquePathCreator.CreateUniquePath(formation);

            //var analyzer = new SquadCreator();
            //analyzer.BuildPerfectSquad(formation, permutations);

            //var resultChecker = new ResultChecker();
            //var result = resultChecker.GetResults();
            //PrintResults(result);

            Console.WriteLine("FINISHED");
            Console.ReadLine();
        }

        private static void BringFormationsToMemory()
        {
            Console.WriteLine("Bring all players to memory - START");

            var formation = new List<Formation>();

            DirectoryInfo d = new DirectoryInfo(Path);

            foreach (var file in d.GetFiles("*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        var root = JsonConvert.DeserializeObject<Formation>(line);

                        //foreach (var item in root.Positions)
                        //{

                        //    PositionEnum itemPosition;
                        //    bool couldParse = Enum.TryParse(item.position, out itemPosition);
                        //    formation.Add(new Player
                        //    {
                        //        Id = item.id,
                        //        BaseId = item.baseId,
                        //        Name = item.name,
                        //        Club = item.club.name,
                        //        League = item.league != null ? item.league.name : string.Empty,
                        //        Nation = item.nation != null ? item.nation.name : string.Empty,
                        //        Position = itemPosition,
                        //        Rating = item.rating,
                        //        IsSpecialType = item.isSpecialType,
                        //        Color = item.color
                        //    });
                        //}
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

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

            //foreach (var squad in allSquads)
            //{
            //    var bestPlayers = squad.Cards.Where(c => c.Player.Rating > 88).ToList();
            //    var terriblePlayers = squad.Cards.Where(c => c.Player.Rating < 80).ToList();

            //    if (!terriblePlayers.Any())
            //    {
            //        Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "]");
            //        foreach (var card in squad.Cards)
            //        {
            //            Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
            //        }
            //    }
            //}


            Console.WriteLine("------------------------- Printing results got from json [" + allSquads.Count() + "] -------------------------");
            for (int i = 0; i < allSquads.Count(); i++)
            {
                var squad = allSquads.ElementAt(i);
                Console.WriteLine("------------------------- Squad Rating: [" + squad.Rating + "] Base Player: [" + squad.BasePlayer + "] Permutation [" + squad.Permutation + "]");
                foreach (var card in squad.Cards)
                {
                    Console.WriteLine("[" + card.PositionEnum + "][" + card.Player.Rating + "] " + card.Player.Name);
                }
            }

        }
    }
}
