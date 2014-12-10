using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Livet;
using Livet.Messaging;
using Livet.Messaging.Windows;
using Livet.Messaging.IO;

namespace Livet.UITest
{
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            Maximize = true;
            ConfirmCaption = "キャプションバインディングテスト";
        }

        public void Init()
        {
            Messenger.RaiseAsync(new InformationMessage("ViewModelのInitが呼ばれました", "情報", System.Windows.MessageBoxImage.Information, "Info"));
            
        }

        #region CanClose変更通知プロパティ
        private bool _CanClose;

        public bool CanClose
        {
            get
            { return _CanClose; }
            set
            { 
                if (EqualityComparer<bool>.Default.Equals(_CanClose, value))
                    return;
                _CanClose = value;
#if NET4
                RaisePropertyChanged("CanClose");
#elif NET45
                RaisePropertyChanged();
#endif
            }
        }
        #endregion

        #region Maximize変更通知プロパティ
        private bool _Maximize;

        public bool Maximize
        {
            get
            { return _Maximize; }
            set
            { 
                if (EqualityComparer<bool>.Default.Equals(_Maximize, value))
                    return;
                _Maximize = value;
#if NET4
                RaisePropertyChanged("Maximize");
#elif NET45
                RaisePropertyChanged();
#endif
            }
        }
        #endregion

//        #region FolderDialogKind変更通知プロパティ
//        private FolderSelectionDialogPreference _FolderDialogKind;

//        public FolderSelectionDialogPreference FolderDialogKind
//        {
//            get
//            { return _FolderDialogKind; }
//            set
//            { 
//                if (EqualityComparer<FolderSelectionDialogPreference>.Default.Equals(_FolderDialogKind, value))
//                    return;
//                _FolderDialogKind = value;
//#if NET4
//                RaisePropertyChanged("FolderDialogKind");
//#elif NET45
//                RaisePropertyChanged();
//#endif
//            }
//        }
//        #endregion

        public void RequestClose(ConfirmationMessage message)
        {
            if (message.Response.HasValue && message.Response.Value)
            {
                CanClose = true;
                Messenger.Raise(new WindowActionMessage(WindowAction.Close, "Close"));
            }
        }

        public void CloseCanceled()
        {
            var result = Messenger.GetResponse(new ConfirmationMessage("Windowを閉じれるようにしたければYesを押してください", "情報", System.Windows.MessageBoxImage.Information, "CloseCanceled")).Response;

            if (result.HasValue)
            {
                CanClose = result.Value;
            }
        }

        public void ChangeMaximize()
        {
            Maximize = !Maximize;
        }




        #region ConfirmCaption変更通知プロパティ
        private string _ConfirmCaption;

        public string ConfirmCaption
        {
            get
            { return _ConfirmCaption; }
            set
            { 
                if (_ConfirmCaption == value)
                    return;
                _ConfirmCaption = value;
                RaisePropertyChanged("ConfirmCaption");
            }
        }
        #endregion




        //public void FolderSelected(FolderSelectionMessage message)
        //{
        //    if (!string.IsNullOrEmpty(message.Response))
        //    {
        //        Messenger.RaiseAsync(new InformationMessage(message.Response + " が選択されました！","情報",System.Windows.MessageBoxImage.Information,"Info"));
        //    }
        //}
    }
}
