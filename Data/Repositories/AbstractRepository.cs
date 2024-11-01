using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public abstract class AbstractRepository<TEntity>
        where TEntity : BaseEntity
    {
        protected AbstractRepository(TradeMarketDbContext context)
        {
            this.Context = context;
        }

        protected TradeMarketDbContext Context { get; }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this.Context.Set<TEntity>().ToListAsync();
        }

        public Task<TEntity> GetByIdAsync(int id) =>
            this.Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public Task AddAsync(TEntity entity) =>
            this.Context.AddAsync(entity).AsTask();

        public void Delete(TEntity entity) =>
            this.Context.Remove(entity);

        public virtual async Task DeleteByIdAsync(int id)
        {
            if (id > 0)
            {
                var entity = await this.GetByIdAsync(id);
                if (entity != null)
                {
                    this.Delete(entity);
                }
            }
        }

        public void Update(TEntity entity) =>
            this.Context.Update(entity);
    }
}
