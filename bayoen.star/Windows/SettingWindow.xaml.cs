﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
using bayoen.library.Metro.Controls;
using bayoen.library.Metro.Windows;
using bayoen.star.Controls;
using bayoen.star.Localizations;

namespace bayoen.star.Windows
{
    public partial class SettingWindow : BaseWindow
    {
        public SettingWindow()
        {
            //// Window
            this.InitializeComponent();
            this.Title += $": {Config.Title}";

            //// General
            // General
            this.AutoUpdateSwitch.Value = Core.Option.AutoUpdate;
            this.BuildLanguageCodeComboBox();

            this.StartMinimizedSwitch.Value = Core.Option.StartMinimized;
            this.OpenMiniSwitch.Value = Core.Option.OpenMini;
            this.TopmostSwitch.Value = Core.Option.TopMost;

            //// Update
            this.BuildVersionSelector();
            //this.ChangeVersionCodeComboBox

            // EnglishDisplay
            //this.EnglishDisplaySwitch.Value = Core.Project.EnglishDisplay;

            ////// Record
            //// MatchItemNumber
            //this.MatchItemNumberNumericUpDown.Value = Core.Project.MatchPageSize;
            //this.MatchItemNumberNumericUpDown.NumericUpDown.Minimum = 5;
            //this.MatchItemNumberNumericUpDown.NumericUpDown.Maximum = 20;
            //this.MatchItemNumberNumericUpDown.NumericUpDown.Width = 80;

            ////// Streaming
            //// ChromaKey
            //this.ChromaKeyComboBox.ItemSource = Core.GetChromaComboBoxItemList();
            //this.ChromaKeyComboBox.ComboBoxBackground = Brushes.Black;
            //this.ChromaKeyComboBox.ComboBoxWidth = 130;
            //this.ChromaKeyComboBox.SelectedIndex = (int)Core.Project.ChromaKey;



            //

            //// Advanced
            // EnableRapidMode
            //this.EnableRapidModeSwitch.Value = Core.Project.EnableSlowMode;
            ////this.EnableRapidModeSwitch.TrueLabel = $"{Config.DisplayIntervalSlow.TotalMilliseconds} ms";
            ////this.EnableRapidModeSwitch.FalseLabel = $"{Config.DisplayIntervalNormal.TotalMilliseconds} ms";  
            //this.DisableEarlyRefreshSwitch.Value = Core.Project.DisableEarlyRefresh;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Restart(RestartingModes.RestartWithSetting);
        }

        private void StartMinimizedSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.Option.StartMinimized = this.StartMinimizedSwitch.Value;
        }

        private void OpenMiniSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.Option.OpenMini = this.OpenMiniSwitch.Value;
        }

        private void AutoUpdateSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.Option.AutoUpdate = this.AutoUpdateSwitch.Value;
        }

        private void TopmostSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.Option.TopMost = this.TopmostSwitch.Value;
        }

        private void LanguageComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.LanguageCodeComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Core.Option.LanguageCode = Config.CultureCodes[selectedIndex];
            }
        }

        private void EnglishDisplaySwitch_Toggled(object sender, RoutedEventArgs e)
        {
            //Core.Project.EnglishDisplay = this.EnglishDisplaySwitch.Value;
        }

        //private void CaptureModeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        //{
        //    int selectedIndex = this.CaptureModeComboBox.SelectedIndex;
        //    if (selectedIndex > -1)
        //    {
        //        Core.Project.ChromaKey = Config.ChromaSets[selectedIndex].Item1;
        //    }
        //}

        private void ChromaKeyComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.ChromaKeyComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Core.Option.ChromaKey = Config.ChromaSets[selectedIndex].Item1;
            }
        }

        private void EnableRapidModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            //Core.Project.EnableSlowMode = this.EnableRapidModeSwitch.Value;
        }

        private void EnableRapidModeDetailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.EnableRapidModeSwitch.Detail = string.Format(this.EnableRapidModeDetailBox.Text
                , $"'{Config.TrackingIntervalNormal.TotalMilliseconds} ms'"
                , $"'{Config.TrackingIntervalSlow.TotalMilliseconds} ms'"
                , $"'{Config.DisplayIntervalNormal.TotalMilliseconds} ms'"
                , $"'{Config.DisplayIntervalSlow.TotalMilliseconds} ms'");
        }

        private void DisableEarlyRefreshSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            //Core.Project.DisableEarlyRefresh = this.DisableEarlyRefreshSwitch.Value;
        }

        private void MatchItemNumberNumericUpDown_ValueChanged(object sender, RoutedEventArgs e)
        {
            //Core.Project.MatchPageSize = this.MatchItemNumberNumericUpDown.Value;
        }

        private void ChangeVersionCodeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Core.TrayIcon.ContextMenu.PlacementTarget == null)
            {
                Core.TrayIcon.ContextMenu.PlacementTarget = this;
            }
        }
    }
}
