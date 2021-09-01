using CosmosDBFunction.Core.Entities.Base;
using Microsoft.Azure.Cosmos;

namespace CosmosDBFunction.Infrastructure.CosmosDbData.Interfaces
{
    public interface IContainerContext<in T> where T : EntityBase
    {
        string ContainerName { get; }
        string GenerateId(T entity);
        PartitionKey ResolvePartitionKey(string entityId);
    }
}
