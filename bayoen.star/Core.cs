using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using bayoen.star.Localizations;
using Octokit;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            Culture.Set(Core.Project.LanguageCode);

            Core.InitialWorker = new BackgroundWorker();
            Core.InitialWorker.DoWork += InitialWorker_DoWork;
            Core.InitialWorker.RunWorkerCompleted += InitialWorker_RunWorkerCompleted;
            Core.InitialWorker.RunWorkerAsync();
        }

        private static void InitialWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Core.Project.JustUpdated)
            {
                Core.Project.JustUpdated = false;
                Core.Project.Save();
                Thread.Sleep(Config.ThreadSleepMilliseconds);
                return;
            }
            

            Core.Invoke(delegate
            {
                Core.MainWindow.Show();
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Begin-String";
            });

            if (Core.Project.AutoUpdate)
            {
                Core.Update();
                Thread.Sleep(Config.ThreadSleepMilliseconds);
            }
            else
            {
                Thread.Sleep(Config.ThreadLongSleepMilliseconds); // Long
            }


            Core.Invoke(delegate
            {
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-End-String";
            });
            Thread.Sleep(Config.ThreadLongSleepMilliseconds); // Long
        }

        private static void InitialWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Core.Invoke(delegate
            {
                Core.MainWindow.IsInitial = false;
                Core.TrayIcon.Show();
#if DEBUG
                //Core.CapturableWindow.Show();
                //Core.DebugWindow.Show();
                //Core.DashboardWindow.Show();
                //Core.LeagueWindow.Show();
                //Core.MiniWindow.Show();
                //Core.MiniOverlay.Show();
                //Core.SettingWindow.Show();
#endif
            });

            Core.InitialWorker.Dispose();
        }

        
    }
}
