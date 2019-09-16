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
using bayoen.star.Variables;

namespace bayoen.star.Controls
{
    public partial class PlayerPanel : Grid
    {
        public PlayerPanel()
        {
            this.InitializeComponent();
            this.Reset();
        }

        private string _playerName = "";
        public string PlayerName
        {
            get => this._playerName;
            set
            {
                if (this._playerName == value) return;

                this.PlayerNameBlock.Text = value;

                this._playerName = value;
            }
        }

        private int _playerRating = -1;
        public int PlayerRating
        {
            get => this._playerRating;
            set
            {
                int token = Math.Max(-1, Math.Min(Config.CheatRatingMax, value));

                if (this._playerRating == token) return;

                this.PlayerRatingBlock.Text = (token == -1) ? "" : $"{value}";

                this._playerRating = value;
            }
        }

        private GameAvatars _avatar = 0;
        public GameAvatars Avatar
        {
            get => this._avatar;
            set
            {
                if (this._avatar == value) return;

                this.PlayerCharacterBlock.Text = $"{value}";

                this._avatar = value;
            }
        }

        public void Reset()
        {
            this.PlayerName = "";
            this.PlayerRating = -1;
            this.Avatar = 0;
        }

        public void Read(PlayerData player)
        {
            this.PlayerName = player.Name;
            this.PlayerRating = player.Rating;
        }
    }
}
