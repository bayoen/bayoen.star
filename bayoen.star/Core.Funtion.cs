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

using LiteDB;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using bayoen.library.General.Enums;
using bayoen.star.Localizations;
using bayoen.star.Windows;
using bayoen.star.Variables;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            Culture.Set(Core.Project.LanguageCode);

            Core.Project.Version = Config.Assembly.GetName().Version;
            Core.TrayIcon.Visibility = Visibility.Visible;
            Core._settingWindow = new SettingWindow();

            if (!Core.MainWorker.IsEnabled) Core.MainWorker.Initiate();
            if (!Core.GameWorker.IsEnabled) Core.GameWorker.Initiate();

            Core.MainWindow.Show();

#if DEBUG
            //Core.CapturableWindow.Show();
            Core.DebugWindow.Show();
            Core.DashboardWindow.Show();
            //Core.MiniWindow.Show();
            //Core.MiniOverlay.Show();
            Core.SettingWindow.Show();
#endif
        }

        public static void ResetScore()
        {
            Core.Project.CountedStars = new List<int>(4) { 0, 0, 0, 0 };
            Core.Project.CountedGames = new List<int>(4) { 0, 0, 0, 0 };
        }

        /// <summary>
        /// Restart 'bayoen-star' application with <paramref name="mode"/> parameter
        /// </summary>
        /// <param name="mode"></param>
        public static void Restart(RestartingModes mode)
        {
            Core.Project.RestartingMode = mode;
            Core.Project.Save();
            Core.TrayIcon.Terminate();

            Process.Start(Application.ResourceAssembly.Location);
            Environment.Exit(0);
        }

        public static void Save()
        {
            Core.Project.Save();
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
