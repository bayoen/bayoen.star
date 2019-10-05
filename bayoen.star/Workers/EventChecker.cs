//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//using bayoen.star.Variables;

//namespace bayoen.star.Workers
//{
//    public class EventChecker
//    {
//        public EventChecker()
//        {
//            this.Matches = new List<MatchRecord>();
//            this._pageIndex = 0;
//            this.PageMax = 0;

//            this.AnchorIndex = 1;
//        }

//        public List<MatchRecord> Matches;

//        private int _pageIndex = -1;
//        public int PageIndex
//        {
//            get => this._pageIndex;
//            set
//            {
//                int tokenIndex = Math.Max(0, Math.Min(this.PageMax, value));
//                if (this._pageIndex == tokenIndex) return;                

//                this.CheckNavigator(tokenIndex, this.PageMax);
//                this._pageIndex = tokenIndex;
//            }
//        }

//        private int _pageMax = -1;
//        public int PageMax
//        {
//            get => this._pageMax;
//            set
//            {
//                if (this._pageMax == value) return;

//                this.CheckNavigator(this.PageIndex, value);
//                this._pageMax = value;
//            }
//        }

//        private int AnchorIndex { get; set; }

//        private void CheckNavigator(int index, int max)
//        {            
//            if (max > 1)
//            {
//                Core.MainWindow.EventListNavigatorPanel.Visibility = Visibility.Visible;

//                Core.MainWindow.PrevEventListButton.IsAccented = (index > 0);
//                Core.MainWindow.PrevEventListButton.IsEnabled = (index > 0);

//                Core.MainWindow.NextEventListButton.IsAccented = (max < 0);
//                Core.MainWindow.NextEventListButton.IsEnabled = (max < 0);

//                Core.MainWindow.EventListNavigatorBlock.Text = $"{(index + 1).ToString().PadLeft(4)} / {max.ToString().PadRight(4)}";
//            }
//            else
//            {
//                Core.MainWindow.EventListNavigatorPanel.Visibility = Visibility.Collapsed;
//            }


//        }

//        public void ScanEvents()
//        {
//            //string matchFolderPath = Path.Combine(Config.RecordFolderName, Config.MatchFolderName);

//            //if (Directory.Exists(matchFolderPath))
//            //{
//            //    List<string> files = Directory.GetFiles(matchFolderPath, "Match-*.json").ToList();
//            //    files.Reverse();


//            //    //List<string> files = Directory.GetFiles(matchFolderPath, "Match-*.json").ToList();
//            //    //files.Reverse();
//            //}
//            //else
//            //{

//            //}

            
//        }

//        //private int _eventListPages = -1;
//        //public int EventListPages
//        //{
//        //    get => this._eventListPages;
//        //    set
//        //    {
//        //        if (this._eventListPages == value) return;

//        //        this.EventListNavigatorPanel.Visibility = (value > 1) ? Visibility.Visible : Visibility.Collapsed;
//        //        this.EventListNavigatorBlock.Text = $"{(this.EventListAnchor + 1).ToString().PadLeft(4)} / {value.ToString().PadRight(4)}";

//        //        this._eventListPages = value;
//        //    }
//        //}

//        public int GetEventListPages()
//        {
//            return -1;
//        }

//        private const int AnchorRange = 3;
//    }
//}
