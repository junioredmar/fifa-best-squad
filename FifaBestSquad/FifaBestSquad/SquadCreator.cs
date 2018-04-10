using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FifaBestSquad
{


    //    A    
    //C       B
    //    D    
    //  E   F  
    //H       G
    //  I   J  
    //    K    
    // KJGBACHIED

    public class SquadCreator
    {
        private const string Path = "../../content";

        private List<Player> players;

        private Formation formation;

        private List<Queue<char>> positionQueues;

        private const string PathResults = "../../results";

        private FormationViewModel result;


        public void BuildPerfectSquad(List<string> permutations)
        {
            this.result = new FormationViewModel();
            // this.GetFromPlayersFromEa();

            this.SetPlayersToMemory();

            this.BuildSquad(permutations);
        }

        private void BuildSquad(List<string> permutations)
        {
            //this.players = this.players.Where(p => p.Club != "Icons" && !p.IsSpecialType && p.Rating > 81 && p.Rating <= 86 && p.League.ToLower().StartsWith("pre")).ToList();
            this.players = this.players.Where(p => p.Club != "Icons" && !p.IsSpecialType && p.Rating > 79 && p.Rating <= 87 && p.League.ToLower().StartsWith("pre")).ToList();
            this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            this.formation = new Formation("4-3-3");

            this.BuildAll(permutations);

            // this.BuildAllTest(permutations);

        }

        private void BuildAllTest(List<string> uniquePaths)
        {
            var player = this.players.FirstOrDefault(pl => pl.Name.ToLower() == "diego costa" && pl.Rating == 86);
            var playerPosition = this.formation.Positions.FirstOrDefault(pos => pos.PositionEnum == player.Position);
            if (playerPosition == null)
            {
                //NOT IN THIS POSITION
                return;
            }

            var permutations = uniquePaths.Where(up => up.StartsWith(playerPosition.Index.ToString()));
            int count = 0;
            Console.WriteLine("Will iterate [" + permutations.Count() + "] times");
            for (var i = 0; i < permutations.Count(); i++)
            {
                var permutation = permutations.ElementAt(i);

                BuildByPermutation(permutation, 0, player);

                //if (count == 100)
                //{
                //    count = 0;
                Console.Write("[" + i + "]");
                //}
                //count++;

                // IF NOT 11 POSITIONS = CLEAN AND CONTINUE
                var allPlayers = formation.Positions.Where(pos => pos.Player != null);
                if (allPlayers.Count() < 11)
                {
                    Clean();
                    continue;
                }

                AddToResults(player.Name, permutation);
                PrintResults(permutation);
                Clean();
            }
            Console.WriteLine();
        }

        private void BuildAll(List<string> uniquePaths)
        {
            for (var pi = 0; pi < players.Count(); pi++)
            {
                var player = this.players.ElementAt(pi);
                var playerPosition = this.formation.Positions.FirstOrDefault(pos => pos.PositionEnum == player.Position);
                Console.WriteLine("--------------------------- [" + player.Position + "]" + player.Name + " - [" + pi + "] --------------------------- ");
                if (playerPosition == null)
                {
                    continue;
                }

                //var count = 0;
                var permutations = uniquePaths.Where(up => up.StartsWith(playerPosition.Index.ToString()));

                Console.WriteLine("Will iterate [" + permutations.Count() + "] times");
                for (var i = 0; i < permutations.Count(); i++)
                {
                    var permutation = permutations.ElementAt(i);

                    //if (count == 100)
                    //{
                    //    count = 0;
                    Console.Write("[" + i + "]");
                    //}
                    //count++;

                    this.BuildByPermutation(permutation, 0, player);

                    // IF NOT 11 POSITIONS = CLEAN AND CONTINUE
                    var allPlayers = this.formation.Positions.Where(pos => pos.Player != null);
                    if (allPlayers.Count() < 11)
                    {
                        this.Clean();
                        continue;
                    }

                    this.AddToResults(player.Name, permutation);
                    this.PrintResults(permutation);
                    this.Clean();
                }
                Console.WriteLine();

            }

            if (!this.result.Squads.Any())
            {
                return;
            }

            // SAVING TO JSON
            var toSave = JsonConvert.SerializeObject(this.result);
            Directory.CreateDirectory(PathResults);
            File.WriteAllText(string.Format("{0}/{1}.json", PathResults, this.formation.Pattern), toSave);
        }

        private string BuildByPermutation(string permutation, int indexNumber, Player player)
        {
            if (permutation.Count() <= indexNumber)
            {
                return "COMPLETED";
            }

            char index = permutation[indexNumber];
            var position = formation.Positions.First(p => p.Index == index);

            position.Player = player;

            // GETTING NEXTINDEX
            var nextIndexNumber = indexNumber + 1;
            if (permutation.Count() <= nextIndexNumber)
            {
                return "COMPLETED";
            }
            char nextIndex = permutation[nextIndexNumber];
            var nextPosition = formation.Positions.First(p => p.Index == nextIndex);

            string status = string.Empty;
            var alreadyTested = new List<int>();
            do
            {
                var tiedPlayers = nextPosition.TiedPositions.Where(tp => tp.Player != null).Select(tp => tp.Player).ToList();
                tiedPlayers.Add(player);
                var usedPlayers = formation.Positions.Where(pos => pos.Player != null).Select(pos => pos.Player).Select(pl => pl.BaseId);
                var nextPlayer = players.FirstOrDefault(p => p.Position == nextPosition.PositionEnum &&
                                                             !alreadyTested.Contains(p.BaseId) &&
                                                             !usedPlayers.Contains(p.BaseId) &&
                                                             p.IsAnyGreen(tiedPlayers));

                if (nextPlayer == null)
                {
                    position.Player = null;
                    return "ERROR";
                }

                alreadyTested.Add(nextPlayer.BaseId);

                status = BuildByPermutation(permutation, nextIndexNumber, nextPlayer);

            } while (status == "ERROR");

            return "DONE";
        }

        private Player GetNextPlayer(Player player, ICollection<int> notMathing, Position tiedPosition)
        {
            var nextPlayer = this.players.FirstOrDefault(
                pl => !notMathing.Contains(pl.BaseId) && pl.Position == tiedPosition.PositionEnum
                                                      && ((pl.Club == player.Club)
                                                          || (pl.Nation == player.Nation && pl.League == player.League))
                                                      && !this.formation.Positions.Any(
                                                          pos => pos.Player != null && pos.Player.BaseId == pl.BaseId));
            return nextPlayer;
        }

        private void AddToResults(string basePlayer, string permutation)
        {
            Squad squad = new Squad();
            var soma = 0;
            foreach (var pos in this.formation.Positions)
            {
                if (pos.Player != null)
                {
                    squad.Cards.Add(new Card
                    {
                        PositionEnum = pos.PositionEnum,
                        Player = pos.Player
                    });

                    soma = soma + pos.Player.Rating;
                }
            }
            squad.Rating = soma / 11;
            squad.BasePlayer = basePlayer;
            squad.Permutation = permutation;
            this.result.Squads.Add(squad);
        }

        private void PrintResults(string permutation)
        {
            Console.WriteLine("------------------[" + permutation + "]-------------------------");
            var soma = 0;
            foreach (var pos in this.formation.Positions)
            {
                if (pos.Player != null)
                {
                    soma = soma + pos.Player.Rating;
                    Console.WriteLine("[" + pos.Player.Rating + "][" + pos.PositionEnum + "] " + pos.Player.Name);
                }
            }
            Console.WriteLine("Rating Geral: [" + (soma / 11) + "]");
        }

        private void Clean()
        {
            foreach (var pos in this.formation.Positions)
            {
                pos.Player = null;
            }
        }


        private void SetPlayersToMemory()
        {

            Console.WriteLine("Bring all players to memory - START");

            this.players = new List<Player>();

            DirectoryInfo d = new DirectoryInfo(Path);

            foreach (var file in d.GetFiles("*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        var root = JsonConvert.DeserializeObject<RootObject>(line);

                        foreach (var item in root.items)
                        {

                            PositionEnum itemPosition;
                            bool couldParse = Enum.TryParse(item.position, out itemPosition);
                            if (!couldParse)
                            {
                                Console.WriteLine(item.position);
                            }

                            this.players.Add(new Player
                            {
                                BaseId = item.baseId,
                                Name = item.name,
                                Club = item.club.name,
                                League = item.league != null ? item.league.name : string.Empty,
                                Nation = item.nation != null ? item.nation.name : string.Empty,
                                Position = itemPosition,
                                Rating = item.rating,
                                IsSpecialType = item.isSpecialType
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

            Console.WriteLine("Bring all players to memory - DONE");
        }

        private static void GetFromPlayersFromEa()
        {
            Directory.CreateDirectory(Path);


            for (int page = 1; page > 0; page++)
            {
                var client = new RestClient(string.Format("http://www.easports.com/fifa/ultimate-team/api/fut/item?page={0}", page));

                // client.Authenticator = new HttpBasicAuthenticator(username, password);

                var request = new RestRequest(Method.GET);
                //request.AddParameter("name", "value"); // adds to POST or URL querystring based on Method
                //request.AddUrlSegment("id", "123"); // replaces matching token in request.Resource

                // easily add HTTP Headers
                //request.AddHeader("header", "value");

                // add files to upload (works with compatible verbs)
                //request.AddFile(path);

                // execute the request
                IRestResponse response = client.Execute(request);
                var content = response.Content; // raw content as string


                File.WriteAllText(string.Format("{0}/{1}.json", Path, page), content);

                var root = JsonConvert.DeserializeObject<RootObject>(content);

                //Console.WriteLine(root.totalPages);

                if (!root.items.Any())
                {
                    page = -1;
                    Console.WriteLine("Done");
                }
                Console.WriteLine(page);
            }
        }
    }
}
