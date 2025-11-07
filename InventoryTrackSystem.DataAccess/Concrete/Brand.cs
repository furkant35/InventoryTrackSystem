using InventoryTrackSystem.Model.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace InventoryTrackSystem.Model.Concrete
{
    public class Brand : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Desc { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
