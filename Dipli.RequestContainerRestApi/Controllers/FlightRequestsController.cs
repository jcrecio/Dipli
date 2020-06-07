using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dipli.RequestContainerStore.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Client;
using Dipli.RequestContainerRestApi.Model;

namespace Dipli.RequestContainerRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightRequestsController : ControllerBase
    {
        private readonly IFlightRequestService flightRequestService;
        public FlightRequestsController()
        {
            var serviceProxyFactory = new ServiceProxyFactory(s => new FabricTransportServiceRemotingClientFactory());

            flightRequestService = serviceProxyFactory.CreateServiceProxy<IFlightRequestService>(
                new Uri("fabric:/Dipli/Dipli.RequestContainerStore"),
                new ServicePartitionKey(0));

        }
        // GET api/flightrequests
        [HttpGet]
        public async Task<IEnumerable<FlightRequestReadResponse>> GetAllAsync()
        {
            var allFlights = await flightRequestService.GetFlightRequests();

            return allFlights.Select(f => new FlightRequestReadResponse
            {
                From = f.From,
                To = f.To,
                Id = f.Id,
                MaxPrice = f.MaxPrice
            });
        }

        // GET api/flightrequests/0fc43766-a8a6-4721-8a7d-be0042803b57
        [HttpGet("{id}")]
        public async Task<FlightRequest> GetAsync(int id)
        {
            return null;
        }

        // POST api/flightrequests
        [HttpPost]
        public async Task PostAsync([FromBody] FlightRequestCreatePayload flightRequest)
        {
            await flightRequestService.InsertFlightRequest(new FlightRequest
            {
                From = flightRequest.From,
                MaxPrice = flightRequest.MaxPrice,
                To = flightRequest.To
            });
        }

        // DELETE api/flightrequests/5
        [HttpDelete("{id}")]
        public async Task DeleteAsync(int id)
        {

        }
    }
}
