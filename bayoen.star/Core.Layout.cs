using bayoen.star.Windows;

namespace bayoen.star
{
    public static partial class Core
    {

        private static TrayIcon _trayIcon;
        public static TrayIcon TrayIcon => Core._trayIcon ?? (Core._trayIcon = new TrayIcon());


        private static MainWindow _mainWindow;
        public static MainWindow MainWindow => Core._mainWindow ?? (Core._mainWindow = new MainWindow());


        private static SettingWindow _settingWindow;
        public static SettingWindow SettingWindow => Core._settingWindow ?? (Core._settingWindow = new SettingWindow());

        // public static EventViewer EventViewer => Core.MainWindow.EventViewer;
    }
}
