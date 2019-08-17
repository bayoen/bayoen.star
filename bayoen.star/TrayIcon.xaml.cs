using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Hardcodet.Wpf.TaskbarNotification;

namespace bayoen.star
{
    public partial class TrayIcon : TaskbarIcon
    {
        public TrayIcon()
        {
            this.InitializeComponent();
            this.Icon = bayoen.star.Properties.Resources.StarCarbyIcon;
            this.ToolTipText = Config.AssemblyTitle;

            this.MenuHeader.Header = $"{Config.AssemblyTitle} - v{Config.Assembly.GetName().Version}";

#if DEBUG
            MenuItem DebugMenuItem = new MenuItem() { Header = "_Debug" };
            DebugMenuItem.Click += (sender, e) => Core.DebugWindow.Show();
            this.ContextMenu.Items.Insert(4, DebugMenuItem);
#endif
        }

        public void Terminate()
        {
            this.Visibility = Visibility.Collapsed;
            this.Dispose();
        }

        private void TaskbarIcon_TrayMouseDoubleClick(object sender, RoutedEventArgs e) => Core.MainWindow.Show();

        private void ShowMenuItem_Click(object sender, RoutedEventArgs e) => Core.MainWindow.Show();

        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) => Core.SettingWindow.Show();

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) => Core.Terminate();
    }
}
