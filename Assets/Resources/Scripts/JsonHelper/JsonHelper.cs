using Newtonsoft.Json;
using UnityEngine;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace SteelHeart
{
    public static class JsonHelper
    {
        private static JsonSerializerSettings MakeSettings<T>()
        {
            return new JsonSerializerSettings
            {
                Error = delegate(object sender, ErrorEventArgs args)
                {
                    Debug.LogError(
                        $"Deserialization entity: {typeof(T).FullName} error: {args.ErrorContext.Error.Message}");
                    args.ErrorContext.Handled = true;
                },
                
                MissingMemberHandling = MissingMemberHandling.Error,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
            };
        }
        
        public static T DeserializeObject<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default;

            try
            {
                var settings = MakeSettings<T>();
                return JsonConvert.DeserializeObject<T>(json, settings);
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"Deserialize error: {e.Message}");
                return default;
            }
        }

        public static string SerializeObject(object obj)
        {
            try
            {
                var settings = MakeSettings<object>();
                return JsonConvert.SerializeObject(obj, settings);
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"Serialize error: {e.Message}");
                return default;
            }
        }
    }
}