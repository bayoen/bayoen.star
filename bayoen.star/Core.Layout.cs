using bayoen.star.Windows;

namespace bayoen.star
{
    public static partial class Core
    {

        private static TrayIcon _trayIcon;
        public static TrayIcon TrayIcon => _trayIcon ?? (_trayIcon = new TrayIcon());


        private static MainWindow _mainWindow;
        public static MainWindow MainWindow => _mainWindow ?? (_mainWindow = new MainWindow());


        private static CapturableWindow _capturableWindow;
        public static CapturableWindow CapturableWindow => _capturableWindow ?? (_capturableWindow = new CapturableWindow());
  

        private static SettingWindow _settingWindow;
        public static SettingWindow SettingWindow => _settingWindow ?? (_settingWindow = new SettingWindow());


        private static MiniWindow _miniWindow;
        public static MiniWindow MiniWindow => _miniWindow ?? (_miniWindow = new MiniWindow());


        private static MiniOverlay _miniOverlay;
        public static MiniOverlay MiniOverlay => _miniOverlay ?? (_miniOverlay = new MiniOverlay());
        
    }
}
