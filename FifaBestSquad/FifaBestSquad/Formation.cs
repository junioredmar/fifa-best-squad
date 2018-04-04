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
            
            var st = new Position { PositionEnum = PositionEnum.ST };
            AddLigations(st, PositionEnum.RW, PositionEnum.LW, PositionEnum.CAM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(st);

            var rw = new Position { PositionEnum = PositionEnum.RW };
            AddLigations(rw, PositionEnum.ST, PositionEnum.RB, PositionEnum.CM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(rw);

            var lw = new Position { PositionEnum = PositionEnum.LW };
            AddLigations(lw, PositionEnum.ST, PositionEnum.CM, PositionEnum.LB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(lw);

            var cam = new Position { PositionEnum = PositionEnum.CAM };
            AddLigations(cam, PositionEnum.ST, PositionEnum.CM, PositionEnum.CM, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(cam);

            var cm1 = new Position { PositionEnum = PositionEnum.CM };
            AddLigations(cm1, PositionEnum.CAM, PositionEnum.LW, PositionEnum.LB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(cm1);

            var cm2 = new Position { PositionEnum = PositionEnum.CM };
            AddLigations(cm2, PositionEnum.CAM, PositionEnum.RW, PositionEnum.RB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(cm2);

            var rb = new Position { PositionEnum = PositionEnum.RB };
            AddLigations(rb, PositionEnum.RW, PositionEnum.CM, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(rb);

            var lb = new Position { PositionEnum = PositionEnum.LB };
            AddLigations(lb, PositionEnum.LW, PositionEnum.CM, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(lb);

            var cb1 = new Position { PositionEnum = PositionEnum.CB };
            AddLigations(cb1, PositionEnum.LB, PositionEnum.CM, PositionEnum.CB, PositionEnum.GK, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(cb1);

            var cb2 = new Position { PositionEnum = PositionEnum.CB };
            AddLigations(cb2, PositionEnum.RB, PositionEnum.CM, PositionEnum.CB, PositionEnum.GK, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(cb2);

            var gk = new Position { PositionEnum = PositionEnum.GK };
            AddLigations(gk, PositionEnum.CB, PositionEnum.CB, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE, PositionEnum.NONE);
            Positions.Add(gk);

            gk.TiedPositions.Add(cb1);
            gk.TiedPositions.Add(cb2);

            cb1.TiedPositions.Add(gk);
            cb1.TiedPositions.Add(cb2);
            cb1.TiedPositions.Add(lb);

            cb2.TiedPositions.Add(gk);
            cb2.TiedPositions.Add(cb1);
            cb2.TiedPositions.Add(rb);

            rb.TiedPositions.Add(cb2);
            rb.TiedPositions.Add(rw);
            rb.TiedPositions.Add(cm2);

            lb.TiedPositions.Add(cb1);
            lb.TiedPositions.Add(lw);
            lb.TiedPositions.Add(cm1);

            cm2.TiedPositions.Add(cb2);
            cm2.TiedPositions.Add(rb);
            cm2.TiedPositions.Add(cam);
            cm2.TiedPositions.Add(rw);

            cm1.TiedPositions.Add(cb1);
            cm1.TiedPositions.Add(lb);
            cm1.TiedPositions.Add(cam);
            cm1.TiedPositions.Add(lw);

            cam.TiedPositions.Add(st);
            cam.TiedPositions.Add(cm1);
            cam.TiedPositions.Add(cm2);

            rw.TiedPositions.Add(rb);
            rw.TiedPositions.Add(st);
            rw.TiedPositions.Add(cm2);

            lw.TiedPositions.Add(lb);
            lw.TiedPositions.Add(st);
            lw.TiedPositions.Add(cm1);

            st.TiedPositions.Add(cam);
            st.TiedPositions.Add(rw);
            st.TiedPositions.Add(lw);
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