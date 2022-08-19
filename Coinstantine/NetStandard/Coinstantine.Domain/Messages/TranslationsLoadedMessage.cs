using MvvmCross.Plugin.Messenger;

namespace Coinstantine.Domain.Messages
{
    public class TranslationsLoadedMessage : MvxMessage
    {
        public TranslationsLoadedMessage(object sender) : base(sender)
        {
        }
    }

    public class SyncDoneMessage : MvxMessage
    {
        public SyncDoneMessage(object sender) : base(sender)
        {
        }
    }

    public class BuyingDoneMessage : MvxMessage
    {
        public BuyingDoneMessage(object sender) : base(sender)
        {
        }
    }

    public class BalanceChangedMessage : MvxMessage
    {
        public BalanceChangedMessage(object sender) : base(sender)
        {
        }
    }
}
