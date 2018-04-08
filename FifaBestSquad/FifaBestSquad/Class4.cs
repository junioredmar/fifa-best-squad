using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    // Classe usada para criar os CAMINHOS UNICOS
    public class Class4
    {
        private const string Path = "../../content";

        private List<Player> players;

        private Formation formation;

        private List<Queue<char>> positionQueues;


        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            // SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            this.players = this.players.OrderByDescending(p => p.Rating).ToList();
            //this.players = this.players.Where(p => p.Rating > 78 && p.Rating <= 86).ToList();
            this.formation = new Formation("4-3-3");

            this.BuildOrder();

        }

        private void BuildOrder()
        {

            foreach (var position in this.formation.Positions)
            {

                List<char> charList = new List<char>();
                this.SetupOrder(position, charList);
                position.Paths.Add(charList);

                // LOGGING
                foreach (var item in charList)
                {
                    Console.Write(item);
                }
                Console.WriteLine("-------------------");

                // Cleaning
                foreach (var pos in this.formation.Positions)
                {
                    pos.Visited = false;
                }

            }
        }



        //    A    
        //C       B
        //    D    
        //  E   F  
        //H       G
        //  I   J  
        //    K    

        private void SetupOrder(Position position, List<char> stack)
        {
            stack.Add(position.Index);
            position.Visited = true;

            var nextPosition = position.TiedPositions.FirstOrDefault(tp => !tp.Visited);
            if (nextPosition == null)
            {
                return;
            }

            this.SetupOrder(nextPosition, stack);
        }


    }
}
