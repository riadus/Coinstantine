using Coinstantine.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Coinstantine.Domain.Extensions
{
    public static class JsonSerializerExtensions
    {
        static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = { new FlexibleStringEnumConverter() },
            NullValueHandling = NullValueHandling.Ignore
        };

        public static T DeserializeTo<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString, _settings);
        }

        public static string Serialize<T>(this T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize, _settings);
        }

        public static string SerializeEnum<T>(this T objectToSerialize)
        {
            var serializeValue = JsonConvert.SerializeObject(objectToSerialize, _settings);
            if (serializeValue.Contains("\""))
            {
                serializeValue = serializeValue.Replace("\"", "");
            }
            return serializeValue;
        }
    }
}
