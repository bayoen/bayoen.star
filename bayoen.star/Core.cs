using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using bayoen.library.General.Enums;
using bayoen.star.Localizations;
using Octokit;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            Core.InitialWorker = new BackgroundWorker();
            Core.InitialWorker.DoWork += InitialWorker_DoWork;
            Core.InitialWorker.RunWorkerCompleted += (sender, e) => Core.InitialWorker.Dispose();
            Core.InitialWorker.RunWorkerAsync();
        }

        private static void InitialWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Culture.Set(Core.Option.LanguageCode);

            // Open windows and begin display
            Core.Invoke(delegate
            {
                if (!Core.Option.StartMinimized)
                {
                    Core.MainWindow.Show();
                }
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Begin-String";
            });
            Thread.Sleep(Config.ThreadSleepTimeout);


            // Update
            Core.Download.UpdatingNow = false;
            if (Core.Option.JustUpdated)
            {
                Core.Option.JustUpdated = false;
                Core.PostInitialization();
            }
            else if (Core.Option.AutoUpdate)
            {
                Core.Update();
            }
            else
            {
                Thread.Sleep(Config.ThreadLongSleepTimeout);
                Core.PostInitialization();
            }
        }

        /// <summary>
        /// Terminate 'bayoen-star' application
        /// </summary>
        public static void PostInitialization()
        {
            // Load database
            Core.Invoke(delegate
            {
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Database-String";
            });
            Thread.Sleep(Config.ThreadSleepTimeout);
            Core.CheckDB();


            // Finish
            Core.Invoke(delegate
            {
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-End-String";
            });
            Thread.Sleep(Config.ThreadSleepTimeout);


            Core.Invoke(delegate
            {
                Core.MainWindow.IsInitialProgress = false;
                Core.TrayIcon.Show();

                if (Core.Option.RestartingMode > RestartingModes.RestartOnly)
                {
                    Core.Option.RestartingMode = RestartingModes.None;
                }

                if (Core.Option.RestartingMode == RestartingModes.RestartWithSetting)
                {
                    Core.SettingWindow.Show();
                }

                if (Core.Option.OpenMini)
                {
                    Core.MiniWindow.Show();
                }
#if DEBUG
                //Core.FloatingOverlay.Show();
                Core.OverlayTimer.Start();

                //Core.MainOverlay.Show();

                //Core.CapturableWindow.Show();
                Core.DebugWindow.Show();
                //Core.LeagueWindow.Show();
                //Core.MiniWindow.Show();
                //Core.MiniOverlay.Show();
                //Core.SettingWindow.Show();
#endif
            });
        }
    }
}
