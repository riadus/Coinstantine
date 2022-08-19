using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Core.UIServices
{
    public interface IMessageService
    {
        void Alert(MessageViewModel context);
        void Dismiss();
        void Alert(TranslationKey translationKey);
        void Alert(string message);
    }
}
