using System;
using System.Windows;
using System.Windows.Data;

namespace Livet.Messaging
{
    /// <summary>
    /// 相互作用メッセージの基底クラスです。<br/>
    /// Viewからのアクション実行後、戻り値情報が必要ない相互作用メッセージを作成する場合はこのクラスを継承して相互作用メッセージを作成します。
    /// </summary>
    public class InteractionMessage : DependencyObject
    {
        public InteractionMessage()
        {
        }

        /// <summary>
        /// メッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="messageKey">メッセージキー</param>
        public InteractionMessage(string messageKey)
        {
            MessageKey = messageKey;
        }

        /// <summary>
        /// メッセージキーを指定、または取得します。
        /// </summary>
        public string MessageKey
        {
            get { return (string)GetValue(MessageKeyProperty); }
            set { SetValue(MessageKeyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MessageKey.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageKeyProperty =
            DependencyProperty.Register("MessageKey", typeof(string), typeof(InteractionMessage), new PropertyMetadata(null));

        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(object), typeof(InteractionMessage), new PropertyMetadata(DataContextChanged));

        private static void DataContextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisReference = obj as InteractionMessage;
            if (thisReference == null) return;

            var locallySetProperties = thisReference.GetLocalValueEnumerator();
            while (locallySetProperties.MoveNext())
            {
                var binding = BindingOperations.GetBinding(thisReference, locallySetProperties.Current.Property);
                if (binding != null)
                {
                    thisReference.ClearValue(locallySetProperties.Current.Property);
                    BindingOperations.SetBinding(thisReference, locallySetProperties.Current.Property, binding.Clone(thisReference.DataContext));
                }
            }
        }
    }

}