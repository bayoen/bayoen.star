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

using bayoen.library.General.Enums;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MiniWindow : BaseWindow
    {
        public MiniWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.Title}";
            
            this.IsMouseEnter = false;
        }

        private void BaseWindow_MouseEnter(object sender, MouseEventArgs e) => this.IsMouseEnter = true;
        private void BaseWindow_MouseLeave(object sender, MouseEventArgs e) => this.IsMouseEnter = false;
        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;
        private void ResetMeniItem_Click(object sender, RoutedEventArgs e) { }
        private void ModeSubMenuItem_Click(object sender, RoutedEventArgs e) { }
    }
}
