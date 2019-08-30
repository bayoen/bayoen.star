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
using System.Windows.Shapes;

using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = $"{Config.AssemblyTitle}";
#if DEBUG
            this.Title += " [DEBUG]";
#endif

            this.MenuButton.ContextMenu.PlacementTarget = this.MenuButton;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {            
            this.MenuButton.ContextMenu.IsOpen = true;
        }

        private void ResetMeniItem_Click(object sender, RoutedEventArgs e)
        {
            Core.ResetScore();
        }

        private void SettingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Core.SettingWindow.Show();
        }

        private void ModeSubMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem SelectedSubModeMenuItem = sender as MenuItem;
            MenuItem ModeMenuItem = SelectedSubModeMenuItem.Parent as MenuItem;

            foreach (MenuItem item in ModeMenuItem.Items)
            {
                item.IsChecked = false;
            }
            SelectedSubModeMenuItem.IsChecked = true;

            int seledtedIndex = ModeMenuItem.Items.IndexOf(SelectedSubModeMenuItem);
        }

        
    }
}
