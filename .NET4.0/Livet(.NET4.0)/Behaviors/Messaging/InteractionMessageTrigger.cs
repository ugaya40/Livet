using System;
using System.Windows.Interactivity;
using System.Windows;
using Livet.Messaging;
using Livet.EventListeners.WeakEvents;

namespace Livet.Behaviors.Messaging
{
    /// <summary>
    /// ViewModelからの相互作用メッセージを受信し、アクションを実行します。
    /// </summary>
    public class InteractionMessageTrigger : TriggerBase<FrameworkElement>, IDisposable
    {
        private LivetWeakEventListener<EventHandler<InteractionMessageRaisedEventArgs>, InteractionMessageRaisedEventArgs> _listener;
        private bool _disposed;
        private bool _loaded = true;

        /// <summary>
        /// ViewModelのMessengerを指定、または取得します。
        /// </summary>
        public InteractionMessenger Messenger
        {
            get { return (InteractionMessenger)GetValue(MessengerProperty); }
            set { SetValue(MessengerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Messenger.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessengerProperty =
            DependencyProperty.Register("Messenger", typeof(InteractionMessenger), typeof(InteractionMessageTrigger),
                                        new PropertyMetadata(MessengerChanged));


        /// <summary>
        /// アタッチされたオブジェクトがロードされている期間(Loaded~Unloaded)だけActionを実行するかどうかを指定、または取得します。デフォルトはfalseです。
        /// </summary>
        public bool InvokeActionsOnlyWhileAttatchedObjectLoaded
        {
            get { return (bool)GetValue(InvokeActionsOnlyWhileAttatchedObjectLoadedProperty); }
            set { SetValue(InvokeActionsOnlyWhileAttatchedObjectLoadedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FireActionsOnlyWhileAttatchedObjectLoading.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InvokeActionsOnlyWhileAttatchedObjectLoadedProperty =
            DependencyProperty.Register("InvokeActionsOnlyWhileAttatchedObjectLoaded", typeof(bool), typeof(InteractionMessageTrigger), new PropertyMetadata(false));



        /// <summary>
        /// このトリガーが有効かどうかを指定、または取得します。デフォルトはtrueです。
        /// </summary>
        public bool IsEnable
        {
            get { return (bool)GetValue(IsEnableProperty); }
            set { SetValue(IsEnableProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnable.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.Register("IsEnable", typeof(bool), typeof(InteractionMessageTrigger), new PropertyMetadata(true));



        /// <summary>
        /// このトリガーが反応する相互作用メッセージのメッセージキーを指定、または取得します。<br/>
        /// このプロパティが指定されていない場合、このトリガーは全ての相互作用メッセージに反応します。
        /// </summary>
        public string MessageKey
        {
            get;
            set;
        }

        private static void MessengerChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var thisReference = obj as InteractionMessageTrigger;

            if (e.OldValue == e.NewValue)
            {
                return;
            }

            if (e.OldValue != null)
            {
                if (thisReference._listener != null)
                {
                    thisReference._listener.Dispose();
                }
            }

            if (e.NewValue != null && thisReference != null)
            {
                var newMessenger = (InteractionMessenger)e.NewValue;

                thisReference._listener = new LivetWeakEventListener<EventHandler<InteractionMessageRaisedEventArgs>, InteractionMessageRaisedEventArgs>(
                    h => h,
                    h => newMessenger.Raised += h,
                    h => newMessenger.Raised -= h,
                    thisReference.MessageReceived);
            }
        }

        private void MessageReceived(object sender, InteractionMessageRaisedEventArgs e)
        {
            var message = e.Message;

            var checkResult = false;

            Action checkAction = () =>
                {
                    if (!IsEnable) return;

                    if (InvokeActionsOnlyWhileAttatchedObjectLoaded && (!_loaded)) return;

                    if (!(string.IsNullOrEmpty(MessageKey) || MessageKey == message.MessageKey)) return;

                    checkResult = true;
                };

            DoActionOnDispatcher(checkAction);

            if (!checkResult)
            {
                return;
            }

            DoActionOnDispatcher(() => InvokeActions(message));
        }

        private void DoActionOnDispatcher(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.Invoke(action);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded += AssociatedObjectLoaded;
                AssociatedObject.Unloaded += AssociatedObjectUnloaded;
            }
        }

        void AssociatedObjectLoaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
        }

        void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
        {
            _loaded = false;
        }

        protected override void OnDetaching()
        {
            if (Messenger != null && _listener != null)
            {
                _listener.Dispose();
            }

            if (AssociatedObject != null)
            {
                AssociatedObject.Loaded -= AssociatedObjectLoaded;
                AssociatedObject.Unloaded -= AssociatedObjectUnloaded;
            }

            base.OnDetaching();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _listener.Dispose();
            _disposed = true;
        }
    }
}