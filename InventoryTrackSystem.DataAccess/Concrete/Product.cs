using System.ComponentModel.DataAnnotations;

namespace InventoryTrackSystem.Model.Concrete
{
    public class Product
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(100)]
        public string Name { get; set; } = null!;
        [Required,MaxLength(50)]
        public string Code { get; set; }
        [Required]
        public int Stock { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public long BrandId { get; set; }
        [Required]
        public Brand Brand { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
