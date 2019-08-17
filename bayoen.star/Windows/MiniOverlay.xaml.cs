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
    public partial class MiniOverlay : BaseWindow
    {
        public MiniOverlay()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";
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

        private void FixMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.FixMenuItem.IsChecked = this.IsFixed = !this.IsFixed;
            Core.MainWindow.Title = $"Core.MainWindow.IsFixed: {this.IsFixed}";
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
