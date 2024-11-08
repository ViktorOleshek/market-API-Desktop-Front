using System.Collections.Generic;
using System.Threading.Tasks;

namespace Abstraction.IRepositories
{
    public interface IRepository<TModel>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        Task<TModel> GetByIdAsync(int id);

        Task AddAsync(TModel entity);

        void Delete(TModel entity);

        Task DeleteByIdAsync(int id);

        void Update(TModel entity);
    }
}
