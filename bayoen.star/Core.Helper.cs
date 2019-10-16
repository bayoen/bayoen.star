using System;
using System.ComponentModel;
using System.Windows;

using bayoen.star.Variables;

namespace bayoen.star
{
    public static partial class Core
    {
        private static OptionData _project;
        public static OptionData Project => Core._project ?? (Core._project = OptionData.Load());

        /// <summary>
        /// Worker for initialzation
        /// </summary>
        public static BackgroundWorker InitialWorker { get; set; }


        private static void Invoke(Action callback) => Application.Current.Dispatcher.Invoke(callback);
    }
}
