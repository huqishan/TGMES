using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.PackMethod
{
    public static class JsonHelper
    {
        public static bool SaveJson<T>(T obj,string filePath)
        {
            if (string.IsNullOrEmpty(Path.GetFileName(filePath)))
            {
                filePath += $"\\{typeof(T).Name}.json";
            }
            string? path = Path.GetDirectoryName(filePath);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (Stream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                writer.Write(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.Indented)));
            }
            return true;
        }
        public static T? ReadJson<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader read = File.OpenText(filePath))
                {
                    return JsonConvert.DeserializeObject<T>(read.ReadToEnd());
                }
            }
            return default(T);
        }
        public static string GetJsonValue(string json,string key)
        {
            JObject jsonObj=JObject.Parse(json);
            return GetNestJsonValue(jsonObj.Children(),key);
        }
        private static string GetNestJsonValue(JEnumerable<JToken> jToken, string key)
        {
            IEnumerator<JToken> enumerator = jToken.GetEnumerator();
            while (enumerator.MoveNext())
            {
                JToken jc = (JToken)enumerator.Current;
                if (jc is JObject || ((JProperty)jc).Value is JObject)
                {
                    return GetNestJsonValue(jc.Children(), key);
                }
                else
                {
                    if (((JProperty)jc).Name == key)
                    {
                        return ((JProperty)jc).Value.ToString();
                    }
                }
            }
            return null;
        }
    }
}
