using System.ComponentModel.DataAnnotations;
using InventoryTrackSystem.Core.Utilities.Constants;

namespace InventoryTrackSystem.Model.Dtos.Brand
{
    public class BrandCreateDto
    {
        [Required(ErrorMessage = Messages.BrandNameRequired)]
        [StringLength(120, ErrorMessage = Messages.BrandNameMaxLength)]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = Messages.BrandDescMaxLength)]
        public string? Desc { get; set; }

        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
    }
}
