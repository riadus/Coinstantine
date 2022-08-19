using System;
using Coinstantine.Common;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Messages
{
    public class MessageViewModel : MvxViewModel
    {
        public MessageViewModel(string message, Action okAction, Action cancelAction, string okText, string cancelText)
        {
            Message = message;
            OkCommand = new MvxCommand(okAction);
            CancelCommand = new MvxCommand(cancelAction);
			OkText = okText;
			CancelText = cancelText;
        }

        public string Message { get; }
        public string OkText { get; }
        public string CancelText { get; }

        public IMvxCommand OkCommand { get; }
        public IMvxCommand CancelCommand { get; }

        public bool WithCancel => CancelText.IsNotNull();
    }
}
