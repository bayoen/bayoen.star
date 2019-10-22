using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

using bayoen.library.General.Utilities.Win32;

namespace bayoen.star.Overlays
{
    public class OverlayTimer : DispatcherTimer
    {
        public OverlayTimer()
        {
            this.Interval = Config.OverlayIntervalSlow;
            this.Tick += OverlayTimer_Tick;

        }

        private void OverlayTimer_Tick(object sender, EventArgs e)
        {
            if (Core.Memory.CheckProcess())
            {
                WindowStatus status = PPTRect.GetWindowStatus();
                if (status == WindowStatus.Closed
                    || status == WindowStatus.Minimized)
                {
                    Core.FloatingOverlay.Hide();

                    this.Interval = Config.OverlayIntervalNormal;
                }
                else if (status == WindowStatus.Lost)
                {
                    //Core.MainOverlay.Hide();
                    Core.FloatingOverlay.Topmost = false;
                    

                    this.Interval = Config.OverlayIntervalNormal;
                }
                else
                {
                    Core.FloatingOverlay.Show();
                    PPTRect.UpdateLocation(Core.FloatingOverlay);
                    Core.FloatingOverlay.Topmost = true;

                    this.Interval = Config.OverlayIntervalNormal;
                }
            }
            else
            {
                Core.FloatingOverlay.Hide();
                this.Interval = Config.OverlayIntervalSlow;
            }


        }
    }
}
