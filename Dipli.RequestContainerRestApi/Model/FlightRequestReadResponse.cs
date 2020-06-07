using Newtonsoft.Json;
using System;

namespace Dipli.RequestContainerRestApi.Model
{
    public class FlightRequestReadResponse
    {
        [JsonProperty]
        public Guid Id { get; set; }
        [JsonProperty]
        public string From { get; set; }
        [JsonProperty]
        public string To { get; set; }
        [JsonProperty]
        public int MaxPrice { get; set; }
    }
}
