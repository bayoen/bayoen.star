using bayoen.star.Overlays;
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

        private static FloatingOverlay _floatingOverlay;
        public static FloatingOverlay FloatingOverlay => Core._floatingOverlay ?? (Core._floatingOverlay = new FloatingOverlay());

#if DEBUG
        private static DebugWindow _debugWindow;
        public static DebugWindow DebugWindow => Core._debugWindow ?? (Core._debugWindow = new DebugWindow());
#endif
    }
}
