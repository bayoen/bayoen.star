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

            if (Core.Data.States.Main > MainStates.Offline)
            {
                Core.Temp.IsPPTOn = true;

                // Check ppt is in match -> broken
                if (Core.Data.States.Sub == SubStates.InMatch)
                {                    
                    // Recover broken match
                    Core.Match.BrokenType = BrokenTypes.MissingBegin;
                    Core.Match.Begin = DateTime.UtcNow;
                    Core.Match.WinCount = Core.Memory.WinCountForced;

                    List<int> currentStars = new List<int>(Core.Memory.Stars);
                    if (currentStars.Contains(Core.Match.WinCount))
                    {
                        Core.Match.SetFinished();
                    }
                    else
                    {
                        this.SetPreMatchInfo();

                        // Pad missing games
                        List<int> starQueue = new List<int>(currentStars);
                        List<int> team = new List<int>(Core.Memory.Teams);
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
                                List<int> winners = matched.Zip(Enumerable.Range(1, 4), (x, y) => x * y).ToList();
                                winners.RemoveAll(x => (x == 0));

                                GameRecord tokenGame = new GameRecord()
                                {
                                    IsDummy = true,

                                    Frame = null,
                                    FrameTicks = null,
                                    ScoreTicks = null,
                                    Begin = null,
                                    End = null,

                                    Winners = winners,
                                };
                                Core.Match.Games.Add(tokenGame);

                                // Update
                                starQueue = starQueue.Zip(intersection, (x, y) => x - y).ToList();
                            }
                        }                                              
                    }

                    //if (!Core.Game.GetFinished())
                    //{
                    //    if (Core.Memory.IsGameFinished)
                    //    {
                    //        Core.Game.End = DateTime.UtcNow;
                    //        Core.Game.SetFinished();
                    //    }
                    //}
                }             
            }
            else
            {
                Core.Temp.IsPPTOn = false;                               
            }

            Core.MainWindow.CheckStatus();

            //this.MainTick();

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

            if (Core.Data.States.Main > MainStates.Offline)
            {
                Core.Temp.IsPPTOn = true;

                if (Core.Temp.MyName.Length == 0)
                {
                    Core.Temp.MyName = Core.Memory.MyName;
                }

                if (Core.Match.GetInitialized())
                {
                    this.MainGeneralTick();
                }
                else
                {
                    this.SetPreMatchInfo();
                    Core.Match.SetInitialized();
                }
            }
            else
            {
                Core.Temp.IsPPTOn = false;
                Core.MainWindow.CheckStatus();
            }
            
            if (this.ProjectSaveFlag) Core.Project.Save();

#if DEBUG
            this.CheckDuration();
#endif

            // Next tick
            Core.Old = Core.Data.Clone() as PPTData;
        }        

        private void MainGeneralTick()
        {
            if (Core.Data.States.Main != Core.Old.States.Main)
            {
                Core.MainWindow.CheckStatus();
            }

            // Post update
            if (Core.Data.States.Sub == SubStates.InMatch)
            {
                if (!Core.Data.Stars.SequenceEqual(Core.Old.Stars))
                {
                    List<int> diffs = Core.Data.Stars.Zip(Core.Old.Stars, (x, y) => x - y).ToList();
               
                    if (Core.Match.RoomSize == 2)
                    {
                        diffs.RemoveRange(2, 2);
                        if (Core.Memory.MyIndex == 1) diffs.Reverse();
                    }                    

                    for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
                    {
                        if (diffs[playerIndex] == 1)
                        {
                            List<int> tokenStars = new List<int>(Core.Project.CountedStars);
                            tokenStars[playerIndex]++;
                            Core.Project.CountedStars = tokenStars;

                            // Game point
                            Core.Game.Frame = Core.Data.GameFrame;
                            Core.Game.Winners.Add(playerIndex + 1);

                            // Last score of the game
                            if (Core.Game.Frame > Core.Game.FrameTicks.Last())
                            {
                                Core.Game.FrameTicks.Add(Core.Game.Frame.Value);
                                Enumerable.Range(0, Core.Match.RoomSize).ToList().ForEach(x =>
                                {
                                    Core.Game.ScoreTicks[x].Add(Core.Data.Scores[x]);
                                });
                            }

                            Core.Match.Games.Add(Core.Game.Clone() as GameRecord);
                            Core.Game = new GameRecord();

                            if (Core.Data.Stars[playerIndex] == Core.Match.WinCount)
                            {
                                List<int> tokenGames = new List<int>(Core.Project.CountedGames);
                                tokenGames[playerIndex]++;
                                Core.Project.CountedGames = tokenGames;

                                // Match point
                                Core.Match.Winner = playerIndex + 1;
                                Core.Match.AdjustGames();
                                Core.Match.End = DateTime.UtcNow;
                            }

                            if (!this.ProjectSaveFlag) this.ProjectSaveFlag = true;                            
                        }
                    }
                }
            }
        }

        private void SetPreMatchInfo()
        {
            if (Core.Data.States.Main == MainStates.PuzzleLeague)
            {
                Core.Match.MatchCategory = MatchCategories.PuzzleLeague;
                Core.Match.RoomSize = 2;
                Core.Match.RoomMax = 2;
            }
            else if (Core.Data.States.Main == MainStates.FreePlay)
            {
                Core.Match.MatchCategory = MatchCategories.FreePlay;
                Core.Match.RoomSize = Core.Memory.LobbySize;
                Core.Match.RoomMax = Core.Memory.LobbyMax;
            }
            else if (Core.Data.States.Main == MainStates.SoloArcade || Core.Data.States.Main == MainStates.MultiArcade)
            {
                Core.Match.MatchCategory = MatchCategories.Arcade;
                Core.Match.RoomSize = Core.Memory.LobbySizeInGame;
                Core.Match.RoomMax = Core.Memory.LobbySizeInGame;
            }

            Core.Match.WinCount = Core.Memory.WinCountForced;
            Core.Match.Teams = Core.Memory.Teams;
            if (Core.Match.RoomSize < 4)
            {
                Core.Match.Teams.RemoveRange(Core.Match.RoomSize, 4 - Core.Match.RoomSize);
                if (Core.Match.RoomSize == 2)
                {
                    if (Core.Memory.MyIndex == 1)
                    {
                        Core.Match.Teams.Reverse();
                    }
                }
            }

            for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
            {
                PlayerData tokenPlayer = new PlayerData(playerIndex);
                if (Core.Match.MatchCategory == MatchCategories.PuzzleLeague) tokenPlayer.Name = tokenPlayer.NameOnline;
                else if (Core.Match.MatchCategory == MatchCategories.FreePlay) tokenPlayer.Name = tokenPlayer.NameOnline;
                else if (Core.Match.MatchCategory == MatchCategories.Arcade) tokenPlayer.Name = tokenPlayer.NameOnline;

                //
                // ratings, style, etc...
                //

                Core.Match.Players.Add(tokenPlayer);
            }

            Core.Match.Begin = DateTime.UtcNow;
        }
    }
}
