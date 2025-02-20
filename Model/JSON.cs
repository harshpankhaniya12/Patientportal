using Newtonsoft.Json;

namespace Innovura.CSharp.Core
{
    public static class JSON
    {
        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }

        public static TObject Deserialize<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }
}
