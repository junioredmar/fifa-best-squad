using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    public class Class3
    {
        private const string Path = "../../content";

        private List<Player> players;

        private Formation formation;
        
        private List<Queue<char>> positionQueues;


        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            //SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            //this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            //this.players = this.players.Where(p => p.Rating > 78 && p.Rating <= 86).ToList();
            this.formation = new Formation("4-3-3");
            
            this.BuildOrder();

        }

        private void BuildOrder()
        {

            foreach (var position in this.formation.Positions)
            {
                bool allPossiblePathsGone = false;
                do
                {
                    var reached = this.SetupOrder(position, stack);
                    Console.WriteLine(reached);
                    position.Paths.Add(stack);

                    foreach (var item in stack)
                    {
                        Console.WriteLine(item);
                    }
                    Console.WriteLine("-------------------");
                }
                while (allPossiblePathsGone);

                // Cleaning
                position.Visited = false;
                foreach (var pos in this.formation.Positions)
                {
                    pos.Visited = false;
                }

            }
        }
        

        private string SetupOrder(Position position, List<char> stack)
        {
            stack.Add(position.Index);
            position.Visited = true;

            foreach (var tiedPosition in position.TiedPositions)
            {
                if (tiedPosition.Visited)
                {
                    continue;
                }

                string status;
                do
                {

                    var nextPosition = position.TiedPositions.FirstOrDefault(tp => !tp.Visited);

                    if (nextPosition == null)
                    {
                        return "REACHED";
                    }

                    status = this.SetupOrder(tiedPosition, stack);
                }
                while (status == "REACHED");
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
