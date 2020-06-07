using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dipli.RequestContainerStore.Model;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace Dipli.RequestContainerStore
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class RequestContainerStore : StatefulService, IFlightRequestService
    {
        private IFlightRequestRepository flightRequestRepository;

        public RequestContainerStore(StatefulServiceContext context)
            : base(context)
        { }

        public async Task DeleteFlightRequest(Guid id)
        {
            await flightRequestRepository.DeleteFlightRequest(id);
        }

        public async Task<FlightRequest> GetFlightRequest(Guid id)
        {
            return await flightRequestRepository.GetFlightRequest(id);
        }

        public async Task<FlightRequest[]> GetFlightRequests()
        {
            return (await flightRequestRepository.GetFlightRequests()).ToArray();
        }

        public async Task InsertFlightRequest(FlightRequest flightRequest)
        {
            flightRequest.Id = Guid.NewGuid();
            await flightRequestRepository.InsertFlightRequest(flightRequest);
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return new[]
            {
                new ServiceReplicaListener(context => new FabricTransportServiceRemotingListener(context, this))
            };
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            flightRequestRepository = new FlightRequestRepository(StateManager);

        }
    }
}