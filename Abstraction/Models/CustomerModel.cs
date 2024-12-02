using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class CustomerModel : BaseModel
    {
        public CustomerModel()
            : base()
        {
        }

        public CustomerModel(int id, string name, string surname, DateTime birthDate, int discountValue)
            : base(id)
        {
            this.Name = name;
            this.Surname = surname;
            this.BirthDate = birthDate;
            this.DiscountValue = discountValue;
        }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime BirthDate { get; set; }

        public int DiscountValue { get; set; }

        public byte []? Photo { get; set; }

        public virtual ICollection<int> ReceiptsIds { get; set; }
    }
}
