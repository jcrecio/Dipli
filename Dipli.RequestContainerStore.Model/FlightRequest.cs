using System;

namespace Dipli.RequestContainerStore.Model
{
    public class FlightRequest
    {
        public Guid Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public int MaxPrice { get; set; }
    }
}
