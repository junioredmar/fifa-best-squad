﻿using FifaBestSquad.Utils;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FifaBestSquad
{
    public class Class1
    {
        List<Player> _players;
        Formation _formation;

        private const string Path = "../../content";

        public void BuildPerfectSquad()
        {
            //GetFromPlayersFromEa();

            SetPlayersToMemory();

            BuildSquad();
        }

        private void BuildSquad()
        {
            _players = _players.OrderByDescending(p => p.Rating).ToList();

            _players = _players.Where(p => p.League.ToLower().StartsWith("pre")).ToList();

            string formationPattern = "4-3-3";
            _formation = new Formation(formationPattern);





            var startPlayer = _players.FirstOrDefault();

            Setup(startPlayer, null);

        }

        private string Setup(Player player, Player previousPlayer)
        {
            Position position;
            if (previousPlayer == null)
            {
                // PRIMEIRA VEZ PASSA POR AQUI
                position = _formation.Positions.FirstOrDefault(p => p.PositionEnum == player.Position);
            }
            else
            {
                // PEGA A POSITION QUE O PLAYER VAI SER COLOCADO
                position = _formation.Positions.FirstOrDefault(
                    p => p.PositionEnum == player.Position &&
                    p.Ligations.Any(l => l.Player1 == null && l.PositionPlayer2 == previousPlayer.Position));


                // VERIFICANDO SE AS POSIÇÕES AO REDOR ESTAO VERDE, SE NAO ESTIVER, UNDO

                // ESSE FILTRO DEVE PEGAR AS POSIÇÕES CORRETAS!!!!
                var nextPositionsWithPlayers = _formation.Positions.Where(p => p.Player != null &&
                    position.Ligations.Select(l => l.PositionPlayer2).Contains(p.PositionEnum)
                    ).ToList();

                if (nextPositionsWithPlayers.Any())
                {
                    foreach (var nextPosition in nextPositionsWithPlayers)
                    {
                        // check if nextPosition.player is null
                        if (!player.IsGreen(nextPosition.Player))
                        {
                            player.Name = player.Name + " UNDO";
                            // UNDO!
                        }
                    }
                }
                
                // LIGA A POSITION ATUAL COM A ANTERIOR
                var ligationWithPrevious = position.Ligations.FirstOrDefault(l => l.Player2 == null && l.PositionPlayer2 == previousPlayer.Position);
                ligationWithPrevious.Player1 = player;
                ligationWithPrevious.Player2 = previousPlayer;
            }


            // COLOCA O PLAYER NA POSIÇÃO
            position.Player = player;

            // PEGA A PROXIMA LIGAÇÃO VAZIA 
            var ligation = position.Ligations.FirstOrDefault(l => l.Player2 == null);

            while (ligation != null)
            {
                bool found = false;

                // PARA ESSA LIGAÇÃO, SETA O PLAYER ATUAL COMO PLAYER 1
                ligation.Player1 = player;

                // BUSCA TODAS AS POSITIONS DA FORMATION QUE TEM A POSITION DO PLAYER 2
                var nextPositionInFormation = _formation.Positions.FirstOrDefault(pos =>
                    pos.Ligations.Any(l => l.Player1 != null
                                           && l.Player1 != player
                                           && l.PositionPlayer1 == ligation.PositionPlayer2)
                    && !pos.Ligations.Any(l => l.Player2 == player));
                
                if (nextPositionInFormation != null)
                {
                    // O PROXIMO JOGADOR JA ESTÁ NO SQUAD - COMPARAR
                    if (player.IsGreen(nextPositionInFormation.Player))
                    {
                        // JA QUE ESTAMOS VALIDANDO OS VERDES ANTES, 
                        // TALVEZ NAO PRECISE DESSE STEP
                        // SOMENTE PEGAR A PROXIMA LIGATION
                        // ligation = position.Ligations.FirstOrDefault(l => l.Player2 == null);
                        // CONTINUE;

                        ligation.Player2 = nextPositionInFormation.Player;
                        Setup(nextPositionInFormation.Player, player);
                        // DEU MATCH
                        found = true;
                    }
                    else
                    {
                        // TAMBEM ACHO QUE NUNCA VAI CAIR AQUI. MESMO ASSIM VALIDAR - COLOCAR UM BREAK POINT AQUI
                        // DESFAZ - UNDO!

                    }
                }
                else
                {
                    // A PROXIMA POSITION ESTA VAZIA, ENTAO BUSCA O PROXIMO PLAYER

                    var nextPlayer = _players.FirstOrDefault(pl => pl.Position == ligation.PositionPlayer2 &&
                                                                  ((pl.Club == ligation.Player1.Club) ||
                                                                   (pl.Nation == ligation.Player1.Nation &&
                                                                    pl.League == ligation.Player1.League)) &&
                                                                    !_formation.Positions.Any(
                                                                        pos => pos.Player != null &&
                                                                        pos.Player.BaseId == pl.BaseId)
                                                                    );

                    if (nextPlayer != null)
                    {
                        if (ligation.Player2 != null)
                        {
                            //JA TEM LIGAÇÃO COM O PROXIMO
                            // ACHO QUE NAO É PRA CAIR AQUI. JA QUE FOI FILTRADO POR LIGATION.PLAYER2 == NULL
                            // MESMO ASSIM, COLOCAR UM BREAKPOINT AQUI
                        }
                        else
                        {
                            ligation.Player2 = nextPlayer;

                            // FAZ PROXIMA LIGACAO
                            Setup(nextPlayer, player);
                            
                            
                            found = true;
                        }
                        
                    }
                    else
                    {
                        //UNDO
                        // found = false
                    }
                }

                
                if (found)
                {
                    ligation = position.Ligations.FirstOrDefault(l => l.Player2 == null);
                }
                else
                {
                    // AINDA TEM POSIÇÕES PENDENTES?
                    ligation = null;
                }
            }

            return string.Empty;
        }




        private void SetPlayersToMemory()
        {
            _players = new List<Player>();

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

                            _players.Add(new Player
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
