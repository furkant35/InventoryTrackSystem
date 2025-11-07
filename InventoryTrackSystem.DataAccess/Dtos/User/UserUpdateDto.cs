using InventoryTrackSystem.Core.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace InventoryTrackSystem.Model.Dtos.User
{
    public class UserUpdateDto : IValidatableObject
    {
        [Required(ErrorMessage = Messages.IdRequired)]
        public long Id { get; set; }

        [Required(ErrorMessage = Messages.NameRequired)]
        [MaxLength(50, ErrorMessage = Messages.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.SurnameRequired)]
        [MaxLength(50, ErrorMessage = Messages.SurnameMaxLength)]
        public string Surname { get; set; } = string.Empty;

        [MaxLength(11, ErrorMessage = Messages.PhoneMaxLength)]
        [Phone(ErrorMessage = Messages.PhoneInvalid)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = Messages.EmailRequired)]
        [MaxLength(100, ErrorMessage = Messages.EmailMaxLength)]
        [EmailAddress(ErrorMessage = Messages.EmailInvalid)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100, ErrorMessage = Messages.CityMaxLength)]

        public bool IsActive { get; set; } = true;
        public DateTimeOffset? UpdatedDate { get; set; } = DateTimeOffset.UtcNow;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name) && string.IsNullOrWhiteSpace(Surname))
            {
                yield return new ValidationResult(
                    Messages.NameOrSurnameRequired,
                    new[] { nameof(Name), nameof(Surname) });
            }
        }
    }
}
