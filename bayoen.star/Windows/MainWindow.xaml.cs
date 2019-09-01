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
            this.Title = $"{Config.AssemblyTitle}";
#if DEBUG
            this.Title += " [DEBUG]";
#endif
            this.LogoImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyImage);


            this.AlwaysTopModeButton.IsAccented = true;
            this.GoalStarButton.IsAccented = true;
            this.EventListModeButton.IsAccented = true;

            this.MenuButton.ContextMenu.PlacementTarget = this.MenuButton;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;
        private void ResetMeniItem_Click(object sender, RoutedEventArgs e) => Core.ResetScore();
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) => Core.SettingWindow.Show();

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

        private void EditFavoriteGoalButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
        
        private void AlwaysTopModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.TrackingMode = TrackingModes.Always;
        private void NormalTopModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.TrackingMode = TrackingModes.Friendly;
        private void LeagueTopModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.TrackingMode = TrackingModes.League;
        private void NoneTopModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.TrackingMode = TrackingModes.None;

        private void GoalStarButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.GoalCounter = GoalCounters.Star;
        private void GoalGameButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.GoalCounter = GoalCounters.Game;

        private void EventListModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.RecordDisplayMode = RecordDisplayModes.Event;
        private void MatchListModeButton_Click(object sender, RoutedEventArgs e) => Core.ProjectData.RecordDisplayMode = RecordDisplayModes.Match;

        private void PrevEventListButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NextEventListButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
