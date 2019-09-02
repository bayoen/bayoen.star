#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class DashboardWindow : BaseWindow
    {
        public DashboardWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";

            Core.DashboardTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Core.DashboardTimer.Tick += (sender, e) =>
            {
                this.RootFrameBlock.Text  = $"Root Frame: {Core.Memory.RootFrame.ToString().PadLeft(15)}";
                this.SceneFrameBlock.Text = $"Scene Frame: {Core.Memory.SceneFrame.ToString().PadLeft(14)}";
                this.GameFrameBlock.Text  = $"Game Frame: {Core.Memory.GameFrame.ToString().PadLeft(15)}";
                this.PauseFrameBlock.Text = $"Pause Frame: {Core.Memory.PauseFrame.ToString().PadLeft(14)}";

                this.MainWorkerTickBlock.Text            = $"Tick: {Core.MainWorker.WorkerTick.ToString().PadLeft(17)}";
                this.MainWorkerUnitIntervalBlock.Text    = $"Unit Interval: {Core.MainWorker.Interval.TotalMilliseconds.ToString("F2").PadLeft(8)} [ms]";
                this.MainWorkerDurationBlock.Text        = $"Duration: {Core.MainWorker.WorkerDuration.TotalMilliseconds.ToString("F2").PadLeft(13)} [ms]";
                this.MainWorkerDurationAverageBlock.Text = $"Average: {Core.MainWorker.WorkerDurationAverage.TotalMilliseconds.ToString("F2").PadLeft(14)} [ms]"
                                                         + $"\n{(1 / (Core.MainWorker.WorkerDurationAverage + Core.MainWorker.Interval).TotalSeconds).ToString("F2").PadLeft(23)} [cps]";

                //Core.DebugWindow.TextOut2.Text = $"Core.MainWorker.Interval: { Core.MainWorker.Interval.Milliseconds} [ms]"
                //                                + $"\nCore.MainWorker.Data:\n{Core.MainWorker.Data.ToJson()}";
                //Core.DebugWindow.TextOut3.Text = $"ReadTimer.Interval: {Core.ReadTimer.Interval.Milliseconds} [ms]"
                //                                + $"\nOverlayTimer.LayoutTimer.Interval: {Core.OverlayTimer.LayoutTimer.Interval.TotalMilliseconds} [ms]"
                //                                + $"\nOverlayTimer.ContentTimer.Interval: {Core.OverlayTimer.ContentTimer.Interval.TotalMilliseconds} [ms]";                

                this.CurrentStar1.Score = Core.MainWorker.Data.Stars[0];
                this.CurrentStar2.Score = Core.MainWorker.Data.Stars[1];
                this.CurrentStar3.Score = Core.MainWorker.Data.Stars[2];
                this.CurrentStar4.Score = Core.MainWorker.Data.Stars[3];

                this.CountedStar1.Score = Core.ProjectData.CountedStars[0];
                this.CountedStar2.Score = Core.ProjectData.CountedStars[1];
                this.CountedStar3.Score = Core.ProjectData.CountedStars[2];
                this.CountedStar4.Score = Core.ProjectData.CountedStars[3];

                this.CountedGame1.Score = Core.ProjectData.CountedGames[0];
                this.CountedGame2.Score = Core.ProjectData.CountedGames[1];
                this.CountedGame3.Score = Core.ProjectData.CountedGames[2];
                this.CountedGame4.Score = Core.ProjectData.CountedGames[3];

                this.GoalTypeBlock.Text = $"Goal ~ Type: {Core.ProjectData.GoalType}, ";
                this.GoalCounterBlock.Text = $"Counter: {Core.ProjectData.GoalCounter}, ";
                this.GOdlScoreBlock.Text = $"Score: {Core.ProjectData.GoalScore}";
            };
            Core.DashboardTimer.Start();

        }

        private void ResetDurationButton_Click(object sender, RoutedEventArgs e)
        {
            Core.MainWorker.ResetDuration();
        }

        private void ResetScoreButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ResetScore();
        }
    }
}
#endif
