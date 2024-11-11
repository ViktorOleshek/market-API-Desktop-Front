using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Abstraction.IServices;
using Abstraction.Models;
using AutoMapper;
using Business.Validation;

namespace Business.Services
{
    public abstract class AbstractService<TModel>
        where TModel : BaseModel
    {
        private readonly IUnitOfWork unitOfWork;

        protected AbstractService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        protected IUnitOfWork UnitOfWork => this.unitOfWork;

        public virtual async Task AddAsync(TModel model)
        {
            this.Validation(model);
            await this.GetRepository().AddAsync(model);
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            this.Validation(model);
            await Task.Run(() => this.GetRepository().Update(model));
            await this.UnitOfWork.SaveAsync();
        }

        public virtual async Task DeleteAsync(int modelId)
        {
            await this.GetRepository().DeleteByIdAsync(modelId);
            await this.UnitOfWork.SaveAsync();
        }

        protected abstract IRepository<TModel> GetRepository();

        protected abstract void Validation(TModel model);
    }
}
