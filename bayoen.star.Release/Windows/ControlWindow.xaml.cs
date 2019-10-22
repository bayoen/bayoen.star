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
using bayoen.library.General.ExtendedMethods;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class ControlWindow : BaseWindow
    {
        public ControlWindow()
        {
            this.InitializeComponent();
        }

        private void MiniButton_Click(object sender, RoutedEventArgs e) { }
        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) {}        

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
