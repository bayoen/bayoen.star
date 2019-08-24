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
    public partial class MiniScorePanel : Grid
    {        
        public MiniScorePanel()
        {
            this.InitializeComponent();

            this.GoalPanelImage.SetBitmap(bayoen.star.Properties.Resources.Score2GoalImage);

            this._stars = new List<int>() { -1, -1, -1, -1 };
            this._games = new List<int>() { -1, -1, -1, -1 };

            this.Containers = new List<MiniScoreContainer>();
            for (int playerIndex = 0; playerIndex < 4; playerIndex++)
            {
                this.Containers.Add(new MiniScoreContainer());
            }

            this.Separators = new List<TextBlock>();
            for (int splitIndex = 0; splitIndex < 3; splitIndex++)
            {
                this.Separators.Add(new TextBlock()
                {
                    Text = ":",
                    FontSize = 28,
                    Margin = new Thickness(1,0,1,8),
                    VerticalAlignment = VerticalAlignment.Center,
                });                
            }

            this._playerNumber = 0;
            this.PlayerNumber = 2;

            this._goalCounter = GoalCounters.Star;
            this.GoalCounter = GoalCounters.Game;

            this._panelLength = ScorePanelLength.Short;
            this.PanelLength = ScorePanelLength.Fit;

            this._displayModes = DisplayModes.Star_Plus_only;
            this.DisplayMode = DisplayModes.Game_and_Star_Plus;

            this._goalScore = -1;
            this.GoalScore = 1;

            this._isFit = true;
            this.IsFit = false;
        }

        private List<MiniScoreContainer> Containers;
        private List<TextBlock> Separators;

        private bool _isFit;
        public bool IsFit
        {
            get => this._isFit;
            set
            {
                if (this._isFit == value) return;

                if (value && this.PlayerNumber == 2)
                {
                    this.Containers[1].Margin = new Thickness(0, 0, (this.DisplayMode > DisplayModes.Game_only) ? 0 : FitMarginOffset, 0);
                    this.Containers[2].Margin = new Thickness((this.DisplayMode > DisplayModes.Game_only) ? 0 : FitMarginOffset, 0, 0, 0);
                }
                else
                {
                    this.Containers[1].Margin = new Thickness(0);
                    this.Containers[2].Margin = new Thickness(0);
                }

                this._isFit = value;
            }
        }

        private DisplayModes _displayModes;
        public DisplayModes DisplayMode
        {
            get => this._displayModes;
            set
            {
                if (this._displayModes == value) return;

                // Visibility
                if (this.PlayerNumber == 2)
                {
                    this.Containers[0].Visibility = (value > DisplayModes.Game_only) ? Visibility.Visible : Visibility.Collapsed;
                    this.Containers[1].Visibility = Visibility.Visible;
                    this.Containers[2].Visibility = Visibility.Visible;
                    this.Containers[3].Visibility = (value > DisplayModes.Game_only) ? Visibility.Visible : Visibility.Collapsed;

                    switch (value)
                    {
                        case DisplayModes.Star_Plus_only:
                            this.Containers[1].ScoreType = ScoreTypes.CountedStar;
                            this.Containers[2].ScoreType = ScoreTypes.CountedStar;
                            break;
                        case DisplayModes.Game_only:
                            this.Containers[1].ScoreType = ScoreTypes.CountedGame;
                            this.Containers[2].ScoreType = ScoreTypes.CountedGame;
                            break;
                        case DisplayModes.Game_and_Star:
                            this.Containers[0].ScoreType = ScoreTypes.CountedGame;
                            this.Containers[1].ScoreType = ScoreTypes.CurrentStar;
                            this.Containers[2].ScoreType = ScoreTypes.CurrentStar;
                            this.Containers[3].ScoreType = ScoreTypes.CountedGame;
                            break;
                        case DisplayModes.Game_and_Star_Plus:
                            this.Containers[0].ScoreType = ScoreTypes.CountedGame;
                            this.Containers[1].ScoreType = ScoreTypes.CountedStar;
                            this.Containers[2].ScoreType = ScoreTypes.CountedStar;
                            this.Containers[3].ScoreType = ScoreTypes.CountedGame;
                            break;
                        case DisplayModes.Star_Plus_and_Star:
                            this.Containers[0].ScoreType = ScoreTypes.CountedStar;
                            this.Containers[1].ScoreType = ScoreTypes.CurrentStar;
                            this.Containers[2].ScoreType = ScoreTypes.CurrentStar;
                            this.Containers[3].ScoreType = ScoreTypes.CountedStar;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }                    
                }
                else
                {
                    this.Containers[0].Visibility = Visibility.Visible;
                    this.Containers[1].Visibility = Visibility.Visible;
                    this.Containers[2].Visibility = Visibility.Visible;
                    this.Containers[3].Visibility = (this.PlayerNumber == 4) ? Visibility.Visible : Visibility.Collapsed;

                    switch (value)
                    {
                        case DisplayModes.Star_Plus_only:
                            this.Containers[0].ScoreType = ScoreTypes.CountedStar;
                            break;
                        case DisplayModes.Game_only:
                            this.Containers[0].ScoreType = ScoreTypes.CountedGame;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }

                if (this.IsFit && this.PlayerNumber == 2)
                {
                    this.Containers[1].Margin = new Thickness(0, 0, (this.DisplayMode > DisplayModes.Game_only) ? 0 : FitMarginOffset, 0);
                    this.Containers[2].Margin = new Thickness((this.DisplayMode > DisplayModes.Game_only) ? 0 : FitMarginOffset, 0, 0, 0);
                }
                else
                {
                    this.Containers[1].Margin = new Thickness(0);
                    this.Containers[2].Margin = new Thickness(0);
                }

                this._displayModes = value;
            }
        }

        
        private GoalTypes _goalTypes;
        public GoalTypes GoalType
        {
            get => this._goalTypes;
            set
            {
                if (this._goalTypes == value) return;

                this.GoalGrid.Visibility = (value == GoalTypes.None) ? Visibility.Hidden : Visibility.Visible;

                switch (value)
                {
                    case GoalTypes.None:
                        // Do nothing
                        break;
                    case GoalTypes.First:
                        this.GoalTypeBlock.Text = "FIRST";
                        break;
                    case GoalTypes.Total:
                        this.GoalTypeBlock.Text = "TOTAL";
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                this._goalTypes = value;
            }
        }

        private int _goalScore;
        public int GoalScore
        {
            get => this._goalScore;
            set
            {
                if (this._goalScore == value) return;

                int score = Math.Max(Math.Min(value, 9999), 1);

                this.GoalScoreBlock.Text = score.ToString();

                this._goalScore = score;
            }
        }

        private GoalCounters _goalCounter;
        public GoalCounters GoalCounter
        {
            get => this._goalCounter;
            set
            {
                if (this._goalCounter == value) return;

                switch (value)
                {
                    case GoalCounters.Star:
                        this.GoalCounterImage.SetBitmap(bayoen.star.Properties.Resources.StarPlusImage);
                        break;
                    case GoalCounters.Game:
                        this.GoalCounterImage.SetBitmap(bayoen.star.Properties.Resources.CrownLightImage);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                this._goalCounter = value;
            }
        }

        private ScorePanelLength _panelLength;
        public ScorePanelLength PanelLength
        {
            get => this._panelLength;
            set
            {
                if (this._panelLength == value) return;

                switch (value)
                {
                    case ScorePanelLength.Short:
                        this.PanelImage.SetBitmap(bayoen.star.Properties.Resources.Score2ShortImage);
                        break;
                    case ScorePanelLength.Fit:
                        this.PanelImage.SetBitmap(bayoen.star.Properties.Resources.Score2FitImage);
                        break;
                    case ScorePanelLength.Long:
                        this.PanelImage.SetBitmap(bayoen.star.Properties.Resources.Score2LongImage);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                this._panelLength = value;
            }
        }

        private int _playerNumber;
        public int PlayerNumber
        {
            get => this._playerNumber;
            set
            {
                if (this._playerNumber == value) return;

                int token = Math.Min(4, Math.Max(2, value));

                this.ContainerPanel.Children.Clear();                

                switch (token)
                {
                    case 2:
                        this.ContainerPanel.Children.Add(this.Containers[0]); this.Containers[0].ScoreOrientation = ScoreOrientation.Right;
                        this.ContainerPanel.Children.Add(this.Containers[1]); this.Containers[1].ScoreOrientation = ScoreOrientation.Right;
                        this.ContainerPanel.Children.Add(this.Separators[0]); 
                        this.ContainerPanel.Children.Add(this.Containers[2]); this.Containers[2].ScoreOrientation = ScoreOrientation.Left;
                        this.ContainerPanel.Children.Add(this.Containers[3]); this.Containers[3].ScoreOrientation = ScoreOrientation.Left;
                        break;
                    case 3:
                        this.ContainerPanel.Children.Add(this.Containers[0]); this.Containers[0].ScoreOrientation = ScoreOrientation.Right;
                        this.ContainerPanel.Children.Add(this.Separators[0]);
                        this.ContainerPanel.Children.Add(this.Containers[1]); this.Containers[1].ScoreType = ScoreTypes.None;
                        this.ContainerPanel.Children.Add(this.Separators[1]);
                        this.ContainerPanel.Children.Add(this.Containers[2]); this.Containers[2].ScoreType = ScoreTypes.None;
                        break;
                    case 4:
                        this.ContainerPanel.Children.Add(this.Containers[0]); this.Containers[0].ScoreOrientation = ScoreOrientation.Right;
                        this.ContainerPanel.Children.Add(this.Separators[0]);
                        this.ContainerPanel.Children.Add(this.Containers[1]); this.Containers[1].ScoreType = ScoreTypes.None;
                        this.ContainerPanel.Children.Add(this.Separators[1]);
                        this.ContainerPanel.Children.Add(this.Containers[2]); this.Containers[2].ScoreType = ScoreTypes.None;
                        this.ContainerPanel.Children.Add(this.Separators[2]);
                        this.ContainerPanel.Children.Add(this.Containers[3]); this.Containers[3].ScoreType = ScoreTypes.None;
                        break;
                    default:
                        break;
                }

                this._playerNumber = token;

                this.CheckPanel();
            }
        }


        private List<int> _stars;
        private List<int> _games;

        public List<int> Stars
        {
            get => this._stars;
            set
            {               
                if (this._stars.SequenceEqual(value)) return;

                this._stars = value;
            }
        }           

        public List<int> Games
        {
            get => this._games;
            set
            {
                if (this._games.SequenceEqual(value)) return;

                this._games = value;
            }
        }        

        void CheckPanel()
        {

        }

        private const double FitMarginOffset = 30;
    }
}
