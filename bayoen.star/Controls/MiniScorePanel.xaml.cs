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
        public GoalTypes GoalType { get; internal set; }

        public MiniScorePanel()
        {
            this.InitializeComponent();

            this.GoalPanelImage.SetBitmap(bayoen.star.Properties.Resources.Score2GoalImage);
            this.GoalTypeImage.SetBitmap(bayoen.star.Properties.Resources.StarPlusImage);

            this._isFit = false;
            this.IsFit = true;

            this._playerNumber = 2;

            this._stars = new List<int>() { -1, -1, -1, -1 };
            this._games = new List<int>() { -1, -1, -1, -1 };

            this.Containers1 = new List<MiniScoreContainer>();
            this.Containers2 = new List<MiniScoreContainer>();

            for (int playerIndex = 0; playerIndex < 4; playerIndex++)
            {
                this.Containers1.Add(new MiniScoreContainer());
                this.Containers2.Add(new MiniScoreContainer());
            }
            
        }

        private List<MiniScoreContainer> Containers1;
        private List<MiniScoreContainer> Containers2;

        private Score2PanelTypes _panelType;
        public Score2PanelTypes PanelType
        {
            get => this._panelType;
            set
            {
                if (this._panelType == value) return;

                switch (value)
                {
                    case Score2PanelTypes.Short:
                        break;
                    case Score2PanelTypes.Fit:
                        break;
                    case Score2PanelTypes.Long:
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                this._panelType = value;
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

                this._playerNumber = token;
            }
        }

        private bool _isFit;
        public bool IsFit
        {
            get => this._isFit;
            set
            {
                if (this._isFit == value) return;

                this.PanelImage.SetBitmap(value ? bayoen.star.Properties.Resources.Score2FitImage : bayoen.star.Properties.Resources.Score2ShortImage);

                this._isFit = value;
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

        public int Star1
        {
            get => this.Stars[0];
            set
            {
                if (this.Stars[0] == value) return;

                this.Stars[0] = value;
            }
        }

        public int Star2
        {
            get => this.Stars[1];
            set
            {
                if (this.Stars[1] == value) return;

                this.Stars[1] = value;
            }
        }

        public int Star3
        {
            get => this.Stars[2];
            set
            {
                if (this.Stars[2] == value) return;

                this.Stars[2] = value;
            }
        }

        public int Star4
        {
            get => this.Stars[3];
            set
            {
                if (this.Stars[3] == value) return;

                this.Stars[3] = value;
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

        public int Game1
        {
            get => this.Games[0];
            set
            {
                if (this.Games[0] == value) return;

                this.Games[0] = value;
            }
        }

        public int Game2
        {
            get => this.Games[1];
            set
            {
                if (this.Games[1] == value) return;

                this.Games[1] = value;
            }
        }

        public int Game3
        {
            get => this.Games[2];
            set
            {
                if (this.Games[2] == value) return;

                this.Games[2] = value;
            }
        }

        public int Game4
        {
            get => this.Games[3];
            set
            {
                if (this.Games[3] == value) return;

                this.Games[3] = value;
            }
        }
    }
}
