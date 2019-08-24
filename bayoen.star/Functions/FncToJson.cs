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
