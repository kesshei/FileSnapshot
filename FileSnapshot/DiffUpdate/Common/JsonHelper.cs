using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffUpdate
{
    public static class JsonHelper
    {
        public static string ToJson(this object data)
        {
            return JsonConvert.SerializeObject(data, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
        public static T ToObj<T>(this string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
