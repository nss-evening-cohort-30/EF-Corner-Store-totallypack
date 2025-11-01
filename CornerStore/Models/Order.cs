using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CornerStore.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public int CashierId { get; set; }

    public Cashier Cashier { get; set; }

    [NotMapped]
    public decimal Total
    {
        get
        {
            return OrderProducts?.Sum(op => op.Product?.Price * op.Quantity ?? 0) ?? 0;
        }
    }

    public DateTime? PaidOnDate { get; set; }

    public List<OrderProduct> OrderProducts { get; set; }
}
