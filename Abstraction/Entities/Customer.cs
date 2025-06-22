using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("Customer")]
public class Customer
    : BaseEntity
{
    public Customer()
        : base()
    {
        this.Person = new Person();
    }

    public Customer(int id)
        : base(id)
    {
        this.Person = new Person();
    }

    public int PersonId { get; set; }

    public int DiscountValue { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<Receipt> Receipts { get; init; }
}
