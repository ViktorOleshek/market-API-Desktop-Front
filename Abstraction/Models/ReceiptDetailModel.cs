using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Models
{
    public class ReceiptDetailModel : BaseModel
    {
        public ReceiptDetailModel()
            : base()
        {
        }

        public ReceiptDetailModel(int id, int receiptId, int productId, decimal discountUnitPrice, decimal unitPrice, int quantity)
            : base(id)
        {
            this.ReceiptId = receiptId;
            this.ProductId = productId;
            this.DiscountUnitPrice = discountUnitPrice;
            this.UnitPrice = unitPrice;
            this.Quantity = quantity;
        }

        public int ReceiptId { get; set; }

        public int ProductId { get; set; }

        public decimal DiscountUnitPrice { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }
    }
}
