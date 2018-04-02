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

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CF,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CF,
                    PositionPlayer2 = PositionEnum.RW
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CF,
                    PositionPlayer2 = PositionEnum.LW
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CF,
                    PositionPlayer2 = PositionEnum.CAM
                }
            });

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.RW,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RW,
                    PositionPlayer2 = PositionEnum.CF
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RW,
                    PositionPlayer2 = PositionEnum.RB
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RW,
                    PositionPlayer2 = PositionEnum.CM
                }
            });

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.LW,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LW,
                    PositionPlayer2 = PositionEnum.CF
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LW,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LW,
                    PositionPlayer2 = PositionEnum.LB
                }
            });


            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CAM,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CAM,
                    PositionPlayer2 = PositionEnum.ST
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CAM,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CAM,
                    PositionPlayer2 = PositionEnum.CM
                }
            });


            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CM,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.CAM
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.RW
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.RB
                },
                Ligation4 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.CB
                }
            });

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CM,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.CAM
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.LW
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.LB
                },
                Ligation4 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CM,
                    PositionPlayer2 = PositionEnum.CB
                }
            });

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.RB,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RB,
                    PositionPlayer2 = PositionEnum.RW
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RB,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.RB,
                    PositionPlayer2 = PositionEnum.CB
                }
            });


            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.LB,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LB,
                    PositionPlayer2 = PositionEnum.LW
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LB,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.LB,
                    PositionPlayer2 = PositionEnum.CB
                }
            });


            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CB,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.RB
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.CB
                },
                Ligation4 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.GK
                }
            });


            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.CB,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.LB
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.CM
                },
                Ligation3 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.CB
                },
                Ligation4 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.CB,
                    PositionPlayer2 = PositionEnum.GK
                }
            });

            Positions.Add(new Position
            {
                PositionEnum = PositionEnum.GK,
                Ligation1 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.GK,
                    PositionPlayer2 = PositionEnum.CB
                },
                Ligation2 = new Ligation
                {
                    PositionPlayer1 = PositionEnum.GK,
                    PositionPlayer2 = PositionEnum.CB
                }
            });
        }
    }
}