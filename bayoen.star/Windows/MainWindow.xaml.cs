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
using bayoen.star.Overlays;

namespace bayoen.star.Windows
{
    public partial class MainWindow : BaseWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.Title = Config.Title;
#if DEBUG
            this.StatusBlock.Text = $"[DEBUG] v{Config.VersionString}";
#else
            this.StatusBlock.Text = $"v{Config.VersionShortString}";
#endif

            this.InitialLogoImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyImage);
        }

        private void MiniButton_Click(object sender, RoutedEventArgs e) { }
        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) => Core.SettingWindow.Show();

        private bool _isInitialProgress = true;
        public bool IsInitialProgress
        {
            get => this._isInitialProgress;
            set
            {
                if (this._isInitialProgress == value) return;

                this.InitialGrid.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.RightWindowCommands.Visibility = value ? Visibility.Collapsed : Visibility.Visible;

                this._isInitialProgress = value;
            }
        }

        private bool _showInitialDownloadPanel = false;
        public bool ShowInitialDownloadPanel
        {
            get => this._showInitialDownloadPanel;
            set
            {
                if (this._showInitialDownloadPanel == value) return;

                this.InitialDownloadPanel.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                this._showInitialDownloadPanel = value;
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

                this.InitialStatusBlock.Text = this.FindResource(value) as string;

                this._initialStatusResource = value;
            }
        }

        private double _progress = 0;
        public double Progress
        {
            get => this._progress;
            set
            {
                if (this._progress == value) return;

                this.DownloadProgressBar.Value = value;

                this._progress = value;
            }
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.InitialAutoUpdateCheckBox.IsChecked = Core.Option.AutoUpdate;
        }

        private void InitialAutoUpdateCheckBox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (this.InitialAutoUpdateCheckBox.IsChecked.HasValue)
            {
                Core.Option.AutoUpdate = this.InitialAutoUpdateCheckBox.IsChecked.Value;
            }
        }

        private void CalibrateButton_Click(object sender, RoutedEventArgs e)
        {
            if (Core.Memory.CheckProcess())
            {
                PPTRect.UpdateLocation(Core.FloatingOverlay);
                Core.FloatingOverlay.Activate();
                Core.FloatingOverlay.Topmost = true;
            }
            
        }
    }
}
