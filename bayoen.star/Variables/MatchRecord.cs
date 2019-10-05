using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using LiteDB;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.star.Functions;
using System.Linq;

namespace bayoen.star.Variables
{
    public class MatchRecord : FncJson, ICloneable
    {
        public MatchRecord()
        {
            this.Teams = new List<int>();

            this.Games = new List<GameRecord>();
            this.Players = new List<PlayerData>();
            //this.Winners = new List<int>();

            this.Reset();
        }

        [JsonIgnore]
        public int ID { get; set; }
        public BrokenTypes BrokenType { get; set; }
        public MatchCategories Category { get; set; }
        public GameModes Mode { get; set; }
        public int RoomSize { get;  set; }
        public int RoomMax { get; set; }
        public List<int> Teams { get; set; }

        public int MyID32 { get; set; }
        public int RatingGain { get; set; }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        [JsonIgnore]
        public List<GameRecord> Games { get; set; }
        public List<PlayerData> Players { get; set; }
        public int WinnerTeam { get; set; }
        public int WinCount { get; set; }        

        public void Reset()
        {
            this.IsInitialized = false;
            this.IsFinished = false;

            this.BrokenType = BrokenTypes.None;
            this.Category = MatchCategories.None;
            this.Mode = GameModes.None;
            this.RoomSize = -1;
            this.RoomMax = -1;
            this.Teams.Clear();

            this.MyID32 = -1;
            this.RatingGain = 0;

            this.Begin = DateTime.MinValue;
            this.End = DateTime.MinValue;

            this.Games.Clear();
            this.Players.Clear();
            //this.Winners.Clear();
            this.WinnerTeam = -1;
            this.WinCount = -1;            
        }

        public void AdjustGames()
        {
            foreach (GameRecord tokenGame in this.Games)
            {
                tokenGame.MatchCategory = this.Category;
                tokenGame.GameMode = this.Mode;
                tokenGame.RoomSize = this.RoomSize;
                tokenGame.RoomMax = this.RoomMax;
                tokenGame.Teams = this.Teams;

                if (tokenGame.ScoreTicks != null)
                {
                    if (this.RoomSize < 4)
                    {
                        if (tokenGame.ScoreTicks.Count != this.RoomSize)
                        {
                            tokenGame.ScoreTicks.RemoveRange(this.RoomSize, 4 - this.RoomSize);
                        }
                    }
                }                
            }
        }

        [BsonIgnore, JsonIgnore]
        public bool IsInitialized { get; set; }
        [BsonIgnore, JsonIgnore]
        public bool IsFinished { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public object RepeatDeepClone()
        {
            return new MatchRecord
            {
                Category = this.Category,
                Mode = this.Mode,
                RoomSize = this.RoomSize,
                RoomMax = this.RoomMax,
                Teams = new List<int>(this.Teams),

                MyID32 = this.MyID32,
                Players = new List<PlayerData>(this.Players.ConvertAll(x => x.Clone() as PlayerData)),
                WinCount = this.WinCount,
            };                        
        }

        [BsonIgnore, JsonIgnore]
        public string MatchString => this.Serialize();
        [BsonIgnore, JsonIgnore]
        public List<int> WinRecord => this.Games.ConvertAll(x => x.WinnerTeam);
        [BsonIgnore, JsonIgnore]
        public List<int> WinStack
        {
            get
            {
                List<int> output = new List<int>() { 0, 0, 0, 0 };
                List<int> record = this.WinRecord;
                foreach (int winningTeam in record)
                {
                    if (winningTeam > 0) output[winningTeam - 1]++;
                }

                return output;
            }
        }
    }
}
