using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CosmosDBFunction.Core.Interfaces;
using System.Collections.Generic;
using CosmosDBFunction.Core.Entities;
using CosmosDBFunction.Core.Exceptions;
using System.IO;
using Newtonsoft.Json;

namespace CosmosDBFunction.AzureFunctions
{
    public class ProductFunction
    {
        private readonly IProductRepository _repo;

        public ProductFunction(IProductRepository repo)
        {
            this._repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [FunctionName("GetProduct")]
        public async Task<IActionResult> GetProduct(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "product")] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            if (!String.IsNullOrEmpty(id))
            {
                try
                {
                    Product item = await _repo.GetByIdAsync(id);
                    return new OkObjectResult(item);
                }
                catch (EntityNotFoundException)
                {
                    return new NotFoundResult();
                }
            }

            IEnumerable<Product> items = await _repo.GetAllItemAsync();
            return new OkObjectResult(items);
        }

        [FunctionName("AddProduct")]
        public async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "product")] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic item = JsonConvert.DeserializeObject<Product>(requestBody);

            await _repo.AddAsync(item);
            return new OkObjectResult(item);
        }
    }
}
