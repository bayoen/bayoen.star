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
            this.Interval = Config.MainInterval;
            this.Tick += MainWorker_Tick;
        }

        private PPTMemory _memory;
        public PPTMemory Memory => _memory ?? (_memory = new PPTMemory(Config.PPTName));

        private GameWorker _gameWorker;
        public GameWorker GameWorker => _gameWorker ?? (_gameWorker = new GameWorker());

        private GameData _data;
        public GameData Data => _data ?? (_data = new GameData());
        public GameData Old { get; private set; }

        public void Initiate()
        {
            this.CheckGameData(this.Old = new GameData());

            this.Start();
        }        

        public void CheckGameData(GameData data)
        {
            data.GameStates = this.Memory.GetGameState();


            if (data.GameStates.Sub == SubStates.InMatch)
            {
                data.Stars = this.Memory.Stars;
            }
            
        }

        private void MainWorker_Tick(object sender, EventArgs e)
        {
            this.CheckGameData(this.Data);



            Core.IsPPTOn = (this.Data.GameStates.Main > MainStates.None);

            // Next tick
            this.Old = this.Data.Clone() as GameData;
        }
    }
}
