using System.ComponentModel.DataAnnotations;

namespace RetailApp.Dtos
{
    public class ProductDto
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        [Range(0, 10000, ErrorMessage = "Price cannot exceed $10,000.")]
        public decimal Price { get; set; }
    }
}
