using System;
using System.Collections.Generic;
using System.Text;

namespace FifaBestSquad
{
    using System.IO;

    using Newtonsoft.Json;

    public class ResultChecker
    {

        private const string PathResults = "../../results";

        public FormationViewModel GetResults()
        {
            var result = new FormationViewModel();

            DirectoryInfo d = new DirectoryInfo(PathResults);

            foreach (var file in d.GetFiles("*.json"))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(PathResults + "/" + file.Name))
                    {
                        string line = sr.ReadToEnd();
                        result = JsonConvert.DeserializeObject<FormationViewModel>(line);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("The file results could not be read:");
                    Console.WriteLine(e.Message);
                }

            }

            return result;
        }
    }
}
