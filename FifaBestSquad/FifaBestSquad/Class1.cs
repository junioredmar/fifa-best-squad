using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    public class Class1
    {
        List<Player> _players;
        Formation _formation;

        private const string Path = "../../content";

        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            _players = _players.OrderByDescending(p => p.Rating).ToList();

            _players = _players.Where(p => p.League.ToLower().StartsWith("pre")).ToList();

            string formationPattern = "4-3-3";
            _formation = new Formation(formationPattern);





            var startPlayer = _players.FirstOrDefault();

            Setup(startPlayer);

        }

        private string Setup(Player startPlayer)
        {
            var startPosition = _formation.Positions.FirstOrDefault(p => p.PositionEnum == startPlayer.Position);

            startPosition.Player = startPlayer;
            startPosition.Ligation1.Player1 = startPlayer;

            var nextPlayer = _players.FirstOrDefault(pl => pl.Position == startPosition.Ligation1.PositionPlayer2 &&
                                                          ((pl.Club == startPosition.Ligation1.Player1.Club) ||
                                                           (pl.Nation == startPosition.Ligation1.Player1.Nation &&
                                                            pl.League == startPosition.Ligation1.Player1.League)));

            if (nextPlayer != null)
            {
                startPosition.Ligation1.Player2 = nextPlayer;

                // do next ligation

                return nextPlayer.Name;
            }
            else
            {
                // return undo
                return "UNDO";
            }
        }




        private void SetPlayersToMemory()
        {
            _players = new List<Player>();

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

                            _players.Add(new Player
                            {
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
