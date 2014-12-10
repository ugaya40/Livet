using System.Windows;
namespace Livet.Messaging.IO
{
    /// <summary>
    /// ファイルを保存する 用の相互作用メッセージです。
    /// </summary>
    public class SavingFileSelectionMessage : FileSelectionMessage
    {
        public SavingFileSelectionMessage()
        {
        }

        /// <summary>
        /// メッセージキーを指定して新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="messageKey">メッセージキー</param>
        public SavingFileSelectionMessage(string messageKey)
            : base(messageKey)
        {
        }

        /// <summary>
        /// ユーザーが存在しないファイルを指定した場合に、ファイルを作成することを確認するメッセージを表示するかどうかを指定、または取得します。デフォルトはfalseです。
        /// </summary>
        public bool CreatePrompt
        {
            get { return (bool)GetValue(CreatePromptProperty); }
            set { SetValue(CreatePromptProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CreatePrompt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CreatePromptProperty =
            DependencyProperty.Register("CreatePrompt", typeof(bool), typeof(SavingFileSelectionMessage), new PropertyMetadata(false));


        /// <summary>
        /// ユーザーが指定したファイルが存在する場合、上書き確認メッセージを表示するかどうかを指定、または取得します。デフォルトはtrueです。
        /// </summary>
        public bool OverwritePrompt
        {
            get { return (bool)GetValue(OverwritePromptProperty); }
            set { SetValue(OverwritePromptProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OverwritePrompt.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverwritePromptProperty =
            DependencyProperty.Register("OverwritePrompt", typeof(bool), typeof(SavingFileSelectionMessage), new PropertyMetadata(true));

        
        
    }
}