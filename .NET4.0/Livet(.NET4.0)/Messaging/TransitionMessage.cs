using System.Windows;
using System;

namespace Livet.Messaging
{
    /// <summary>
    /// 画面遷移アクション用の相互作用メッセージです。
    /// </summary>
    [System.Windows.Markup.ContentProperty("TransitionViewModel")]
    public class TransitionMessage : ResponsiveInteractionMessage<bool?>
    {
        public TransitionMessage()
        {
        }

        /// <summary>
        /// メッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="messageKey">メッセージキー</param>
        public TransitionMessage(string messageKey) : base(messageKey) { }

        /// <summary>
        /// 新しいWindowのDataContextに設定するViewModelとメッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        /// <param name="messageKey">メッセージキー</param>
        public TransitionMessage(ViewModel transitionViewModel, string messageKey)
            : this(null, transitionViewModel, TransitionMode.UnKnown, messageKey) { }

        /// <summary>
        /// 新しいWindowのDataContextに設定するViewModelと画面遷移モードとメッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        /// <param name="mode">画面遷移の方法を決定するTransitionMode列挙体。初期値はUnKnownです。</param>
        /// <param name="messageKey">メッセージキー</param>
        public TransitionMessage(ViewModel transitionViewModel, TransitionMode mode, string messageKey)
            : this(null, transitionViewModel, mode, messageKey) { }

        /// <summary>
        /// 新しいWindowの型と新しいWindowに設定するViewModel、画面遷移モードとメッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="windowType">新しいWindowの型</param>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        /// <param name="mode">画面遷移の方法を決定するTransitionMode列挙体。初期値はUnKnownです。</param>
        /// <param name="messageKey">メッセージキー</param>
        public TransitionMessage(Type windowType, ViewModel transitionViewModel, TransitionMode mode, string messageKey)
            :base(messageKey)
        {
            Mode = mode;
            TransitionViewModel = transitionViewModel;

            if (windowType != null)
            {
                if (!windowType.IsSubclassOf(typeof(Window)))
                {
                    throw new ArgumentException("Windowの派生クラスを指定してください。", "windowType");
                }
            }

            WindowType = windowType;
        }

        /// <summary>
        /// 新しいWindowのDataContextに設定するViewModelを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        public TransitionMessage(ViewModel transitionViewModel)
            : this(null, transitionViewModel, TransitionMode.UnKnown, null) { }

        /// <summary>
        /// 新しいWindowのDataContextに設定するViewModelと画面遷移モードを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        /// <param name="mode">画面遷移の方法を決定するTransitionMode列挙体。初期値はUnKnownです。</param>
        public TransitionMessage(ViewModel transitionViewModel, TransitionMode mode)
            : this(null, transitionViewModel, mode, null) { }

        /// <summary>
        /// 新しいWindowの型と新しいWindowに設定するViewModel、画面遷移モードとメッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="windowType">新しいWindowの型</param>
        /// <param name="transitionViewModel">新しいWindowのDataContextに設定するViewModel</param>
        /// <param name="mode">画面遷移の方法を決定するTransitionMode列挙体。初期値はUnKnownです。</param>
        public TransitionMessage(Type windowType, ViewModel transitionViewModel, TransitionMode mode)
            : this(windowType,transitionViewModel,mode,null) { }

        /// <summary>
        /// 新しいWindowのDataContextに設定するViewModelを指定、または取得します。
        /// </summary>
        public ViewModel TransitionViewModel
        {
            get { return (ViewModel)GetValue(TransitionViewModelProperty); }
            set { SetValue(TransitionViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TransitionViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TransitionViewModelProperty =
            DependencyProperty.Register("TransitionViewModel", typeof(ViewModel), typeof(TransitionMessage), new PropertyMetadata(null));


        /// <summary>
        /// 新しいWindowの表示方法を決定するTransitionModeを指定、または取得します。<br/>
        /// 初期値はUnKnownです。
        /// </summary>
        public TransitionMode Mode
        {
            get { return (TransitionMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(TransitionMode), typeof(TransitionMessage), new PropertyMetadata(TransitionMode.UnKnown));


        /// <summary>
        /// 新しいWindowの型を指定、または取得します。
        /// </summary>
        public Type WindowType
        {
            get { return (Type)GetValue(WindowTypeProperty); }
            set { SetValue(WindowTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WindowType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WindowTypeProperty =
            DependencyProperty.Register("WindowType", typeof(Type), typeof(TransitionMessage), new PropertyMetadata(null));

        
    }
}
