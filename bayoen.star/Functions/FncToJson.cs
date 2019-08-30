using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bayoen.star.Functions
{
    public class FncToJson
    {
        public JObject ToJson()
        {
            return this.ToJson(Config.JsonSerializerSetting);
        }

        public JObject ToJson(JsonSerializerSettings serializerSettings)
        {
            try { return JObject.Parse(this.Serialize(serializerSettings, false)); }
            catch { return null; }
        }

        public string Serialize()
        {
            return this.Serialize(Config.JsonSerializerSetting, true);
        }

        public string Serialize(JsonSerializerSettings serializerSettings)
        {
            return this.Serialize(serializerSettings, true);
        }

        public string Serialize(bool isIndented)
        {
            return this.Serialize(Config.JsonSerializerSetting, isIndented);
        }

        public string Serialize(JsonSerializerSettings serializerSettings, bool isIndented)
        {
            try { return JsonConvert.SerializeObject(this, (isIndented ? Formatting.Indented : Formatting.None), serializerSettings); }            
            catch { return null; }
        }

        public static T FromJson<T>(JObject json)
        {
            try { return JsonConvert.DeserializeObject<T>(json.ToString()); }
            catch { return default; }
        }
    }
}
