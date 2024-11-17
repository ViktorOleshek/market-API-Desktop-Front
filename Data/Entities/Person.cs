using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;

namespace Data.Entities
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
