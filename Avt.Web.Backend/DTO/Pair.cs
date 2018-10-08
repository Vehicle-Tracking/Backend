using Newtonsoft.Json;

namespace Avt.Web.Backend.DTO
{
    public class Pair<TK, TV>
    {
        [JsonProperty("key")]
        public TK Key { get; set; }

        [JsonProperty("value")]
        public TV Value { get; set; }

        public Pair() { }

        public Pair(TK key, TV value)
        {
            Key = key;
            Value = value;
        }
    }
}