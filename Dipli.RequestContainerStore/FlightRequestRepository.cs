using Dipli.RequestContainerStore.Model;
using Microsoft.ServiceFabric.Data;
using Microsoft.ServiceFabric.Data.Collections;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dipli.RequestContainerStore
{
    public class FlightRequestRepository : IFlightRequestRepository
    {
        private readonly IReliableStateManager stateManager;

        public FlightRequestRepository(IReliableStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public async Task<FlightRequest> GetFlightRequest(Guid id)
        {
            var flightRequestsReliableDictionary = await GetReliableDictionaryAsync("flightRequests");
            using (var transaction = stateManager.CreateTransaction())
            {
                var flightRequest = await flightRequestsReliableDictionary.TryGetValueAsync(transaction, id);
                return flightRequest.HasValue ? flightRequest.Value : throw new KeyNotFoundException();
            }
        }

        public async Task<IEnumerable<FlightRequest>> GetFlightRequests()
        {
            var flightRequestsReliableDictionary = await GetReliableDictionaryAsync("flightRequests");
            var flightRequests = new List<FlightRequest>();

            using(var transaction = stateManager.CreateTransaction())
            {
                var allFlightRequests = 
                    await flightRequestsReliableDictionary.CreateEnumerableAsync(transaction, EnumerationMode.Unordered);

                using (var enumerator = allFlightRequests.GetAsyncEnumerator())
                {
                    while (await enumerator.MoveNextAsync(CancellationToken.None))
                    {
                        var current = enumerator.Current;
                        flightRequests.Add(current.Value);
                    }
                }
            }

            return flightRequests;
        }

        public async Task InsertFlightRequest(FlightRequest flightRequest)
        {
            var flightRequests = await GetReliableDictionaryAsync("flightRequests");

            using (var transaction = stateManager.CreateTransaction())
            {
                await flightRequests.AddOrUpdateAsync(
                    transaction, flightRequest.Id, flightRequest, (id, oldFlightRequest) => flightRequest);

                await transaction.CommitAsync();
            }
        }

        public async Task DeleteFlightRequest(Guid id)
        {
            var flightRequestsReliableDictionary = await GetReliableDictionaryAsync("flightRequests");
            using (var transaction = stateManager.CreateTransaction())
            {
                await flightRequestsReliableDictionary.TryRemoveAsync(transaction, id);
            }
        }

        private async Task<IReliableDictionary<Guid, FlightRequest>> GetReliableDictionaryAsync(string collectionName)
        {
            return await stateManager.GetOrAddAsync<IReliableDictionary<Guid, FlightRequest>>(collectionName);
        }
    }
}
