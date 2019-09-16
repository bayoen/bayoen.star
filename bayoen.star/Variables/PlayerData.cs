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
        
        public string Name { get; set; }
        public int ID32 { get; set; }
        //public long ID64 { get; set; }
        public short Rating { get; set; }
        public short PlayStyle { get; set; }

        public Leagues League { get; set; }
        public short LeagueWin { get; set; }
        public short LeagueLose { get; set; }

        public void Reset()
        {
            this.Name = "";
            this.ID32 = -1;
            //this.ID64 = -1;
            this.Rating = -1;
            this.PlayStyle = -1;

            this.League = (Leagues)0;
            this.LeagueWin = -1;
            this.LeagueLose = -1;
        }

        public void SetOnline(int index)
        {
            this.ID32 = Core.Memory.OnlineID32Forced(index);    // Check 'PlayerAddress'
            //this.ID64 = Core.Memory.OnlineID64(index);    // Require 'PlayerAddress'
            this.Name = Core.Memory.OnlineName(index);      // Require 'PlayerAddress'
            this.Rating = Core.Memory.OnlineRating(index);  // Require 'PlayerAddress'
            this.PlayStyle = Core.Memory.OnlinePlayStyle(index);    // Require 'PlayerAddress'

            this.League = (Leagues)Core.Memory.OnlineLeague(index);  // Require 'PlayerAddress'
            this.LeagueWin = Core.Memory.OnlineWin(index);  // Require 'PlayerAddress'
            this.LeagueLose = Core.Memory.OnlineLose(index);  // Require 'PlayerAddress'
        }

        public void SetLocal(int index)
        {
            this.ID32 = -1;  // Check 'PlayerAddress'
            //this.ID64 = -1;  // Require 'PlayerAddress'
            this.Name = Core.Memory.LocalName(index);
            this.Rating = -1;
            this.PlayStyle = -1;

            this.League = (Leagues)0;
            this.LeagueWin = -1;
            this.LeagueLose = -1;
        }
    }
}
