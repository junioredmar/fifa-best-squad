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


        public void BuildPerfectSquad()
        {
            result = new FormationViewModel();
            //GetFromPlayersFromEa();

            SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            this.players = this.players.Where(p => p.Club != "Icons" && !p.IsSpecialType && p.Rating > 81 && p.Rating <= 86 && p.League.ToLower().StartsWith("pre")).ToList();
            //this.players = this.players.Where(p => p.Club != "Icons" && p.League.ToLower().StartsWith("pre") ).ToList();
            this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            this.formation = new Formation("4-3-3");

            //this.BuildOrder();

            //this.Build();

            this.BuildAll();


            //this.BuildAllTest();

        }

        //private void BuildAllTest()
        //{
        //    Player player = players.FirstOrDefault(pl => pl.Name.ToLower() == "morata" && pl.Rating == 85);
        //    Position playerPosition = formation.Positions.FirstOrDefault(pos => pos.PositionEnum == player.Position);
        //    if (playerPosition == null)
        //    {
        //        //NOT IN THIS POSITION
        //        return;
        //    }

        //    BuildByPermutation("ABCDEFGIHJK", 0, player);

        //    // IF NOT 11 POSITIONS = CLEAN AND CONTINUE
        //    var allPlayers = formation.Positions.Where(pos => pos.Player != null);
        //    if (allPlayers.Count() < 11)
        //    {
        //        Clean();
        //        return;
        //    }
            
        //    PrintResults();
        //    Clean();
        //}

        private void BuildAll()
        {
            Player player = players.FirstOrDefault(pl => pl.Name.ToLower() == "coutinho" && pl.Rating == 86);
            Position playerPosition = formation.Positions.FirstOrDefault(pos => pos.PositionEnum == player.Position);
            if(playerPosition == null)
            {
                //NOT IN THIS POSITION
                return;
            }
            //string permutation = "KJGBACHIED";
            //string permutation = "ACHIKJGBFDE";

            BuildPermutations permutations = new BuildPermutations();
            permutations.Build("ABCDEFGHIJK");

            /* var position = formation.Positions.FirstOrDefault(pos => pos.Index == permutation[0]);
            var player = players.FirstOrDefault(pl => pl.Position == position.PositionEnum); */
            var iterations = permutations.results.Where(r => r.StartsWith(playerPosition.Index.ToString())).ToList();

            iterations.Shuffle();
            Console.WriteLine("TENTATIVES: " + iterations.Count());

            int count = 0;
            for (int i = 0; i < iterations.Count(); i++)
            {
                var permutation = iterations.ElementAt(i);


                if(count == 100)
                {
                    count = 0;
                    // ESTOU PARANDO AQUI PORQUE ESTA LENTO
                    //break;
                    Console.WriteLine("Iteration: " + i + " - " + permutation);
                }
                count++;

                BuildByPermutation(permutation, 0, player);
                
                // IF NOT 11 POSITIONS = CLEAN AND CONTINUE
                var allPlayers = formation.Positions.Where(pos => pos.Player != null);
                if (allPlayers.Count() < 11)
                {
                    Clean();
                    continue;
                }

                Console.WriteLine("------------------[" + permutation + "]-------------------------");
                PrintResults();
                Clean();
            }

            //SAVING TO JSON
            var toSave = JsonConvert.SerializeObject(result);

            Directory.CreateDirectory(PathResults);
            File.WriteAllText(string.Format("{0}/{1}.json", PathResults, player.Name), toSave);
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

                PrintResults();

                // Cleaning
                Clean();

                Console.ReadLine();
            }
        }

        private void PrintResults()
        {
            Squad squad = new Squad();
            // CONSOLE.WRITE RESULTS
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
                    Console.WriteLine("[" + pos.Player.Rating + "][" + pos.PositionEnum + "] " + pos.Player.Name);
                }
            }
            Console.WriteLine("Rating Geral: [" + (soma / 11) + "]");
            squad.Rating = soma / 11;
            result.Squads.Add(squad);
        }

        private void Clean()
        {
            foreach (var pos in this.formation.Positions)
            {
                pos.Player = null;
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

                        //List<string> playerTypes = new List<string>();
                        foreach (var item in root.items)
                        {

                            PositionEnum itemPosition;
                            bool couldParse = Enum.TryParse(item.position, out itemPosition);
                            if (!couldParse)
                            {
                                Console.WriteLine(item.position);
                            }

                            //if(!playerTypes.Contains(item.playerType))
                            //{
                            //    playerTypes.Add(item.playerType);
                            //}

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

                        //foreach (var playerType in playerTypes)
                        //{
                        //    Console.WriteLine(playerType);
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
