using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IRepositories;
using AutoMapper;
using Data.Data;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class PersonRepository : AbstractRepository<Person>, IPersonRepository
    {
        public PersonRepository(TradeMarketDbContext context, IMapper mapper)
            : base(context, mapper)
        {
            ArgumentNullException.ThrowIfNull(context);
        }
    }
}
