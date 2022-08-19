using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Translations;

namespace Coinstantine.Core.ViewModels.Home{    public class TokenBalanceViewModel : BaseViewModel
    {
        public TokenBalanceViewModel(IAppServices appServices) : base(appServices)
        {
        }

        public string Symbole { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public bool AvailableLater { get; set; }
        public string AvailableLaterText => Translate(TranslationKeys.Home.AvailableLater);
    }
}