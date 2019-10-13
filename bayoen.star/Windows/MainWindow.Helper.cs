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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MainWindow
    {
        private bool _isInitial = true;
        public bool IsInitial
        {
            get => this._isInitial;
            set
            {
                if (this._isInitial == value) return;

                this.InitialGrid.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this.RightWindowCommands.Visibility = value ? Visibility.Collapsed : Visibility.Visible;

                this._isInitial = value;
            }
        }

        private string _initialStatus = "";
        public string InitialStatus
        {
            get => this._initialStatus;
            set
            {
                if (this._initialStatus == value) return;

                this.InitialStatusBlock.Text = value;

                this._initialStatus = value;
            }
        }

        private string _initialStatusResource = "";
        public string InitialStatusResource
        {
            get => this._initialStatusResource;
            set
            {
                if (this._initialStatusResource == value) return;

                this.InitialStatusBlock.Text = TryFindResource(value) as string;

                this._initialStatusResource = value;
            }
        }

        public bool SetFormatInitialStatusKey(string key, params object[] args)
        {
            try
            {
                string seedString = TryFindResource(key) as string;
                this.InitialStatusBlock.Text = this._initialStatusResource = string.Format(seedString, args);
            }
            catch
            {
                return false;
            }
            
            return true;
        }

        public void CheckStatus()
        {
            if (Core.Data.States.Main > MainStates.Offline)
            {
                string location;
                switch (Core.Data.States.Main)
                {
                    case MainStates.FreePlay:
                        location = TryFindResource("Main-Status-FreePlay-String") as string;
                        break;

                    case MainStates.PuzzleLeague:
                        location = $"{TryFindResource("Main-Status-PuzzleLeague-String") as string}";
                        break;

                    case MainStates.SoloArcade:
                    case MainStates.MultiArcade:
                        location = TryFindResource("Main-Status-Arcade-String") as string;
                        break;

                    //case MainStates.Adventure:
                    //    location = TryFindResource("Main-Status-Adventure-String") as string;
                    //    break;

                    default:
                        location = TryFindResource("Main-Status-Lobby-String") as string;
                        break;
                }

                if (Core.Temp.IsNameOn)
                {
                    this.WhereAmIBlock.Text = this.WhereAmIDetailBox.Text

                    .Replace("##MyName##", $"{Core.Temp.MyName}")
                    .Replace("##MyLocation##", $"{location}");
                }
                else
                {
                    this.WhereAmIBlock.Text = TryFindResource("Main-Status-Title-String") as string;                    
                }
                
            }
            else
            {
                this.WhereAmIBlock.Text = TryFindResource("Main-Status-Offline-String") as string;
            }
        }
    }
}
