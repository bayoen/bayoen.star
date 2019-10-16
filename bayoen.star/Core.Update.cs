using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

using Octokit;

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
            Thread.Sleep(Config.ThreadSleepMilliseconds);

            // Check online
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

                        if (releases.Count == 0)
                        {
                            // No valid release
                            updateBrokenFlag = true;
                            Core.Invoke(delegate
                            {
                                Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Empty-String";
                            });
                            Thread.Sleep(Config.ThreadLongSleepMilliseconds);
                            break;
                        }
                        else if (latest != null)
                        {
                            if (latest.Assets.Count == 0)
                            {
                                // No valid asset
                                updateBrokenFlag = true;
                                Core.Invoke(delegate
                                {
                                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Empty-String";
                                });
                                Thread.Sleep(Config.ThreadLongSleepMilliseconds);
                                break;
                            }
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
                    Thread.Sleep(Config.ThreadLongSleepMilliseconds); // Long
                }
                else
                {
                    break;
                }
            }

            if (updateBrokenFlag)
            {
                // Failed to update
                Core.Invoke(delegate
                {
                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-Fail-String";
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);
            }
            else
            {
                // Latest vertion found
                if (targetVersion > Config.Version)
                {
                    // Do update
                    Thread.Sleep(Config.ThreadLongSleepMilliseconds);

                    Core.TerminateToUpdate();
                    //Core.SaveDownloadListFile(latest.Assets.ToList().ConvertAll(x => x.BrowserDownloadUrl));
                }
                else if (targetVersion == Config.Version)
                {
                    // Already latest

                }
                else
                {
                    // Dev

                }

                // 
                Core.Invoke(delegate
                {
                    Core.MainWindow.InitialStatusResource = "InitialGrid-Message-Update-String";
                });
                Thread.Sleep(Config.ThreadSleepMilliseconds);

            }
        }

        public static void TerminateToUpdate()
        {
            Core.Project.Save();
            Core.TrayIcon.Terminate();

            if (File.Exists(Config.LauencherFileName))
            {
                Process.Start(Config.LauencherFileName);
                System.Windows.Application.Current.Shutdown();
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
