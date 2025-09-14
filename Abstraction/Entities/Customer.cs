using System.ComponentModel.DataAnnotations.Schema;

namespace Abstraction.Entities;

[Table("Customer")]
public class Customer
    : BaseEntity
{
    public Customer()
        : base()
    {
    }

    public Customer(int id)
        : base(id)
    {
    }

    public int PersonId { get; set; }

    public int DiscountValue { get; set; }

    public virtual Person Person { get; set; }
    public virtual ICollection<Receipt> Receipts { get; init; }
}
