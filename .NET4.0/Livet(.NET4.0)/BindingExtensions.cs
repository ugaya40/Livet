using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Livet
{
    public static class BindingExtensions
    {
        public static Binding Clone(this Binding binding, object source = null)
        {
            var result = new Binding();
            if (source != null)
            {
                result.Source = source;
            }
            result.AsyncState = binding.AsyncState;
            result.BindingGroupName = binding.BindingGroupName;
            result.BindsDirectlyToSource = binding.BindsDirectlyToSource;
            result.Converter = binding.Converter;
            result.ConverterCulture = binding.ConverterCulture;
            result.ConverterParameter = binding.ConverterParameter;

            if (result.Source != null && result.RelativeSource != null)
            {
                result.ElementName = binding.ElementName;
            }

            result.FallbackValue = binding.FallbackValue;
            result.IsAsync = binding.IsAsync;
            result.Mode = binding.Mode;
            result.NotifyOnSourceUpdated = binding.NotifyOnSourceUpdated;
            result.NotifyOnTargetUpdated = binding.NotifyOnTargetUpdated;
            result.NotifyOnValidationError = binding.NotifyOnValidationError;
            result.Path = binding.Path;

            if (result.Source != null && result.ElementName != null)
            {
                result.RelativeSource = binding.RelativeSource;
            }

            result.StringFormat = binding.StringFormat;
            result.TargetNullValue = binding.TargetNullValue;
            result.UpdateSourceExceptionFilter = binding.UpdateSourceExceptionFilter;
            result.UpdateSourceTrigger = binding.UpdateSourceTrigger;
            result.ValidatesOnDataErrors = binding.ValidatesOnDataErrors;
            result.ValidatesOnExceptions = binding.ValidatesOnExceptions;
            result.XPath = binding.XPath;

            foreach (var validationRule in binding.ValidationRules)
            {
                result.ValidationRules.Add(validationRule);
            }

            return result;
        }

        public static MultiBinding Clone(this MultiBinding multiBinding, object source = null)
        {
            var result = new MultiBinding();
            result.BindingGroupName = multiBinding.BindingGroupName;
            result.Converter = multiBinding.Converter;
            result.ConverterCulture = multiBinding.ConverterCulture;
            result.ConverterParameter = multiBinding.ConverterParameter;
            result.FallbackValue = multiBinding.FallbackValue;
            result.Mode = multiBinding.Mode;
            result.NotifyOnSourceUpdated = multiBinding.NotifyOnSourceUpdated;
            result.NotifyOnTargetUpdated = multiBinding.NotifyOnTargetUpdated;
            result.NotifyOnValidationError = multiBinding.NotifyOnValidationError;
            result.StringFormat = multiBinding.StringFormat;
            result.TargetNullValue = multiBinding.TargetNullValue;
            result.UpdateSourceExceptionFilter = multiBinding.UpdateSourceExceptionFilter;
            result.UpdateSourceTrigger = multiBinding.UpdateSourceTrigger;
            result.ValidatesOnDataErrors = multiBinding.ValidatesOnDataErrors;
            result.ValidatesOnExceptions = multiBinding.ValidatesOnDataErrors;

            foreach (var validationRule in multiBinding.ValidationRules)
            {
                result.ValidationRules.Add(validationRule);
            }

            foreach (var childBinding in multiBinding.Bindings)
            {
                result.Bindings.Add(childBinding.Clone(source));
            }

            return result;
        }

        public static PriorityBinding Clone(this PriorityBinding priorityBinding, object source = null)
        {
            var result = new PriorityBinding();
            result.BindingGroupName = priorityBinding.BindingGroupName;
            result.FallbackValue = priorityBinding.FallbackValue;
            result.StringFormat = priorityBinding.StringFormat;
            result.TargetNullValue = priorityBinding.TargetNullValue;

            foreach (var childBinding in priorityBinding.Bindings)
            {
                result.Bindings.Add(childBinding.Clone(source));
            }
            return result;
        }

        public static BindingBase Clone(this BindingBase binding, object source = null)
        {
            if (binding is Binding) return ((Binding)binding).Clone(source);
            if (binding is MultiBinding) return ((MultiBinding)binding).Clone(source);
            if (binding is PriorityBinding) return ((PriorityBinding)binding).Clone(source);

            throw new NotSupportedException();
        }
    }
}
