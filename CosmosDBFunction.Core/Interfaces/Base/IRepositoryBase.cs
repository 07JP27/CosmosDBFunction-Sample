using CosmosDBFunction.Core.Entities.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosDBFunction.Core.Interfaces.Base
{
    public interface IRepositoryBase<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllItemAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
