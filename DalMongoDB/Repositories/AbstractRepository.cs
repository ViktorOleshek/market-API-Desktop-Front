using System.Linq.Expressions;
using Abstraction.IEntities;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public abstract class AbstractRepository<TEntity>
        where TEntity : class, IBaseEntity
    {
        protected AbstractRepository(IMongoDatabase database, string collectionName)
        {
            this.Collection = database.GetCollection<TEntity>(collectionName);
        }

        protected IMongoCollection<TEntity> Collection { get; }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.Collection.Find(FilterDefinition<TEntity>.Empty).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await this.Collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(TEntity entity)
        {
            await this.Collection.InsertOneAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            this.Collection.DeleteOne(x => x.Id == entity.Id);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await this.Collection.DeleteOneAsync(x => x.Id == id);
        }

        public void Update(TEntity entity)
        {
            this.Collection.ReplaceOne(x => x.Id == entity.Id, entity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await this.Collection.Find(filter).ToListAsync();
        }
    }
}
