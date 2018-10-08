using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Avt.Web.Backend.Helper
{
    public class JsonHelper
    {
        public static T ReadObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static List<T> ReadObjects<T>(string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static string SerializeObject<T>(T obj, bool indentedFormat = false)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = indentedFormat ? Formatting.Indented : Formatting.None,
                ContractResolver = new IgnoredContractResolver()
            });
        }
    }

    public class IgnoredContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            properties = properties.Where(p => p.HasMemberAttribute).ToList();
            return properties;
        }
    }
}