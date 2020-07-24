using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public class NewtonsoftJsonContainerSerializer : IContainerSerializer
    {
        private static readonly JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };

        public string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, JsonSerializerSettings);
        }

        public TResult Deserialize<TResult>(string value)
        {
            return JsonConvert.DeserializeObject<TResult>(value, JsonSerializerSettings);
        }
    }
}
