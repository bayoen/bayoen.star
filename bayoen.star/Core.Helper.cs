using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

using bayoen.star.Overlays;
using bayoen.star.Variables;

namespace bayoen.star
{
    public static partial class Core
    {
        private static GameMemory _memory;
        public static GameMemory Memory => Core._memory ?? (Core._memory = new GameMemory(Config.PPTName));


        private static OptionData _option;
        public static OptionData Option => Core._option ?? (Core._option = OptionData.Load());


        private static DownloadData _download;
        public static DownloadData Download => Core._download ?? (Core._download = new DownloadData());


        private static OverlayTimer _overlayTimer;
        public static OverlayTimer OverlayTimer => Core._overlayTimer ?? (Core._overlayTimer = new OverlayTimer());


        /// <summary>
        /// Worker for initialzation
        /// </summary>
        public static BackgroundWorker InitialWorker { get; set; }
        public static BackgroundWorker DownloadWorker { get; set; }

        public static void Invoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);
    }
}
