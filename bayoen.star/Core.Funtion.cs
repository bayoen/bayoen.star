using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.library.General.Enums;
using bayoen.star.Localizations;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Initialize()
        {
            if (!Config.CultureCodes.Contains(Core.ProjectData.CultureCode))
            {
                string cultureCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();
                if (!Config.CultureCodes.Contains(cultureCode)) cultureCode = Config.CultureCodes[0];
                Core.ProjectData.CultureCode = cultureCode;
            }
            Culture.Set(Core.ProjectData.CultureCode);

            Core.ProjectData.Version = Config.Assembly.GetName().Version;
            Core.TrayIcon.Visibility = Visibility.Visible;

            Core.MainWindow.Show();
            Core.MainWorker.Initiate();

            Core.IsPPTOn = false;
#if DEBUG
            Core.DebugWindow.Show();
            Core.MiniWindow.Show();
            //Core.MiniOverlay.Show();
            Core.SettingWindow.Show();
#endif
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

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Environment.Exit(0);
        }
    }
}
