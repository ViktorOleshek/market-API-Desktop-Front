using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services
{
    public abstract class AbstractService<TModel, TEntities>
        where TModel : BaseModel
        where TEntities : BaseEntity
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        protected AbstractService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        protected IUnitOfWork UnitOfWork => this.unitOfWork;

        protected IMapper Mapper => this.mapper;

        public virtual async Task AddAsync(TModel model)
        {
            this.Validation(model);
            var entity = this.Mapper.Map<TEntities>(model);
            await this.GetRepository().AddAsync(entity);
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            this.Validation(model);
            var entity = this.Mapper.Map<TEntities>(model);
            await Task.Run(() => this.GetRepository().Update(entity));
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task DeleteAsync(int modelId)
        {
            await this.GetRepository().DeleteByIdAsync(modelId);
            await this.UnitOfWork.SaveAsync();
        }

        protected abstract IRepository<TEntities> GetRepository();

        protected abstract void Validation(TModel model);
    }
}
