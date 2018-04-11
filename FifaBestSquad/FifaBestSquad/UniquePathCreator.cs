using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    public class UniquePathCreator
    {
        private List<Player> players;

        private Formation formation;

        private const string PathUnique = "../../uniquePaths";

        private List<string> uniquePaths;

        public List<string> CreateUniquePath(Formation formation)
        {
            this.formation = formation;

            this.uniquePaths = new List<string>();
            if (this.GotFromJson(formation.Pattern))
            {
                return this.uniquePaths;
            }

            Console.WriteLine("Building unique paths - START");

            this.GetUniques();

            Console.WriteLine("Building unique paths - DONE ");

            return this.uniquePaths;
        }

        private void GetUniques()
        {

            var permutations = new BuildPermutations();
            permutations.Build("ABCDEFGHIJK");

            foreach (var permutation in permutations.results)
            {
                if (this.IsUnique(permutation))
                {
                    this.uniquePaths.Add(permutation);
                }
            }
            Console.WriteLine("DONE");

            //SAVING TO JSON
            var toSave = JsonConvert.SerializeObject(this.uniquePaths);
            Directory.CreateDirectory(PathUnique);
            File.WriteAllText(string.Format("{0}/{1}.json", PathUnique, this.formation.Pattern), toSave);
        }


        private bool GotFromJson(string formation)
        {
            DirectoryInfo d = new DirectoryInfo(PathUnique);

            foreach (var file in d.GetFiles(formation + ".json"))
            {
                try
                {
                    using (var sr = new StreamReader(PathUnique + "/" + file.Name))
                    {
                        var line = sr.ReadToEnd();
                        this.uniquePaths = JsonConvert.DeserializeObject<List<string>>(line);
                    }

                    if (this.uniquePaths.Any())
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file uniquePaths could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

            return false;
        }

        private bool IsUnique(string permutation)
        {

            for (var i = 0; i < permutation.Count(); i++)
            {
                var position = this.formation.Positions.FirstOrDefault(pos => pos.Index == permutation[i]);
                if (permutation.Count() <= i + 1)
                {
                    return true;
                }
                var tiedPositions = position.TiedPositions.Where(tp => tp.Index == permutation[i + 1]);
                if (!tiedPositions.Any())
                {
                    return false;
                }
            }
            return false;
        }
    }
}
