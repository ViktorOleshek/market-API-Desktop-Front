namespace Abstraction.Models;

public class ReceiptModel : BaseModel
{
    public ReceiptModel()
        : base()
    {
    }

    public ReceiptModel(int id, int customerId, DateTime operationDate, bool isCheckedOut)
        : base(id)
    {
        this.CustomerId = customerId;
        this.OperationDate = operationDate;
        this.IsCheckedOut = isCheckedOut;
    }

    public int CustomerId { get; set; }

    public DateTime OperationDate { get; set; }

    public bool IsCheckedOut { get; set; }

    public virtual ICollection<int> ReceiptDetailsIds { get; set; }
}
