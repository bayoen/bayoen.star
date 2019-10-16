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
using System.Windows.Navigation;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.star.Variables;

namespace bayoen.star.Controls
{    
    public partial class MatchListItem : Grid
    {
        public MatchListItem()
        {
            this.InitializeComponent();
            this.EntryBlocks = new List<TextBlock>()
            {
                this.Player1Block,
                this.Player2Block,
                this.Player3Block,
                this.Player4Block,
            };
            this.EntryPanels = new List<Panel>()
            {
                this.Entry1Panel,
                this.Entry2Panel,
                this.Entry3Panel,
                this.Entry4Panel,
            };
        }

        #region [ MatchString Binding ]
        public static readonly DependencyProperty MatchStringProperty = DependencyProperty.Register(
            "MatchString",
            typeof(string),
            typeof(MatchListItem));

        public string MatchString
        {
            get => this.GetValue(MatchStringProperty) as string;
            set => this.SetValue(MatchStringProperty, value);
        }

        private void MatchStringBindingBox_TextChanged(object sender, TextChangedEventArgs e) => this.CheckBinding();
        #endregion

        #region [ IsSelected Binding ]
        public static readonly DependencyProperty IsSelectedStringProperty = DependencyProperty.Register(
            "IsSelectedString",
            typeof(string),
            typeof(MatchListItem));

        public string IsSelectedString
        {
            get => this.GetValue(IsSelectedStringProperty) as string;
            set => this.SetValue(IsSelectedStringProperty, value);
        }

        private void IsSelectedStringBindingBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool isSelected = false;
            if (this.IsSelectedStringBindingBox.Text.Length > 0)
            {
                isSelected = bool.Parse(this.IsSelectedStringBindingBox.Text);
            }

            //this.SecondRowPanel.Visibility = isSelected ? Visibility.Visible : Visibility.Collapsed;

            if (isSelected)
            {
                this.DisplayMatch();
            }
        }
        #endregion

        public void CheckBinding()
        {
            this.Match = MatchRecord.Parse<MatchRecord>(this.MatchStringBindingBox.Text);

            // Col #1
            this.MatchCategory = this.Match.Category;
            this.EndBlock.Text = this.Match.End.ToCompactString();

            // Col #2
            this.ModeBlock.Text = this.Match.Mode.ToString();
            this.SetWinnerTeam();
            this.WinCountBlock.Text = (this.Match.WinCount == 1) ? "" : $"FT{this.Match.WinCount}";            

            // Col #3
            this.SetTeamEntry();


            // etc
            this.ToolTip = this.Match.ToJson().ToString();
        }

        public MatchRecord Match { get; set; }
        public List<Panel> EntryPanels { get; private set; }
        public List<TextBlock> EntryBlocks { get; private set; }

        private MatchCategories _matchCategory = MatchCategories.None;
        public MatchCategories MatchCategory
        {
            get => this._matchCategory;
            set
            {
                if (this._matchCategory == value) return;

                this.LeaguePanel.Visibility = (value == MatchCategories.PuzzleLeague) ? Visibility.Visible : Visibility.Hidden;
                this.FriendlyPanel.Visibility = (value == MatchCategories.FreePlay) ? Visibility.Visible : Visibility.Hidden;
                this.ArcadePanel.Visibility = (value == MatchCategories.Arcade) ? Visibility.Visible : Visibility.Hidden;

                this._matchCategory = value;
            }
        }

        private MatchResults _matchResult = MatchResults.NotMe;
        public MatchResults MatchResult
        {
            get => this._matchResult;
            set
            {
                if (this._matchResult == value) return;

                this.NotMeIcon.Visibility = (value == MatchResults.NotMe) ? Visibility.Visible : Visibility.Hidden;
                this.WinIcon.Visibility = (value == MatchResults.Win) ? Visibility.Visible : Visibility.Hidden;
                this.LoseIcon.Visibility = (value == MatchResults.Lose) ? Visibility.Visible : Visibility.Hidden;

                this._matchResult = value;
            }
        }

        private void SetWinnerTeam()
        {
            if (this.Match.WinnerTeam == 0)
            {
                this.MatchResult = MatchResults.NotMe;
            }
            else
            {
                List<PlayerData> players = this.Match.Players;
                PlayerData me = players.Find(x => x.ID32 == this.Match.MyID32);

                if (me == null)
                {
                    this.MatchResult = MatchResults.NotMe;
                }
                else
                {
                    this.MatchResult = (me.Team == this.Match.WinnerTeam) ? MatchResults.Win : MatchResults.Lose;
                }
            }            

            this.WinnersBlock.Text = $"Team {this.Match.WinnerTeam}";
            if (this.Match.RatingGain != 0) this.WinnersBlock.Text += $" ({this.Match.RatingGain:+#;-#;0})";
        }

        private void SetTeamEntry()
        {
            bool nameCutFlag = false;
            int count = this.Match.Players.Count;
            for (int entryIndex = 0; entryIndex < 4; entryIndex++)
            {
                if (entryIndex < count)
                {
                    PlayerData player = this.Match.Players[entryIndex];

                    string nametoken = player.Name;
                    if (nametoken.Length > MatchListItem.NameCut)
                    {
                        nametoken = nametoken.Substring(0, MatchListItem.NameCut - 3) + "...";
                        if (!nameCutFlag) nameCutFlag = true;
                    }

                    this.EntryPanels[entryIndex].Visibility = Visibility.Visible;
                    this.EntryBlocks[entryIndex].Text = $"T{player.Team}: {nametoken} ({player.PlayType})";
                }
                else
                {
                    this.EntryPanels[entryIndex].Visibility = Visibility.Collapsed;
                }
            }
            
            if (nameCutFlag)
            {
                this.EntryGrid.ToolTip = string.Join(Environment.NewLine, this.Match.Players.ConvertAll(x =>
                {
                    return $"Team {x.Team}: {x.Name} ({x.PlayType})";
                }));
            }
        }

        private void DisplayMatch()
        {

        }

        private const int NameCut = 14;
    }
}
