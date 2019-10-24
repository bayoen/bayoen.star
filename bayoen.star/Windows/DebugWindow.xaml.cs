#if DEBUG
using bayoen.library.Metro.Windows;
using System;
using System.Windows;
using System.ComponentModel;
using System.Windows.Threading;
using bayoen.star.Overlays;

namespace bayoen.star.Windows
{
    public partial class DebugWindow : BaseWindow
    {
        public DebugWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.Title}";

            this.DebugTimer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(1),
            };
            this.DebugTimer.Tick += DebugTimer_Tick;
        }

        private DispatcherTimer DebugTimer { get; set; }

        private void DebugTimer_Tick(object sender, EventArgs e)
        {
            this.OverlayTimerBlock.Text = 
                 $".Interval: {Core.OverlayTimer.Interval.TotalMilliseconds} [ms]"
                + $"\n.FloatingVisibility: {Core.OverlayTimer.FloatingVisibility}";

            this.FloatingOverlayBlock.Text =
                $".Topmost: {Core.FloatingOverlay.Topmost}";


            this.PPTRectBlock.Text = $".GetWindowStatus(): {PPTRect.GetWindowStatus()}";

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
