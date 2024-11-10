using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class PersonRepository : AbstractRepository<Person>, IPersonRepository
    {
        public PersonRepository(TradeMarketDbContext context)
            : base(context)
        {
            ArgumentNullException.ThrowIfNull(context);
        }
    }
}
