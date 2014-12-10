using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Livet.Messaging
{
    public class GenericResponsiveInteractionMessage<TValue,TResponse> : ResponsiveInteractionMessage<TResponse>
    {

        public GenericResponsiveInteractionMessage(TValue value, string messageKey) : base(messageKey)
        {
            Value = value;
        }

        public GenericResponsiveInteractionMessage(TValue value) :this(value,null)
        {
        }

        public TValue Value
        {
            get { return (TValue)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TValue), typeof(GenericResponsiveInteractionMessage<TValue, TResponse>), new PropertyMetadata(defaultValue: default(TValue)));
    }
}
