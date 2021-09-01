using Microsoft.Azure.Cosmos;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosDBFunction.Infrastructure.CosmosDbData.Interfaces
{
    public interface ICosmosDbContainer
    {
        Container _container { get; }
    }
}
