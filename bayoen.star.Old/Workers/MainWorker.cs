using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using bayoen.library.General.Enums;
using bayoen.library.General.Memories;
using bayoen.star.Variables;

namespace bayoen.star.Workers
{
    public class MainWorker : DispatcherTimer
    {
        public MainWorker()
        {
            this.Interval = Config.DisplayIntervalNormal;
            this.Tick += MainWorker_Tick;
        }

        public void Initiate()
        {
            this.CheckGameData(Core.Data);
            Core.Old = new PPTData();
            
#if DEBUG
            this.ResetDuration();
#endif

            Core.Event = new EventRecord();
            Core.Match = new MatchRecord();
            Core.Game = new GameRecord();

            if (Core.Data.States.Main > MainStates.Title)
            {
                Core.Temp.IsNameOn = true;                

                //// Check ppt is in match -> broken
                if (Core.Data.States.Sub == SubStates.InMatch)
                {
                    // Recover broken match
                    Core.Match.BrokenType = BrokenTypes.MissingBegin;

                    if (Core.Data.States.Main == MainStates.PuzzleLeague)
                    {
                        Core.Match.Category = MatchCategories.PuzzleLeague;
                        Core.Match.RoomSize = 2;
                        Core.Match.RoomMax = 2;
                        Core.Match.WinCount = 2;

                        for (int playerIndex = 0; playerIndex < 2; playerIndex++)
                        {
                            Core.Live.Players[playerIndex].SetReadyOnline(playerIndex);
                        }
                        Core.Live.Check(2);
                    }
                    else if (Core.Data.States.Main == MainStates.FreePlay)
                    {
                        Core.Match.Category = MatchCategories.FreePlay;
                        Core.Match.RoomSize = Core.Memory.LobbySize;
                        Core.Match.RoomMax = Core.Memory.LobbyMax;
                        Core.Match.WinCount = Core.Memory.WinCountForced;

                        int playerCount = Math.Max(2, Core.Memory.LobbySize);
                        for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
                        {
                            Core.Live.Players[playerIndex].SetReadyOnline(playerIndex);
                        }
                        Core.Live.Check(playerCount);
                    }
                    else if (Core.Data.States.Main == MainStates.SoloArcade || Core.Data.States.Main == MainStates.MultiArcade)
                    {
                        Core.Match.Category = MatchCategories.Arcade;
                        Core.Match.RoomSize = Core.Match.RoomMax = Core.Memory.LobbySizeInGame;
                        Core.Match.WinCount = Core.Memory.WinCountForced;
                        
                        for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                        {
                            Core.Live.Players[playerIndex].FromInGame(playerIndex);
                        }
                        Core.Live.Check(4);
                    }                    

                    if (Core.Match.RoomSize == 2)
                    {
                        Core.Match.Teams = new List<int>() { 1, 2 };
                        if (Core.Memory.MyIndex == 1)
                        {
                            Core.Match.Teams.Reverse();
                        }
                    }
                    else
                    {
                        Core.Match.Teams = Core.Memory.Teams;
                        Core.Match.Teams.RemoveRange(Core.Match.RoomSize, 4 - Core.Match.RoomSize);
                    }

                    if (Core.Memory.IsGameFinished)
                    {
                        Core.Match.IsFinished = true;
                    }
                    else
                    {
                        List<int> currentStars = new List<int>(Core.Data.Stars);

                        // Pad missing games
                        List<int> starQueue = new List<int>(currentStars);
                        List<int> team = new List<int>(Core.Match.Teams);
                        while (starQueue.Sum() > 0)
                        {
                            for (int teamScan = 1; teamScan <= 4; teamScan++)
                            {
                                // Filtering
                                List<int> matched = team.ConvertAll(x => (x == teamScan) ? 1 : 0);
                                if (matched.Sum() == 0) continue;
                                List<int> intersection = starQueue.Zip(matched, (x, y) => (x * y > 0) ? 1 : 0).ToList();
                                if (intersection.Sum() == 0) continue;

                                // Generate dummy games
                                GameRecord tokenGame = new GameRecord()
                                {
                                    Index = Core.Match.Games.Count,
                                    IsDummy = true,
                                    Frame = null,
                                    FrameTicks = null,
                                    ScoreTicks = null,
                                    Begin = null,
                                    End = null,
                                    WinnerTeam = teamScan,
                                };
                                Core.Match.Games.Add(tokenGame);

                                // Update queue
                                starQueue = starQueue.Zip(intersection, (x, y) => x - y).ToList();
                            }
                        }

                        Core.Match.IsInitialized = true;
                    }                                       
                }
            }
            else if (Core.Data.States.Main == MainStates.Title)
            {
                Core.Temp.IsNameOn = true;
            }
            else
            {
                Core.Temp.IsNameOn = false;
            }

            Core.MainWindow.CheckStatus();
            this.Start();
        }

        private bool ProjectSaveFlag { get; set; }

#if DEBUG
        private DateTime TimeAnchor { get; set; }
        public TimeSpan WorkerDuration { get; set; }
        public TimeSpan WorkerDurationAverage { get; set; }
        public long WorkerTick { get; set; }
        
        public void ResetDuration()
        {
            this.TimeAnchor = DateTime.Now;
            this.WorkerDurationAverage = TimeSpan.Zero;
            this.WorkerTick = 0;
        }

        public void CheckDuration()
        {
            this.WorkerDuration = TimeSpan.FromTicks(Math.Max((DateTime.Now - this.TimeAnchor).Ticks, this.Interval.Ticks)) - this.Interval;
            this.WorkerDurationAverage = TimeSpan.FromTicks((this.WorkerTick * this.WorkerDurationAverage.Ticks + this.WorkerDuration.Ticks) / (++this.WorkerTick));
            this.TimeAnchor = DateTime.Now;
        }
#endif        

        public void CheckGameData(PPTData data)
        {
            data.States = Core.Memory.GetGameState();

            data.MyRating = Core.Memory.MyRating;
            data.Stars = Core.Memory.Stars;
        }

        private void MainWorker_Tick(object sender, EventArgs e)
        {
            if (this.ProjectSaveFlag) this.ProjectSaveFlag = false;

            this.CheckGameData(Core.Data);

            Core.Temp.IsNameOn = (Core.Data.States.Main >= MainStates.Title);

            if (Core.Match.IsInitialized)
            {
                if (Core.Data.States.Main > MainStates.Title)
                {                                        
                    if (Core.Data.States.Sub == SubStates.InMatch)
                    {
                        if (!Core.Data.Stars.SequenceEqual(Core.Old.Stars))
                        {
                            List<int> diffs = Core.Data.Stars.Zip(Core.Old.Stars, (x, y) => x - y).ToList();
                            for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
                            {
                                if (diffs[playerIndex] < 0)
                                {
                                    Core.Match = Core.Match.RepeatDeepClone() as MatchRecord;
                                    Core.Game.Reset();

                                    Core.Match.Begin = DateTime.UtcNow;
                                    break;
                                }
                                else if (diffs[playerIndex] == 1)
                                {
                                    if (Core.Project.DisableEarlyRefresh)
                                    {
                                        Core.Counting();
                                    }
                                }
                            }
                        }

                        if (Core.Data.States.Main == MainStates.PuzzleLeague)
                        {
                            if (Core.Data.MyRating != Core.Old.MyRating)
                            {                                
                                Core.Match.RatingGain = Core.Data.MyRating - Core.Old.MyRating;

                                Core.DB.Insert(Core.Match);
                                Core.UpdateResult();
                            }
                        }
                    }
                    else
                    {
                        // broken                        
                        if (Core.Old.States.Sub == SubStates.InMatch)
                        {
                            Core.Match.BrokenType = BrokenTypes.MissingEnd;
                            Core.Match.IsInitialized = false;
                            Core.Match.IsFinished = true;
                        }
                    }
                }
                else 
                {

                }
              
            }
            else
            {
                if (Core.Data.States.Main > MainStates.Title)
                {                    
                    if (Core.Data.States.Main == MainStates.PuzzleLeague)
                    {
                        this.InitializePuzzleLeague();                        
                    }
                    else if (Core.Data.States.Main == MainStates.FreePlay)
                    {
                        this.InitializeFreePlay();                        
                    }
                    else if (Core.Data.States.Main == MainStates.SoloArcade || Core.Data.States.Main == MainStates.MultiArcade)
                    {
                        this.InitializeArcade();
                    }
                }
                else if (Core.Data.States.Main == MainStates.Title)
                {
                    Core.MainWindow.CheckStatus();
                }
                else
                {

                }
            }

            if (Core.Data.States.Sub != SubStates.InMatch)
            {
                if (Core.Old.States.Sub == SubStates.InMatch)
                {
                    Core.Match = new MatchRecord();
                    Core.Game.Reset();
                    Core.Live.Reset();
                }
            }

            if (Core.Data.States.Main != Core.Old.States.Main)
            {
                Core.MainWindow.CheckStatus();
            }

            if (this.ProjectSaveFlag) Core.Project.Save();

#if DEBUG
            this.CheckDuration();
#endif

            // Next tick
            Core.Old = Core.Data.Clone() as PPTData;
        }

        private void InitializePuzzleLeague()
        {
            if (Core.Data.States.Sub == SubStates.Matchmaking)
            {
                if (Core.Old.States.Sub != SubStates.Matchmaking)
                {
                    Core.Match.Category = MatchCategories.PuzzleLeague;
                    Core.Match.Mode = Core.Data.States.Mode;
                    Core.Match.MyID32 = Core.Temp.MyID32;
                    Core.Match.RoomSize = 2;
                    Core.Match.RoomMax = 2;
                    Core.Match.WinCount = 2;
                }

                Core.Live.Players[0].FromOnlineMatchmaking(0);
                Core.Live.Players[1].FromOnlineMatchmaking(1);
                Core.Match.Mode = (GameModes)Core.Memory.LeagueMode;
                Core.Live.Check(2);
            }
            else if (Core.Data.States.Sub == SubStates.CharacterSelection)
            {
                Core.Live.Players[0].FromCharacterSelection(0);
                Core.Live.Players[1].FromCharacterSelection(1);
                Core.Live.Check(2);
            }
            else if (Core.Data.States.Sub == SubStates.InMatch)
            {
                if (Core.Old.States.Sub != SubStates.InMatch)
                {
                    Core.Match.Category = MatchCategories.PuzzleLeague;
                    Core.Match.Mode = Core.Data.States.Mode;
                    Core.Match.MyID32 = Core.Temp.MyID32;
                    Core.Match.RoomSize = 2;
                    Core.Match.RoomMax = 2;
                    Core.Match.WinCount = 2;                    

                    Core.Match.Teams = new List<int>() { 1, 2 };
                    if (Core.Memory.MyIndex == 1)
                    {
                        Core.Match.Teams.Reverse();
                    }

                    Core.Live.Players[0].FromInGame(0);
                    Core.Live.Players[1].FromInGame(1);

                    Core.Live.Players[0].Team = 1;
                    Core.Live.Players[1].Team = 2;

                    Core.Live.Check(2);

                    Core.Match.Players.Add(Core.Live.Players[0]);
                    Core.Match.Players.Add(Core.Live.Players[1]);

                    Core.Match.Begin = DateTime.UtcNow;
                    Core.Match.IsInitialized = true;
                }
            }
        }
        private void InitializeFreePlay()
        {
            if (Core.Data.States.Sub == SubStates.InLobby)
            {
                int playerCount = Math.Max(2, Core.Memory.LobbySize);
                for (int playerIndex = 0; playerIndex < playerCount; playerIndex++)
                {
                    if (playerCount > playerIndex) Core.Live.Players[playerIndex].FromOnlineMatchmaking(playerIndex);
                    else Core.Live.Players[playerIndex].Reset();
                }
                Core.Live.Check(playerCount);
            }
            else if (Core.Data.States.Sub == SubStates.CharacterSelection)
            {
                if (Core.Old.States.Sub != SubStates.CharacterSelection)
                {
                    Core.Match.Category = MatchCategories.FreePlay;
                    Core.Match.RoomSize = Core.Memory.LobbySize;
                    Core.Match.RoomMax = Core.Memory.LobbyMax;
                    Core.Match.WinCount = Core.Memory.WinCountForced;

                    if (Core.Match.RoomSize == 2)
                    {
                        Core.Match.Teams = new List<int>() { 1, 2 };
                        if (Core.Memory.MyIndex == 1)
                        {
                            Core.Match.Teams.Reverse();
                        }
                    }
                    else
                    {
                        Core.Match.Teams = Core.Memory.Teams;
                        Core.Match.Teams.RemoveRange(Core.Match.RoomSize, 4 - Core.Match.RoomSize);
                    }

                    Core.Match.Begin = DateTime.UtcNow;
                    Core.Match.IsInitialized = true;
                }
            }
            else if (Core.Data.States.Sub == SubStates.InMatch)
            {
                if (Core.Old.States.Sub != SubStates.InMatch)
                {
                    Core.Match.Category = MatchCategories.FreePlay;
                    Core.Match.RoomSize = Core.Memory.LobbySize;
                    Core.Match.RoomMax = Core.Memory.LobbyMax;
                    Core.Match.WinCount = Core.Memory.WinCountForced;

                    if (Core.Match.RoomSize == 2)
                    {
                        Core.Match.Teams = new List<int>() { 1, 2 };
                        if (Core.Memory.MyIndex == 1)
                        {
                            Core.Match.Teams.Reverse();
                        }
                    }
                    else
                    {
                        Core.Match.Teams = Core.Memory.Teams;
                        Core.Match.Teams.RemoveRange(Core.Match.RoomSize, 4 - Core.Match.RoomSize);
                    }

                    Core.Match.Begin = DateTime.UtcNow;
                    Core.Match.IsInitialized = true;
                }
            }
        }
        private void InitializeArcade()
        {
            if (Core.Data.States.Sub == SubStates.CharacterSelection)
            {
                if (Core.Old.States.Sub != SubStates.CharacterSelection)
                {
                    Core.Match.Category = MatchCategories.Arcade;
                    Core.Match.Mode = Core.Data.States.Mode;
                    Core.Match.MyID32 = Core.Temp.MyID32;
                }

                for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                {
                    Core.Live.Players[playerIndex].FromCharacterSelection(playerIndex);
                }
                Core.Live.Check(4);
            }
            else if (Core.Data.States.Sub == SubStates.InMatch)
            {
                if (Core.Old.States.Sub != SubStates.InMatch)
                {
                    Core.Match.Category = MatchCategories.Arcade;
                    Core.Match.Mode = Core.Data.States.Mode;
                    Core.Match.MyID32 = Core.Temp.MyID32;
                    Core.Match.RoomSize = Core.Match.RoomMax = Core.Memory.LobbySizeInGame;
                    Core.Match.WinCount = Math.Max(1, Core.Memory.WinCountForced);

                    for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                    {
                        if (playerIndex < Core.Match.RoomSize)
                        {
                            Core.Live.Players[playerIndex].ReadyLocal(playerIndex);
                            Core.Match.Players.Add(Core.Live.Players[playerIndex]);
                        }
                        else Core.Live.Players[playerIndex].Reset();
                    }
                    Core.Live.Check(Core.Match.RoomSize);

                    if (Core.Match.RoomSize == 2)
                    {
                        Core.Match.Teams = new List<int>() { 1, 2 };
                    }
                    else
                    {
                        Core.Match.Teams = Core.Memory.Teams;
                        Core.Match.Teams.RemoveRange(Core.Match.RoomSize, 4 - Core.Match.RoomSize);
                    }                  

                    Core.Match.Begin = DateTime.UtcNow;
                    Core.Match.IsInitialized = true;
                }
            }
        }        
          
    }
}
