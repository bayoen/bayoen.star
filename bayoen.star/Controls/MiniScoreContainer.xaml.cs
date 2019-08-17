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

namespace bayoen.star.Controls
{
    public partial class MiniScoreContainer : Grid
    {
        public MiniScoreContainer()
        {
            this.InitializeComponent();

            this._miniScoreOrientation = MiniScoreOrientation.Right;
        }

        private MiniScoreOrientation _miniScoreOrientation;
        public MiniScoreOrientation MiniScoreOrientation
        {
            get => this._miniScoreOrientation;
            set
            {
                if (this._miniScoreOrientation == value) return;
                
                this.ScorePanel.Children.Remove(this.ScoreBlock);
                this.ScorePanel.Children.Insert((value == MiniScoreOrientation.Right) ? 0 : 1, this.ScoreBlock);

                this._miniScoreOrientation = value;
            }
        }

        private int _score;
        public int Score
        {
            get => this._score;
            set
            {
                if (this._score == value) return;

                // FontSize adjustment
                if (value >= 1000)
                {
                    this.ScoreBlock.FontSize = 15;
                }
                else if (value >= 100)
                {
                    this.ScoreBlock.FontSize = 20;
                }
                else
                {
                    this.ScoreBlock.FontSize = 30;
                }

                // Display value adjustment
                if (value >= 9999)
                {
                    this.ScoreBlock.Text = "9999";
                    this._score = 9999;
                }
                else if (value > -1)
                {
                    this.ScoreBlock.Text = value.ToString();
                    this._score = value;
                }
                else
                {
                    this.ScoreBlock.Text = "-";
                    this._score = -1;
                }
            }
        }
    }
}
