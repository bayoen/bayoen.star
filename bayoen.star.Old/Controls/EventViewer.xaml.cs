using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.star.Variables;

namespace bayoen.star.Controls
{
    public partial class EventViewer : Grid
    {
        public EventViewer()
        {
            this.InitializeComponent();
            this.EventTreeView.ItemsSource = Core.DB.Matches;

            //this.PageSize = Core.Project.MatchPageSize;
            //Core.DB.MatchClear();

            //Core.DB.Insert(new MatchRecord()
            //{
            //    MatchCategory = MatchCategories.PuzzleLeague,
            //});
            //Core.DB.Insert(new MatchRecord()
            //{
            //    MatchCategory = MatchCategories.FreePlay,
            //});

            //for (int index = 0; index < 32; index++)
            //{
            //    Core.DB.Insert(new MatchRecord()
            //    {
            //        Games = new List<GameRecord>()
            //        {
            //            new GameRecord(),
            //        },
            //    });
            //}

            this.SetPage(0);
            this.CheckNavigator();
        }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => this._pageIndex;
            set
            {
                int validIndex = Math.Max(0, Math.Min(this.MatchPageMax - 1, value));
                if (this._pageIndex == validIndex) return;

                this.SetPage(validIndex);
                this._pageIndex = validIndex;

                this.CheckNavigator();
            }
        }
        
        public void CheckNavigator()
        {
            int matchPageMax = this.MatchPageMax;
            if (matchPageMax > 1)
            {
                this.PageBlock.Text = $"{(this.PageIndex + 1).ToString().PadLeft(5)} / {matchPageMax.ToString().PadRight(5)}";

                this.PrevPageButton.IsEnabled = (this.PageIndex > 0);
                this.PrevPageButton.IsAccented = (this.PageIndex > 0);

                this.NextPageButton.IsEnabled = (this.PageIndex < matchPageMax - 1);
                this.NextPageButton.IsAccented = (this.PageIndex < matchPageMax - 1);

                this.NavigatorPanel.Visibility = Visibility.Visible;
            }
            else
            {
                this.NavigatorPanel.Visibility = Visibility.Hidden;
            }            
        }

        public void SetPage(int index)
        {
            int matchPageMax = this.MatchPageMax;
            int validIndex = Math.Max(0, Math.Min(matchPageMax - 1, index)) * this.PageSize;
            List<MatchRecord> reversedMatches = Core.DB.ReversedMatches();

            Core.DB.Matches.Clear();
            foreach (MatchRecord tokenMatch in reversedMatches.GetRange(validIndex, Math.Min(this.PageSize, reversedMatches.Count - validIndex)))
            {
                Core.DB.Matches.Add(tokenMatch);
            }
        }

        public void Check()
        {
            this.SetPage(this.PageIndex);
            this.CheckNavigator();
        }

        public int MatchPageMax => (int)Math.Ceiling(((double)Core.DB.MatchCount() / (double) this.PageSize));

        private void PrevPageButton_Click(object sender, RoutedEventArgs e) => this.PageIndex--;
        private void NextPageButton_Click(object sender, RoutedEventArgs e) => this.PageIndex++;
        private void EventModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.RecordDisplayMode = RecordDisplayModes.Event;
        private void MatchModeButton_Click(object sender, RoutedEventArgs e) => Core.Project.RecordDisplayMode = RecordDisplayModes.Match;

        private void NavigatorGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.PrevPageButton.IsEnabled = this.PrevPageButton.IsAccented = false;
            this.NextPageButton.IsEnabled = this.NextPageButton.IsAccented = false;

            if (e.Delta > 0)
            {
                this.PageIndex--;
            }
            else if (e.Delta < 0)
            {
                this.PageIndex++;
            }

            this.PrevPageButton.IsEnabled = (this.PageIndex > 0);
            this.PrevPageButton.IsAccented = (this.PageIndex > 0);

            int matchPageMax = this.MatchPageMax;
            this.NextPageButton.IsEnabled = (this.PageIndex < matchPageMax - 1);
            this.NextPageButton.IsAccented = (this.PageIndex < matchPageMax - 1);
        }

        private void EventTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            //if (this.EventTreeView.SelectedItem is MatchRecord a)
            //{
            //    Core.MainWindow.Title = $"Match {a.Begin}";
            //}
            //else if (this.EventTreeView.SelectedItem is GameRecord b)
            //{
            //    Core.MainWindow.Title = $"Game {b.Begin}";
            //}
        }

        public int PageSize { get; set; }
    }
}
