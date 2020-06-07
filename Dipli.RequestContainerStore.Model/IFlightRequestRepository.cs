using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dipli.RequestContainerStore.Model
{
    public interface IFlightRequestRepository
    {
        Task<IEnumerable<FlightRequest>> GetFlightRequests();
        Task<FlightRequest> GetFlightRequest(Guid id);
        Task InsertFlightRequest(FlightRequest flightRequest);
        Task DeleteFlightRequest(Guid id);
    }
}
