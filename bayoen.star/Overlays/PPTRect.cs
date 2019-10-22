using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using bayoen.library.General.Utilities.Win32;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Overlays
{
    public class PPTRect
    {
        public static RECT GetRect()
        {
            RECT pptRect = new RECT();
            User32.GetWindowRect(Core.Memory.Process.MainWindowHandle, ref pptRect);
            return pptRect;
        }

        public static WindowStatus GetWindowStatus()
        {
            if (Core.Memory.Process == null) return WindowStatus.Closed;

            IntPtr handle = Core.Memory.Process.MainWindowHandle;
            int currentWindowStyle = User32.GetWindowLong(handle, (int)GWL.GWL_STYLE);

            if ((currentWindowStyle & (uint)WS.WS_MINIMIZE) == (uint)WS.WS_MINIMIZE)
            {
                return WindowStatus.Minimized;
            }
            else if ((currentWindowStyle & (uint)WS.WS_MAXIMIZE) == (uint)WS.WS_MAXIMIZE)
            {
                return WindowStatus.Maximized;
            }
            else if (User32.GetForegroundWindow() == handle)
            {
                return WindowStatus.Focused;
            }
            else
            {
                return WindowStatus.Lost;
            }
        }
        
        public static bool UpdateLocation(OverlayWindow window)
        {
            RECT rect = PPTRect.GetRect();
            WindowStatus status = PPTRect.GetWindowStatus();

            FrameworkElement content = window.Content as FrameworkElement;

            int validHeight = rect.Height - OffsetHeight;
            int validWidth = rect.Width - OffsetWidth;

            double scaleY = (double)validHeight / ValidHeight;
            double scaleX = (double)validWidth / PPTRect.ValidWidth;

            if (rect.Width == SystemParameters.PrimaryScreenWidth && rect.Height == SystemParameters.PrimaryScreenHeight)
            {
                content.LayoutTransform = new ScaleTransform(1, 1);
                window.Top = 0;
                window.Left = 0;
                window.Height = SystemParameters.PrimaryScreenHeight;
                window.Width = SystemParameters.PrimaryScreenWidth;

                return true;
            }
            else if (validHeight <= 0 || validWidth <= 0 || status == WindowStatus.Minimized || status == WindowStatus.Closed)
            {
                window.Visibility = Visibility.Hidden;
                return false;
            }
            else
            {
                if (!window.IsCapturable)
                {
                    window.Topmost = false;
                    window.Topmost = (status == WindowStatus.Focused);
                }

                int validTop = rect.Top - OffsetTop;
                int validLeft = rect.Left - OffsetLeft;

                if (window.Visibility != Visibility.Visible) window.Visibility = Visibility.Visible;

                if (window.Top != validTop) window.Top = validTop;
                if (window.Left != validLeft) window.Left = validLeft;
                if (window.Height != validHeight && window.Width != validWidth)
                {
                    content.LayoutTransform = new ScaleTransform(scaleX, scaleY);
                    window.Height = validHeight;
                    window.Width = validWidth;
                }

                return true;
            }
        }

        private static double ValidHeight => SystemParameters.PrimaryScreenHeight - (double)OffsetHeight;
        private static double ValidWidth => SystemParameters.PrimaryScreenWidth - (double)OffsetWidth;

        private const int OffsetTop = -31;
        private const int OffsetLeft = -8;
        private const int OffsetHeight = 39;
        private const int OffsetWidth = 16;
    }
}
