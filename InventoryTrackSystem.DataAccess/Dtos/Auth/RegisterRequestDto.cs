using InventoryTrackSystem.Core.Utilities.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InventoryTrackSystem.Model.Dtos.Auth
{
    public class RegisterRequestDto : IValidatableObject
    {
        [Required(ErrorMessage = Messages.NameRequired)]
        [MaxLength(50, ErrorMessage = Messages.NameMaxLength)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.SurnameRequired)]
        [MaxLength(50, ErrorMessage = Messages.SurnameMaxLength)]
        public string Surname { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.EmailRequired)]
        [EmailAddress(ErrorMessage = Messages.EmailInvalid)]
        [MaxLength(100, ErrorMessage = Messages.EmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = Messages.PhoneInvalid)]
        [MaxLength(11, ErrorMessage = Messages.PhoneMaxLength)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = Messages.PasswordRequired)]
        [MinLength(8, ErrorMessage = Messages.PasswordMinLength)]
        [MaxLength(64, ErrorMessage = Messages.PasswordMaxLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.PasswordConfirmRequired)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = Messages.PasswordsDoNotMatch)]
        public string PasswordConfirm { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!string.IsNullOrWhiteSpace(Email) && !emailRegex.IsMatch(Email))
            {
                yield return new ValidationResult(Messages.EmailInvalid, new[] { nameof(Email) });
            }

            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$");
            if (!string.IsNullOrEmpty(Password) && !passwordRegex.IsMatch(Password))
            {
                yield return new ValidationResult(Messages.PasswordComplexity, new[] { nameof(Password) });
            }

            if (!string.Equals(Password, PasswordConfirm))
            {
                yield return new ValidationResult(Messages.PasswordsDoNotMatch, new[] { nameof(PasswordConfirm) });
            }
        }
    }
}
