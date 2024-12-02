using Abstraction.IEntities;

namespace Abstraction.IRepositories
{
    public interface IRepository<TEntity>
        where TEntity : IBaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task AddAsync(TEntity entity);

        void Delete(TEntity entity);

        Task DeleteByIdAsync(int id);

        void Update(TEntity entity);

        TEntity CreateEntity();
    }
}
