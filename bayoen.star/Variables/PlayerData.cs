using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bayoen.library.General.Enums;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class PlayerData : FncJson, ICloneable
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

        public PuzzleLeagues PuzzleLeague { get; set; }
        public Locations LeagueLocation { get; set; }
        public short LeagueMedal { get; set; }
        public GameAvatars LeagueAvatar { get; set; }
        public int LeagueRegional { get; set; }
        public int LeagueWorldwide { get; set; }
        public short LeagueWin { get; set; }
        public short LeagueLose { get; set; }

        public GameAvatars GameAvatar { get; set; }
        public VoiceTypes GameVoice { get; set; }
        public PlayTypes PlayType { get; set; }
        public short Team { get; set; }

        public void Reset()
        {
            this.Name = "";
            this.ID32 = -1;
            //this.ID64 = -1;
            this.Rating = -1;
            this.PlayStyle = -1;

            this.PuzzleLeague = (PuzzleLeagues)0;
            this.LeagueLocation = (Locations)(-1);
            this.LeagueMedal = -1;
            this.LeagueAvatar = (GameAvatars)(-1);
            this.LeagueRegional = -1;
            this.LeagueWorldwide = -1;
            this.LeagueWin = -1;
            this.LeagueLose = -1;

            this.GameAvatar = (GameAvatars)(-1);
            this.GameVoice = (VoiceTypes)0;
            this.PlayType = (PlayTypes)(-1);
            this.Team = -1;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void FromOnlineMatchmaking(int index)
        {
            this.ID32 = Core.Memory.OnlineID32Forced(index);                    // Check 'PlayerAddress'
            //this.ID64 = Core.Memory.OnlineID64(index);                        // Require 'PlayerAddress'
            this.Name = Core.Memory.OnlineName(index);                          // Require 'PlayerAddress'
            this.Rating = Core.Memory.OnlineRating(index);                      // Require 'PlayerAddress'
            this.PlayStyle = Core.Memory.OnlinePlayStyle(index);                // Require 'PlayerAddress'

            this.PuzzleLeague = (PuzzleLeagues)Core.Memory.OnlineLeague(index); // Require 'PlayerAddress'
            this.LeagueLocation = (Locations)Core.Memory.OnlineLocation(index); // Require 'PlayerAddress'
            this.LeagueMedal = Core.Memory.OnlineMedal(index);                  // Require 'PlayerAddress'
            this.LeagueAvatar = (GameAvatars)Core.Memory.OnlineAvatar(index);   // Require 'PlayerAddress'
            this.LeagueRegional = Core.Memory.OnlineRegional(index);            // Require 'PlayerAddress'
            this.LeagueWorldwide = Core.Memory.OnlineWorldwide(index);          // Require 'PlayerAddress'
            this.LeagueWin = Core.Memory.OnlineWin(index);                      // Require 'PlayerAddress'
            this.LeagueLose = Core.Memory.OnlineLose(index);                    // Require 'PlayerAddress'
        }

        public void SetReadyOnline(int index)
        {
            this.GameAvatar = (GameAvatars)Mapping.SelectionToAvatar(Core.Memory.AvatorSelection(index));    // Check 'BaseAddress'
            this.GameVoice = (VoiceTypes)(Core.Memory.VoiceSelection(index) ? 1 : 0);
            this.PlayType = (PlayTypes)Core.Memory.TypeSelection(index);
            this.Team = (short)Core.Memory.Team(index);
        }

        public void FromCharacterSelection(int index)
        {
            this.GameAvatar = (GameAvatars)Mapping.SelectionToAvatar(Core.Memory.AvatorSelection(index));    // Check 'BaseAddress'
            this.GameVoice = Core.Memory.VoiceSelection(index) ? VoiceTypes.Alternative : VoiceTypes.Default;
            this.PlayType = (PlayTypes)Core.Memory.TypeSelection(index);
            this.Team = (short)Core.Memory.Team(index);
        }

        public void FromInGame(int index)
        {
            this.GameAvatar = (GameAvatars)Core.Memory.AvatorInGame(index);    // Check 'BaseAddress'
            this.GameVoice = Core.Memory.VoiceInGame(index) ? VoiceTypes.Alternative : VoiceTypes.Default;
            this.PlayType = Core.Memory.TypeInGame(index) ? PlayTypes.Tetris : PlayTypes.PuyoPuyo;
            this.Team = (short)Core.Memory.Team(index);
        }

        public void ReadyLocal(int index)
        {            
            //this.ID64 = -1;  // Require 'PlayerAddress'
            this.Name = Core.Memory.LocalName(index);
            this.Rating = -1;
            this.PlayStyle = -1;

            this.PuzzleLeague = (PuzzleLeagues)0;
            this.LeagueLocation = (Locations)(-1);
            this.LeagueMedal = -1;
            this.LeagueAvatar = (GameAvatars)(-1);
            this.LeagueRegional = -1;
            this.LeagueWorldwide = -1;
            this.LeagueWin = -1;
            this.LeagueLose = -1;

            //this.GameAvatar = (GameAvatars)(-1);
            //this.GameVoice = (VoiceTypes)0;
            //this.PlayType = (PlayTypes)(-1);

            if (this.Team == 0)
            {
                this.Team = (short)(index + 1);
            }

            if (index == 0)
            {
                this.ID32 = Core.Temp.MyID32;
            }
            else
            {                
                if (this.Name == $"Player {index + 1}") this.ID32 = Core.Temp.MyID32;
                else this.ID32 = -1;
            }            
        }
    }
}
