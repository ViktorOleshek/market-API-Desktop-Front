using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class CustomerActivityModel
    {
        public int CustomerId { get; set; }

        public string CustomerName { get; set; }

        public decimal ReceiptSum { get; set; }
    }
}
