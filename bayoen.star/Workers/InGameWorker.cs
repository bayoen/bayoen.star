using System;
using System.Collections.Generic;
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
    public class InGameWorker : DispatcherTimer
    {
        public InGameWorker()
        {
            this.Interval = Config.TrackingIntervalNormal;
            this.Tick += GameWorker_Tick;            
        }        

        public void Initiate()
        {
#if DEBUG
            this.ResetDuration();
#endif
            this.Start();
        }

        private DateTime TimeAnchor { get; set; }
        public TimeSpan WorkerDuration { get; set; }
        public TimeSpan WorkerDurationAverage { get; set; }
        public long WorkerTick { get; set; }
        private bool ProjectSaveFlag { get; set; }

        private bool _waitingFlag = false;
        private bool WaitingFlag
        {
            get => this._waitingFlag;
            set
            {
                if (this._waitingFlag == value) return;

                this._waitingFlag = value; 

                this.Stop();
                this.Interval = value ? Config.WaitingInterval : (Core.Project.EnableSlowMode ? Config.TrackingIntervalSlow : Config.TrackingIntervalNormal);
                this.Start();
            }
        }

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

        private void GameWorker_Tick(object sender, EventArgs e)
        {
            if (Core.Data.States.Main <= MainStates.Offline)
            {
                if (!this.WaitingFlag) this.WaitingFlag = true;
            }
            else
            {
                if (this.WaitingFlag) this.WaitingFlag = false;
            }

            this.ProjectSaveFlag = false;

            if (Core.Data.States.Main > MainStates.Title)
            {
                this.GeneralTick();
            }
            else // if (Core.Data.States.Main == MainStates.Title)
            {
                // Do nothing
            }
            
            if (this.ProjectSaveFlag) Core.Project.Save();

#if DEBUG
            this.CheckDuration();
#endif

            // Next tick
            Core.Old = Core.Data.Clone() as PPTData;
        }       

        private void GeneralTick()
        {
            if (Core.Data.States.Sub == SubStates.InMatch)
            {
                Core.Data.SceneFrame = Core.Memory.SceneFrame;
                Core.Data.GameFrame = Core.Memory.GameFrame;
                Core.Data.Scores = Core.Memory.Scores;
                
                if (Core.Match.BrokenType == BrokenTypes.MissingEnd)
                {
                    this.CheckBegin();

                    if (Core.Game.IsInitialized)
                    {
                        Core.Game.End = DateTime.UtcNow;
                        Core.Game.IsInitialized = true;
                    }
                }
                else // if (Core.Match.BrokenType == BrokenTypes.None)
                {
                    this.CheckBegin();
                    this.CheckEnd();
                    this.CheckScore();
                }
            }
        }

        private void CheckBegin()
        {
            if (!Core.Game.IsInitialized)
            {
                if ((Core.Old.GameFrame == 0 && Core.Data.GameFrame > 0)    // Catch first game begin
                || Core.Old.GameFrame > Core.Data.GameFrame)                // Catch next game begin
                {
                    Core.Game.Begin = DateTime.UtcNow;
                    Core.Game.Index = Core.Match.Games.Count;
                    Core.Game.IsInitialized = true;
                }
            }            
        }

        private void CheckEnd()
        {
            // Only for initialized game
            if (Core.Game.IsInitialized)
            {
                // Prevent refreshing
                if (!Core.Game.IsFinished)
                {
                    byte token = Core.Memory.GameWinnerToken;

                    if (token < 0xFF)
                    {
                        if (Core.Memory.IsGameFinished)
                        {
                            // Set Core.Game.WinnerTeam
                            if (Core.Match.RoomSize == 2)
                            {
                                if (token == 0 || token == 1) Core.Game.WinnerTeam = token + 1;                                
                                else Core.Game.WinnerTeam = 0; // Draw                             
                            }
                            else if (Core.Match.RoomSize == 3 || Core.Match.RoomSize == 4)
                            {
                                if (4 <= token || token <= 7) Core.Game.WinnerTeam = token - 3;                                
                                else Core.Game.WinnerTeam = 0;
                            }
                            else throw new InvalidOperationException("ERROR:: Core.Match.RoomSize is Broken");

                            Core.Game.IsFinished = true;
                            Core.Game.End = DateTime.UtcNow;

                            this.TerminateGame();
                        }
                    }                    
                }                
            }
        }

        private void CheckScore()
        {
            if (Core.Game.IsInitialized)
            {
                if (!Core.Game.IsFinished)
                {
                    if (Core.Data.GameFrame - Core.Game.FrameTicks.Last() > Config.ScoreCheckFramePeriod)
                    {
                        Core.Game.FrameTicks.Add(Core.Data.GameFrame);

                        Enumerable.Range(0, Core.Match.RoomSize).ToList().ForEach(x =>
                        {
                            Core.Game.ScoreTicks[x].Add(Core.Data.Scores[x]);
                        });
                    }
                }
            }            
        }

        #region once
        //private void TerminateOnceMatch()
        //{
        //    // Check frame
        //    Core.Game.Frame = Core.Data.GameFrame;
        //    for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
        //    {
        //        // Last score of the game
        //        if (Core.Game.Frame > Core.Game.FrameTicks.Last())
        //        {
        //            Core.Game.FrameTicks.Add(Core.Game.Frame.Value);
        //            Enumerable.Range(0, Core.Match.RoomSize).ToList().ForEach(x =>
        //            {
        //                Core.Game.ScoreTicks[x].Add(Core.Data.Scores[x]);
        //            });
        //        }
        //    }

        //    // Update winner
        //    Core.Match.WinnerTeam = Core.Game.WinnerTeam;
        //    if (Core.Game.WinnerTeam > 0)
        //    {
        //        List<int> tokenStars = new List<int>(Core.Project.CountedStars);
        //        List<int> tokenGames = new List<int>(Core.Project.CountedGames);
        //        for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
        //        {
        //            if (Core.Match.Teams[playerIndex] == Core.Game.WinnerTeam)
        //            {
        //                tokenStars[playerIndex]++;
        //                tokenGames[playerIndex]++;
        //            }
        //        }
        //        Core.Project.CountedStars = tokenStars;
        //        Core.Project.CountedGames = tokenGames;

        //        //List<int> tokenGames = new List<int>(Core.Project.CountedGames);
        //        //for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
        //        //{
        //        //    if (Core.Match.Teams[playerIndex] == Core.Game.WinnerTeam)
        //        //    {
        //        //        tokenGames[playerIndex]++;
        //        //    }

        //        //}                                    
        //        //Core.Project.CountedGames = tokenGames;
        //    }
        //    else
        //    {
        //        // Draw
        //    }


        //    Core.Match.Games.Add(Core.Game);
        //    Core.Game = new GameRecord();

        //    Core.Match.AdjustGames();
        //    Core.Match.End = DateTime.UtcNow;

        //    Core.DB.Insert(Core.Match);
        //    Core.UpdateResult();

        //    if (!this.ProjectSaveFlag) this.ProjectSaveFlag = true;
        //}
        #endregion

        private void TerminateGame()
        {
            // Check frame and last score of the game
            Core.Game.Frame = Core.Data.GameFrame;
            if (Core.Game.Frame > Core.Game.FrameTicks.Last())
            {
                Core.Game.FrameTicks.Add(Core.Game.Frame.Value);
                for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
                {
                    Core.Game.ScoreTicks[playerIndex].Add(Core.Data.Scores[playerIndex]);
                }
            }

            // Update winner            
            if (Core.Game.WinnerTeam > 0)
            {
                int lastWinnerTeam = Core.Game.WinnerTeam;
                DateTime lastGameEnd = Core.Game.End.Value;

                List<int> tokenStars = new List<int>(Core.Project.CountedStars);
                for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
                {
                    if (Core.Match.Teams[playerIndex] == Core.Game.WinnerTeam)
                    {
                        tokenStars[playerIndex]++;
                    }
                }
                Core.Project.CountedStars = tokenStars;

                Core.Match.Games.Add(Core.Game);
                Core.Game = new GameRecord();

                bool isMatchFinished = false;
                List<int> stack = Core.Match.WinStack;
                List<int> tokenGames = new List<int>(Core.Project.CountedGames);
                for (int playerIndex = 0; playerIndex < Core.Match.RoomSize; playerIndex++)
                {                    
                    if (stack[Core.Match.Teams[playerIndex] - 1] == Core.Match.WinCount)
                    {
                        tokenGames[playerIndex]++;
                        if (!isMatchFinished) isMatchFinished = true;
                    }
                }
                Core.Project.CountedGames = tokenGames;

                if (isMatchFinished)
                {
                    Core.Match.WinnerTeam = lastWinnerTeam;
                    Core.Match.AdjustGames();
                    Core.Match.End = lastGameEnd;

                    if (Core.Data.States.Main != MainStates.PuzzleLeague)
                    {
                        Core.DB.Insert(Core.Match);
                        Core.UpdateResult();
                    }                        
                }
            }
            else
            {
                // Draw
                Core.Match.Games.Add(Core.Game);
                Core.Game = new GameRecord();
            }

            if (!this.ProjectSaveFlag) this.ProjectSaveFlag = true;

            if (!Core.Project.DisableEarlyRefresh)
            {
                Core.Counting();
            }
        }
    }
}
