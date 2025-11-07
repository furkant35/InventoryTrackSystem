
namespace InventoryTrackSystem.Model.Dtos.Product
{
    public class ProductGetDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int Stock { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public long BrandId { get; set; }
        public string BrandName { get; set; } = null!;
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
