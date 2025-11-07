using InventoryTrackSystem.Core.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace InventoryTrackSystem.Model.Dtos.User
{
    public class UserCreateDto : IValidatableObject
    {
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

        [Required(ErrorMessage = Messages.PasswordRequired)]
        [MinLength(8, ErrorMessage = Messages.PasswordMinLength)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = Messages.PasswordComplexity)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.PasswordConfirmRequired)]
        [Compare("Password", ErrorMessage = Messages.PasswordsNotMatch)]
        public string ConfirmPassword { get; set; } = string.Empty;

        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedDate { get; set; } = DateTimeOffset.UtcNow;


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Password))
            {
                var fullName = $"{Name}{Surname}".ToLower();
                if (Password.ToLower().Contains(fullName))
                {
                    yield return new ValidationResult(
                        Messages.PasswordContainsName,
                        new[] { nameof(Password) });
                }
            }
        }
    }
}
