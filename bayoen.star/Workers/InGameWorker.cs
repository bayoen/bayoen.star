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

                this.Interval = value ? Config.WaitingInterval : (Core.Project.EnableSlowMode ? Config.TrackingIntervalSlow : Config.TrackingIntervalNormal);

                this._waitingFlag = value; 
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
                if (Core.Data.States.Main == MainStates.PuzzleLeague)
                {
                    this.GamePuzzleLeagueTick();
                }
                else if (Core.Data.States.Main == MainStates.FreePlay)
                {
                    this.GameFreePlayTick();
                }
                else if (Core.Data.States.Main == MainStates.SoloArcade || Core.Data.States.Main == MainStates.MultiArcade)
                {
                    this.GameArcadeTick();
                }

                this.GameGeneralTick();
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

        private void GamePuzzleLeagueTick()
        {

        }

        private void GameFreePlayTick()
        {

        }

        private void GameArcadeTick()
        {

        }

        private void GameGeneralTick()
        {
            if (Core.Data.States.Sub == SubStates.InMatch)
            {
                Core.Data.SceneFrame = Core.Memory.SceneFrame;
                Core.Data.GameFrame = Core.Memory.GameFrame;
                Core.Data.Scores = Core.Memory.Scores;
                
                if (Core.Match.BrokenType == BrokenTypes.MissingBegin)
                {
                    if (!Core.Game.GetInitialized())
                    {
                        Core.Game.Begin = DateTime.UtcNow;
                        Core.Game.SetInitialized();
                    }

                    this.CheckEnd();
                    this.CheckScore();
                }
                else if (Core.Match.BrokenType == BrokenTypes.MissingEnd)
                {
                    this.CheckBegin();

                    if (Core.Game.GetInitialized())
                    {
                        Core.Game.End = DateTime.UtcNow;
                        Core.Game.SetFinished();
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
            if (!Core.Game.GetInitialized())
            {
                if ((Core.Old.GameFrame == 0 && Core.Data.GameFrame > 0)    // Catch first game begin
                || Core.Old.GameFrame > Core.Data.GameFrame)                // Catch next game begin
                {
                    Core.Game.Begin = DateTime.UtcNow;
                    Core.Game.SetInitialized();
                }
            }            
        }

        private void CheckEnd()
        {
            // Only for initialized game
            if (Core.Game.GetInitialized())
            {
                // Prevent refreshing
                if (!Core.Game.GetFinished())
                {
                    if (Core.Memory.IsGameFinished)
                    {
                        Core.Game.End = DateTime.UtcNow;
                        Core.Game.SetFinished();
                    }
                }
            }
        }

        private void CheckScore()
        {
            if (Core.Game.GetInitialized())
            {
                if (!Core.Game.GetFinished())
                {
                    if (Core.Data.GameFrame - Core.Game.FrameTicks.Last() > Config.ScoreCheckFramePeriod)
                    {
                        Core.Game.FrameTicks.Add(Core.Data.GameFrame);

                        var a = Core.Match.RoomSize;
                        var b = Core.Game.ScoreTicks;
                        var c = Core.Data.Scores;

                        Enumerable.Range(0, Core.Match.RoomSize).ToList().ForEach(x =>
                        {
                            Core.Game.ScoreTicks[x].Add(Core.Data.Scores[x]);
                        });
                    }
                }
            }            
        }

    }
}
