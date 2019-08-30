using System;
using System.Collections.Generic;

using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class GameData : FncToJson, ICloneable
    {
        public GameData()
        {
            this.GameStates = new GameStates();
            this.Stars = new List<int>() { 0, 0, 0, 0 };
            this.Players = new List<PlayerData>();
            this.WinCount = -1;
            this.PlayerCount = -1;
            this.PlayerMax = -1;
        }

        public GameStates GameStates { get; set; }
        public List<int> Stars { get; set; }
        public List<PlayerData> Players { get; set; }

        public int WinCount { get; set; }
        public int PlayerCount { get; set; }
        public int PlayerMax { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
