using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using LiteDB;

using bayoen.library.General.Enums;
using bayoen.library.Metro.Controls;

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

        /// <summary>
        /// Visual
        /// </summary>
        private static MainWorker _mainWorker;
        public static MainWorker MainWorker => Core._mainWorker ?? (Core._mainWorker = new MainWorker());

        /// <summary>
        /// In-game 
        /// </summary>
        private static InGameWorker _inGameWorker;
        public static InGameWorker InGameWorker => Core._inGameWorker ?? (Core._inGameWorker = new InGameWorker());

        public static List<MetroComboBoxItem> GetChromaComboBoxItemList()
        {
            List<MetroComboBoxItem> ChromaComboBoxItemList = new List<MetroComboBoxItem>();
            for (int chromaIndex = 0; chromaIndex < Config.ChromaSets.Count; chromaIndex++)
            {
                MetroComboBoxItem TokenChroma = new MetroComboBoxItem()
                {
                    IconForeground = Config.ChromaSets[chromaIndex].Item2,
                };
                TokenChroma.SetResourceReference(MetroComboBoxItem.TextProperty, $"Setting-Streaming-ChromaKey-{chromaIndex}-String");

                ChromaComboBoxItemList.Add(TokenChroma);
            }

            return ChromaComboBoxItemList;
        }        
    }
}
