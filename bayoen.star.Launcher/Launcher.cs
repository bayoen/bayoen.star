using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

using System.Windows;
using System.Diagnostics;
using System.Reflection;

using MahApps.Metro.Controls.Dialogs;

using bayoen.star.Launcher.ExtendedMethods;

namespace bayoen.star.Launcher
{
    public static class Launcher
    {
        private static LauncherWindow _launcherWindow;
        public static LauncherWindow LauncherWindow => Launcher._launcherWindow ?? (Launcher._launcherWindow = new LauncherWindow());

        public static BackgroundWorker LaunchWorker { get; set; }

        public static void Initiate()
        {
            Launcher.LaunchWorker = new BackgroundWorker();
            Launcher.LaunchWorker.DoWork += LaunchWorker_DoWork; ;
            Launcher.LaunchWorker.RunWorkerCompleted += LaunchWorker_RunWorkerCompleted; ;
            Launcher.LaunchWorker.RunWorkerAsync();
        }

        private static void LaunchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Launcher.Invoke(delegate
            {
                Launcher.LauncherWindow.Show();
                Launcher.LauncherWindow.LogoRing.IsActive = true;
            });
            Thread.Sleep(Config.ThreadLongSleepTimeout);

            Launcher.Invoke(delegate
            {
                Launcher.LauncherWindow.Status = "Check update folder";
            });
            Thread.Sleep(Config.ThreadSleepTimeout);

            if (Directory.Exists(Config.UpdateFolderName))
            {
                Launcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "Install new files";
                });
                Thread.Sleep(Config.ThreadSleepTimeout);

                List<string> filePaths = Directory.GetFiles(Config.UpdateFolderName).ToList();
                int removed = filePaths.RemoveAll(x => Path.GetFileName(x) == Config.LauncherAppName);

                foreach (string path in filePaths)
                {
                    string fileName = Path.GetFileName(path);
                    if (File.Exists(fileName)) File.Delete(fileName);
                    File.Move(path, fileName);
                }

                if (removed == 0) Directory.Delete(Config.UpdateFolderName, true);
            }
            else
            {
                Launcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "There is nothing to update";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                Launcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "Start 'bayoen-star'";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                Launcher.StartMainApp();
                return;
            }

            Launcher.Invoke(delegate
            {
                Launcher.LauncherWindow.Status = "Start 'bayoen-star'";
            });
            Thread.Sleep(Config.ThreadLongSleepTimeout);

            Launcher.StartMainApp();
        }

        private static void LaunchWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Launcher.LaunchWorker.Dispose();
        }

        private static void StartMainApp()
        {
            Launcher.Invoke(delegate
            {
                Launcher.LauncherWindow.LogoRing.IsActive = false;
            });


            if (File.Exists(Config.MainAppName))
            {
                Launcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "bayoen~";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                bool isMainAppBroken = false;
                try
                {
                    Process.Start(Config.MainAppName);
                }
                catch (Exception e)
                {
                    isMainAppBroken = true;
                    System.Media.SystemSounds.Hand.Play();
                    Launcher.Invoke(async delegate
                    {
                        MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", $"{Config.LauncherAppName} is broken, '{e.Message}'", MessageDialogStyle.Affirmative);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            Application.Current.Shutdown();
                        }
                    });
                }

                if (!isMainAppBroken)
                {
                    Launcher.Invoke(delegate
                    {
                        Application.Current.Shutdown();
                    });
                }
            }
            else
            {
                Launcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "???";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                System.Media.SystemSounds.Hand.Play();
                Launcher.Invoke(async delegate
                {
                    MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", $"'{Config.MainAppName}' is missing. Please check the directory and contact us!", MessageDialogStyle.Affirmative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        Application.Current.Shutdown();
                    }
                });
            }
        }

        private static void Invoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);
    }
}
