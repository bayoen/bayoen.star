using bayoen.star.Variables;
using bayoen.star.Workers;

namespace bayoen.star
{
    public static partial class Core
    {
        private static ProjectData _project;
        public static ProjectData Project => Core._project ?? (Core._project = ProjectData.Load());

        private static OperationData _temp;
        public static OperationData Temp => Core._temp ?? (Core._temp = new OperationData());

        private static PPTData _data;
        public static PPTData Data => Core._data ?? (Core._data = new PPTData());
        public static PPTData Old { get; set; }

        private static GameMemory _memory;
        public static GameMemory Memory => Core._memory ?? (Core._memory = new GameMemory(Config.PPTName));

        private static Database _db;
        public static Database DB => Core._db ?? (Core._db = new Database());

        private static LiveChecker _live;
        public static LiveChecker Live => Core._live ?? (Core._live = new LiveChecker());

        public static EventRecord Event { get; set; }
        public static MatchRecord Match { get; set; }
        public static GameRecord Game { get; set; }
        public static MatchRecord Last { get; set; }
    }
}
