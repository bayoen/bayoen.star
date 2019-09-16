using System;
using System.Collections.Generic;

using Newtonsoft.Json;

using bayoen.library.General.Enums;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class MatchRecord : FncJson, ICloneable
    {
        public MatchRecord()
        {
            this.Teams = new List<int>();

            this.Games = new List<GameRecord>();
            this.Players = new List<PlayerData>();
            this.Winners = new List<int>();

            this.Reset();
        }

        private bool _isInitialized;
        private bool _isFinished;
        private bool _isTerminated;

        [JsonIgnore]
        public int ID { get; set; }
        public BrokenTypes BrokenType { get; set; }
        public MatchCategories MatchCategory { get; set; }
        public bool IsBroken { get; set; }
        public int RoomSize { get;  set; }
        public int RoomMax { get; set; }
        public List<int> Teams { get; set; }

        public int MyID32 { get; set; }
        public int RatingGain { get; set; }

        public DateTime Begin { get; set; }
        public DateTime End { get; set; }

        public List<GameRecord> Games { get; set; }
        public List<PlayerData> Players { get; set; }
        public List<int> Winners { get; set; }
        public int WinCount { get; set; }

        public void Reset()
        {
            this.BrokenType = BrokenTypes.None;
            this.MatchCategory = MatchCategories.None;
            this.RoomSize = -1;
            this.RoomMax = -1;
            this.Teams.Clear();

            this.MyID32 = -1;
            this.RatingGain = 0;

            this.Begin = DateTime.MinValue;
            this.End = DateTime.MinValue;

            this.Games.Clear();
            this.Players.Clear();
            this.Winners.Clear();
            this.WinCount = -1;
        }

        public void AdjustGames()
        {
            foreach (GameRecord tokenGame in this.Games)
            {
                tokenGame.MatchCategory = this.MatchCategory;
                tokenGame.RoomSize = this.RoomSize;
                tokenGame.RoomMax = this.RoomMax;
                tokenGame.Teams = this.Teams;

                if (this.RoomSize < 4)
                {
                    tokenGame.ScoreTicks.RemoveRange(this.RoomSize, 4 - this.RoomSize);
                }                
            }
        }

        public void Terminate()
        {
            this.RatingGain = Core.Data.MyRating - Core.Old.MyRating;
        }

        public void SetInitialized() => this._isInitialized = true;
        public void RemoveInitialized() => this._isInitialized = false;
        public bool GetInitialized() => this._isInitialized;
        public void SetFinished() => this._isFinished = true;
        public void RemoveFinished() => this._isFinished = true;
        public bool GetFinished() => this._isFinished;
        public void SetTerminated() => this._isTerminated = true;
        public void RemoveTerminated() => this._isTerminated = true;
        public bool GetTerminated() => this._isTerminated;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
