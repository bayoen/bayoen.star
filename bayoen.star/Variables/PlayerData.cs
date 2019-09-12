using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bayoen.library.General.Enums;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class PlayerData : FncJson
    {
        public PlayerData()
        {
            this.Reset();
        }

        public PlayerData(int index)
        {
            this.Set(index);
        }


        public void Reset()
        {
            this.Index = -1;
            this.ID32 = -1;
            this.Name = "";
            this.NameOnline = "";
            this.NameRaw = "";
            this.NameLocal = "";
            this.Rating = -1;
        }

        public void Set(int index) => this.Set(index, Core.Memory.PlayType(index) ? PlayTypes.Tetris : PlayTypes.PuyoPuyo);

        public void Set(int index, PlayTypes playType)
        {
            this.Index = index;
            this.ID32 = Core.Memory.ID32Forced(index);  // Check 'PlayerAddress'
            this.Name = "";
            this.NameOnline = Core.Memory.NameOnline(index);  // Require 'PlayerAddress'
            this.NameRaw = Core.Memory.NameRaw(index);  // Require 'PlayerAddress'
            this.NameLocal = Core.Memory.NameLocal(index);
            this.Rating = Core.Memory.Rating(index);
        }

        public int Index { get; set; }
        public int ID32 { get; set; }
        public string Name { get; set; }
        public string NameOnline { get; set; }
        public string NameRaw { get; set; }
        public string NameLocal { get; set; }
        public int Rating { get; set; }
        public PlayTypes PlayType { get; set; }

    }
}
