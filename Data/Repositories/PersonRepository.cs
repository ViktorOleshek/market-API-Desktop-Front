using System;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;

namespace Data.Repositories
{
    public class PersonRepository : AbstractRepository<IPerson>, IPersonRepository
    {
        public PersonRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }

        public override IPerson CreateEntity()
        {
            return new Person();
        }
    }
}
