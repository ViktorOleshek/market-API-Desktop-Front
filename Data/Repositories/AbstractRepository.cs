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
            var result = await this.Context.Set<TEntity>().ToListAsync();
            return this.Mapper.Map<IEnumerable<TModel>>(result);
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            var entity = await this.Context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            return this.Mapper.Map<TModel>(entity);
        }

        public async Task AddAsync(TModel model)
        {
            var entity = this.Mapper.Map<TEntity>(model);
            await this.Context.AddAsync(entity);
        }

        public void Delete(TModel model)
        {
            var entity = this.Mapper.Map<TEntity>(model);
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

        public void Update(TModel model)
        {
            var entity = this.Mapper.Map<TEntity>(model);
            this.Context.Update(entity);
        }
    }
}
