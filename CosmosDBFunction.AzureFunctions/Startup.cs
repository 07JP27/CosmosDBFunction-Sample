using System.IO;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CosmosDBFunction.Infrastructure.Extensions;
using CosmosDBFunction.Infrastructure.AppSettings;
using CosmosDBFunction.Core.Interfaces;
using CosmosDBFunction.Infrastructure.CosmosDbData.Repository;

[assembly: FunctionsStartup(typeof(CosmosDBFunction.AzureFunctions.Startup))]
namespace CosmosDBFunction.AzureFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();


            builder.Services.AddSingleton<IConfiguration>(configuration);

            var cosmosDbConfig = configuration.GetSection("ConnectionStrings:FunctionDB").Get<CosmosDbSettings>();
            builder.Services.AddCosmosDb(cosmosDbConfig.EndpointUrl,
                                 cosmosDbConfig.PrimaryKey,
                                 cosmosDbConfig.DatabaseName,
                                 cosmosDbConfig.Containers);
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
