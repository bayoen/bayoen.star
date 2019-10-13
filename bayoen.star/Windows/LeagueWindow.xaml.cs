using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
using bayoen.star.Variables;

namespace bayoen.star.Windows
{
    public partial class LeagueWindow : BaseWindow
    {
        public LeagueWindow()
        {
            this.InitializeComponent();
            this.Height = this.MinHeight;
            this.Width = this.MinWidth;
            this.ListView.ItemsSource = Core.DB.Leagues;
            this.MenuButton.ContextMenu.PlacementTarget = this.MenuButton;

            this.Check();

        }

        private void BaseWindow_MouseEnter(object sender, MouseEventArgs e) => this.IsMouseEnter = true;
        private void BaseWindow_MouseLeave(object sender, MouseEventArgs e) => this.IsMouseEnter = false;

        private bool _isMouseEnter = true;
        public bool IsMouseEnter
        {
            get => this._isMouseEnter;
            set
            {
                if (this._isMouseEnter == value) return;

                this.TitlebarHeight = value ? MetroTitlebarHeight : 0;
                this.TopGrid.Margin = new Thickness(0, value ? 0 : MetroTitlebarHeight, 0, 0);

                this._isMouseEnter = value;
            }
        }        

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => this._pageIndex;
            set
            {
                int validIndex = Math.Max(0, Math.Min(this.LeaguePageMax - 1, value));
                if (this._pageIndex == validIndex) return;

                this.SetPage(validIndex);
                this._pageIndex = validIndex;

                this.CheckNavigator();
            }
        }

        public void CheckNavigator()
        {
            //int matchPageMax = this.MatchPageMax;
            //if (matchPageMax > 1)
            //{
            //    this.PageBlock.Text = $"{(this.PageIndex + 1).ToString().PadLeft(5)} / {matchPageMax.ToString().PadRight(5)}";

            //    this.PrevPageButton.IsEnabled = (this.PageIndex > 0);
            //    this.PrevPageButton.IsAccented = (this.PageIndex > 0);

            //    this.NextPageButton.IsEnabled = (this.PageIndex < matchPageMax - 1);
            //    this.NextPageButton.IsAccented = (this.PageIndex < matchPageMax - 1);

            //    this.NavigatorPanel.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    this.NavigatorPanel.Visibility = Visibility.Hidden;
            //}
        }

        public void SetPage(int index)
        {
            int matchPageMax = this.LeaguePageMax;
            int validIndex = Math.Max(0, Math.Min(matchPageMax - 1, index)) * this.PageSize;
            List<MatchRecord> reversedLeagues = Core.DB.ReversedMatches(x => x.Category == MatchCategories.PuzzleLeague);
            reversedLeagues = reversedLeagues.FindAll(x => x.End.Date == DateTime.Today);

            Core.DB.Leagues.Clear();
            foreach (MatchRecord tokenMatch in reversedLeagues.GetRange(validIndex, Math.Min(this.PageSize, reversedLeagues.Count - validIndex)))
            {
                Core.DB.Leagues.Add(tokenMatch);
            }
        }

        public void Check()
        {
            this.SetPage(this.PageIndex);

            List<MatchRecord> todayMatches = Core.DB.GetMatches().FindAll(x => x.End.Date == DateTime.Today);
            int winNumber = todayMatches.ConvertAll(x => (x.WinValue == 1) ? 1 : 0).Sum();
            int loseNumber = todayMatches.ConvertAll(x => (x.WinValue == 0) ? 1 : 0).Sum();
            int ratingGain = todayMatches.ConvertAll(x => x.RatingGain).Sum();
            this.GainBlock.Text = $"{ratingGain:+#;-#;0}";
            this.WinBlock.Text = $"{winNumber}W";
            this.LoseBlock.Text = $"{loseNumber}L";
            this.CheckNavigator();
        }

        public int LeaguePageMax => (int)Math.Ceiling(((double)Core.DB.MatchCount(x => x.Category == MatchCategories.PuzzleLeague) / (double)this.PageSize));
        public int PageSize { get; set; }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.PageSize = Core.Project.LeaguePageSize;
            this.Check();
        }

        private const int MetroTitlebarHeight = 30;

        private void MenuButton_Click(object sender, RoutedEventArgs e) => this.MenuButton.ContextMenu.IsOpen = true;

        private void ResetMeniItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenCaptureiItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OpenOverlayMeniItem_Click(object sender, RoutedEventArgs e)
        {

        }
        private void SettingMenuItem_Click(object sender, RoutedEventArgs e) => Core.SettingWindow.Show();
    }
}
