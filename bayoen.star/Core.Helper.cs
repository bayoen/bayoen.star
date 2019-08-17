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
        private static ChromaKeys _miniChromaKey;
        public static ChromaKeys MiniChromaKey
        {
            get => Core._miniChromaKey;
            set
            {
                if (Core._miniChromaKey == value) return;

                Core.MiniWindow.Background = Config.ChromaSets.Find(x => x.Item1 == value).Item2;
                Core.MiniWindow.BorderThickness = new Thickness(Convert.ToInt32(value == ChromaKeys.None));

                Core._miniChromaKey = value;
            }
        }

        private static GoalTypes _goalType;
        public static GoalTypes GoalType
        {
            get => Core._goalType;
            set
            {
                if (Core._goalType == value) return;

                Core.MiniWindow.GoalType = value;
                Core.MiniOverlay.GoalType = value;

                Core._goalType = value;
            }
        }
    }
}
