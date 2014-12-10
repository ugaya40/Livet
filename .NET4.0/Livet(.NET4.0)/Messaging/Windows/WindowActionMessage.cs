using System.Windows;

namespace Livet.Messaging.Windows
{
    /// <summary>
    /// Windowを最大化・最小化・閉じる・通常化させるための相互作用メッセージです。
    /// </summary>
    public class WindowActionMessage : InteractionMessage
    {
        public WindowActionMessage()
        {
        }

        /// <summary>
        /// メッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="messageKey">メッセージキー</param>
        public WindowActionMessage(string messageKey)
            : base(messageKey) { }

        /// <summary>
        /// メッセージキーとWindowが遷移すべき状態を定義して、新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="action">Windowが遷移すべき状態を表すWindowAction列挙体</param>
        /// <param name="messageKey">メッセージキー</param>
        public WindowActionMessage(WindowAction action,string messageKey)
            :this(messageKey)
        {
            Action = action;
        }

        /// <summary>
        /// Windowが遷移すべき状態を定義して、新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="action">Windowが遷移すべき状態を表すWindowAction列挙体</param>
        public WindowActionMessage(WindowAction action)
            : this(action,null) { }


        /// <summary>
        /// Windowが遷移すべき状態を表すWindowAction列挙体を指定、または取得します。
        /// </summary>
        public WindowAction Action
        {
            get { return (WindowAction)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Action.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ActionProperty =
            DependencyProperty.Register("Action", typeof(WindowAction), typeof(WindowActionMessage), new PropertyMetadata());

        
    }
}
