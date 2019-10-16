using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MiniWindow
    {
        private bool _isMouseEnter = true;
        public bool IsMouseEnter
        {
            get => this._isMouseEnter;
            set
            {
                if (this._isMouseEnter == value) return;

                this.TitlebarHeight = value ? MetroTitlebarHeight : 0;
                this.TopGrid.Margin = new Thickness(0, value ? 0 : MetroTitlebarHeight, 0, 0);

                this._isMouseEnter = value;
            }
        }

        private GoalCounters _goalType;
        public GoalCounters GoalCounter
        {
            get => this._goalType;
            set
            {
                if (this._goalType == value) return;

                this.MiniScorePanel.GoalCounter = value;

                this._goalType = value;
            }
        }

        private const int MetroTitlebarHeight = 30;
    }
}
