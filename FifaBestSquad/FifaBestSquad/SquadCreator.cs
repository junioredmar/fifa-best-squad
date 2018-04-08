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

    public class SquadCreator
    {
        private const string Path = "../../content";

        private List<Player> players;

        private Formation formation;

        private List<Queue<char>> positionQueues;


        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            this.players = this.players.Where(p => p.Club != "Icons" && p.Rating > 78 && p.Rating <= 86).ToList();
            this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            this.formation = new Formation("4-3-3");

            //this.BuildOrder();

            //this.Build();

            this.BuildAll();

        }

        private void BuildAll()
        {
            string permutation = "ACHIKJGBFDE";

            //BuildPermutations permutations = new BuildPermutations();
            //permutations.Build("ABCDEFGHIJK");

            //foreach (var permutation in permutations.results)
            //{
                var position = formation.Positions.FirstOrDefault(pos => pos.Index == permutation[0]);
                var player = players.FirstOrDefault(pl => pl.Position == position.PositionEnum);
                BuildByPermutation(permutation, 0, player);
            //}
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
                var tiedPlayers = nextPosition.TiedPositions.Where(tp => tp.Player != null).Select(tp => tp.Player);
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

        private void Build()
        {
            BuildPermutations permutations = new BuildPermutations();
            permutations.Build("ABCDEFGHIJK");

            foreach (var permutation in permutations.results)
            {

                foreach (char index in permutation)
                {
                    var player = SetupByIndex(index);
                    if (player == null)
                    {
                        break;
                    }
                    Console.WriteLine(player.Name);
                }

                Console.WriteLine("------------------[" + permutation + "]-------------------------");
                var allPlayers = formation.Positions.Where(pos => pos.Player != null);
                if (allPlayers.Count() < 11)
                {
                    Clean();
                    Console.WriteLine("DID NOT FIND A SQUAD");
                    continue;
                }

                // CONSOLE.WRITE RESULTS
                var soma = 0;
                foreach (var pos in this.formation.Positions)
                {
                    soma = soma + pos.Player.Rating;
                    Console.WriteLine("[" + pos.Player.Rating + "][" + pos.PositionEnum + "] " + pos.Player.Name);
                }
                Console.WriteLine("Rating Geral: [" + (soma / 11) + "]");

                // Cleaning
                Clean();

                Console.ReadLine();
            }
        }

        private void Clean()
        {
            foreach (var pos in this.formation.Positions)
            {
                pos.Visited = false;
            }
        }

        private Player SetupByIndex(char index)
        {
            var position = formation.Positions.First(p => p.Index == index);
            var tiedPlayers = position.TiedPositions.Where(tp => tp.Player != null).Select(tp => tp.Player);
            var usedPlayers = formation.Positions.Where(pos => pos.Player != null).Select(pos => pos.Player).Select(pl => pl.BaseId);
            position.Player = players.FirstOrDefault(p => p.Position == position.PositionEnum && !usedPlayers.Contains(p.BaseId) && p.IsAnyGreen(tiedPlayers));


            return position.Player;
        }

        private void BuildOrder()
        {

            foreach (var position in this.formation.Positions)
            {

                List<char> charList = new List<char>();
                this.SetupOrder(position, charList);
                position.Paths.Add(charList);

                // LOGGING
                foreach (var item in charList)
                {
                    Console.Write(item);
                }
                Console.WriteLine("-------------------");

                // Cleaning
                foreach (var pos in this.formation.Positions)
                {
                    pos.Visited = false;
                }

            }
        }



        private void SetupOrder(Position position, List<char> stack)
        {
            stack.Add(position.Index);
            position.Visited = true;

            var nextPosition = position.TiedPositions.FirstOrDefault(tp => !tp.Visited);
            if (nextPosition == null)
            {
                return;
            }

            this.SetupOrder(nextPosition, stack);
        }


        private void SetPlayersToMemory()
        {
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
                                Rating = item.rating
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
