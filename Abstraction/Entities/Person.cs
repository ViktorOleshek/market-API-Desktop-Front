using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("Person")]
public class Person
    : BaseEntity
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
