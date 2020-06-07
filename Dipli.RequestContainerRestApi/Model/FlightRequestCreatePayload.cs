using Newtonsoft.Json;

namespace Dipli.RequestContainerRestApi.Model
{
    public class FlightRequestCreatePayload
    {
        [JsonProperty]
        public string From { get; set; }
        [JsonProperty]
        public string To { get; set; }
        [JsonProperty]
        public int MaxPrice { get; set; }
    }
}
