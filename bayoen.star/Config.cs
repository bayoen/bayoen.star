
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using bayoen.library.General.Enums;
namespace bayoen.star
{
    public static class Config
    {
        // Update Properties
        public static readonly string GitHubUserName = "bayoen";
        public static readonly string GitHubRepositoryName = "bayoen.star";
        public const int OnlineTrialMax = 5;
        public const int OnlineThreadSleepMilliseconds = 50;

        // Properties
        public static readonly string PPTName = "puyopuyotetris";

        public static readonly TimeSpan OverlayIntervalNormal = TimeSpan.FromMilliseconds(150);
        public static readonly TimeSpan OverlayIntervalSlow = TimeSpan.FromMilliseconds(1500);

        public static readonly TimeSpan TrackingIntervalNormal = TimeSpan.FromMilliseconds(1);      // 1 ms
        public static readonly TimeSpan DisplayIntervalNormal = TimeSpan.FromMilliseconds(50);     // 50 ms

        public static readonly TimeSpan TrackingIntervalSlow = TimeSpan.FromMilliseconds(5);      // 5 ms
        public static readonly TimeSpan DisplayIntervalSlow = TimeSpan.FromMilliseconds(300);    // 300 ms

        public static readonly TimeSpan WaitingInterval = TimeSpan.FromMilliseconds(500);    // 100 ms

        public static readonly List<string> CultureCodes = new List<string>() { "en", "ko", "ja", "es", "fr", "pt" };
        public static readonly List<Tuple<ChromaKeys, Brush>> ChromaSets = new List<Tuple<ChromaKeys, Brush>>()
        {
            new Tuple<ChromaKeys, Brush>(ChromaKeys.None,  new SolidColorBrush(Color.FromRgb(120, 120, 120))),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Magenta, Brushes.Magenta),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Green, Brushes.Green),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Blue, Brushes.Blue),
        };

        public const int ScoreCheckFramePeriod = 50;
        public const int ThreadSleepTimeout = 150;
        public const int ThreadLongSleepTimeout = 1000;


        // File Names
        public const string LauncherFileName = "bayoen-star-launcher.exe";
        public const string OptionDataFileName = "option.json";
        public const string ProjectDataFileName = "data.json";
        public const string DataBaseFileName = "records.db";
        public const string UpdateFolderName = "__update__";

        public static readonly DispatcherPriority DispatcherPriority = DispatcherPriority.Normal;
        public static readonly Encoding TextEncoding = Encoding.Unicode;
        public static readonly JsonSerializerSettings JsonSerializerSetting = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
            Converters = new List<JsonConverter>()
            {
                new StringEnumConverter()
                {
                    NamingStrategy = new DefaultNamingStrategy(),
                },
                new IsoDateTimeConverter()
                {
                    Culture = CultureInfo.CurrentCulture,
                    DateTimeStyles = DateTimeStyles.AdjustToUniversal,
                },
            }
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
    }
}
