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

            this.InitialHeight = this.Height;
            this.InitialWidth = this.Width;
            this.OverlayRatio = 1;
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

        public double InitialHeight { get; private set; }
        public double InitialWidth { get; private set; }

        private double _overlayRatio;
        public double OverlayRatio
        {
            get => this._overlayRatio;
            set
            {
                if (this._overlayRatio == value) return;

                this.Height = this.InitialHeight * value;
                this.Width = this.InitialWidth * value;
                this.LayoutTransform = new ScaleTransform(value, value);

                this._overlayRatio = value;
            }
        }

        public List<double> GetLayout() => new List<double>()
        {
            this.OverlayRatio,
            this.Left,
            this.Top,
        };


        public void SetLayout(List<double> layout)
        {
            this.OverlayRatio = layout[0];
            this.Left = layout[1];
            this.Top = layout[2];
        }

        public void ResetLayout()
        {
            this.OverlayRatio = 1;
            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (SystemParameters.PrimaryScreenWidth - this.Height) / 2;
        }

        private void FixMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.FixMenuItem.IsChecked = this.IsFixed = !this.IsFixed;
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void MiniOverlay_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (this.OverlayRatio < 1.5)
                {
                    this.OverlayRatio += WheelDelta;

                    this.Left -= 0.5 * WheelDelta * this.InitialWidth;
                    this.Top -= 0.5 * WheelDelta * this.InitialHeight;
                }
            }
            else if (e.Delta < 0)
            {
                if (this.OverlayRatio > 0.5)
                {
                    this.OverlayRatio -= WheelDelta;

                    this.Left += 0.5 * WheelDelta * this.InitialWidth;
                    this.Top += 0.5 * WheelDelta * this.InitialHeight;
                }
            }            
        }

        private const double WheelDelta = 0.1;
    }
}
