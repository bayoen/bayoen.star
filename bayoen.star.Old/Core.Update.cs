using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

using Octokit;

namespace bayoen.star
{
    public static partial class Core
    {
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
