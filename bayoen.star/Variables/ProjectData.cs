using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using bayoen.library.General.Enums;

namespace bayoen.star.Variables
{
    public class ProjectData
    {
        public ProjectData()
        {
            this.Version = Config.Assembly.GetName().Version;
            this.RestartingMode = RestartingModes.None;

            this._goalCounter = GoalCounters.Game;
            this.GoalCounter = GoalCounters.Star;

            this._miniChromaKey = ChromaKeys.Magenta;
            this.MiniChromaKey = ChromaKeys.None;
        }

        public Version Version;
        public RestartingModes RestartingMode;

        private GoalCounters _goalCounter;
        public GoalCounters GoalCounter
        {
            get => this._goalCounter;
            set
            {
                if (this._goalCounter == value) return;

                Core.MiniWindow.GoalType = value;
                Core.MiniOverlay.GoalType = value;

                this._goalCounter = value;
            }
        }

        private ChromaKeys _miniChromaKey;
        public ChromaKeys MiniChromaKey
        {
            get => this._miniChromaKey;
            set
            {
                if (this._miniChromaKey == value) return;

                Core.MiniWindow.Background = Config.ChromaSets.Find(x => x.Item1 == value).Item2;
                Core.MiniWindow.BorderThickness = new Thickness(Convert.ToInt32(value == ChromaKeys.None));

                this._miniChromaKey = value;
                this.Save();
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

                this._countedGames = value;
            }
        }

        public string CultureCode { get; internal set; }

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

        public static ProjectData FromJson(JObject json)
        {
            try { return JsonConvert.DeserializeObject<ProjectData>(json.ToString()); }
            catch { return null; }
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

        public JObject ToJson()
        {
            return this.ToJson(Config.JsonSerializerSetting, false);
        }

        public JObject ToJson(JsonSerializerSettings serializerSettings)
        {
            return this.ToJson(serializerSettings, false);
        }

        public JObject ToJson(bool isIndented)
        {
            return this.ToJson(Config.JsonSerializerSetting, isIndented);
        }

        public JObject ToJson(JsonSerializerSettings serializerSettings, bool isIndented)
        {
            string jsonString = JsonConvert.SerializeObject(this, (isIndented ? Formatting.Indented : Formatting.None), serializerSettings);

            try { return JObject.Parse(jsonString); }
            catch { return null; }
        }

    }
}
