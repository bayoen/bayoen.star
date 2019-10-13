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

using Octokit;
using MahApps.Metro.Controls.Dialogs;

using bayoen.star.Launcher.ExtendedMethods;

namespace bayoen.star.Launcher
{
    public static class Launcher
    {
        private static LauncherWindow _launcherWindow;
        public static LauncherWindow LauncherWindow => Launcher._launcherWindow ?? (Launcher._launcherWindow = new LauncherWindow());

        // 'Thread' will be changed into 'BackgroundWorker'
        public static Thread CheckThread { get; set; } // Thread 1
        public static Thread DownloadThread { get; set; } // Thread 2
        public static Thread TerminateThread { get; set; } // Thread 3

        public static List<string> Queue { get; set; }
        public static int FileIndex { get; set; }
        public static int FileMax { get; set; }
        public static Version TargetVersion { get; set; }

        public static void Initiate()
        {
            Launcher.LauncherWindow.Show();

            Launcher.Check();
        }

        #region [ Thread 1: Check ]

        public static void Check()
        {
            Launcher.CheckThread = new Thread(() =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = $"Check update... Now v{Config.VersionShortString}";
                    Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = true;
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);

                if (Launcher.IsGoogleOn)
                {
                    GitHubClient client = new GitHubClient(new ProductHeaderValue("bayoen"));
                    List<Release> releases = client.Repository.Release.GetAll("bayoen", "bayoen.star").Result.ToList();

                    Launcher.Queue = new List<string>();
                    if (releases.Count == 0)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        System.Windows.Application.Current.Dispatcher.Invoke(async delegate
                        {
                            Launcher.LauncherWindow.Status = $"No Releases";
                            MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", "Your internet is disconnected or the project server is down. Please contact us", MessageDialogStyle.Affirmative);
                            if (result == MessageDialogResult.Affirmative)
                            {
                                Launcher.StartMainApp();
                            }
                        });
                    }
                    else
                    {
                        Release latest = null;
                        while (releases.Count > 0)
                        {
                            try
                            {
                                latest = releases[0];
                                Launcher.TargetVersion = Version.Parse(latest.TagName);
                            }
                            catch
                            {
                                latest = null;
                                releases.RemoveAt(0);
                            }
                        }

                        if (latest == null)
                        {
                            System.Media.SystemSounds.Exclamation.Play();
                            System.Windows.Application.Current.Dispatcher.Invoke(async delegate
                            {
                                Launcher.LauncherWindow.Status = $"No valid release";
                                MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", "The project server is down. Please contact us", MessageDialogStyle.Affirmative);
                                if (result == MessageDialogResult.Affirmative)
                                {
                                    Launcher.StartMainApp();
                                }
                            });
                            Thread.Sleep(Config.ThreadSleepMilliseconds);
                        }
                        else
                        {
                            if (Launcher.TargetVersion > Config.Version)
                            {
                                // Update
                                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                                {
                                    Launcher.LauncherWindow.Status = $"A new version has been found! {Config.VersionShortString} → {Launcher.TargetVersion}";
                                    Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = false;
                                    Launcher.LauncherWindow.LogoRing.IsActive = true;
                                });

                                latest.Assets.ToList().ForEach(x => Launcher.Queue.Add(x.BrowserDownloadUrl));
                                //Launcher.Queue = Config.SampleFileUrlList; // Download test files

                                Launcher.FileIndex = 0;
                                Launcher.FileMax = Launcher.Queue.Count;

                                Launcher.Download();
                            }
                            else if (Launcher.TargetVersion == Config.Version)
                            {
                                // Up-to-date
                                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                                {
                                    Launcher.LauncherWindow.Status = $"It's already up to date";
                                });
                                Thread.Sleep(Config.ThreadSleepMilliseconds);

                                Launcher.StartMainApp();
                            }
                            else
                            {
                                // Dev
                                System.Media.SystemSounds.Exclamation.Play();
                                System.Windows.Application.Current.Dispatcher.Invoke(async delegate
                                {
                                    Launcher.LauncherWindow.Status = $"Skynet rules this world!!";
                                    MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("Dev", "Hm... we are doing well, right?", MessageDialogStyle.Affirmative);
                                    if (result == MessageDialogResult.Affirmative)
                                    {
                                        // Do nothing
                                    }
                                });
                            }
                        }
                    }
                }
                else
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    System.Windows.Application.Current.Dispatcher.Invoke(async delegate
                    {
                        Launcher.LauncherWindow.Status = $"Internet Disconnected";
                        MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", "Your internet is disconnected", MessageDialogStyle.Affirmative);
                        if (result == MessageDialogResult.Affirmative)
                        {
                            Launcher.StartMainApp();
                        }
                    });
                }
                Thread.Sleep(Config.ThreadSleepMilliseconds);
                               
            });
            Launcher.CheckThread.Start();
        }

        #endregion

        #region [ Thread 2: Download ]

        public static void Download()
        {
            Launcher.DownloadThread = new Thread(() => Launcher.DownloadFirstQueue());
            Launcher.DownloadThread.Start();
        }

        private static void DownloadWeb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                string currentFileName = Path.GetFileName(Launcher.Queue[Launcher.FileIndex]);
                double MBReceived = (double)e.BytesReceived / 1048576;
                double MBToReceive = (double)e.TotalBytesToReceive / 1048576;

                Launcher.LauncherWindow.CurrentFileName = $"Downloading... {currentFileName} ({Launcher.FileIndex + 1}/{Launcher.FileMax} files)";
                Launcher.LauncherWindow.CurrentFileProgress = $"{e.ProgressPercentage}% ({MBReceived:F2} / {MBToReceive:F2} MB)";
                Launcher.LauncherWindow.Progress = e.ProgressPercentage;
            });
        }

        private static void DownloadWeb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Launcher.FileIndex++;

            if (Launcher.FileIndex == Launcher.FileMax)
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = $"Downloaded!";
                    Launcher.LauncherWindow.LogoRing.IsActive = false;
                    Launcher.LauncherWindow.CurrentFileName = "";
                    Launcher.LauncherWindow.CurrentFileProgress = "";
                    Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = true;
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);

                Launcher.DownloadThread.Abort();
                Launcher.DownloadThread = null;

                Launcher.Terminate();
            }
            else
            {
                Launcher.DownloadFirstQueue();
            }
        }

        private static void DownloadFirstQueue()
        {
            using (WebClient downloadWeb = new WebClient())
            {
                downloadWeb.DownloadProgressChanged += DownloadWeb_DownloadProgressChanged;
                downloadWeb.DownloadFileCompleted += DownloadWeb_DownloadFileCompleted;

                string path = Launcher.Queue[Launcher.FileIndex];
                string name = Path.GetFileName(path);

                if (!Directory.Exists(Config.UpdateFolderName))
                {
                    Directory.CreateDirectory(Config.UpdateFolderName);
                }

                downloadWeb.DownloadFileAsync(new Uri(path), Path.Combine(Config.UpdateFolderName, name));
            }
        }

        #endregion

        #region [ Thread 3: Terminate ]

        public static void Terminate()
        {
            Launcher.TerminateThread = new Thread(() =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = $"Extracting... v{Launcher.TargetVersion}";
                    Launcher.LauncherWindow.LogoRing.IsActive = true;
                    Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = false;
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);

                if (Directory.Exists(Config.UpdateFolderName))
                {                    
                    List<string> zipList = Directory.GetFiles(Config.UpdateFolderName).ToList()
                                            .Where(x => x.EndsWith(".zip", StringComparison.OrdinalIgnoreCase)).ToList();

                    if (zipList.Count > 0)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            Launcher.LauncherWindow.Progress = 0;
                        });

                        for (int fileIndex = 0; fileIndex < zipList.Count; fileIndex++)
                        {
                            string fileNameToken = zipList[fileIndex];

                            using (ZipArchive readingToken = ZipFile.Open(fileNameToken, ZipArchiveMode.Update))
                            {
                                readingToken.ExtractToDirectory(Config.UpdateFolderName, true);                                
                            }
                            if (File.Exists(fileNameToken)) File.Delete(fileNameToken);

                            System.Windows.Application.Current.Dispatcher.Invoke(delegate
                            {
                                double progressPercentage = 100 * ((double)fileIndex + 1) / ((double)zipList.Count);
                                Launcher.LauncherWindow.CurrentFileName = $"Extracting... {Path.GetFileName(fileNameToken)}";
                                Launcher.LauncherWindow.CurrentFileProgress = $"{progressPercentage}% ({fileIndex + 1} / {zipList.Count})";
                                Launcher.LauncherWindow.Progress = progressPercentage;
                            });
                        }

                        List<string> fileList = Directory.GetFiles(Config.UpdateFolderName).ToList();
                        fileList.RemoveAll(x => Path.GetFileName(x) == Config.LauncherAppName);
                        for (int fileIndex = 0; fileIndex < fileList.Count; fileIndex++)
                        {
                            if (File.Exists(Path.GetFileName(fileList[fileIndex]))) File.Delete(Path.GetFileName(fileList[fileIndex]));
                            File.Move(fileList[fileIndex], Path.GetFileName(fileList[fileIndex]));
                        }
                    }                    
                }                

                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = $"Extracted!";
                    Launcher.LauncherWindow.LogoRing.IsActive = false;
                    Launcher.LauncherWindow.CurrentFileName = "";
                    Launcher.LauncherWindow.CurrentFileProgress = "";
                    Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = true;
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);

                Launcher.StartMainApp();

            });
            Launcher.TerminateThread.Start();
        }

        #endregion


        private static bool IsGoogleOn
        {
            get
            {
                try
                {
                    using (WebClient client = new WebClient())
                    {
                        using (client.OpenRead("http://google.com/generate_204"))
                        {
                            return true;
                        }
                    }                    
                }
                catch
                {
                    return false;
                }
            }
        }
        private static void StartMainApp()
        {
            System.Windows.Application.Current.Dispatcher.Invoke(delegate
            {
                Launcher.LauncherWindow.DownloadProgressBar.IsEnabled = false;
                Launcher.LauncherWindow.DownloadProgressBar.IsIndeterminate = false;
            });

            if (File.Exists(Config.MainAppName))
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "bayoen~";
                });
                Thread.Sleep(5 * Config.ThreadSleepMilliseconds);

                Process.Start(Config.MainAppName);
                Thread.Sleep(Config.ThreadSleepMilliseconds);

                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    System.Windows.Application.Current.Shutdown();
                });
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    Launcher.LauncherWindow.Status = "???";
                });
                Thread.Sleep(5 * Config.ThreadSleepMilliseconds);

                System.Media.SystemSounds.Hand.Play();
                System.Windows.Application.Current.Dispatcher.Invoke(async delegate
                {
                    MessageDialogResult result = await Launcher.LauncherWindow.ShowMessageAsync("ERROR", $"'{Config.MainAppName}' is missing. Please check the directory and contact us!", MessageDialogStyle.Affirmative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        System.Windows.Application.Current.Shutdown();
                    }
                });
            }
        }
    }
}
