using System;
using System.Collections.Generic;

using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class GameData : FncToJson, ICloneable
    {
        public GameStates GameStates { get; set; }
        public List<int> Stars { get; set; }
        public List<PlayerData> Players { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
