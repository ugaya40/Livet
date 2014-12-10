using Livet.Messaging;
using System.Windows;
using System.Windows.Input;

namespace Livet.Behaviors.Messaging
{
    /// <summary>
    /// Viewから直接相互作用メッセージを定義する際に使用します。
    /// </summary>
    [System.Windows.Markup.ContentProperty("Message")]
    public class DirectInteractionMessage : Freezable
    {
        private MethodBinderWithArgument _callbackMethod = new MethodBinderWithArgument();

        /// <summary>
        /// 相互作用メッセージ(各種InteractionMessage)を指定、または取得します。
        /// </summary>
        public InteractionMessage Message
        {
            get { return (InteractionMessage)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(InteractionMessage), typeof(DirectInteractionMessage), new UIPropertyMetadata(MessageChanged));

        /// <summary>
        /// アクション実行後に実行するコマンドを指定、または取得します<br/>
        /// このプロパティが設定されていた場合、アクションの実行後アクションの実行に使用した相互作用メッセージをパラメータとしてコマンドを呼び出します。
        /// </summary>
        public ICommand CallbackCommand
        {
            get { return (ICommand)GetValue(CallbackCommandProperty); }
            set { SetValue(CallbackCommandProperty, value);}
        }

        // Using a DependencyProperty as the backing store for CallbackCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallbackCommandProperty =
            DependencyProperty.Register("CallbackCommand", typeof(ICommand), typeof(DirectInteractionMessage), new UIPropertyMetadata(null));

        /// <summary>
        /// アクション実行後に実行するメソッドを持つオブジェクトを指定、または取得します<br/>
        /// このプロパティとCallbackMethodNameが設定されていた場合、アクションの実行後アクションの実行に使用した相互作用メッセージをパラメータとしてメソッドを呼び出します。
        /// </summary>
        public object CallbackMethodTarget
        {
            get { return GetValue(CallbackMethodTargetProperty); }
            set { SetValue(CallbackMethodTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CallbackMethodTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallbackMethodTargetProperty =
            DependencyProperty.Register("CallbackMethodTarget", typeof(object), typeof(DirectInteractionMessage), new UIPropertyMetadata(null));

        /// <summary>
        /// アクション実行後に実行するメソッドの名前を指定、または取得します<br/>
        /// このプロパティとCallbackMethodTargetが設定されていた場合、アクションの実行後アクションの実行に使用した相互作用メッセージをパラメータとしてメソッドを呼び出します。
        /// </summary>
        public string CallbackMethodName
        {
            get { return (string)GetValue(CallbackMethodNameProperty); }
            set { SetValue(CallbackMethodNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MethodName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CallbackMethodNameProperty =
            DependencyProperty.Register("CallbackMethodName", typeof(string), typeof(DirectInteractionMessage), new UIPropertyMetadata(null));


        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DataContext.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(object), typeof(DirectInteractionMessage), new PropertyMetadata(DataContextChanged));

        

        internal void InvokeCallbacks(InteractionMessage message)
        {
            if (CallbackCommand != null)
            {
                if (CallbackCommand.CanExecute(message))
                {
                    CallbackCommand.Execute(message);
                }
            }
            if (CallbackMethodTarget != null && CallbackMethodName != null)
            {
                _callbackMethod.Invoke(CallbackMethodTarget, CallbackMethodName, message);
            }
        }

        private static void DataContextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MessageChangedCore(obj, e);
        }

        private static void MessageChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            MessageChangedCore(obj, e);
        }

        private static void MessageChangedCore(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisReference = obj as DirectInteractionMessage;
            if (thisReference == null) return;
            if (thisReference.Message == null) return;
            thisReference.Message.DataContext = thisReference.DataContext;
        }

        protected override Freezable CreateInstanceCore()
        {
            return new DirectInteractionMessage();
        }
    }
}
