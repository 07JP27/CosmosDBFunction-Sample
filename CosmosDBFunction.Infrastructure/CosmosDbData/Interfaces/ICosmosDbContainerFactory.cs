using System.Threading.Tasks;

namespace CosmosDBFunction.Infrastructure.CosmosDbData.Interfaces
{
    public interface ICosmosDbContainerFactory
    {
        ICosmosDbContainer GetContainer(string containerName);

        Task EnsureDbSetupAsync();
    }
}
