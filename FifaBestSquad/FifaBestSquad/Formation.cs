using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace FifaBestSquad
{
    internal class Formation
    {

        public Formation(string formationPattern)
        {
            this.Pattern = formationPattern;

            this.Positions = new List<Position>();

            this.SetPositions();

        }

        public string Pattern { get; set; }

        public List<Position> Positions { get; set; }
        
        //    _    
        //_       _
        //    _    
        //  _   _  
        //_       _
        //  _   _  
        //    _    

        private void SetPositions()
        {
            // CREATING 4-3-3
            
            var st = new Position { PositionEnum = PositionEnum.ST, Index = 'A'};
            this.Positions.Add(st);
            var rw = new Position { PositionEnum = PositionEnum.RW, Index = 'B' };
            this.Positions.Add(rw);
            var lw = new Position { PositionEnum = PositionEnum.LW, Index = 'C' };
            this.Positions.Add(lw);
            var cam = new Position { PositionEnum = PositionEnum.CAM, Index = 'D' };
            this.Positions.Add(cam);
            var cm1 = new Position { PositionEnum = PositionEnum.CM, Index = 'E' };
            this.Positions.Add(cm1);
            var cm2 = new Position { PositionEnum = PositionEnum.CM, Index = 'F' };
            this.Positions.Add(cm2);
            var rb = new Position { PositionEnum = PositionEnum.RB, Index = 'G' };
            this.Positions.Add(rb);
            var lb = new Position { PositionEnum = PositionEnum.LB, Index = 'H' };
            this.Positions.Add(lb);
            var cb1 = new Position { PositionEnum = PositionEnum.CB, Index = 'I' };
            this.Positions.Add(cb1);
            var cb2 = new Position { PositionEnum = PositionEnum.CB, Index = 'J' };
            this.Positions.Add(cb2);
            var gk = new Position { PositionEnum = PositionEnum.GK, Index = 'K' };
            this.Positions.Add(gk);

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

    }
}