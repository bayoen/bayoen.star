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
using System.Windows.Shapes;

using bayoen.library.General.Enums;
using bayoen.library.General.ExtendedMethods;
using bayoen.library.Metro.Windows;

namespace bayoen.star.Windows
{
    public partial class MainWindow
    {

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

                if (Core.Temp.MyName == null || Core.Temp.MyName.Length == 0)
                {
                    this.WhereAmIBlock.Text = TryFindResource("Main-Status-Title-String") as string;
                }
                else
                {
                    this.WhereAmIBlock.Text = this.WhereAmIDetailBox.Text

                    .Replace("##MyName##", $"{Core.Temp.MyName}")
                    .Replace("##MyLocation##", $"{location}");
                }
                
            }
            else
            {
                this.WhereAmIBlock.Text = TryFindResource("Main-Status-Offline-String") as string;
            }
        }

        private const int EventListPageMax = 10;
    }
}
