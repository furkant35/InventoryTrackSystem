namespace InventoryTrackSystem.Model.Dtos.Brand
{
    public class BrandGetDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Desc { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
