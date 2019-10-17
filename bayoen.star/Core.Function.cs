using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;

using bayoen.library.General.Enums;

namespace bayoen.star
{
    public static partial class Core
    {
        /// <summary>
        /// Terminate 'bayoen-star' application
        /// </summary>
        public static void Save()
        {
            Core.Option.Save();
        }

        /// <summary>
        /// Terminate 'bayoen-star' application
        /// </summary>
        public static void CheckDB()
        {

        }

        public static void ShowFolder()
        {
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        /// <summary>
        /// Terminate 'bayoen-star' application
        /// </summary>
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
            Core.Option.RestartingMode = mode;
            Core.TrayIcon.Terminate();

            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

    }
}
