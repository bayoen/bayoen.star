using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

using MahApps.Metro.Controls;

using bayoen.library.Metro.Windows;
using bayoen.library.General.ExtendedMethods;
using System.Reflection;
using System.Globalization;
using System.Collections;
using System.Drawing;

namespace bayoen.star.Launcher
{
    public partial class LauncherWindow : BaseWindow
    {
        public LauncherWindow()
        {
            this.InitializeComponent();

            this.LogoImage.SetBitmap(Config.LogoBitmapList[(new Random().Next(0, Config.LogoBitmapList.Count))]);

            //this.Background = new SolidColorBrush(Color.FromRgb(25, 25, 25)) { Opacity = 0.95 };

            this.Left = (SystemParameters.PrimaryScreenWidth - this.Width) / 2;
            this.Top = (SystemParameters.PrimaryScreenHeight - this.Height) / 2;
        }

        private string _status = "";
        public string Status
        {
            get => this._status;
            set
            {
                if (this._status == value) return;

                this.StatusBlock.Text = value;

                this._status = value;
            }
        }

        private string _currentFileName = "";
        public string CurrentFileName
        {
            get => this._currentFileName;
            set
            {
                if (this._currentFileName == value) return;

                this.CurrentFileNameBlock.Text = value;

                this._currentFileName = value;
            }
        }

        private string _currentFileProgress = "";
        public string CurrentFileProgress
        {
            get => this._currentFileProgress;
            set
            {
                if (this._currentFileProgress == value) return;

                this.CurrentFileDownloadBlock.Text = value;

                this._currentFileProgress = value;
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
    }
}
