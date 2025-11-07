using InventoryTrackSystem.Core.Utilities.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace InventoryTrackSystem.Model.Dtos.Product
{
    [PricePositiveIfHasValue("Price")]
    public class ProductCreateDto
    {
        [Required(ErrorMessage = Messages.ProductNameRequired)]
        [StringLength(100, ErrorMessage = Messages.ProductNameMaxLength)]
        [NotWhitespaceIfNotEmpty(ErrorMessage = Messages.ProductNameCannotBeWhitespace)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = Messages.ProductCodeRequired)]
        [StringLength(50, ErrorMessage = Messages.ProductCodeMaxLength)]
        [RegexIfNotEmpty(@"^[A-Za-z0-9._-]+$", ErrorMessage = Messages.ProductCodeInvalidChars)]
        public string Code { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = Messages.StockCannotBeNegative)]
        public int Stock { get; set; }

        [StringLength(500, ErrorMessage = Messages.DescriptionMaxLength)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = Messages.PriceMustBePositive)]
        [NonNegativeDecimalIfHasValue(ErrorMessage = Messages.PriceCannotBeNegative)]
        public decimal Price { get; set; }

        [RangeIfHasValue(1, long.MaxValue, ErrorMessage = Messages.SelectValidBrand)]
        public long BrandId { get; set; }

        public DateTimeOffset? CreatedDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? UpdatedDate { get; set; } = DateTimeOffset.UtcNow;
    }

    // ---------- Custom Attributes ----------

    /// Boş değilse regex’e uymalı
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RegexIfNotEmptyAttribute : ValidationAttribute
    {
        private readonly Regex _regex;
        public RegexIfNotEmptyAttribute(string pattern)
        {
            _regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is null) return ValidationResult.Success;
            if (value is string s && string.IsNullOrEmpty(s)) return ValidationResult.Success;
            if (value is string s2 && _regex.IsMatch(s2)) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? Messages.InvalidFormat);
        }
    }

    /// Boş değilse yalnızca boşluklardan oluşamaz
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NotWhitespaceIfNotEmptyAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is null) return ValidationResult.Success;
            if (value is string s && string.IsNullOrEmpty(s)) return ValidationResult.Success;
            if (value is string s2 && !string.IsNullOrWhiteSpace(s2)) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? Messages.ValueCannotBeWhitespace);
        }
    }

    /// Nullable decimal negatif olamaz
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class NonNegativeDecimalIfHasValueAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is null) return ValidationResult.Success;
            if (value is decimal d && d >= 0) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? Messages.ValueCannotBeNegative);
        }
    }

    /// long değer varsa range kontrolü yapılır
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RangeIfHasValueAttribute : ValidationAttribute
    {
        public long Min { get; }
        public long Max { get; }
        public RangeIfHasValueAttribute(long min, long max)
        {
            Min = min; Max = max;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext _)
        {
            if (value is null) return ValidationResult.Success;
            if (value is long lv && lv >= Min && lv <= Max) return ValidationResult.Success;
            return new ValidationResult(ErrorMessage ?? Messages.ValueOutOfRange);
        }
    }

    /// Fiyat varsa sıfırdan büyük olmalı
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class PricePositiveIfHasValueAttribute : ValidationAttribute
    {
        public string PriceProperty { get; }

        public PricePositiveIfHasValueAttribute(string priceProperty)
        {
            PriceProperty = priceProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is null) return ValidationResult.Success;

            var priceProp = ctx.ObjectType.GetProperty(PriceProperty);
            if (priceProp == null) return ValidationResult.Success;

            var price = priceProp.GetValue(value) as decimal?;
            if (price is null || price.Value > 0) return ValidationResult.Success;

            return new ValidationResult(ErrorMessage ?? Messages.PriceMustBePositive);
        }
    }
}
