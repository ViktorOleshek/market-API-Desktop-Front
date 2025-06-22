using Abstraction.Entities;
using Abstraction.IRepositories;
using Abstraction.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace Business.Services;

public abstract class AbstractService<TModel, TEntity>
       where TModel : BaseModel
       where TEntity : class, IBaseEntity, new()
{
    private readonly IRepository<TEntity> repository;

    protected AbstractService(IUnitOfWork unitOfWork, IMapper mapper, IRepository<TEntity> repository)
    {
        this.UnitOfWork = unitOfWork;
        this.Mapper = mapper;
        this.repository = repository;
    }

    protected IUnitOfWork UnitOfWork { get; }

    protected IMapper Mapper { get; }

    public virtual async Task AddAsync(TModel model)
    {
        this.Validation(model);
        var entity = new TEntity();
        this.Mapper.Map(model, entity);
        await this.repository.AddAsync(entity);
        await this.UnitOfWork.SaveAsync();
    }

    public virtual async Task UpdateAsync(TModel model)
    {
        this.Validation(model);
        var entity = new TEntity();
        this.Mapper.Map(model, entity);
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
