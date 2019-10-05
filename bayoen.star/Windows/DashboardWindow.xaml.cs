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
using bayoen.library.General.Enums;
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
            Core.DashboardTimer.Tick += DashboardTimer_Tick;
            Core.DashboardTimer.Start();

        }

        private void DashboardTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                this.MainWorkerTickBlock.Text = $"MainWorker\n\nTick: {Core.MainWorker.WorkerTick.ToString().PadLeft(17)}";
                this.MainWorkerUnitIntervalBlock.Text = $"Unit Interval: {Core.MainWorker.Interval.TotalMilliseconds.ToString("F2").PadLeft(8)} [ms]";
                this.MainWorkerDurationBlock.Text = $"Duration: {Core.MainWorker.WorkerDuration.TotalMilliseconds.ToString("F2").PadLeft(13)} [ms]";
                this.MainWorkerDurationAverageBlock.Text = $"Average: {Core.MainWorker.WorkerDurationAverage.TotalMilliseconds.ToString("F2").PadLeft(14)} [ms]"
                                                         + $"\n{(1 / ((Core.MainWorker.WorkerDurationAverage + Core.MainWorker.Interval).TotalSeconds)).ToString("F2").PadLeft(23)} [cps]";

                this.GameWorkerTickBlock.Text = $"GameWorker\n\nTick: {Core.InGameWorker.WorkerTick.ToString().PadLeft(17)}";
                this.GameWorkerUnitIntervalBlock.Text = $"Unit Interval: {Core.InGameWorker.Interval.TotalMilliseconds.ToString("F2").PadLeft(8)} [ms]";
                this.GameWorkerDurationBlock.Text = $"Duration: {Core.InGameWorker.WorkerDuration.TotalMilliseconds.ToString("F2").PadLeft(13)} [ms]";
                this.GameWorkerDurationAverageBlock.Text = $"Average: {Core.InGameWorker.WorkerDurationAverage.TotalMilliseconds.ToString("F2").PadLeft(14)} [ms]"
                                                         + $"\n{(1 / ((Core.InGameWorker.WorkerDurationAverage + Core.InGameWorker.Interval).TotalSeconds)).ToString("F2").PadLeft(23)} [cps]";

                this.CurrentMatchBlock.Text = $"{Core.Match.ToJson()}";
                this.CurrentGameBlock.Text = $"{Core.Game.ToJson()}";

                if (Core.Data.States.Main <= MainStates.Offline) return;

                this.RootFrameBlock.Text = $"Root Frame: {Core.Memory.RootFrame.ToString().PadLeft(15)}";
                this.SceneFrameBlock.Text = $"Scene Frame: {Core.Memory.SceneFrame.ToString().PadLeft(14)}";
                this.GameFrameBlock.Text = $"Game Frame: {Core.Memory.GameFrame.ToString().PadLeft(15)}";
                this.PauseFrameBlock.Text = $"Pause Frame: {Core.Memory.PauseFrame.ToString().PadLeft(14)}";

                //Core.DebugWindow.TextOut2.Text = $"Core.MainWorker.Interval: { Core.MainWorker.Interval.Milliseconds} [ms]"
                //                                + $"\nCore.MainWorker.Data:\n{Core.MainWorker.Data.ToJson()}";
                //Core.DebugWindow.TextOut3.Text = $"ReadTimer.Interval: {Core.ReadTimer.Interval.Milliseconds} [ms]"
                //                                + $"\nOverlayTimer.LayoutTimer.Interval: {Core.OverlayTimer.LayoutTimer.Interval.TotalMilliseconds} [ms]"
                //                                + $"\nOverlayTimer.ContentTimer.Interval: {Core.OverlayTimer.ContentTimer.Interval.TotalMilliseconds} [ms]";                

                this.CurrentStar1.Score = Core.Data.Stars[0];
                this.CurrentStar2.Score = Core.Data.Stars[1];
                this.CurrentStar3.Score = Core.Data.Stars[2];
                this.CurrentStar4.Score = Core.Data.Stars[3];

                this.CountedStar1.Score = Core.Project.CountedStars[0];
                this.CountedStar2.Score = Core.Project.CountedStars[1];
                this.CountedStar3.Score = Core.Project.CountedStars[2];
                this.CountedStar4.Score = Core.Project.CountedStars[3];

                this.CountedGame1.Score = Core.Project.CountedGames[0];
                this.CountedGame2.Score = Core.Project.CountedGames[1];
                this.CountedGame3.Score = Core.Project.CountedGames[2];
                this.CountedGame4.Score = Core.Project.CountedGames[3];

                this.GoalTypeBlock.Text = $"Goal ~ Type: {Core.Project.GoalType}, ";
                this.GoalCounterBlock.Text = $"Counter: {Core.Project.GoalCounter}, ";
                this.GOalScoreBlock.Text = $"Score: {Core.Project.GoalScore}";
            }
            catch (Exception ex)
            {
                Core.DebugWindow.TextOut1.Text = ex.Message;

            }
        }

        private void ResetDurationButton_Click(object sender, RoutedEventArgs e)
        {
            Core.MainWorker.ResetDuration();
            Core.InGameWorker.ResetDuration();
        }

        private void ResetScoreButton_Click(object sender, RoutedEventArgs e)
        {
            Core.ResetScore();
        }

        private void ResetMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Match.Reset();
            Core.Game.Reset();
        }

        private void ResetRecordButton_Click(object sender, RoutedEventArgs e)
        {
            Core.DB.MatchClear();            
            Core.MainWindow.EventViewer.SetPage(0);
            Core.MainWindow.EventViewer.CheckNavigator();
        }
    }
}
#endif
