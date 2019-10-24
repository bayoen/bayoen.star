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

using bayoen.library.General.ExtendedMethods;
using bayoen.library.General.Utilities.Win32;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Overlays
{
    public partial class FloatingOverlay : OverlayWindow
    {
        public FloatingOverlay()
        {
            this.InitializeComponent();

            this.TestingImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyImage);
        }

        private void OverlayWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Core.Memory.CheckProcess())
            {
                User32.SetForegroundWindow(Core.Memory.Process.MainWindowHandle);
            }
            else
            {

            }
        }
    }
}
