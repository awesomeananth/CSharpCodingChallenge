using System.ComponentModel.DataAnnotations;

namespace RetailApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Range(0, 10000, ErrorMessage = "Price cannot exceed $10,000.")]
        public decimal Price { get; set; }

        [StringLength(20)]
        public string Status { get; set; } // e.g., "Active", "Inactive"

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
