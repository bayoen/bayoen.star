using System;
using System.ComponentModel;
using System.Windows;

using bayoen.star.Variables;

namespace bayoen.star
{
    public static partial class Core
    {
        private static OptionData _option;
        public static OptionData Option => Core._option ?? (Core._option = OptionData.Load());

        private static DownloadData _download;
        public static DownloadData Download => Core._download ?? (Core._download = new DownloadData());

        /// <summary>
        /// Worker for initialzation
        /// </summary>
        public static BackgroundWorker InitialWorker { get; set; }
        public static BackgroundWorker DownloadWorker { get; set; }

        private static void Invoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);
    }
}
