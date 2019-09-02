using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using bayoen.library.General.Enums;
using bayoen.library.Metro.Controls;

using bayoen.star.Variables;
using bayoen.star.Workers;

namespace bayoen.star
{
    public static partial class Core
    {
        private static ProjectData _projectData;
        public static ProjectData ProjectData => _projectData ?? (_projectData = ProjectData.Load());

        private static GameMemory _memory;
        public static GameMemory Memory => _memory ?? (_memory = new GameMemory(Config.PPTName));

        private static MainWorker _mainWorker;
        public static MainWorker MainWorker => _mainWorker ?? (_mainWorker = new MainWorker());

        private static GameWorker _gameWorker;
        public static GameWorker GameWorker => _gameWorker ?? (_gameWorker = new GameWorker());
        
        private static bool _isPPTOn;
        public static bool IsPPTOn
        {
            get => Core._isPPTOn;
            set
            {
                if (Core._isPPTOn == value) return;

                Core.MainWindow.PPTOffIconPanel.Visibility = value ? Visibility.Hidden : Visibility.Visible;
                Core.MainWindow.PPTOnIconPanel.Visibility = value ? Visibility.Visible : Visibility.Hidden;

                Core._isPPTOn = value;
            }
        }

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
