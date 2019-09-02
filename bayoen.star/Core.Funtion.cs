using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.library.General.Enums;

using bayoen.star.Localizations;
using bayoen.star.Windows;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            Culture.Set(Core.ProjectData.LanguageCode);

            Core.ProjectData.Version = Config.Assembly.GetName().Version;
            Core.TrayIcon.Visibility = Visibility.Visible;
            Core._settingWindow = new SettingWindow();

            if (!Core.MainWorker.IsEnabled) Core.MainWorker.Initiate();

            Core.MainWindow.Show();

            Core.IsPPTOn = false;
#if DEBUG
            Core.CapturableWindow.Show();
            Core.DebugWindow.Show();
            Core.DashboardWindow.Show();
            Core.MiniWindow.Show();
            Core.MiniOverlay.Show();
            Core.SettingWindow.Show();
#endif
        }

        public static void ResetScore()
        {
            Core.ProjectData.CountedStars = new List<int>(4) { 0, 0, 0, 0 };
            Core.ProjectData.CountedGames = new List<int>(4) { 0, 0, 0, 0 };
        }

        /// <summary>
        /// Restart 'bayoen-star' application with <paramref name="mode"/> parameter
        /// </summary>
        /// <param name="mode"></param>
        public static void Restart(RestartingModes mode)
        {
            Core.ProjectData.RestartingMode = mode;
            Core.ProjectData.Save();
            Core.TrayIcon.Terminate();

            Process.Start(Application.ResourceAssembly.Location);
            Environment.Exit(0);
        }

        public static void Save()
        {
            Core.ProjectData.Save();
        }

        public static void ShowFolder()
        {
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Environment.Exit(0);
        }
    }
}
