using Newtonsoft.Json;
using UnityEngine;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace SteelHeart
{
    public static class JsonHelper
    {
        public static T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default;

            try
            {
                return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        Debug.LogError(
                            $"Deserialization entity: {typeof(T).FullName} error: {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error,
                });
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"Deserialize error: {e.Message}");
                return default;
            }
        }

        public static string SerializeObject(object obj, Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(obj, formatting);
        }
    }
}