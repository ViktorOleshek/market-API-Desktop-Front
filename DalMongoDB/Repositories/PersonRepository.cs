using Abstraction.IEntities;
using Abstraction.IRepositories;
using DalMongoDB.Entities;
using MongoDB.Driver;

namespace DalMongoDB.Repositories
{
    public class PersonRepository : AbstractRepository<IPerson>, IPersonRepository
    {
        public PersonRepository(IMongoDatabase database)
            : base(database, "Persons")
        {
            ArgumentNullException.ThrowIfNull(database);
        }

        public IPerson CreateEntity()
        {
            return new Person();
        }
    }
}
