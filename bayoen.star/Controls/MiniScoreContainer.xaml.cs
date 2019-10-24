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

namespace bayoen.star.Controls
{
    public partial class MiniScoreContainer : StackPanel
    {
        public MiniScoreContainer()
        {
            this.InitializeComponent();

            this._scoreOrientation = ScoreOrientation.Right;

            this._scoreTypes = ScoreTypes.None;
            this.ScoreType = ScoreTypes.CountedStar;

            this._score = -1;
            this.Score = 0;
        }

        private ScoreOrientation _scoreOrientation;
        public ScoreOrientation ScoreOrientation
        {
            get => this._scoreOrientation;
            set
            {
                if (this._scoreOrientation == value) return;
                
                this.Children.Remove(this.ScoreBlock);
                this.Children.Insert((value == ScoreOrientation.Left) ? 0 : 1, this.ScoreBlock);

                this._scoreOrientation = value;
            }
        }

        private int _score;
        public int Score
        {
            get => this._score;
            set
            {
                if (this._score == value) return;

                int score = Math.Min(Math.Max(value, -1), 9999);

                // FontSize adjustment
                if (score >= 1000)
                {
                    this.ScoreBlock.FontSize = 15;
                }
                else if (score >= 100)
                {
                    this.ScoreBlock.FontSize = 20;
                }
                else
                {
                    this.ScoreBlock.FontSize = 30;
                }

                this.ScoreBlock.Text = (score == -1) ? "-" : value.ToString();
                this._score = score;                
            }
        }

        private ScoreTypes _scoreTypes;
        public ScoreTypes ScoreType
        {
            get => this._scoreTypes;
            set
            {
                if (this._scoreTypes == value) return;

                this.SymbolImage.Visibility = (value == ScoreTypes.None) ? Visibility.Collapsed : Visibility.Visible;

                switch (value)
                {                    
                    case ScoreTypes.CurrentStar:
                        this.SymbolImage.SetBitmap(bayoen.star.Properties.Resources.StarPlainImage);
                        break;
                    case ScoreTypes.CountedStar:
                        this.SymbolImage.SetBitmap(bayoen.star.Properties.Resources.StarPlusImage);
                        break;
                    case ScoreTypes.CountedGame:
                        this.SymbolImage.SetBitmap(bayoen.star.Properties.Resources.CrownLightImage);
                        break;
                    default:
                        break;
                }

                this._scoreTypes = value;
            }
        }
    }
}
