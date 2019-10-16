using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using bayoen.library.General.Enums;
using bayoen.library.Metro.Controls;
using bayoen.star.Localizations;
using bayoen.star.Workers;

namespace bayoen.star
{
    public static partial class Core
    {
        /// <summary>
        /// Worker for visual component
        /// </summary>
        private static MainWorker _mainWorker;
        public static MainWorker MainWorker => Core._mainWorker ?? (Core._mainWorker = new MainWorker());

        /// <summary>
        /// Worker for In-game infomation
        /// </summary>
        private static InGameWorker _inGameWorker;
        public static InGameWorker InGameWorker => Core._inGameWorker ?? (Core._inGameWorker = new InGameWorker());

        /// <summary>
        /// Worker for initialzation
        /// </summary>
        public static BackgroundWorker InitialWorker { get; set; }


        public static List<MetroChromaComboBoxItem> GetChromaComboBoxItemList()
        {
            List<MetroChromaComboBoxItem> ChromaComboBoxItemList = new List<MetroChromaComboBoxItem>();
            for (int chromaIndex = 0; chromaIndex < Config.ChromaSets.Count; chromaIndex++)
            {
                MetroChromaComboBoxItem TokenChroma = new MetroChromaComboBoxItem()
                {
                    IconForeground = Config.ChromaSets[chromaIndex].Item2,
                };
                TokenChroma.SetResourceReference(MetroChromaComboBoxItem.TextProperty, $"Dictionary-ChromaKey-{chromaIndex}-String");

                ChromaComboBoxItemList.Add(TokenChroma);
            }

            return ChromaComboBoxItemList;
        }

        public static void ResetScore()
        {
            Core.Project.CountedStars = new List<int>(4) { 0, 0, 0, 0 };
            Core.Project.CountedGames = new List<int>(4) { 0, 0, 0, 0 };
        }

        /// <summary>
        /// Restart 'bayoen-star' application with <paramref name="mode"/> parameter
        /// </summary>
        /// <param name="mode"></param>
        public static void Restart(RestartingModes mode)
        {
            Core.Project.RestartingMode = mode;
            Core.Project.Save();
            Core.TrayIcon.Terminate();

            Process.Start(Application.ResourceAssembly.Location);
            Environment.Exit(0);
        }

        public static void Save()
        {
            Core.Project.Save();
        }

        public static void ShowFolder()
        {
            Process.Start(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        public static void Terminate()
        {
            Core.TrayIcon.Terminate();
            Core.Save();
            Environment.Exit(0);
        }

        public static void Counting()
        {

        }

        public static void UpdateResult()
        {
            Core.EventViewer.Check();
            Core.LeagueWindow.Check();
        }
    }
}
