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
using bayoen.star.Variables;

namespace bayoen.star.Windows
{
    public partial class DebugWindow : BaseWindow
    {
        public DebugWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";                                   

            Core.DebugTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Core.DebugTimer.Tick += (sender, e) =>
            {
                //Core.DebugWindow.TextOut1.Text = $"Core.MainWorker.WorkerDuration:\n{Core.MainWorker.WorkerDuration.TotalMilliseconds.ToString("F2").PadLeft(10)} ms (Avg: {Core.MainWorker.WorkerDurationAverage.TotalMilliseconds:F2} ms)";
                //Core.DebugWindow.TextOut2.Text = $"Core.MainWorker.Interval: { Core.MainWorker.Interval.Milliseconds} [ms]"
                //                                +$"\nCore.MainWorker.Data:\n{Core.MainWorker.Data.ToJson()}";
                //Core.DebugWindow.TextOut3.Text = $"ReadTimer.Interval: {Core.ReadTimer.Interval.Milliseconds} [ms]"
                //                                + $"\nOverlayTimer.LayoutTimer.Interval: {Core.OverlayTimer.LayoutTimer.Interval.TotalMilliseconds} [ms]"
                //                                + $"\nOverlayTimer.ContentTimer.Interval: {Core.OverlayTimer.ContentTimer.Interval.TotalMilliseconds} [ms]";

                this.MainWorkerDataBlock.Text = $"Core.MainWorker.Data:\n{Core.MainWorker.Data.Serialize()}";
                this.ProjectDataBlock.Text = $"Core.ProjectData:\n{Core.ProjectData.Serialize()}";
            };
            Core.DebugTimer.Start();

        }
    }
}
#endif
