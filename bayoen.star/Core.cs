using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using bayoen.star.Variables;
using bayoen.star.Windows;
using bayoen.star.Workers;

namespace bayoen.star
{
    public static partial class Core
    {
        private static TrayIcon _trayIcon;
        /// <summary>
        /// System tray icon
        /// </summary>
        public static TrayIcon TrayIcon => _trayIcon ?? (_trayIcon = new TrayIcon());

        private static MainWindow _mainWindow;
        /// <summary>
        /// Main window of application. <see cref="MainWindow"/> contains @@@.
        /// </summary>
        public static MainWindow MainWindow => _mainWindow ?? (_mainWindow = new MainWindow());

        /// <summary>
        /// Main worker of 'bayoen-star'
        /// </summary>
        private static MainWorker _mainWorker;
        public static MainWorker MainWorker => _mainWorker ?? (_mainWorker = new MainWorker());        

        private static SettingWindow _settingWindow;
        /// <summary>
        /// Setting window of application. '<see cref="SettingWindow"/>'
        /// </summary>
        public static SettingWindow SettingWindow => _settingWindow ?? (_settingWindow = new SettingWindow());

        private static ProjectData _projectData;
        /// <summary>
        /// Setting data of 'bayoen-star'
        /// </summary>
        public static ProjectData ProjectData => _projectData ?? (_projectData = ProjectData.Load());

        private static MiniWindow _miniWindow;
        /// <summary>
        /// Mini window of 'bayoen-star' from its initial version
        /// </summary>
        public static MiniWindow MiniWindow => _miniWindow ?? (_miniWindow = new MiniWindow());

        private static MiniOverlay _miniOverlay;
        /// <summary>
        /// Mini overlay of 'bayoen-star' from its initial version
        /// </summary>
        public static MiniOverlay MiniOverlay => _miniOverlay ?? (_miniOverlay = new MiniOverlay());

#if DEBUG
        private static DebugWindow _debugWindow;
        /// <summary>
        /// Mini window of 'bayoen-star' from its initial version
        /// </summary>
        public static DebugWindow DebugWindow => _debugWindow ?? (_debugWindow = new DebugWindow());

        private static DispatcherTimer _debugTimer;
        /// <summary>
        /// Mini window of 'bayoen-star' from its initial version
        /// </summary>
        public static DispatcherTimer DebugTimer => _debugTimer ?? (_debugTimer = new DispatcherTimer());
#endif

    }
}
