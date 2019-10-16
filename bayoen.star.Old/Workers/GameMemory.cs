using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.library.General.Memories;
using bayoen.star.Variables;

namespace bayoen.star.Workers
{
    public class GameMemory : ProcessMemory
    {
        public GameMemory(string name) : base(name)
        {
            this.MenuID = -1;
            this.MainID = 1;
            this.ModeID = 0;

            if (this.CheckProcess())
            {
                this.BaseAddress = (IntPtr)this.GetBaseAddress();
            }
        }

        public int RootFrame => this.ReadInt32(this.BaseAddress + 0x461B7C);
        public int SceneFrame => this.ReadInt32(this.BaseAddress + 0x460C08, 0x18, 0x268, 0x140, 0x58);
        public int GameFrame => this.ReadInt32(this.BaseAddress + 0x461B20, 0x424);
        public int PauseFrame => this.ReadInt32(this.BaseAddress + 0x461B38, 0xE4);

        private IntPtr _scoreAddress;
        public IntPtr ScoreAddress => this._scoreAddress = new IntPtr(this.ReadInt32(this.BaseAddress + 0x57F048) + 0x38);

        private IntPtr _playerAddress;
        public IntPtr PlayerAddress => this._playerAddress = new IntPtr(this.ReadInt32(this.BaseAddress + 0x473760, 0x20) + 0xD8);

        private IntPtr _leagueAddress;
        public IntPtr LeagueAddress => this._leagueAddress = new IntPtr(this.ReadInt32(this.BaseAddress + 0x473760, 0x68, 0x20, 0x970) - 0x38);

        public Process Process { get; private set; }
        public IntPtr BaseAddress { get; private set; }
        public int MenuID { get; private set; }
        public int MainID { get; private set; }
        public int ModeID { get; private set; }

        public bool InInitial => (this.ReadByte(this.BaseAddress + 0x4640C2) & 0b00100000) == 0b00100000;
        public bool InAdventure => (this.ReadByte(this.BaseAddress + 0x451C50) == 0b00000001) && (this.ReadByte(this.BaseAddress + 0x573854) == 0);
        public bool InOnlineReplay => this.ReadByte(this.BaseAddress + 0x5989D0, 0x40, 0x28) != 0;
        public bool InLocalReplay => this.ReadByte(this.BaseAddress + 0x598BC8) != 0;
        public bool InMatch => this.ReadInt32(this.BaseAddress + 0x461B20) != 0;
        public int OnlineType => this.ReadByte(this.BaseAddress + 0x573797) & 0b00000001;
        public bool PuzzleLeagueGameFinishFlag => this.ReadInt32(new IntPtr(0x140461B20), 0x454) == 7;
        public bool IsGameFinished => this.ReadByte(this.BaseAddress + 0x461B20, 0x454) != 0;
        public byte GameWinnerToken => this.ReadByte(this.BaseAddress + 0x461B20, 0x42C);

        public bool InReady
        {
            get
            {
                int pointer = this.ReadByte(this.BaseAddress + 0x460690, 0x280);
                return pointer != 0 && pointer != -1;
            }
        }
        public bool InCharacterSelection
        {
            get
            {
                int player1State = this.ReadByte(this.BaseAddress + 0x460690, 0x274);
                return 0 < player1State && player1State < 16;
            }
        }

        public string MyName => this.ReadValidString(this.BaseAddress + 0x59B418, PlayerNameSize);
        public int MyID32 => this.ReadInt32(this.BaseAddress + 0x5A2010);
        //public long MyID64 => this.ReadInt64(this.BaseAddress + 0x5A2010);
        public int MyRating => this.ReadInt16(this.BaseAddress + 0x599FF0);
        public int MyIndex
        {
            get
            {
                int players = this.LobbySize;
                if (players < 2) return 0;
                int steam = this.MyID32;
                for (int i = 0; i < players; i++)
                {
                    if (steam == this.OnlineID32Forced(i)) return i;
                }
                return 0;
            }
        }

        public int LobbySize => this.ReadInt32(this.BaseAddress + 0x473760, 0x20, 0xB4);
        public int LobbySizeInGame => this.ReadInt32(this.BaseAddress + 0x460690, 0xCC);
        public int LobbyMax => this.ReadInt32(this.BaseAddress + 0x473760, 0x20, 0xB8);

        public int Team(int index) => (this.ReadByte(this.BaseAddress + 0x598C1D + index * 0x68) / 4) % 16;
        public List<int> Teams => Enumerable.Range(0, 4).Select(x => Team(x)).ToList();

        private int Star(int index) => this.ReadInt32(this.BaseAddress + 0x57F048, index * 0x04 + 0x38);
        public List<int> Stars => Enumerable.Range(0, 4).Select(x => Star(x)).ToList();

        private int Score(int index)
        {
            switch (index)
            {
                case 0: return this.ReadInt32(new IntPtr(0x140461B28), 0x380, 0x18, 0xE0, 0x3C);
                case 1: return this.ReadInt32(new IntPtr(0x140460690), 0x2D0, 0x0, 0x38, 0x78, 0xE0, 0x3C);
                case 2:
                case 3: return -1;
                default: return -1;
            }
        }
        public List<int> Scores => Enumerable.Range(0, 4).Select(x => Score(x)).ToList();

        public int WinCount => this.ReadInt32(this._scoreAddress + 0x10);
        public int WinCountForced => this.ReadInt32(this.ScoreAddress + 0x10);

        public string LocalName(int index) => this.ReadValidString(new IntPtr((long)this.BaseAddress + 0x598BD4 + index * 0x68), PlayerNameSize);
        public bool PlayType(int index) => this.ReadBinary(6, new IntPtr((long)this.BaseAddress + 0x598C27 + index * 0x68));


        public int OnlineID32(int index) => this.ReadInt32(this._playerAddress + index * 0x50 + 0x40);
        public int OnlineID32Forced(int index) => this.ReadInt32(this.PlayerAddress + index * 0x50 + 0x40);
        //public long OnlineID64(int index) => this.ReadInt64(this._playerAddress + index * 0x50 + 0x40);
        public string OnlineName(int index) => this.ReadValidString(this._playerAddress + index * 0x50, PlayerNameSize);
        //public string OnlineNameRaw(int index) => this.ReadStringUnicode(this._playerAddress + index * 0x50, PlayerNameSize);
        public byte OnlineAvatar(int index) => this.ReadByte(this._playerAddress + index * 0x50 + 0x24);
        public short OnlineRating(int index) => this.ReadInt16(this._playerAddress + index * 0x50 + 0x30);
        public short OnlinePlayStyle(int index) => this.ReadByte(this._playerAddress + index * 0x50 + 0x32);
        public byte OnlineLeague(int index) => this.ReadByte(this._playerAddress + index * 0x50 + 0x27);
        public int OnlineRegional(int index) => this.ReadInt32(this._playerAddress + index * 0x50 + 0x28);
        public int OnlineWorldwide(int index) => this.ReadInt32(this._playerAddress + index * 0x50 + 0x2c);
        public short OnlineWin(int index) => this.ReadInt16(this._playerAddress + index * 0x50 + 0x36);
        public short OnlineLose(int index) => this.ReadInt16(this._playerAddress + index * 0x50 + 0x38);
        public byte OnlineLocation(int index) => this.ReadByte(this._playerAddress + index * 0x50 + 0x3B);
        public short OnlineMedal(int index) => (short)(this.ReadByte(this._playerAddress + index * 0x50 + 0x3A)
                                                      + this.ReadInt32(this._playerAddress + index * 0x50 + 0x3C));
        public int LeagueMode
        {
            get
            {
                byte raw = this.ReadByte(this.BaseAddress + 0x460690, 0x30, 0x7C8);
                if (0 <= raw && raw <= 4) return raw + 1;
                else return 0;
            }
        }

        public byte AvatorSelection(int index) => this.ReadByte(this.BaseAddress + 0x460690, 0x1c8 + index * 0x30);
        public byte TypeSelection(int index) => this.ReadByte(this.BaseAddress + 0x460690, 0x980 + index * 0x28);
        public bool VoiceSelection(int index) => this.ReadBinary(index, this.BaseAddress + 0x460690, 0x594);

        public byte AvatorInGame(int index) => this.ReadByte(this.BaseAddress + 0x598C28 + index * 0x68);
        public bool TypeInGame(int index) => this.ReadBinary(6, this.BaseAddress + 0x598C27 + index * 0x68);
        public bool VoiceInGame(int index) => this.ReadBinary(7, this.BaseAddress + 0x598C27 + index * 0x68);

        public void CheckMenuID()
        {
            int menuBase = this.ReadInt32(this.BaseAddress + 0x573A78);
            if (menuBase == 0x0)
            {
                this.MenuID = -1;
                return;
            }
            this.MenuID = this.ReadInt32(new IntPtr(this.ReadInt32(new IntPtr(menuBase + 0xE8)) * 0x04 + menuBase + 0xA4));
        }
        public void CheckMainID()
        {
            // Online            
            if ((this.ReadByte(this.BaseAddress + 0x59894C) & 0b00000001) > 0)
            {
                this.MainID = 4;
                return;
            }

            // Flags            
            if ((this.ReadByte(this.BaseAddress + 0x451C50) & 0b00010000) > 0)
            {
                this.MainID = 2;
                return;
            }

            this.MainID = 1;
        }
        public void CheckModeID()
        {
            switch (this.MainID)
            {
                case 1:
                case 2:
                    this.ModeID = (this.ReadByte(this.BaseAddress + 0x451C50) & 0b11101111) - 2;
                    return;
                case 4:
                    this.ModeID = (this.ReadByte(this.BaseAddress + 0x4385C4) > 0)
                        ? this.ReadByte(this.BaseAddress + 0x438584) - 1
                        : this.ReadByte(this.BaseAddress + 0x573794);
                    return;
            }

            this.ModeID = 0;
        }

        public PPTStates GetGameState()
        {          
            if (!this.CheckProcess()) return new PPTStates() { Main = MainStates.MissingProcess };
            this.BaseAddress = (IntPtr)this.GetBaseAddress();
            if (this.BaseAddress == IntPtr.Zero) return new PPTStates() { Main = MainStates.MissingAddress };
            this.CheckMenuID();

            if (this.MenuID > -1) return new PPTStates()
            {
                Main = MenuMainState(this.MenuID),
                Sub = MenuSubState(this.MenuID),
            };
            if (this.InAdventure) return new PPTStates()
            {
                Main = MainStates.Adventure,
            };
            if (this.InInitial) return new PPTStates()
            {
                Main = MainStates.Title,
            };
            if (this.InOnlineReplay) return new PPTStates()
            {
                Main = this.InLocalReplay ? MainStates.LocalReplay : MainStates.OnlineReplay,
            };

            this.CheckMainID();
            this.CheckModeID();

            PPTStates states = new PPTStates()
            {
                Main = MainMainState(this.MainID),
                Mode = ModeGameMode(this.ModeID),
                IsEndurance = IsEndurance(this.ModeID),
            };

            if (this.InCharacterSelection)
            {
                states.Sub = SubStates.CharacterSelection;
                return states;
            }
            if (this.InReady)
            {
                states.Sub = SubStates.InReady;
                return states;
            }
            if (this.InMatch)
            {
                states.Sub = SubStates.InMatch;
                // pass
            }
            if (this.IsDemo(states))
            {
                states.Main = MainStates.Title;
            }

            return states;
        }

        // Main state
        private MainStates MenuMainState(int menuID)
        {
            switch (menuID)
            {
                case 0:
                case 12: return MainStates.Loading;
                case 1: return MainStates.MainMenu;
                case 2: return MainStates.Adventure;
                case 3:
                case 18: return MainStates.SoloArcade;
                case 4: return MainStates.MultiArcade;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9: return MainStates.Option;
                case 10:
                case 32:
                case 33:
                case 37: return MainStates.Online;
                case 11: return MainStates.Lessons;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 19: return MainStates.CharacterSetection;
                case 20:
                case 25:
                case 27:
                case 34: return MainStates.PuzzleLeague;
                case 21:
                case 23:
                case 24:
                case 26:
                case 28: return MainStates.FreePlay;

            }
            return MainStates.Invalid;
        }
        private MainStates MainMainState(int mainID)
        {
            switch (mainID)
            {
                case 0: return MainStates.Adventure;
                case 1: return MainStates.SoloArcade;
                case 2: return MainStates.MultiArcade;
                case 3: return MainStates.Option;
                case 4:
                    switch (this.OnlineType)
                    {
                        case 0: return MainStates.PuzzleLeague;
                        case 1: return MainStates.FreePlay;
                        default: return MainStates.None;
                    }
                case 5: return MainStates.Lessons;
            }
            return MainStates.None;
        }

        // Sub state
        private SubStates MenuSubState(int menuID)
        {
            switch (menuID)
            {
                case 3:
                case 4:
                case 23: return SubStates.ModeSelection;
                case 6: return SubStates.Stats;
                case 7: return SubStates.Options;
                case 8: return SubStates.Theatre;
                case 9: return SubStates.Shop;
                case 18: return SubStates.ChallengeModeSelection;
                case 20: return SubStates.Standby;
                case 21: return SubStates.RoomSelection;
                case 24: return SubStates.RoomCreation;
                case 25:
                case 27: return SubStates.Matchmaking;
                case 26:
                case 28: return SubStates.InLobby;
                case 32: return SubStates.Replays;
                case 33:
                case 37: return SubStates.ReplayUpload;
                case 34: return SubStates.Rankings;
            }
            return SubStates.None;
        }

        // Game mode
        private GameModes ModeGameMode(int modeID)
        {
            switch (modeID)
            {
                case 0:
                case 5: return GameModes.Versus;
                case 1:
                case 6: return GameModes.Fusion;
                case 2:
                case 7: return GameModes.Swap;
                case 3:
                case 8: return GameModes.Party;
                case 4:
                case 9: return GameModes.BigBang;
                case 10: return GameModes.EndlessFever;
                case 11: return GameModes.TinyPuyo;
                case 12: return GameModes.EndlessPuyo;
                case 13: return GameModes.Sprint;
                case 14: return GameModes.Marathon;
                case 15: return GameModes.Ultra;
            }

            return GameModes.None;
        }

        private bool IsEndurance(int id)
        {
            return (4 < id) && (id < 10);
        }

        private bool IsDemo(PPTStates states)
        {
            if (states.Main == MainStates.SoloArcade || states.Main == MainStates.MultiArcade)
            {
                if (this.LocalName(0) + this.LocalName(1) == "")
                {
                    return true;
                }
            }

            return false;
        }

        private const uint PlayerNameSize = 36;
    }
}
