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
            DirectoryInfo d = new DirectoryInfo(Path);
            var files = d.GetFiles("*.json");
            return this.GetFromJson(files);
        }

        public Formation GetFormationByPattern(string pattern)
        {
            DirectoryInfo d = new DirectoryInfo(Path);
            var files = d.GetFiles(string.Format("{0}.json", pattern));
            return this.GetFromJson(files).FirstOrDefault();
        }

        private List<Formation> GetFromJson(FileInfo[] files)
        {
            var formations = new List<Formation>();

            foreach (var file in files)
            {
                try
                {
                    using (StreamReader sr = new StreamReader(Path + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        dynamic form = JObject.Parse(line);
                        string pattern = (string)form.Pattern;
                        Formation formation = new Formation(pattern);
                        formation.Permutations = form.Permutations.ToObject<List<string>>();

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
