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
            this.Interval = Config.NormalInterval;
            this.Tick += MainWorker_Tick;
        }

        private DateTime TimeAnchor { get; set; }
        public TimeSpan WorkerDuration { get; set; }
        public TimeSpan WorkerDurationAverage { get; set; }
        public long WorkerTick { get; set; }


        private bool ProjectSaveFlag { get; set; }

        private GameMemory _memory;
        public GameMemory Memory => _memory ?? (_memory = new GameMemory(Config.PPTName));

        private GameWorker _gameWorker;
        public GameWorker GameWorker => _gameWorker ?? (_gameWorker = new GameWorker());

        private PPTData _data;
        public PPTData Data => _data ?? (_data = new PPTData());
        public PPTData Old { get; private set; }

        public void Initiate()
        {
            this.CheckGameData(this.Old = new PPTData());


            this.ResetDuration();            


            Core.IsPPTOn = (this.Data.PPTStates.Main > MainStates.None);

            this.Start();
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


        public void CheckGameData(PPTData data)
        {
            data.PPTStates = this.Memory.GetGameState();        
        }

        private void MainWorker_Tick(object sender, EventArgs e)
        {
            this.ProjectSaveFlag = false;

            this.CheckGameData(this.Data);

            // Check
            if (this.Data.PPTStates.Main > MainStates.None)
            {
                // Early update
                if (this.Data.PPTStates.Sub == SubStates.InMatch)
                {
                    if (this.Old.PPTStates.Sub != SubStates.InMatch)
                    {
                        this.Data.WinCount = this.Memory.WinCountForced;
                        this.Data.PlayerCount = this.Memory.LobbySizeInGame;
                        this.Data.PlayerMax = this.Memory.LobbyMax;
                    }
                }


                // Core
                if (this.Data.PPTStates.Sub == SubStates.InMatch)
                {
                    if (this.Old.PPTStates.Sub == SubStates.InMatch)
                    {
                        this.Data.Stars = this.Memory.Stars;
                    }                    
                }


                // Post update
                if (this.Data.PPTStates.Sub == SubStates.InMatch)
                {
                    if (!this.Data.Stars.SequenceEqual(this.Old.Stars))
                    {
                        var diffs = this.Data.Stars.Zip(this.Old.Stars, (x, y) => x - y);

                        for (int playerIndex = 0; playerIndex < 4; playerIndex++)
                        {
                            int diff = diffs.ElementAt(playerIndex);

                            if (diff == 1)
                            {
                                List<int> tokenStars = new List<int>(Core.ProjectData.CountedStars);
                                tokenStars[playerIndex]++;
                                Core.ProjectData.CountedStars = tokenStars;

                                if (this.Data.Stars[playerIndex] == this.Memory.WinCountForced)
                                {
                                    List<int> tokenGames = new List<int>(Core.ProjectData.CountedGames);
                                    tokenGames[playerIndex]++;
                                    Core.ProjectData.CountedGames = tokenGames;
                                }

                                if (!this.ProjectSaveFlag) this.ProjectSaveFlag = true;
                            }
                        }
                    }
                }
            }

            if (this.ProjectSaveFlag) Core.ProjectData.Save();


            this.CheckDuration();


            Core.IsPPTOn = (this.Data.PPTStates.Main > MainStates.None);

            // Next tick
            this.Old = this.Data.Clone() as PPTData;
        }
    }
}
