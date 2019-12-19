#if DEBUG
using bayoen.library.Metro.Windows;
using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Threading;
using bayoen.star.Overlays;
using System.Windows.Input;
using System.Windows.Documents;
using System.Windows.Media;

using bayoen.star.Functions;

namespace bayoen.star.Windows
{
    public partial class DebugWindow : BaseWindow
    {
        public DebugWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.Title}";

            Control.MakeDraggableControl(this.YellowRectangle, true);
            Control.MakeDraggableControl(this.GreenRectangle, true);

            this.DebugTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(1),
            };
            this.DebugTimer.Tick += DebugTimer_Tick;
        }

        private DispatcherTimer DebugTimer { get; set; }

        private void DebugTimer_Tick(object sender, EventArgs e)
        {
            this.OptionBlock.Text =
                $"{Core.Option.ToJson()}";

            this.OverlayTimerBlock.Text =
                 $".Interval: {Core.OverlayTimer.Interval.TotalMilliseconds} [ms]"
                + $"\n.FloatingVisibility: {Core.OverlayTimer.FloatingVisibility}";

            this.FloatingOverlayBlock.Text =
                $".Topmost: {Core.FloatingOverlay.Topmost}";


            this.PPTRectBlock.Text =
                $".GetWindowStatus(): {PPTRect.GetWindowStatus()}";

        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DebugTimer.Start();
        }

        private void BaseWindow_Closing(object sender, CancelEventArgs e)
        {
            this.DebugTimer.Stop();
        }

        public new void Show()
        {
            base.Show();
            this.Activate();

            this.DebugTimer.Start();
        }
    }
}
#endif
