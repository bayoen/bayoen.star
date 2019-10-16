using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using bayoen.star.Controls;
using bayoen.star.Functions;
using bayoen.star.Variables;

namespace bayoen.star.Workers
{
    public class LiveChecker
    {
        public LiveChecker()
        {
            this.Players = Enumerable.Range(0, 4).ToList()
                            .ConvertAll(x => new PlayerData()).ToList();
            this.Panels = Enumerable.Range(0, 4).ToList()
                            .ConvertAll(x => new PlayerPanel()).ToList();
            this.Reversed = Enumerable.Range(0, 2).ToList()
                            .ConvertAll(x => new PlayerPanel()
                            {
                                Visibility = Visibility.Collapsed,
                            }).ToList();
            this.Reversed.Reverse();
            this.Reset();
        }

        public PlayerData RoomMaker { get; set; }
        public List<PlayerData> Players { get; set; }
        public List<PlayerPanel> Panels { get; set; }
        public List<PlayerPanel> Reversed { get; set; }

        private bool _isReversed = false;
        public bool IsReversed
        {
            get => this._isReversed;
            set
            {
                if (this._isReversed == value) return;

                this.Panels.ForEach(x => x.Visibility = value ? Visibility.Collapsed : Visibility.Visible);
                this.Reversed.ForEach(x => x.Visibility = value ? Visibility.Visible : Visibility.Collapsed);

                this._isReversed = value;
            }
        }

        private int _roomSize = 4;
        private int RoomSize
        {
            get => this._roomSize;
            set
            {
                int size = Math.Max(2, Math.Min(4, value));

                if (this._roomSize == size) return;

                this.Panels[2].Visibility = (size > 2) ? Visibility.Visible : Visibility.Collapsed;
                this.Panels[3].Visibility = (size > 3) ? Visibility.Visible : Visibility.Collapsed;

                this._roomSize = size;
            }
        }

        public void Reset()
        {
            this.Players.ForEach(x => x.Reset());
            this.Panels.ForEach(x => x.Reset());
            this.Reversed.ForEach(x => x.Reset());
        }

        public void Check(int roomSize)
        {
            this.RoomSize = roomSize;
            this.IsReversed = (roomSize == 2) ? (this.Players[0].ID32 != Core.Temp.MyID32) : false;

            if (this.IsReversed)
            {                
                this.Reversed[0].Read(this.Players[1]);
                this.Reversed[1].Read(this.Players[0]);
            }
            else
            {
                for (int playerIndex = 0; playerIndex < roomSize; playerIndex++)
                {
                    this.Panels[playerIndex].Read(this.Players[playerIndex]);
                }
            }

        }
    }
}
