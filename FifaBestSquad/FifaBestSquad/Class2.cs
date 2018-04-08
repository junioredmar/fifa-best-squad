using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    // funciona, mas nao eh o perfeito (escolhe um caminho aleatorio)
    public class Class2
    {
        private const string Path = "../../content";

        private List<Player> players;

        private Formation formation;


        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            this.players = this.players.Where(p => p.Rating > 78 && p.Rating <= 86).ToList();
            this.formation = new Formation("4-3-3");
            var topTen = this.players.Take(50);

            foreach (var player in topTen)
            {
                var position = this.formation.Positions.FirstOrDefault(p => p.PositionEnum == player.Position);
                if (position == null)
                {
                    continue;
                }

                var status = this.Setup(position, player);

                if (status == "ERROR")
                {
                    continue;
                }

                // CONSOLE.WRITE RESULTS
                Console.WriteLine("------------------[" + position.PositionEnum + "][" + player.Name + "]-------------------------");
                
                var soma = 0;
                foreach (var pos in this.formation.Positions)
                {
                    soma = soma + pos.Player.Rating;
                    Console.WriteLine("[" + pos.Player.Rating + "][" + pos.PositionEnum + "] " + pos.Player.Name);
                }

                Console.WriteLine("Rating Geral: [" + (soma / 11) + "]");
                

                // CLEANING POSITIONS
                foreach (var pos in this.formation.Positions)
                {
                    pos.Player = null;
                }
            }

            foreach (var position in this.formation.Positions)
            {
                Player firstPlayer = this.players.FirstOrDefault(pl => pl.Position == position.PositionEnum);

                var status = this.Setup(position, firstPlayer);

                if (status == "ERROR")
                {
                    continue;
                }

                // CONSOLE.WRITE RESULTS
                Console.WriteLine("------------------[" + position.PositionEnum + "]-------------------------");
                var soma = 0;
                foreach (var pos in this.formation.Positions)
                {
                    soma = soma + pos.Player.Rating;
                    Console.WriteLine("[" + pos.Player.Rating + "][" + pos.PositionEnum + "] " + pos.Player.Name);
                }

                Console.WriteLine("Rating Geral: [" + (soma / 11) + "]");

                // CLEANING POSITIONS
                foreach (var pos in this.formation.Positions)
                {
                    pos.Player = null;
                }
            }

            Console.WriteLine("______________________________________COMPLETED______________________________________");

        }

        private string Setup(Position position, Player player)
        {
            var notMathingNeighbors = position.TiedPositions.Where(tp => tp.Player != null && !player.IsGreen(tp.Player));
            if (notMathingNeighbors.Any())
            {
                return "ERROR";
            }

            // RELATE PLAYER AT POSITION
            position.Player = player;

            foreach (var tiedPosition in position.TiedPositions)
            {
                if (tiedPosition.Player != null)
                {
                    continue;
                }

                var alreadyTested = new List<int>();
                string status;
                do
                {
                    var nextPlayer = this.GetNextPlayer(player, alreadyTested, tiedPosition);

                    if (nextPlayer == null)
                    {
                        position.Player = null;
                        return "ERROR";
                    }

                    alreadyTested.Add(nextPlayer.BaseId);

                    status = this.Setup(tiedPosition, nextPlayer);
                }
                while (status == "ERROR");
            }

            return string.Empty;
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
