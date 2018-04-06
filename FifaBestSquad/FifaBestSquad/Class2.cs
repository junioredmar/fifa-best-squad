using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
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

            this.players = this.players.Where(p => p.League.ToLower().StartsWith("pre")).ToList();

            string formationPattern = "4-3-3";
            this.formation = new Formation(formationPattern);



            var startPlayer = this.players.FirstOrDefault();

            
            foreach (var position in formation.Positions)
            {
                Console.WriteLine(Setup());
                foreach (var pos in formation.Positions)
                {
                    pos.Player = null;
                }
            }

        }

        private string Setup(Position position = null, Player player = null)
        {
            if (position == null)
            {
                position = this.formation.Positions.FirstOrDefault(p => p.Player == null);
                if (position == null)
                {
                    // DONE
                    return "DONE";
                }
            }

            if (player == null)
            {
                player = this.players.FirstOrDefault(pl => pl.Position == position.PositionEnum);
            }
            
            var notMathingNeighbors = position.TiedPositions.Where(tp => tp.Player != null && !player.IsGreen(tp.Player));
            if (notMathingNeighbors.Any())
            {
                return "ERROR";
            }

            position.Player = player;

            foreach (var tiedPosition in position.TiedPositions)
            {
                if (tiedPosition.Player == null)
                {

                    List<int> notMathing = new List<int>();
                    string status = string.Empty;
                    do
                    {
                        var nextPlayer = this.players.FirstOrDefault(pl => !notMathing.Contains(pl.BaseId) &&
                                                             pl.Position == tiedPosition.PositionEnum &&
                                                             ((pl.Club == player.Club) || (pl.Nation == player.Nation && pl.League == player.League)) &&
                                                             !this.formation.Positions.Any(pos => pos.Player != null && pos.Player.BaseId == pl.BaseId));

                        if (nextPlayer == null)
                        {
                            position.Player = null;
                            return "ERROR";
                        }
                        notMathing.Add(nextPlayer.BaseId);

                        status = this.Setup(tiedPosition, nextPlayer);

                    } while (status == "ERROR");                    

                }
            }

            return string.Empty;
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
