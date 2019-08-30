using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.library.General.Enums;
using bayoen.library.Metro.Controls;

namespace bayoen.star
{
    public static partial class Core
    {
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
