namespace test1app.Models;

using System.ComponentModel.DataAnnotations;

public class Product
{
    public enum Unit
    {
        kg,
        lb,
        litre,
        gallon,
        oz
    }

    [Key]
    public int Id { get; set; }

    public string? Description { get; set; }

    public decimal PricePerUnit { get; set; } = 0;

    public string? ProductName { get; set; } = "Unknown";

    public string? ProductCode { get; set; } = "0";

    public Unit SalesUnit { get; set; }

    public DateTime ManufactureDate { get; set; } = DateTime.Now;

    public DateTime ExpiryDate { get; set; } = DateTime.Now;

    public float Stock { get; set; } = 0;

    public byte[]? Image { get; set; }
}
