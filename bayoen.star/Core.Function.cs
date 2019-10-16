using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

using bayoen.library.General.Enums;

namespace bayoen.star
{
    public static partial class Core
    {
        public static void Save()
        {

        }

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Application.Current.Shutdown();
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
            Application.Current.Shutdown();
        }

    }
}
