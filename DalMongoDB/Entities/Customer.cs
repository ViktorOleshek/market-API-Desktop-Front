using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.IEntities;

namespace DalMongoDB.Entities
{
    [Table("Customer")]
    public class Customer : BaseEntity, ICustomer
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

        IPerson ICustomer.Person
        {
            get => this.Person;
            set => this.Person = (Person)value;
        }

        public virtual ICollection<Receipt> Receipts { get; init; }

        ICollection<IReceipt> ICustomer.Receipts
        {
            get => this.Receipts as ICollection<IReceipt>;
            init => this.Receipts = (ICollection<Receipt>)value;
        }
    }
}
