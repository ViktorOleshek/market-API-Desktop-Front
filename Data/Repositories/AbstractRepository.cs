using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Models;
using AutoMapper;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public abstract class AbstractRepository<TEntity, TModel>
        where TEntity : BaseEntity
        where TModel : BaseModel
    {
        private readonly IMapper mapper;

        protected AbstractRepository(TradeMarketDbContext context, IMapper mapper)
        {
            this.Context = context;
            this.mapper = mapper;
        }

        protected TradeMarketDbContext Context { get; }

        protected IMapper Mapper => this.mapper;

        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await this.Context.Set<TEntity>().ToListAsync();
        }

        public Task<TModel> GetByIdAsync(int id)
        {
            return this.Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task AddAsync(TModel entity)
        {
            return this.Context.AddAsync(entity).AsTask();
        }

        public void Delete(TModel entity)
        {
            this.Context.Remove(entity);
        }

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

        public void Update(TModel entity) =>
            this.Context.Update(entity);
    }
}
