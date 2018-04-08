using FifaBestSquad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifaBestSquadMain
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");


            //Class4 analyzer = new Class4();
            SquadCreator analyzer = new SquadCreator();

            analyzer.BuildPerfectSquad();

            Console.ReadLine();
        }
    }
}
