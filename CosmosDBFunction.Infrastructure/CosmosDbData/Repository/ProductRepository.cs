using CosmosDBFunction.Core.Entities;
using CosmosDBFunction.Core.Interfaces;
using CosmosDBFunction.Infrastructure.CosmosDbData.Interfaces;
using CosmosDBFunction.Infrastructure.CosmosDbData.Repository.Base;
using Microsoft.Azure.Cosmos;

namespace CosmosDBFunction.Infrastructure.CosmosDbData.Repository
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ICosmosDbContainerFactory factory) : base(factory) { }

        public override string ContainerName { get; } = "Product";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);
    }
}
