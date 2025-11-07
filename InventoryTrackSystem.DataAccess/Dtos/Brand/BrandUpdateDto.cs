using System.ComponentModel.DataAnnotations;
using InventoryTrackSystem.Core.Utilities.Constants;

namespace InventoryTrackSystem.Model.Dtos.Brand
{
    public class BrandUpdateDto
    {
        [Required(ErrorMessage = Messages.IdRequired)]
        public long Id { get; set; }

        [Required(ErrorMessage = Messages.BrandNameRequired)]
        [StringLength(120, ErrorMessage = Messages.BrandNameMaxLength)]
        public string Name { get; set; } = null!;

        [StringLength(500, ErrorMessage = Messages.BrandDescMaxLength)]
        public string? Desc { get; set; }
        public DateTimeOffset? UpdatedDate = DateTimeOffset.UtcNow;
    }
}
