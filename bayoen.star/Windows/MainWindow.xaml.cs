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
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = Config.Title;

            this.InitialLogoImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyImage);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) => Core.SettingWindow.Show();

        private bool _isInitial = true;
        public bool IsInitial
        {
            get => this._isInitial;
            set
            {
                if (this._isInitial == value) return;

                this.InitialGrid.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.RightWindowCommands.Visibility = value ? Visibility.Collapsed : Visibility.Visible;

                this._isInitial = value;
            }
        }

        private string _initialStatus = "";
        public string InitialStatus
        {
            get => this._initialStatus;
            set
            {
                if (this._initialStatus == value) return;

                this.InitialStatusBlock.Text = value;

                this._initialStatus = value;
            }
        }

        private string _initialStatusResource = "";
        public string InitialStatusResource
        {
            get => this._initialStatusResource;
            set
            {
                if (this._initialStatusResource == value) return;

                this.InitialStatusBlock.Text = TryFindResource(value) as string;

                this._initialStatusResource = value;
            }
        }

        public bool SetFormatInitialStatusKey(string key, params object[] args)
        {
            try
            {
                string seedString = TryFindResource(key) as string;
                this.InitialStatusBlock.Text = this._initialStatusResource = string.Format(seedString, args);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitialAutoUpdateCheckBox.IsChecked = Core.Project.AutoUpdate;
        }

        private void InitialAutoUpdateCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.InitialAutoUpdateCheckBox.IsChecked.HasValue)
            {
                Core.Project.AutoUpdate = this.InitialAutoUpdateCheckBox.IsChecked.Value;
            }
        }
    }
}
