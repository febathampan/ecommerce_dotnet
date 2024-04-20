namespace test1app.Models;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

public class ProductView
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(
        150,
        MinimumLength = 2,
        ErrorMessage = "Product Description should have 2-150 charachters"
    )]
    [DisplayName("Product Description")]
    public string Description { get; set; }

    [Required]
    [DisplayName("Price per unit")]
    [DataType(DataType.Currency)]
    [RegularExpression(
        @"^\d{1,7}(\.\d{1,2})?$",
        ErrorMessage = "Accepts only upto 7 digits and 2 decimal places"
    )]
    public decimal PricePerUnit { get; set; }

    [Required]
    [StringLength(
        60,
        MinimumLength = 2,
        ErrorMessage = "Product Name should have 2-60 charachters"
    )]
    [DisplayName("Product Name")]
    public string ProductName { get; set; }

    [Required]
    [RegularExpression(
        @"^[A-Z]{3}-\d{5}-[A-Z]\d$",
        ErrorMessage = "Product code should match the pattern : XXX-11111-X1"
    )]
    [DisplayName("Product Code")]
    public string ProductCode { get; set; }

    [Required]
    [EnumDataType(typeof(Product.Unit), ErrorMessage = "Not a valid unit")]
    public Product.Unit SalesUnit { get; set; }

    [Required(ErrorMessage = "Manufacturing date is required")]
    [DataType(DataType.Date)]
    [DisplayName("Manufacture Date")]
    public required DateTime ManufactureDate { get; set; }

    [Required(ErrorMessage = "Expiry date is required")]
    [DataType(DataType.Date)]
    [DisplayName("Expiry Date")]
    public required DateTime ExpiryDate { get; set; }

    [Required]
    [DisplayName("Available Stock")]
    public required float Stock { get; set; }

    [DisplayName("Image")]
    //Image is made required in the cshtml page. This is done to avoid compulsory image change during edit.
    public IFormFile? Image { get; set; }

    public ProductView() { }
}
