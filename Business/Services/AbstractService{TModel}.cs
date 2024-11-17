using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public abstract class AbstractService<TModel, TEntity>
        where TModel : BaseModel
        where TEntity : class, IBaseEntity
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IRepository<TEntity> repository;

        protected AbstractService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repository)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.repository = repository;
        }

        protected IUnitOfWork UnitOfWork => this.unitOfWork;

        protected IMapper Mapper => this.mapper;

        public virtual async Task AddAsync(TModel model)
        {
            this.Validation(model);
            var entity = this.Mapper.Map<TEntity>(model);
            await this.repository.AddAsync(entity);
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            this.Validation(model);
            var entity = this.Mapper.Map<TEntity>(model);
            await Task.Run(() => this.repository.Update(entity));
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task DeleteAsync(int modelId)
        {
            await this.repository.DeleteByIdAsync(modelId);
            await this.UnitOfWork.SaveAsync();
        }

        protected abstract void Validation(TModel model);
    }
}
