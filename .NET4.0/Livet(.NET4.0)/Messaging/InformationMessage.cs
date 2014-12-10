using System.Windows;

namespace Livet.Messaging
{
    /// <summary>
    /// 情報をViewに通知するためのメッセージです。
    /// </summary>
    public class InformationMessage : InteractionMessage
    {
        public InformationMessage()
        {
        }

        /// <summary>
        /// 表示するメッセージ・キャプション・メッセージキーを指定して、新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="caption">キャプション</param>
        /// <param name="messageKey">メッセージキー</param>
        public InformationMessage(string text, string caption, string messageKey) : base(messageKey)
        {
            Text = text;
            Caption = caption;
            MessageKey = messageKey;
        }

        /// <summary>
        /// 表示するメッセージ・キャプション・メッセージボックスイメージ・メッセージキーを指定して、新しい相互作用メッセージのインスタンスを生成します。
        /// </summary>
        /// <param name="text">表示するメッセージ</param>
        /// <param name="caption">キャプション</param>
        /// <param name="image">メッセージボックスイメージ</param>
        /// <param name="messageKey">メッセージキー</param>
        public InformationMessage(string text, string caption, MessageBoxImage image, string messageKey)
            : base(messageKey)
        {
            Text = text;
            Caption = caption;
            MessageKey = messageKey;
            Image = image;
        }

        /// <summary>
        /// 表示するメッセージを指定、または取得します。
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(InformationMessage), new PropertyMetadata(null));

        /// <summary>
        /// キャプションを指定、または取得します。
        /// </summary>
        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(InformationMessage), new PropertyMetadata(null));


        /// <summary>
        /// メッセージボックスイメージを指定、または取得します。
        /// </summary>
        public MessageBoxImage Image
        {
            get { return (MessageBoxImage)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Image.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(MessageBoxImage), typeof(InformationMessage), new PropertyMetadata());

        
        
    }
}