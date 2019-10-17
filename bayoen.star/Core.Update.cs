using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;

using Octokit;

using bayoen.library.General.ExtendedMethods;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Update()
        {
            Core.Invoke(delegate
            {
                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-String";
            });
            Thread.Sleep(Config.ThreadSleepTimeout);

            // Check online and server
            bool updateBrokenFlag = false;
            Version targetVersion = null;
            Release latest = null;
            for (int onlineTrial = 1; onlineTrial <= Config.OnlineTrialMax; onlineTrial++)
            {
                bool failFlag = false;
                if (Core.IsGoogleOn)
                {
                    try
                    {
                        GitHubClient client = new GitHubClient(new ProductHeaderValue(Config.GitHubUserName));
                        List<Release> releases = client.Repository.Release.GetAll(Config.GitHubUserName, Config.GitHubRepositoryName).Result.ToList();

                        // Reject invalid tagname
                        while (releases.Count > 0)
                        {
                            try
                            {
                                latest = releases[0];
                                targetVersion = Version.Parse(latest.TagName);
                            }
                            catch
                            {
                                // Reject versions with broken tagnames
                                targetVersion = null;
                                latest = null;
                                releases.RemoveAt(0);
                            }
                        }

                        if (releases.Count == 0) // No valid release
                        {
                            updateBrokenFlag = true;
                            Core.Invoke(delegate
                            {
                                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Empty-String";
                            });
                            Thread.Sleep(Config.ThreadLongSleepTimeout);
                            break;
                        }

                    }
                    catch
                    {
                        failFlag = true;
                    }
                }
                else
                {
                    failFlag = true;
                }

                if (failFlag)
                {
                    if (onlineTrial == Config.OnlineTrialMax) updateBrokenFlag = true;
                    Core.Invoke(delegate
                    {
                        Core.MainWindow.SetFormatInitialStatusKey("InitialGrid-Message-Internet-Check-String", onlineTrial);
                    });
                    Thread.Sleep(Config.ThreadLongSleepTimeout); // Long
                }
                else
                {
                    break;
                }
            }

            if (latest == null)
            {
                // No valid latest
                updateBrokenFlag = true;
                Core.Invoke(delegate
                {
                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Empty-String";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);
            }
            else
            {
                if (latest.Assets.Count == 0)
                {
                    // No valid asset
                    updateBrokenFlag = true;
                    Core.Invoke(delegate
                    {
                        Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Empty-String";
                    });
                    Thread.Sleep(Config.ThreadLongSleepTimeout);
                }
            }


            /// [ Instant dev. part ]
            targetVersion = new Version(1, 1, 0); // forced update (because the GitHub server is unavailable now)
            //// Failed to update, skip update
            //if (updateBrokenFlag)
            //{
            //    Core.Invoke(delegate
            //    {
            //        Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Fail-String";
            //    });
            //    Thread.Sleep(Config.ThreadSleepTimeout);
            //    return;
            //}


            // Latest vertion found
            if (targetVersion > Config.Version)
            {
                // Do update
                Core.Invoke(delegate
                {
                    Core.MainWindow.SetFormatInitialStatusKey(
                        "InitialGrid-Message-Update-Found-String", 
                        $"v{Config.Version}", 
                        $"v{targetVersion}");
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                Core.Download.UpdatingNow = true;
                Core.DownloadAssets(latest);
            }
            else if (targetVersion == Config.Version)
            {
                // Already latest
                Core.Invoke(delegate
                {
                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Latest-String";
                });
                Thread.Sleep(Config.ThreadSleepTimeout);
            }
            else
            {
                // Dev
                Core.Invoke(delegate
                {
                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Dev-String";
                });
                Thread.Sleep(Config.ThreadLongSleepTimeout);

                ////
                /// DO TO

            }

        }

        public static void DownloadAssets(Release release)
        {
            //Core.Temp.Queue = release.Assets.ToList().ConvertAll(x => x.BrowserDownloadUrl);
            Core.Download.FilePaths = new List<string>()
            {
                "https://file-examples.com/wp-content/uploads/2017/02/zip_10MB.zip",
                "https://file-examples.com/wp-content/uploads/2017/02/zip_9MB.zip",
                "https://file-examples.com/wp-content/uploads/2017/04/file_example_MP4_1920_18MG.mp4"
            };

            Core.Download.FileLengths = new List<long>(Enumerable.Repeat((long)-1, Core.Download.FilePaths.Count));

            Core.Invoke(delegate
            {
                Core.MainWindow.InitialDownloadPanel.Visibility = Visibility.Visible;
            });
            Thread.Sleep(Config.ThreadSleepTimeout);

            Core.Download.Trial = 0;
            Core.Download.FileIndex = 0;
            Core.Download.FileMax = Core.Download.FilePaths.Count;

            Core.DownloadWorker = new BackgroundWorker();
            Core.DownloadWorker.DoWork += (sender, e) => Core.DownloadSelectedFile();
            Core.DownloadWorker.RunWorkerAsync();

        }

        private static void DownloadSelectedFile()
        {
            using (WebClient downloadWeb = new WebClient())
            {
                string path = Core.Download.FilePaths[Core.Download.FileIndex];
                string name = Path.GetFileName(path);

                downloadWeb.DownloadProgressChanged += DownloadWeb_DownloadProgressChanged;
                downloadWeb.DownloadFileCompleted += DownloadWeb_DownloadFileCompleted;

                if (!Directory.Exists(Config.UpdateFolderName)) Directory.CreateDirectory(Config.UpdateFolderName);
                downloadWeb.DownloadFileAsync(new Uri(path), Path.Combine(Config.UpdateFolderName, name));
            }
        }

        private static void DownloadWeb_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Core.Invoke(delegate
            {
                string currentFileName = Path.GetFileName(Core.Download.FilePaths[Core.Download.FileIndex]);
                double MBReceived = (double)e.BytesReceived / 1048576;
                if (Core.Download.FileLengths[Core.Download.FileIndex] == -1) Core.Download.FileLengths[Core.Download.FileIndex] = e.TotalBytesToReceive;
                double MBToReceive = (double)Core.Download.FileLengths[Core.Download.FileIndex] / 1048576;

                Core.MainWindow.CurrentFileName = $"Downloading... {currentFileName} ({Core.Download.FileIndex + 1}/{Core.Download.FileMax} files)";
                Core.MainWindow.CurrentFileProgress = $"{e.ProgressPercentage}% ({MBReceived:F2} / {MBToReceive:F2} MB)";
                Core.MainWindow.Progress = e.ProgressPercentage;
            });
        }

        private static void DownloadWeb_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            bool failFlag = false;
            string path = Path.Combine(Config.UpdateFolderName, Path.GetFileName(Core.Download.FilePaths[Core.Download.FileIndex]));

            FileInfo info = new FileInfo(path);
            if (info.Exists)
            {
                if (info.Length == Core.Download.FileLengths[Core.Download.FileIndex])
                {



                }
                else
                {
                    failFlag = true;
                }
            }
            else
            {
                failFlag = true;
            }

            if (failFlag)
            {
                Core.Download.Trial++;

                if (Core.Download.Trial == Config.OnlineTrialMax)
                {
                    Core.Invoke(delegate
                    {
                        Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Fail-String";
                    });
                    Thread.Sleep(Config.ThreadLongSleepTimeout);
                }
                else
                {
                    Core.DownloadSelectedFile();
                }
            }
            else
            {
                Core.Download.Trial = 0;
                Core.Download.FileIndex++;

                if (Core.Download.FileIndex == Core.Download.FileMax)
                {
                    Core.Invoke(delegate
                    {
                        Core.MainWindow.InitialStatus = $"Downloaded!";
                        Core.MainWindow.InitialLogoRing.IsActive = false;
                        Core.MainWindow.CurrentFileName = "";
                        Core.MainWindow.CurrentFileProgress = "";
                        Core.MainWindow.DownloadProgressBar.IsIndeterminate = true;
                    });
                    Thread.Sleep(Config.ThreadSleepTimeout);

                    Core.DownloadWorker.Dispose();
                    Core.Download.FilePaths.Clear();
                    Core.Download.FilePaths = null;

                    Core.TerminateToUpdate();
                }
                else
                {
                    Core.DownloadSelectedFile();
                }
            }

            (sender as WebClient).Dispose();
        }

        public static void TerminateToUpdate()
        {
            string tempLauencherPath = Path.Combine(Config.UpdateFolderName, Config.LauencherFileName);
            if (File.Exists(tempLauencherPath))
            {
                if (File.Exists(Config.LauencherFileName)) File.Delete(Config.LauencherFileName);
                File.Move(tempLauencherPath, Config.LauencherFileName);

                Core.Option.JustUpdated = true;
                Core.Option.Save();
                Core.TrayIcon.Terminate();

                Process.Start(Config.LauencherFileName);
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                // missing launcher from update assets
                Core.PostInitialization();
            }

        }

        public static List<string> BuildUpdateList()
        { // in thread
            GitHubClient client = new GitHubClient(new ProductHeaderValue(Config.GitHubUserName));
            List<Release> releases = client.Repository.Release.GetAll(Config.GitHubUserName, Config.GitHubRepositoryName).Result.ToList();

            if (releases.Count == 0)
            {
                return null;
            }
            else
            {
                Release latest = null;
                do
                {
                    try
                    {
                        latest = releases[0];
                    }
                    catch
                    {
                        latest = null;
                        releases.RemoveAt(0);
                        continue;
                    }

                    return (latest == null) ? null : new List<string>(latest.Assets.ToList().ConvertAll(x => x.BrowserDownloadUrl));

                } while (releases.Count > 0);
            }

            return null;
        }

        public static bool IsGoogleOn
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
    }
}
