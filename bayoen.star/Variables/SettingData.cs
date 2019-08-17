using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using bayoen.library.General.Enums;

namespace bayoen.star.Variables
{
    public class SettingData
    {
        public SettingData()
        {
            this.Version = Config.Assembly.GetName().Version;
            this.RestartingMode = RestartingModes.None;
        }

        public Version Version;
        public RestartingModes RestartingMode;

        public static SettingData Load()
        {
            return SettingData.Load(Config.SettingDataFileName);
        }

        public static SettingData Load(string path)
        {
            bool failFlag = false;
            string rawString;
            if (File.Exists(path))
            {
                rawString = File.ReadAllText(path);
            }
            else
            {
                rawString = "";
                failFlag = true;
            }

            SettingData settingData = null;
            try
            {
                settingData = JsonConvert.DeserializeObject<SettingData>(rawString);
            }
            catch
            {
                failFlag = true;
            }

            if (settingData == null) failFlag = true;
            if (failFlag)
            {
                settingData = new SettingData();
                settingData.Save(path);
            }

            return settingData;
        }

        public static SettingData FromJson(JObject json)
        {
            try { return JsonConvert.DeserializeObject<SettingData>(json.ToString()); }
            catch { return null; }
        }

        public void Save()
        {
            this.Save(Config.SettingDataFileName);
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
