﻿using FifaBestSquad;
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


            Class3 analyzer = new Class3();

            analyzer.BuildPerfectSquad();

            Console.ReadLine();
        }
    }
}
