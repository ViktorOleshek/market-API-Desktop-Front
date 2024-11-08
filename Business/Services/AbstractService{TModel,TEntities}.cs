using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public abstract class AbstractService<TModel>
        where TModel : BaseModel
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
