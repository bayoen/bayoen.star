using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.library.General.Enums;

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


    }
}
