using Abstraction.Entities;
using Abstraction.IRepositories;
using Data.Data;
using System;

namespace Data.Repositories;

public class PersonRepository
    : AbstractRepository<Person>, IPersonRepository
{
    public PersonRepository(TradeMarketDbContext context)
        : base(context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}
