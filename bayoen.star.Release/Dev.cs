using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using bayoen.star.Windows;

namespace bayoen.star.Release
{
    public static partial class Dev
    {
        private static ControlWindow _controlWindow;
        public static ControlWindow ControlWindow => Dev._controlWindow ?? (Dev._controlWindow = new ControlWindow());

        public static void Initialize()
        {
            Dev.ControlWindow.Show();
        }
    }
}
