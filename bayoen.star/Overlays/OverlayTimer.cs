using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using bayoen.library.General.Enums;
using bayoen.library.General.Utilities.Win32;

namespace bayoen.star.Overlays
{
    public class OverlayTimer : DispatcherTimer
    {
        public OverlayTimer()
        {
            this.Interval = Config.OverlayIntervalNormal;
            this.Tick += OverlayTimer_Tick;

        }

        private OverlayVisibility _floatingVisibility;
        public OverlayVisibility FloatingVisibility
        {
            get => this._floatingVisibility;
            private set
            {
                if (this._floatingVisibility == value) return;

                Core.FloatingOverlay.Topmost = (value == OverlayVisibility.Active);

                if (value == OverlayVisibility.Hidden)
                {
                    Core.FloatingOverlay.Hide();
                }
                else if (value == OverlayVisibility.Neutral)
                {
                    User32.SetWindowPos((new WindowInteropHelper(Core.FloatingOverlay)).Handle, (IntPtr)HWND.BOTTOM, 0, 0, 0, 0, (uint)(SWP.NOMOVE | SWP.NOSIZE));
                }
                else if (value == OverlayVisibility.Active)
                {
                    Core.FloatingOverlay.Show();
                }
                else
                {
                    throw new InvalidOperationException($"ERROR: Invalid OverlayVisibility for 'FloatingOverlayVisibility with OverlayVisibility.{value}'");
                }

                this._floatingVisibility = value;
            }
        }

        private void OverlayTimer_Tick(object sender, EventArgs e)
        {
            if (Core.Memory.CheckProcess())
            {
                WindowStatus status = PPTRect.GetWindowStatus();
                if (status == WindowStatus.Closed || status == WindowStatus.Minimized)
                {
                    this.FloatingVisibility = OverlayVisibility.Hidden;
                }
                else if (status == WindowStatus.Lost)
                {
                    this.FloatingVisibility = OverlayVisibility.Neutral;
                }
                else
                {
                    this.FloatingVisibility = OverlayVisibility.Active;
                    PPTRect.UpdateLocation(Core.FloatingOverlay);
                }

                this.Interval = Config.OverlayIntervalNormal;
            }
            else
            {
                this.FloatingVisibility = OverlayVisibility.Hidden;

                this.Interval = Config.OverlayIntervalSlow;
            }
        }
    }
}
