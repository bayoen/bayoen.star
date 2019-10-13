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
            this.LogoImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyPlusImage);


            //this.AlwaysTopModeButton.IsAccented = true;
            this.GoalStarButton.IsAccented = true;

            this.EventViewer.EventModeButton.IsAccented = true;            

            this.MenuButton.ContextMenu.PlacementTarget = this.MenuButton;


            Core.Live.Panels.ForEach(x =>
            {                
                this.PlayerListPanel.Children.Add(x);
            });

            for (int panelIndex = 0; panelIndex < Core.Live.Panels.Count; panelIndex++)
            {
                Core.Live.Panels[panelIndex].PlayerName = $"Player {panelIndex + 1}";
            }

            Core.Live.Reversed.ForEach(x =>
            {
                x.PlayerName = "R";
                this.PlayerListPanel.Children.Add(x);
            });

            //Core.EventChecker.ScanEvents();
            //Core.EventChecker.PageIndex = 0;

            // InitialGrid
            this.InitialLogoImage.SetBitmap(bayoen.star.Properties.Resources.StarCarbyImage);
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

        private void EditFavoriteGoalButton_Click(object sender, RoutedEventArgs e) { }        
        private void AlwaysTopModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.TrackingMode = TrackingModes.Always;
        private void LeagueTopModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.TrackingMode = TrackingModes.League;
        private void FriendlyTopModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.TrackingMode = TrackingModes.Friendly;
        private void ArcadeTopModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.TrackingMode = TrackingModes.Arcade;        
        private void NoneTopModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.TrackingMode = TrackingModes.None;

        private void GoalStarButton_Click(object sender, RoutedEventArgs e) => Core.Project.GoalCounter = GoalCounters.Star;
        private void GoalGameButton_Click(object sender, RoutedEventArgs e) => Core.Project.GoalCounter = GoalCounters.Game;

        private void WhereAmIDetailBox_TextChanged(object sender, TextChangedEventArgs e) => this.CheckStatus();

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.EventViewer.PageSize = Core.Project.MatchPageSize;
            this.EventViewer.Check();
        }

        private void ResetRecordButton_Click(object sender, RoutedEventArgs e)
        {
            Core.DB.ClearMatch();
            Core.EventViewer.PageIndex = 0;
            Core.UpdateResult();
        }
    }
}
