using InventoryTrackSystem.Core.Utilities.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InventoryTrackSystem.Model.Dtos.Auth
{
    public class LoginRequestDto : IValidatableObject
    {
        [Required(ErrorMessage = Messages.EmailRequired)]
        [EmailAddress(ErrorMessage = Messages.EmailInvalid)]
        [MaxLength(100, ErrorMessage = Messages.EmailMaxLength)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = Messages.PasswordRequired)]
        [MinLength(8, ErrorMessage = Messages.PasswordMinLength)]
        [MaxLength(64, ErrorMessage = Messages.PasswordMaxLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

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
        }
    }
}
