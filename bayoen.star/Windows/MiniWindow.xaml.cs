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
    public partial class MiniWindow : BaseWindow
    {
        public MiniWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";

            this._isMouseEnter = true;
            this.IsMouseEnter = false;
        }

        private void BaseWindow_MouseEnter(object sender, MouseEventArgs e) => this.IsMouseEnter = true;

        private void BaseWindow_MouseLeave(object sender, MouseEventArgs e) => this.IsMouseEnter = false;

        private bool _isMouseEnter;
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

        private GoalTypes _goalType;
        public GoalTypes GoalType
        {
            get => this._goalType;
            set
            {
                if (this._goalType == value) return;

                this.Score2Panel.GoalType = value;

                this._goalType = value;
            }
        }

        private const int MetroTitlebarHeight = 30;
    }
}
