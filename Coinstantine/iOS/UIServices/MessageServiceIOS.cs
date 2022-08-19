using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Messages;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.iOS.Views.Extensions;
using CoreGraphics;

namespace Coinstantine.iOS.UIServices
{
	[RegisterInterfaceAsDynamic]
    public class MessageServiceIOS : OverlayManager<MessageView>, IMessageService
    {
        private readonly ITranslationService _translationService;

        protected override bool UseDefaultOverlay => true;

        public MessageServiceIOS(ITranslationService translationService)
        {
            Alpha = 0.8f;
            _translationService = translationService;
        }

        public void Dismiss()
        {
            RemoveView(true);
        }

        public void Alert(MessageViewModel context)
        {
            BeginInvokeOnMainThread(() =>
            {
                ShowView(true);
                View.DataContext = context;
            });
        }

        public void Alert(TranslationKey translationKey)
        {
            Alert(_translationService.Translate(translationKey));
        }

        public void Alert(string message)
        {
            Alert(new MessageViewModel(message, Dismiss, null, _translationService.Translate(TranslationKeys.General.Ok), null));
        }
    }
}
