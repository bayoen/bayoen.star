using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using bayoen.library.General.Enums;
using bayoen.star.Localizations;
using bayoen.star.Functions;

namespace bayoen.star.Variables
{
    public class OptionData : FncJson
    {
        public OptionData()
        {
            // Preferences
            this.AutoUpdate = true;
            //this.LanguageCode

            // Operations
            this.RestartingMode = RestartingModes.None;
            this.JustUpdated = false;
        }

        public RestartingModes RestartingMode { get; set; }
        public bool JustUpdated { get; set; }

        private bool _autoUpdate;
        public bool AutoUpdate
        {
            get => this._autoUpdate;
            set
            {
                if (this._autoUpdate == value) return;


                this._autoUpdate = value;
                this.Save();
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

        private ChromaKeys _chromaKey;
        public ChromaKeys ChromaKey
        {
            get => this._chromaKey;
            set
            {
                if (this._chromaKey == value) return;

                int chromaKeyIndex = Config.ChromaSets.FindIndex(x => x.Item1 == value);
                if (chromaKeyIndex > -1)
                {
                    //if (chromaKeyIndex == 0)
                    //{
                    //    Core.LeagueWindow.BorderThickness = new Thickness(1);
                    //    Core.LeagueWindow.SetResourceReference(Control.BackgroundProperty, "WindowBackgroundBrush");
                    //}
                    //else
                    //{
                    //    Core.LeagueWindow.BorderThickness = new Thickness(0);
                    //    Core.LeagueWindow.Background = Config.ChromaSets[chromaKeyIndex].Item2;
                    //}
                }

                this._chromaKey = value;
            }
        }

        public static OptionData Load()
        {
            return OptionData.Load(Config.OptionDataFileName);
        }

        public static OptionData Load(string path)
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

            OptionData optionData = null;
            try
            {
                optionData = JsonConvert.DeserializeObject<OptionData>(rawString, Config.JsonSerializerSetting);
            }
            catch
            {
                failFlag = true;
            }

            if (optionData == null) failFlag = true;
            if (failFlag)
            {
                optionData = new OptionData();
                optionData.Save(path);
            }

            return optionData;
        }

        public void Save()
        {
            this.Save(Config.OptionDataFileName);
        }
    }
}
