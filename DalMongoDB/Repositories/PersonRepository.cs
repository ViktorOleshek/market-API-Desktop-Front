using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Data;
using DalMongoDB.Entities;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore;

namespace DalMongoDB.Repositories
{
    public class PersonRepository : AbstractRepository<IPerson>, IPersonRepository
    {
        public PersonRepository(IMongoDatabase database)
            : base(database, "Persons")
        {
            ArgumentNullException.ThrowIfNull(database);
        }
    }
}
