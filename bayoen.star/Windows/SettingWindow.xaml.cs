using System;
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
            this.Title += $": {Config.AssemblyTitle}";

            //// General
            // Topmost
            this.TopmostSwitch.Value = Core.ProjectData.TopMost;            

            // AutoUpdate
            this.AutoUpdateSwitch.Value = Core.ProjectData.AutoUpdate;

            // LanguageCode
            this.LanguageCodeComboBox.ComboBoxMinWidth = 135;
            this.LanguageCodeComboBox.ItemSource = Config.CultureCodes.ConvertAll(x =>
            {
                CultureInfo info = new CultureInfo(x);
                return $"{info.EnglishName} ({info.NativeName})";
            });
            this.LanguageCodeComboBox.SelectedIndex = Config.CultureCodes.IndexOf(Core.ProjectData.LanguageCode);

            // EnglishDisplay
            this.EnglishDisplaySwitch.Value = Core.ProjectData.EnglishDisplay;

            //// Streaming
            // ChromaKey
            this.ChromaKeyComboBox.ComboBoxMinWidth = 120;
            this.ChromaKeyComboBox.ComboBoxBackground = Brushes.Black;
            this.ChromaKeyComboBox.ItemSource = Core.GetChromaComboBoxItemList();
            this.ChromaKeyComboBox.SelectedIndex = (int)Core.ProjectData.ChromaKey;

            //

            //// Advanced
            // EnableRapidMode
            this.EnableRapidModeSwitch.Value = Core.ProjectData.EnableSlowMode;
            //this.EnableRapidModeSwitch.TrueLabel = $"{Config.DisplayIntervalSlow.TotalMilliseconds} ms";
            //this.EnableRapidModeSwitch.FalseLabel = $"{Config.DisplayIntervalNormal.TotalMilliseconds} ms";         
        }        

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            Core.Restart(RestartingModes.RestartWithSetting);
        }

        private void TopmostSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.ProjectData.TopMost = this.TopmostSwitch.Value;
        }

        private void AutoUpdateSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.ProjectData.AutoUpdate = this.AutoUpdateSwitch.Value;
        }

        private void LanguageComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.LanguageCodeComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Core.ProjectData.LanguageCode = Config.CultureCodes[selectedIndex];
            }
        }

        private void EnglishDisplaySwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.ProjectData.EnglishDisplay = this.EnglishDisplaySwitch.Value;
        }

        private void CaptureModeComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.CaptureModeComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Core.ProjectData.ChromaKey = Config.ChromaSets[selectedIndex].Item1;
            }
        }

        private void ChromaKeyComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            int selectedIndex = this.ChromaKeyComboBox.SelectedIndex;
            if (selectedIndex > -1)
            {
                Core.ProjectData.ChromaKey = Config.ChromaSets[selectedIndex].Item1;
            }
        }

        private void EnableRapidModeSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            Core.ProjectData.EnableSlowMode = this.EnableRapidModeSwitch.Value;
        }

        private void EnableRapidModeDetailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.EnableRapidModeSwitch.Detail = this.EnableRapidModeDetailBox.Text
                .Replace("##TrackingIntervalNormal##", $"'{Config.TrackingIntervalNormal.TotalMilliseconds} ms'")
                .Replace("##TrackingIntervalSlow##", $"'{Config.TrackingIntervalSlow.TotalMilliseconds} ms'")
                .Replace("##DisplayIntervalNormal##", $"'{Config.DisplayIntervalNormal.TotalMilliseconds} ms'")
                .Replace("##DisplayIntervalSlow##", $"'{Config.DisplayIntervalSlow.TotalMilliseconds} ms'");
        }


    }
}
