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

        private const string PathResults = "../../uniquePaths";

        private List<string> uniquePaths;

        public List<string> CreateUniquePath()
        {
            this.formation = new Formation("4-3-3");

            uniquePaths = new List<string>();
            this.GetUniques();

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
            Directory.CreateDirectory(PathResults);
            File.WriteAllText(string.Format("{0}/{1}.json", PathResults, formation.Pattern), toSave);
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
