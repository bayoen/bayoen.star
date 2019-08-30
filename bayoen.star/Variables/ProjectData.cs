using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using bayoen.library.General.Enums;
using bayoen.star.Localizations;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class ProjectData : FncToJson
    {
        public ProjectData()
        {
            this.Version = Config.Assembly.GetName().Version;
            this.RestartingMode = RestartingModes.None;

            this._goalCounter = GoalCounters.Game;
            this.GoalCounter = GoalCounters.Star;

            this.AutoUpdate = true;
            this.EnglishDisplay = false;

            this._enableSlowMode = true;
            this.EnableSlowMode = false;
        }

        //// From Configurations
        public Version Version { get; set; }        

        //// From Setting Window
        /// General
        private bool _topMost;
        public bool TopMost
        {
            get => this._topMost;
            set
            {
                if (this._topMost == value) return;

                Core.MainWindow.Topmost = value;

                this._topMost = value;
            }
        }

        private bool _autoUpdate;
        public bool AutoUpdate
        {
            get => this._autoUpdate;
            set
            {
                if (this._autoUpdate == value) return;


                this._autoUpdate = value;
            }
        }

        private string _languageCode;
        public string LanguageCode
        {
            get
            {
                if (this._languageCode != null)
                {
                    if (Config.CultureCodes.Contains(this._languageCode))
                    {
                        return this._languageCode;
                    }
                }

                string cultureCode = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                if (!Config.CultureCodes.Contains(cultureCode)) cultureCode = Config.CultureCodes[0];
                return cultureCode;
            }
            set
            {
                if (this._languageCode == value) return;

                Culture.Set(value);

                this._languageCode = value;
                this.Save();
            }
        }

        private bool _englishDisplay;
        public bool EnglishDisplay
        {
            get => this._englishDisplay;
            set
            {
                if (this._englishDisplay == value) return;

                this._englishDisplay = value;
                this.Save();
            }
        }

        /// Streaming
        private ChromaKeys _chromaKey;
        public ChromaKeys ChromaKey
        {
            get => this._chromaKey;
            set
            {
                if (this._chromaKey == value) return;

                //int chromaKeyIndex = Config.ChromaKeySets.FindIndex(x => x.Item1 == value);
                //if (chromaKeyIndex > 0)
                //{
                //    Core.CapturableWindow.Background = Config.ChromaKeySets[chromaKeyIndex].Item2;
                //}

                this._chromaKey = value;
            }
        }

        /// Advanced
        private bool _enableSlowMode;
        public bool EnableSlowMode
        {
            get => this._enableSlowMode;
            set
            {
                if (this._enableSlowMode == value) return;

                if (!Core.MainWorker.IsEnabled) Core.MainWorker.Initiate();
                Core.MainWorker.Stop();

                Core.MainWorker.Interval = value ? Config.SlowInterval : Config.NormalInterval;

                Core.MainWorker.Start();

                this._enableSlowMode = value;
                this.Save();
            }
        }

        



        //// For Operation
        public RestartingModes RestartingMode { get; set; }

        private DisplayModes _displayModes;
        public DisplayModes DisplayMode
        {
            get => this._displayModes;
            set
            {
                if (this._displayModes == value) return;



                this._displayModes = value;
            }
        }

        private List<int> _countedStars;
        public List<int> CountedStars
        {
            get => this._countedStars ?? (this._countedStars = new List<int>(4) { 0, 0, 0, 0 });
            set
            {
                if (value != null && this._countedStars != null)
                    if (value.SequenceEqual(this._countedStars)) return;

                Core.MiniWindow.MiniScorePanel.Stars = value;
                Core.MiniOverlay.MiniScorePanel.Stars = value;

                this._countedStars = value;
                this.Save();
            }
        }

        private List<int> _countedGames;
        public List<int> CountedGames
        {
            get => this._countedGames ?? (this._countedGames = new List<int>(4) { 0, 0, 0, 0 });
            set
            {
                if (value != null && this._countedGames != null)
                    if (value.SequenceEqual(this._countedGames)) return;

                Core.MiniWindow.MiniScorePanel.Games = value;
                Core.MiniOverlay.MiniScorePanel.Games = value;

                this._countedGames = value;
                this.Save();
            }
        }

        private GoalTypes _goalType;
        public GoalTypes GoalType
        {
            get => this._goalType;
            set
            {
                if (this._goalType == value) return;

                

                this._goalType = value;
            }
        }

        private GoalCounters _goalCounter;
        public GoalCounters GoalCounter
        {
            get => this._goalCounter;
            set
            {
                if (this._goalCounter == value) return;

                Core.MiniWindow.GoalCounter = value;
                Core.MiniOverlay.GoalCounter = value;

                this._goalCounter = value;
            }
        }

        private int _goalScore;
        public int GoalScore
        {
            get => this._goalScore;
            set
            {
                if (this._goalScore == value) return;



                this._goalScore = value;
            }
        }

        public static ProjectData Load()
        {
            return ProjectData.Load(Config.ProjectDataFileName);
        }

        public static ProjectData Load(string path)
        {
            bool failFlag = false;
            string rawString;
            if (File.Exists(path))
            {
                rawString = File.ReadAllText(path, Config.TextEncoding);
            }
            else
            {
                rawString = "";
                failFlag = true;
            }

            ProjectData settingData = null;
            try
            {
                settingData = JsonConvert.DeserializeObject<ProjectData>(rawString, Config.JsonSerializerSetting);
            }
            catch
            {
                failFlag = true;
            }

            if (settingData == null) failFlag = true;
            if (failFlag)
            {
                settingData = new ProjectData();
                settingData.Save(path);
            }

            return settingData;
        }

        public void Save()
        {
            this.Save(Config.ProjectDataFileName);
        }

        public void Save(string path)
        {
            string jsonString = JsonConvert.SerializeObject(this, Formatting.Indented, Config.JsonSerializerSetting);
            File.WriteAllText(path, jsonString, Config.TextEncoding);
        }
    }
}
