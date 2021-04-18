﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace Framework.UI.MVC.ModelBinders
{
    public class DateTimeBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
                return Task.CompletedTask;

            var value = valueProviderResult.FirstValue;

            try
            {
                var actualValue = DateTime.Parse(value, CultureInfo.CurrentCulture, DateTimeStyles.None);
                bindingContext.Result = ModelBindingResult.Success(actualValue);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                var type = bindingContext.ModelType;
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    bindingContext.Result = ModelBindingResult.Success((DateTime?)null);
                    return Task.CompletedTask;
                }


                var isFormatException = ex is FormatException;
                if (!isFormatException && ex.InnerException != null)
                {
                    ex = ExceptionDispatchInfo.Capture(ex.InnerException).SourceException;
                }

                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    ex,
                    bindingContext.ModelMetadata);

                return Task.CompletedTask;
            }
        }
    }
}
