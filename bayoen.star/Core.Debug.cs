#if DEBUG
using System.Windows.Threading;

using bayoen.star.Windows;

namespace bayoen.star
{
    public static partial class Core
    {

        private static DebugWindow _debugWindow;
        public static DebugWindow DebugWindow => _debugWindow ?? (_debugWindow = new DebugWindow());


        private static DispatcherTimer _debugTimer;
        public static DispatcherTimer DebugTimer => _debugTimer ?? (_debugTimer = new DispatcherTimer());


        private static DashboardWindow _dashboardWindow;
        public static DashboardWindow DashboardWindow => _dashboardWindow ?? (_dashboardWindow = new DashboardWindow());


        private static DispatcherTimer _dashboardTimer;
        public static DispatcherTimer DashboardTimer => _dashboardTimer ?? (_dashboardTimer = new DispatcherTimer());

    }
}
#endif
