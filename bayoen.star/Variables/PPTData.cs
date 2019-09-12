using System;
using System.Collections.Generic;

using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class PPTData : FncJson, ICloneable
    {
        public PPTData()
        {
            this.States = new PPTStates();
            this.Stars = new List<int>(4) { 0, 0, 0, 0 };
            this.Scores = new List<int>(4) { 0, 0, 0, 0 };
            this.GameFrame = 0;
        }

        public PPTStates States { get; set; }
        public int MyRating { get; set; }
        public List<int> Stars { get; set; }


        public int GameFrame { get; set; }
        public List<int> Scores { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
