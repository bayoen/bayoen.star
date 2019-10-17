using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace bayoen.star.Launcher
{
    public static class Config
    {
        public const string MainAppName = "bayoen-star.exe";
        public static readonly string LauncherAppName = $"{Assembly.GetExecutingAssembly().GetName().Name}.exe";
        public const string UpdateFolderName = "__update__";
        public const int ThreadSleepMilliseconds = 100;

        //public static readonly List<string> SampleFileUrlList = new List<string>()
        //{
        //    "https://file-examples.com/wp-content/uploads/2017/02/zip_10MB.zip",
        //    "https://file-examples.com/wp-content/uploads/2017/02/zip_9MB.zip",
        //    "https://file-examples.com/wp-content/uploads/2017/04/file_example_MP4_1920_18MG.mp4"
        //};

        public static readonly List<Bitmap> LogoBitmapList = new List<Bitmap>()
        {
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle00,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle01,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle02,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle03,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle04,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle05,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle06,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle07,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle08,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle09,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle10,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle11,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle12,
            bayoen.star.Launcher.Properties.Resources.DailyCarbuncle13,
        };

        public static readonly string Title = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public const int VersionMajorNumber = 0;
        public static string VersionString => $"{Config.VersionMajorNumber}.{Assembly.GetExecutingAssembly().GetName().Version.ToString()}";
        public static string VersionShortString
        {
            get
            {
                Version version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{Config.VersionMajorNumber}.{version.Major}.{version.Minor}";
            }
        }
        public static Version Version => Version.Parse(Config.VersionShortString);

        public static readonly DispatcherPriority DispatcherPriority = DispatcherPriority.Normal;
    }
}
