using System;
using Cinemachine.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace SteelHeart
{
    public class JsonPropertyResolver : DefaultContractResolver
    {
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateObjectContract(objectType);
            contract.ItemRequired = Required.AllowNull;
            return contract;
        }
    }
    
    public class JsonHelper
    {
        public static T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
                return default;

            try
            {
                var serializeSettings = new JsonSerializerSettings
                {
                    Error = delegate(object sender, ErrorEventArgs args)
                    {
                        Debug.LogError($"Deserialization entity: {typeof(T).FullName} error: {args.ErrorContext.Error.Message}");
                        args.ErrorContext.Handled = true;
                    },
                    MissingMemberHandling = MissingMemberHandling.Error,
                    ContractResolver = new JsonPropertyResolver(),
                };

                return JsonConvert.DeserializeObject<T>(json, serializeSettings);
            }
            catch (JsonSerializationException e)
            {
                Debug.LogError($"Deserialize error: {e.Message}");
                return default;
            }
        }
    }
}
