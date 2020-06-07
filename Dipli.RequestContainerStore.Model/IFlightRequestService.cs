using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Threading.Tasks;

namespace Dipli.RequestContainerStore.Model
{
    public interface IFlightRequestService: IService
    {
        Task<FlightRequest[]> GetFlightRequests();
        Task<FlightRequest> GetFlightRequest(Guid id);
        Task InsertFlightRequest(FlightRequest flightRequest);
        Task DeleteFlightRequest(Guid id);
    }
}
