using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace test1app.Models;

public class Cart
{
    public enum PaymentStatus
    {
        PAID,
        PENDING,
        FAILED
    }

    [Key]
    public int Id { get; set; }

    public required Product Product { get; set; }
    public User User { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.PENDING;

    public Cart() { }
}
