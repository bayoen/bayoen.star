using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.library.General.Enums;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            Core.Setting.Version = Config.Assembly.GetName().Version;
            Core.TrayIcon.Visibility = Visibility.Visible;

            Core.MainWindow.Show();

#if DEBUG
            Core.DebugWindow.Show();
            Core.MiniWindow.Show();
            Core.MiniOverlay.Show();
            Core.SettingWindow.Show();
#endif
        }

        /// <summary>
        /// Restart 'bayoen-star' application with <paramref name="mode"/> parameter
        /// </summary>
        /// <param name="mode"></param>
        public static void Restart(RestartingModes mode)
        {
            Core.Setting.RestartingMode = mode;
            Core.Setting.Save();
            Core.TrayIcon.Terminate();

            Process.Start(Application.ResourceAssembly.Location);
            Environment.Exit(0);
        }

        public static void Save()
        {
            Core.Setting.Save();
        }

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Environment.Exit(0);
        }
    }
}
