using System.Windows;
using System.Windows.Interactivity;

using Livet.Messaging;
using System.ComponentModel;
using System.Windows.Data;

namespace Livet.Behaviors.Messaging
{
    /// <summary>
    /// ViewModelからの相互作用メッセージに対応するアクションの基底抽象クラスです<br/>
    /// 独自のアクションを定義する場合はこのクラスを継承してください。
    /// </summary>
    /// <typeparam name="T">このアクションがアタッチ可能な型を示します。</typeparam>
    [System.Windows.Markup.ContentProperty("DirectInteractionMessage")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
    public abstract class InteractionMessageAction<T> : TriggerAction<T> where T : DependencyObject
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            if (DirectInteractionMessage == null) return;

            var frameworkElement = this.AssociatedObject as FrameworkElement;
            if (frameworkElement == null) return;

            frameworkElement.DataContextChanged += (sender, e) => DirectInteractionMessage.DataContext = e.NewValue;
            DirectInteractionMessage.DataContext = frameworkElement.DataContext;
        }

        protected override sealed void Invoke(object parameter)
        {
            if ((bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue)) return;

            var message = parameter as InteractionMessage;
            
            if(DirectInteractionMessage != null)
            {
                message = DirectInteractionMessage.Message;
            }

            var window = Window.GetWindow(AssociatedObject);

            if (window == null) return;

            if ((!window.IsActive) && InvokeActionOnlyWhenWindowIsActive)
            {
                return;
            }

            if (message != null)
            {
                InvokeAction(message);

                if (DirectInteractionMessage != null)
                {
                    DirectInteractionMessage.InvokeCallbacks(message);
                }
            }

        }

        protected abstract void InvokeAction(InteractionMessage message);

        /// <summary>
        /// Viewで直接相互作用メッセージを定義する場合に使用する、DirectInteractionMessageを指定、または取得します。
        /// </summary>
        public DirectInteractionMessage DirectInteractionMessage
        {
            get { return (DirectInteractionMessage)GetValue(DirectInteractionMessageProperty); }
            set { SetValue(DirectInteractionMessageProperty, value);}
        }

        // Using a DependencyProperty as the backing store for DirectInteractionMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectInteractionMessageProperty =
            DependencyProperty.Register("DirectInteractionMessage", typeof(DirectInteractionMessage), typeof(InteractionMessageAction<T>), new PropertyMetadata());


        /// <summary>
        /// Windowがアクティブな時のみアクションを実行するかどうかを指定、または取得します。初期値はtrueです。
        /// </summary>
        public bool InvokeActionOnlyWhenWindowIsActive
        {
            get { return (bool)GetValue(InvokeActionOnlyWhenWindowIsActiveProperty); }
            set { SetValue(InvokeActionOnlyWhenWindowIsActiveProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InvokeActionOnlyWhenWindowIsActive.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvokeActionOnlyWhenWindowIsActiveProperty =
            DependencyProperty.Register("InvokeActionOnlyWhenWindowIsActive", typeof(bool), typeof(InteractionMessageAction<T>), new PropertyMetadata(true));

        
    }
}
