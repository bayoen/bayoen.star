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
using System.Windows.Navigation;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.star.Variables;

namespace bayoen.star.Controls
{
    public partial class LeagueListItem : Grid
    {
        public LeagueListItem()
        {
            this.InitializeComponent();
            this.MyPuyoImage.SetBitmap(bayoen.star.Properties.Resources.PuyoSymbolImage);
            this.MyTetrisImage.SetBitmap(bayoen.star.Properties.Resources.TetrisSymbolImage);
            this.OpponentPuyoImage.SetBitmap(bayoen.star.Properties.Resources.PuyoSymbolImage);
            this.OpponentTetrisImage.SetBitmap(bayoen.star.Properties.Resources.TetrisSymbolImage);
        }

        #region [ MatchString Binding ]
        public static readonly DependencyProperty MatchStringProperty = DependencyProperty.Register(
            "MatchString",
            typeof(string),
            typeof(LeagueListItem));

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
            typeof(LeagueListItem));

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
            //this.EndBlock.Text = this.ToSimpleString(this.Match.End);

            // Col #2            
            if (this.Match.WinnerTeam == 0)
            {
                this.MatchResult = MatchResults.NotMe;
            }
            else
            {
                List<PlayerData> players = this.Match.Players;
                int myIndex = players.FindIndex(x => x.ID32 == this.Match.MyID32);                

                if (myIndex > -1)
                {
                    PlayerData me = players[myIndex];
                    PlayerData opponent = players[1 - myIndex];

                    this.MatchResult = (me.Team == this.Match.WinnerTeam) ? MatchResults.Win : MatchResults.Lose;

                    string nametoken = opponent.Name;
                    if (nametoken.Length > LeagueListItem.NameCut)
                    {
                        nametoken = nametoken.Substring(0, LeagueListItem.NameCut - 3) + "...";
                    }
                    this.OpponentBlock.Text = nametoken;

                    string record = "";
                    this.Match.WinRecord.ForEach(x =>
                    {
                        if (x > 0) record += (me.Team == x) ? "W" : "L";
                        else record += "D";
                    });
                    this.RecordBlock.Text = record;
                    this.RatingBlock.Text = $"{((double)opponent.Rating / 1000):F1}k";

                    this.MyType = me.PlayType;
                    this.OpponentType = opponent.PlayType;
                }
                else
                {
                    this.MatchResult = MatchResults.NotMe;
                    this.OpponentBlock.Text = "Missing Opponent";
                    this.RecordBlock.Text = "";
                    this.RatingBlock.Text = "";

                    this.MyType = PlayTypes.None;
                    this.OpponentType = PlayTypes.None;
                }
            }

            this.GainBlock.Text = $"{this.Match.RatingGain:+#;-#;0}";                        

            // etc
            //this.ToolTip = this.Match.ToJson().ToString();
        }

        public MatchRecord Match { get; set; }

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

        private PlayTypes _myType = PlayTypes.None;
        private PlayTypes MyType
        {
            get => this._myType;
            set
            {
                if (this._myType == value) return;

                this.MyPuyoImage.Visibility = (value == PlayTypes.PuyoPuyo) ? Visibility.Visible : Visibility.Hidden;
                this.MyTetrisImage.Visibility = (value == PlayTypes.Tetris) ? Visibility.Visible : Visibility.Hidden;

                this._myType = value;
            }
        }

        private PlayTypes _opponentType = PlayTypes.None;
        private PlayTypes OpponentType
        {
            get => this._opponentType;
            set
            {
                if (this._opponentType == value) return;

                this.OpponentPuyoImage.Visibility = (value == PlayTypes.PuyoPuyo) ? Visibility.Visible : Visibility.Hidden;
                this.OpponentTetrisImage.Visibility = (value == PlayTypes.Tetris) ? Visibility.Visible : Visibility.Hidden;

                this._opponentType = value;
            }
        }

        private void DisplayMatch()
        {

        }

        private string ToSimpleString(DateTime dt)
        {
            TimeSpan diff = DateTime.Now - dt;
            
            if (diff.TotalDays > 365)
            {
                return "Long ago";
            }
            else if (diff.TotalHours > 24)
            {
                double token = Math.Ceiling(diff.TotalDays);
                return $"{token} day{PluralS(token)}";
            }
            else if (diff.TotalMinutes > 60)
            {
                double token = Math.Ceiling(diff.TotalHours);
                return $"{token} hour{PluralS(token)}";
            }
            else if (diff.TotalSeconds > 60)
            {
                double token = Math.Ceiling(diff.TotalMinutes);
                return $"{token} min{PluralS(token)}";
            }
            else if (diff.TotalSeconds > 30)
            {
                double token = Math.Ceiling(diff.TotalSeconds);
                return $"{token} sec{PluralS(token)}";
            }
            else
            {
                return "Just now";
            }

            string PluralS(double number) => (number < 1) ? "" : "s";
        }

        private const int NameCut = 12;
    }
}
