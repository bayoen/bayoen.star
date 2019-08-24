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
    public partial class DebugWindow : BaseWindow
    {
        public DebugWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";

            Core.DebugTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            Core.DebugTimer.Tick += (sender, e) =>
            {
                Core.DebugWindow.TextOut1.Text = $"Core.ProjectData:\n{Core.ProjectData.ToJson()}";
                Core.DebugWindow.TextOut2.Text = $"Core.MainWorker.Data:\n{Core.MainWorker.Data.ToJson()}";
                //Core.DebugWindow.TextOut3.Text = $"ReadTimer.Interval: {Core.ReadTimer.Interval.Milliseconds} [ms]"
                //                                + $"\nOverlayTimer.LayoutTimer.Interval: {Core.OverlayTimer.LayoutTimer.Interval.Milliseconds} [ms]"
                //                                + $"\nOverlayTimer.ContentTimer.Interval: {Core.OverlayTimer.ContentTimer.Interval.Milliseconds} [ms]";
            };
            Core.DebugTimer.Start();

        }
    }
}
