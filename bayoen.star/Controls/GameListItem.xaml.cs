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
    public partial class GameListItem : Grid
    {
        public GameListItem()
        {
            this.InitializeComponent();
        }

        #region [ GameString Binding ]
        public static readonly DependencyProperty GameStringProperty = DependencyProperty.Register(
            "GameString",
            typeof(string),
            typeof(GameListItem));

        public string GameString
        {
            get => this.GetValue(GameStringProperty) as string;
            set => this.SetValue(GameStringProperty, value);
        }

        private void GameStringBindingBox_TextChanged(object sender, TextChangedEventArgs e) => this.CheckBinding();
        #endregion

        #region [ IsSelected Binding ]
        public static readonly DependencyProperty IsSelectedStringProperty = DependencyProperty.Register(
            "IsSelectedString",
            typeof(string),
            typeof(GameListItem));

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
        }
        #endregion

        public void CheckBinding()
        {
            this.Game = GameRecord.Parse<GameRecord>(this.GameStringBindingBox.Text);

            this.OrderBlock.Text = $"{this.Game.Index + 1}";
            this.EndBlock.Text = this.Game.End.Value.ToCompactString();
            this.WinnerTeamBlock.Text = $"Team {this.Game.WinnerTeam} won!";
            this.GameTimeBlock.Text = $"{(this.Game.End-this.Game.Begin).Value.TotalSeconds.ToString("F2").PadLeft(6)} [sec]";

            this.ToolTip = this.Game.ToJson().ToString();
        }

        public GameRecord Game { get; set; }

        
    }
}
