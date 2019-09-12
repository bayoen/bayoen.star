using bayoen.star.Windows;

namespace bayoen.star
{
    public static partial class Core
    {

        private static TrayIcon _trayIcon;
        public static TrayIcon TrayIcon => Core._trayIcon ?? (Core._trayIcon = new TrayIcon());


        private static MainWindow _mainWindow;
        public static MainWindow MainWindow => Core._mainWindow ?? (Core._mainWindow = new MainWindow());


        private static CapturableWindow _capturableWindow;
        public static CapturableWindow CapturableWindow => Core._capturableWindow ?? (Core._capturableWindow = new CapturableWindow());
  

        private static SettingWindow _settingWindow;
        public static SettingWindow SettingWindow => Core._settingWindow ?? (Core._settingWindow = new SettingWindow());


        private static MiniWindow _miniWindow;
        public static MiniWindow MiniWindow => Core._miniWindow ?? (Core._miniWindow = new MiniWindow());


        private static MiniOverlay _miniOverlay;
        public static MiniOverlay MiniOverlay => Core._miniOverlay ?? (Core._miniOverlay = new MiniOverlay());
        
    }
}
