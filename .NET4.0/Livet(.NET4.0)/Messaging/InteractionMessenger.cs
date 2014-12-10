using System;
using System.Threading.Tasks;
using System.Threading;
namespace Livet.Messaging
{
    /// <summary>
    /// ViewModelで使用するMessengerクラスです。
    /// </summary>
    public class InteractionMessenger
    {
        /// <summary>
        /// 指定された相互作用メッセージを同期的に送信します。
        /// </summary>
        /// <param name="message">相互作用メッセージ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void Raise(InteractionMessage message)
        {

            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            var threadSafeHandler = Interlocked.CompareExchange(ref Raised, null, null);

            if (threadSafeHandler != null)
            {
                threadSafeHandler(this, new InteractionMessageRaisedEventArgs(message));
            }
        }

        /// <summary>
        /// 指定された、戻り値情報のある相互作用メッセージを同期的に送信します。
        /// </summary>
        /// <typeparam name="T">戻り値情報のある相互作用メッセージの型</typeparam>
        /// <param name="message">戻り値情報のある相互作用メッセージ</param>
        /// <returns>アクション実行後に、戻り情報を含んだ相互作用メッセージ</returns>
        public T GetResponse<T>(T message)
            where T : ResponsiveInteractionMessage
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            var threadSafeHandler = Interlocked.CompareExchange(ref Raised, null, null);

            if (threadSafeHandler != null)
            {
                threadSafeHandler(this, new InteractionMessageRaisedEventArgs(message));
                return message;
            }

            return null;
        }

        /// <summary>
        /// 相互作用メッセージが送信された時に発生するイベントです。
        /// </summary>
        public event EventHandler<InteractionMessageRaisedEventArgs> Raised;

#if NET4
        /// <summary>
        /// 指定された相互作用メッセージを非同期で送信します。
        /// </summary>
        /// <param name="message">相互作用メッセージ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseAsync(InteractionMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            var task = Task.Factory.StartNew(() => Raise(message), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
            task.ContinueWith(t =>
                {
                    var e = new AsyncMessageFailedEventArgs(t.Exception);
                    OnAsyncMessageFailed(e);
                    t.Exception.Handle(ex => true);
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
        }

        /// <summary>
        /// 指定された、戻り値情報のある相互作用メッセージを非同期で送信します。
        /// </summary>
        /// <typeparam name="T">戻り値情報のある相互作用メッセージの型</typeparam>
        /// <param name="message">戻り値情報のある相互作用メッセージ</param>
        /// <param name="callback">コールバック</param>
        public void GetResponseAsync<T>(T message, Action<T> callback)
            where T : ResponsiveInteractionMessage
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            var task = Task<T>.Factory.StartNew(() => GetResponse(message));

            task.ContinueWith(t =>
                {
                    var e = new AsyncMessageFailedEventArgs(t.Exception);
                    OnAsyncMessageFailed(e);
                    t.Exception.Handle(ex => true);
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

            task.ContinueWith(t => callback(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        /// <summary>
        /// 非同期で発信された相互作用メッセージが失敗した際に発生するイベントです。
        /// </summary>
        public event EventHandler<AsyncMessageFailedEventArgs> AsyncMessageFailed;

        protected virtual void OnAsyncMessageFailed( AsyncMessageFailedEventArgs e )
        {
            if ( e == null )
            {
                throw new ArgumentNullException( "e" );
            }

            var threadSafeHandler = Interlocked.CompareExchange( ref AsyncMessageFailed, null, null );

            if (threadSafeHandler != null)
            {
                threadSafeHandler(this, e);
            }
        }
#elif NET45
        /// <summary>
        /// 指定された相互作用メッセージを非同期で送信します。
        /// </summary>
        /// <param name="message">相互作用メッセージ</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public async Task RaiseAsync(InteractionMessage message)
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            await Task.Factory.StartNew(() => Raise(message));
        }

        /// <summary>
        /// 指定された、戻り値情報のある相互作用メッセージを非同期で送信します。
        /// </summary>
        /// <typeparam name="T">戻り値情報のある相互作用メッセージの型</typeparam>
        /// <param name="message">戻り値情報のある相互作用メッセージ</param>
        public async Task<T> GetResponseAsync<T>(T message)
            where T : ResponsiveInteractionMessage
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            return await Task<T>.Factory.StartNew(() => GetResponse(message));
        }

        /// <summary>
        /// 指定された、戻り値情報のある相互作用メッセージを非同期で送信します。
        /// </summary>
        /// <typeparam name="T">戻り値情報のある相互作用メッセージの型</typeparam>
        /// <param name="message">戻り値情報のある相互作用メッセージ</param>
        /// <param name="callback">コールバック</param>
        [Obsolete("callbackを取らないオーバーロードの使用を検討してください。")]
        public async Task<T> GetResponseAsync<T>(T message, Action<T> callback)
            where T : ResponsiveInteractionMessage
        {
            if (message == null)
            {
                throw new ArgumentException("messageはnullにできません");
            }

            var msg = await Task<T>.Factory.StartNew(() => GetResponse(message));
            if (callback != null)
            {
                callback(msg);
            }
            return msg;
        }
#endif
    }


    /// <summary>
    /// 相互作用メッセージ送信時イベント用のイベント引数です。
    /// </summary>
    public class InteractionMessageRaisedEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="message">InteractionMessage</param>
        public InteractionMessageRaisedEventArgs(InteractionMessage message)
        {
            Message = message;
        }

        /// <summary>
        /// 送信されたメッセージ
        /// </summary>
        public InteractionMessage Message
        {
            get;
            set;
        }
    }

#if NET4
    /// <summary>
    /// 非同期メッセージ失敗イベント用のイベント引数です。
    /// </summary>
    public class AsyncMessageFailedEventArgs : EventArgs
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="exception">AggregateException</param>
        public AsyncMessageFailedEventArgs(AggregateException exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException("exception");
            }

            Exception = exception;
        }

        public AggregateException Exception { get; private set; }
    }
#endif
}