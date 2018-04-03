using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FifaBestSquad
{
    internal class Formation
    {
        public string Pattern { get; set; }

        public List<Position> Positions { get; set; }

        public Formation(string formationPattern)
        {
            Pattern = formationPattern;

            Positions = new List<Position>();


            SetPositions();

        }

        private void SetPositions()
        {
            // CREATING 4-3-3
            
            var position = new Position { PositionEnum = PositionEnum.ST };
            AddLigations(position, PositionEnum.RW, PositionEnum.LW, PositionEnum.CAM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.RW };
            AddLigations(position, PositionEnum.ST, PositionEnum.RB, PositionEnum.CM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.LW };
            AddLigations(position, PositionEnum.ST, PositionEnum.CM, PositionEnum.LB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.CAM };
            AddLigations(position, PositionEnum.ST, PositionEnum.CM, PositionEnum.CM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.CM };
            AddLigations(position, PositionEnum.CAM, PositionEnum.RW, PositionEnum.RB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.CM };
            AddLigations(position, PositionEnum.CAM, PositionEnum.LW, PositionEnum.LB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.RB };
            AddLigations(position, PositionEnum.RW, PositionEnum.CM, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.LB };
            AddLigations(position, PositionEnum.LW, PositionEnum.CM, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.CB };
            AddLigations(position, PositionEnum.RB, PositionEnum.CM, PositionEnum.CB, PositionEnum.GK, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.CB };
            AddLigations(position, PositionEnum.LB, PositionEnum.CM, PositionEnum.CB, PositionEnum.GK, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

            position = new Position { PositionEnum = PositionEnum.GK };
            AddLigations(position, PositionEnum.CB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(position);

        }

        private void AddLigations(Position p, PositionEnum p1, PositionEnum p2, PositionEnum p3, PositionEnum p4, PositionEnum p5, PositionEnum p6)
        {
            p.Ligations.Add(new Ligation
            {
                PositionPlayer1 = p.PositionEnum,
                PositionPlayer2 = p1
            });

            if(p2 != PositionEnum.NONE)
            {
                p.Ligations.Add(new Ligation
                {
                    PositionPlayer1 = p.PositionEnum,
                    PositionPlayer2 = p2
                });
            }
            if (p3 != PositionEnum.NONE)
            {
                p.Ligations.Add(new Ligation
                {
                    PositionPlayer1 = p.PositionEnum,
                    PositionPlayer2 = p3
                });
            }
            if (p4 != PositionEnum.NONE)
            {
                p.Ligations.Add(new Ligation
                {
                    PositionPlayer1 = p.PositionEnum,
                    PositionPlayer2 = p4
                });
            }
            if (p5 != PositionEnum.NONE)
            {
                p.Ligations.Add(new Ligation
                {
                    PositionPlayer1 = p.PositionEnum,
                    PositionPlayer2 = p5
                });
            }
            if (p6 != PositionEnum.NONE)
            {
                p.Ligations.Add(new Ligation
                {
                    PositionPlayer1 = p.PositionEnum,
                    PositionPlayer2 = p6
                });
            }
        }
    }
}