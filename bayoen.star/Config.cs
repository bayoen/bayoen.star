using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

using bayoen.library.General.Enums;
using LiteDB;

namespace bayoen.star
{
    public class Config
    {
        // Project Info.
        public static readonly Assembly Assembly = System.Reflection.Assembly.GetExecutingAssembly();
        public static readonly string AssemblyTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;

        // Game Info.        
        public const int RatingMax = 50000;
        public const int CheatRatingMax = 65535;

        // Properties
        public static readonly string PPTName = "puyopuyotetris";

        public static readonly TimeSpan TrackingIntervalNormal  = TimeSpan.FromMilliseconds(1);      // 1 ms
        public static readonly TimeSpan DisplayIntervalNormal   = TimeSpan.FromMilliseconds(50);     // 50 ms

        public static readonly TimeSpan TrackingIntervalSlow    = TimeSpan.FromMilliseconds(5);      // 5 ms
        public static readonly TimeSpan DisplayIntervalSlow     = TimeSpan.FromMilliseconds(300);    // 300 ms

        public static readonly TimeSpan WaitingInterval         = TimeSpan.FromMilliseconds(500);    // 100 ms

        public static readonly List<string> CultureCodes = new List<string>() { "en", "ko", "ja" };
        public static readonly List<Tuple<ChromaKeys, Brush>> ChromaSets = new List<Tuple<ChromaKeys, Brush>>()
        {
            new Tuple<ChromaKeys, Brush>(ChromaKeys.None,  new SolidColorBrush(Color.FromRgb(120, 120, 120))),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Magenta, Brushes.Magenta),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Green, Brushes.Green),
            new Tuple<ChromaKeys, Brush>(ChromaKeys.Blue, Brushes.Blue),
        };

        public const int ScoreCheckFramePeriod = 50;


        // File Names
        public const string ProjectDataFileName = "data.json";
        public const string DataBaseFileName = "records.db";
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
        public static readonly ConnectionString DBConnectionString = new ConnectionString(Config.DataBaseFileName)
        {
            Password = "546050", // 546050, Only SemiRain Knows
        };
    }
}
