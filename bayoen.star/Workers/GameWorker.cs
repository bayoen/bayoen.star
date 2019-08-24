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

namespace bayoen.star.Workers
{
    public class GameWorker : DispatcherTimer
    {
        public GameWorker()
        {
            this.Interval = Config.GameInterval;
            this.Tick += GameWorker_Tick;
        }

        public void Run()
        {


            this.Start();
        }

        private void GameWorker_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
