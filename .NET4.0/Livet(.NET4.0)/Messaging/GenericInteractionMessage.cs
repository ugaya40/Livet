using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Livet.Messaging
{
    public class GenericInteractionMessage<T> : InteractionMessage
    {
        public GenericInteractionMessage(T value,string messageKey) : base(messageKey)
        {
            Value = value;
        }

        public GenericInteractionMessage(T value) :this(value,null)
        {
        }

        public T Value
        {
            get { return (T)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(T), typeof(GenericInteractionMessage<T>), new PropertyMetadata(defaultValue: default(T)));

    }
}
