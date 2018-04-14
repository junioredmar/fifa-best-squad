using FifaBestSquad;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FifaBestSquadMain.Repository
{
    public class FormationRepository
    {

        //private const string Path = "../../content/data/formation";
        private const string Path = "C:/MY_STUFFS/Projects/fifa/my_project/fifa-best-squad/FifaBestSquad/FifaBestSquadMain/content/data/formation";

        public List<Formation> GetFormationsFromJson()
        {
            Console.WriteLine("Bring all players to memory - START");

            var formations = new List<Formation>();

            DirectoryInfo d = new DirectoryInfo(Path);

            foreach (var file in d.GetFiles("*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        dynamic form = JObject.Parse(line);
                        string pattern = (string)form.Pattern;
                        Formation formation = new Formation(pattern);

                        for (int i = 0; i < form.Positions.Count; i++)
                        {
                            var jsonPosition = form.Positions[i];
                            var position = new Position
                            {
                                Index = jsonPosition.Index,
                                PositionEnum = jsonPosition.PositionEnum
                            };
                            formation.Positions.Add(position);
                        }
                        for (int i = 0; i < form.Positions.Count; i++)
                        {
                            var position = formation.Positions.ElementAt(i);

                            for (int j = 0; j < form.Positions[i].TiedPositions.Count; j++)
                            {
                                var tiedIndex = (char)form.Positions[i].TiedPositions[j].Index;
                                var tiedPosition = formation.Positions.First(pos => pos.Index == tiedIndex);
                                position.TiedPositions.Add(tiedPosition);
                            }
                        }

                        formations.Add(formation);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

            return formations;
        }
    }
}
