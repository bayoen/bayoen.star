using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;

using bayoen.library.General.Enums;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class GameRecord : FncJson, ICloneable
    {
        public GameRecord()
        {
            this.Winners = new List<int>();

            this.FrameTicks = new List<int>();
            this.ScoreTicks = new List<List<int>>(4);

            this.Reset();
        }

        private bool _isInitialized;
        private bool _isFinished;
        private bool _isTerminated;


        [JsonIgnore]
        public int ID { get; set; }
        public bool IsDummy { get; set; }
        public BrokenTypes BrokenType { get; set; }
        public MatchCategories MatchCategory { get; set; }        
        public int RoomSize { get; set; }
        public int RoomMax { get; set; }
        public List<int> Teams { get; set; }

        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
        public int? Frame { get; set; }
        public List<int> Winners { get; set; }

        public List<int> FrameTicks { get; set; }
        public List<List<int>> ScoreTicks { get; set; }

        public void Reset()
        {
            this._isInitialized = false;
            this._isFinished = false;

            this.IsDummy = false;
            this.BrokenType = BrokenTypes.None;
            this.MatchCategory = MatchCategories.None;            
            this.RoomSize = -1;
            this.RoomMax = -1;
            
            this.Begin = DateTime.MinValue;
            this.End = DateTime.MinValue;
            this.Frame = -1;

            this.Winners.Clear();

            this.FrameTicks.Clear();
            this.FrameTicks.Add(0);

            this.ScoreTicks.Clear();
            Enumerable.Range(0, 4).ToList().ForEach(x =>
            {
                this.ScoreTicks.Add(new List<int>() { 0 });
            });
        }

        public void SetInitialized() => this._isInitialized = true;
        public bool GetInitialized() => this._isInitialized;
        public void SetFinished() => this._isFinished = true;
        public bool GetFinished() => this._isFinished;
        public void SetTerminated() => this._isTerminated = true;
        public bool GetTerminated() => this._isTerminated;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
