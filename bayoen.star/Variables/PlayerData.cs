using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bayoen.star.Variables
{
    public class PlayerData
    {
        public PlayerData()
        {
            this.Name = "";
            this.RawName = "";
            this.ID32 = -1;
            this.Rating = -1;
        }

        public PlayerData(int number)
        {
            this.Name = $"Player {number}";
            this.RawName = $"Player {number}";
            this.ID32 = -1;
            this.Rating = -1;
        }

        public string Name { get; set; }
        public string RawName { get; set; }
        public int ID32 { get; set; }
        public int Rating { get; set; }

        public int RatingGain { get; set; }
    }
}
