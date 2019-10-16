using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using bayoen.library.General.Enums;
using bayoen.star.Functions;
using LiteDB;

namespace bayoen.star.Variables
{
    public class GameRecord : FncJson, ICloneable
    {
        public GameRecord()
        {
            //this.Winners = new List<int>();

            this.FrameTicks = new List<int>();
            this.ScoreTicks = new List<List<int>>(4);

            this.Reset();
        }
        
        public int Index { get; set; }
        public bool IsDummy { get; set; }
        public BrokenTypes BrokenType { get; set; }
        public MatchCategories MatchCategory { get; set; }
        public GameModes GameMode { get; set; }
        public int RoomSize { get; set; }
        public int RoomMax { get; set; }
        public List<int> Teams { get; set; }

        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public int? Frame { get; set; }
        public int WinnerTeam { get; set; }

        public List<int> FrameTicks { get; set; }
        public List<List<int>> ScoreTicks { get; set; }

        public void Reset()
        {
            this.IsInitialized = false;
            this.IsFinished = false;

            this.Index = -1;
            this.IsDummy = false;
            this.BrokenType = BrokenTypes.None;
            this.MatchCategory = MatchCategories.None;
            this.GameMode = GameModes.None;
            this.RoomSize = -1;
            this.RoomMax = -1;
            
            this.Begin = DateTime.MinValue;
            this.End = DateTime.MinValue;
            this.Frame = -1;

            this.WinnerTeam = -1;
            //this.Winners.Clear();

            this.FrameTicks.Clear();
            this.FrameTicks.Add(0);

            this.ScoreTicks.Clear();
            Enumerable.Range(0, 4).ToList().ForEach(x =>
            {
                this.ScoreTicks.Add(new List<int>() { 0 });
            });
        }

        [BsonIgnore, JsonIgnore]
        public bool IsInitialized { get; set; }
        [BsonIgnore, JsonIgnore]
        public bool IsFinished { get; set; }

        public object Clone()
        {
            GameRecord output = new GameRecord();



            return output;
        }


        [BsonIgnore, JsonIgnore]
        public string GameString => this.Serialize();
    }
}
