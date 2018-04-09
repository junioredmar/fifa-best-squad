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

        public List<string> CreateUniquePath()
        {
            Console.WriteLine("Building unique paths - START");
            this.formation = new Formation("4-3-3");

            uniquePaths = new List<string>();

            if (GotFromJson(formation.Pattern))
            {
                return uniquePaths;
            }

            this.GetUniques();

            Console.WriteLine("Building unique paths - DONE ");

            return uniquePaths;
        }

        private void GetUniques()
        {

            BuildPermutations permutations = new BuildPermutations();
            permutations.Build("ABCDEFGHIJK");

            foreach (var permutation in permutations.results)
            {
                if (IsUnique(permutation))
                {
                    uniquePaths.Add(permutation);
                }
            }
            Console.WriteLine("DONE");

            //SAVING TO JSON
            var toSave = JsonConvert.SerializeObject(uniquePaths);
            Directory.CreateDirectory(PathUnique);
            File.WriteAllText(string.Format("{0}/{1}.json", PathUnique, formation.Pattern), toSave);
        }


        public bool GotFromJson(string formation)
        {
            DirectoryInfo d = new DirectoryInfo(PathUnique);

            foreach (var file in d.GetFiles(formation + ".json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(PathUnique + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        uniquePaths = JsonConvert.DeserializeObject<List<string>>(line);
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

            for (int i = 0; i < permutation.Count(); i++)
            {
                var position = formation.Positions.FirstOrDefault(pos => pos.Index == permutation[i]);
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
