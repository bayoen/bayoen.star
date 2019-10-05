#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
using bayoen.star.Variables;

namespace bayoen.star.Windows
{
    public partial class DebugWindow : BaseWindow
    {
        public DebugWindow()
        {
            this.InitializeComponent();
            this.Title += $": {Config.AssemblyTitle}";

            Core.DebugTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            Core.DebugTimer.Tick += (sender, e) =>
            {
                this.TextOut1.Text = $"Core.MainWorker.Interval: { Core.MainWorker.Interval.TotalMilliseconds} [ms]"
                                               + $"\nCore.GameWorker.Interval: { Core.InGameWorker.Interval.TotalMilliseconds} [ms]";

                if (Core.Data.States.Main <= MainStates.Offline) return;

                this.TextOut2.Text = $"Core.Memory.LobbySize: {Core.Memory.LobbySize}"
                                   + $"\nCore.Memory.LobbySizeInGame: {Core.Memory.LobbySizeInGame}"
                                   + $"\nCore.Memory.LobbyMax: {Core.Memory.LobbyMax}"
                                   + $"\nCore.Memory.IsGameFinished: {Core.Memory.IsGameFinished}"
                                   + $"\nCore.Memory.GameWinnerToken: {Core.Memory.GameWinnerToken}"
                                   + $"\nCore.Memory.LeagueMode: {Core.Memory.LeagueMode}";
                                   //+ $"\nPlayer 1:\n{(new PlayerData(0)).ToJson()}"
                                   //+ $"\nPlayer 2:\n{(new PlayerData(1)).ToJson()}"
                                   //+ $"\nPlayer 3:\n{(new PlayerData(2)).ToJson()}"
                                   //+ $"\nPlayer 4:\n{(new PlayerData(3)).ToJson()}";

                this.TextOut3.Text = $"Core.Memory.LobbySize: {Core.Memory.LobbySize}"
                                 + $"\nLive.P1: {Core.Live.Players[0].ToJson()}"
                                 + $"\nLive.P2: {Core.Live.Players[1].ToJson()}"
                                 + $"\nLive.P3: {Core.Live.Players[2].ToJson()}"
                                 + $"\nLive.P4: {Core.Live.Players[3].ToJson()}";

                this.TextOut4.Text = $"Core.Memory.Teams: {string.Join(", ", Core.Memory.Teams)}";
                this.MainWorkerDataBlock.Text = $"Core.Data:\n{Core.Data.Serialize()}";
                this.ProjectDataBlock.Text = $"Core.Project:\n{Core.Project.Serialize()}";
                this.OperationDataBlock.Text = $"Core.Temp:\n{Core.Temp.Serialize()}";
            };
            Core.DebugTimer.Start();

        }
    }
}
#endif
