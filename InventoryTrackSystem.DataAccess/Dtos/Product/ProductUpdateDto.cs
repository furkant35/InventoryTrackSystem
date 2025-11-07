namespace InventoryTrackSystem.Model.Dtos.Product
{
    public class ProductUpdateDto : ProductCreateDto
    {
        public long Id { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
