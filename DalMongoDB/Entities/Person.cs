using System.ComponentModel.DataAnnotations.Schema;
using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    [Table("Person")]
    public class Person : BaseEntity, IPerson
    {
        public Person()
            : base()
        {
        }

        public Person(int id)
            : base(id)
        {
        }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }
    }
}
