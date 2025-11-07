using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace InventoryTrackSystem.WebUI.Middlewares.ModelBinding
{
    public class InvariantDecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var rawValue = valueResult.FirstValue?.Trim();
            if (string.IsNullOrWhiteSpace(rawValue))
                return Task.CompletedTask;

            // "." yerine "," kullanılması durumunu düzelt
            var normalized = rawValue.Replace(" ", "").Replace(".", ",");

            if (decimal.TryParse(normalized, NumberStyles.Any, new CultureInfo("tr-TR"), out var parsedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(parsedValue);
            }
            else if (decimal.TryParse(rawValue, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(parsedValue);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Geçerli bir sayı giriniz.");
            }

            return Task.CompletedTask;
        }
    }
}
