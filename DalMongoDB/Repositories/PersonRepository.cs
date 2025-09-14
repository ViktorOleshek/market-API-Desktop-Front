using Abstraction.Entities;
using Abstraction.IRepositories;
using MongoDB.Driver;

namespace DalMongoDB.Repositories;

public class PersonRepository
    : AbstractRepository<Person>, IPersonRepository
{
    public PersonRepository(IMongoDatabase database)
        : base(database, "Persons")
    {
        ArgumentNullException.ThrowIfNull(database);
    }
}
