using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using bayoen.library.General.Enums;
using bayoen.star.Localizations;


namespace bayoen.star
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            List<Process> overlappedProcesses = Process.GetProcessesByName(Assembly.GetExecutingAssembly().GetName().Name).ToList();
            overlappedProcesses.RemoveAll(x => x.Id == Process.GetCurrentProcess().Id);

            if (overlappedProcesses.Count == 0)
            {
                Core.Initialize();
            }
            else if (Core.Option.RestartingMode > RestartingModes.None)
            {
                Core.Initialize();
            }
            else
            {
                Culture.Set(Core.Option.LanguageCode);
                string overlappedString = this.FindResource("InitialGrid-Message-Overlapped-String") as string;

                System.Media.SystemSounds.Hand.Play();
                MessageBox.Show(overlappedString, Config.Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                Application.Current.Shutdown();
            }
        }
    }
}
