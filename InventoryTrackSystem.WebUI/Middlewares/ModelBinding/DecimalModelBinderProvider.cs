using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace InventoryTrackSystem.WebUI.Middlewares.ModelBinding
{
    public class DecimalModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var modelType = context.Metadata.UnderlyingOrModelType;
            if (modelType == typeof(decimal) || modelType == typeof(decimal?))
            {
                return new BinderTypeModelBinder(typeof(InvariantDecimalModelBinder));
            }

            return null;
        }
    }
}
