using CosmosDBFunction.Core.Entities.Base;
using CosmosDBFunction.Core.Exceptions;
using CosmosDBFunction.Core.Interfaces.Base;
using CosmosDBFunction.Infrastructure.CosmosDbData.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CosmosDBFunction.Infrastructure.CosmosDbData.Repository.Base
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>, IContainerContext<T> where T : EntityBase
    {

        private readonly ICosmosDbContainerFactory _cosmosDbContainerFactory;
        private readonly Microsoft.Azure.Cosmos.Container _container;

        public abstract string ContainerName { get; }
        public abstract PartitionKey ResolvePartitionKey(string entityId);

        public virtual string GenerateId(T entity) => Guid.NewGuid().ToString();

        public RepositoryBase(ICosmosDbContainerFactory cosmosDbContainerFactory)
        {
            this._cosmosDbContainerFactory = cosmosDbContainerFactory ?? throw new ArgumentNullException(nameof(ICosmosDbContainerFactory));
            this._container = this._cosmosDbContainerFactory.GetContainer(ContainerName)._container;
        }

        public async Task<IEnumerable<T>> GetAllItemAsync()
        {
            try
            {
                List<T> items = new List<T>();
                FeedIterator<T> feedIterator =  _container.GetItemQueryIterator<T>();
                while(feedIterator.HasMoreResults)
                {
                    items.AddRange(await feedIterator.ReadNextAsync());
                }
                return items;
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task<T> GetByIdAsync(string id)
        {
            try
            {
                var item = await _container.ReadItemAsync<T>(id, ResolvePartitionKey(id));
                return item;
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            try
            {
                entity.Id = GenerateId(entity);
                var item = await _container.CreateItemAsync(entity);
                return item;
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new EntityAlreadyExistsException();
                }

                throw;
            }
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                await _container.ReplaceItemAsync<T>(entity, entity.Id);
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                await _container.DeleteItemAsync<T>(entity.Id, ResolvePartitionKey(entity.Id));
            }
            catch (CosmosException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new EntityNotFoundException();
                }

                throw;
            }
        }
    }
}
