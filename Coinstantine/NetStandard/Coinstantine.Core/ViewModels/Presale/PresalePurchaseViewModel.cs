using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Presale
{
    public class PresalePurchaseViewModel : GenericInfoViewModel<BuyingReceipt>
    {
        private readonly IAppOpener _appOpener;
        private readonly IEnvironmentInfoProvider _environmentInfoProvider;

        public PresalePurchaseViewModel(IAppServices appServices,
                                        IProfileProvider profileProvider,
                                        IUserService userService,
                                        IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor,
                                        IAppOpener appOpener,
                                        IEnvironmentInfoProvider environmentInfoProvider) : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.General.Coinstantine;
            TitleIcon = "C";
            _appOpener = appOpener;
            _environmentInfoProvider = environmentInfoProvider;
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Presale page");
        }

        public override void Prepare(BuyingReceipt parameter)
        {
            base.Prepare(parameter);

            _transactionHash = parameter.TransactionHash;
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo> {
                { (TranslationKeys.Presale.Amount, $"{parameter.AmountBought.ToString("N")} CSN") },
                { (TranslationKeys.Presale.Value, $"{parameter.Value.ToString("N")} ETH", Display.Grouped) },
                { (TranslationKeys.Presale.Date, FormatD(parameter.PurchaseDate)) },
                { (TranslationKeys.Presale.Gas, parameter.GasUsed, Display.Grouped) },
                { (TranslationKeys.Presale.TransactionHash, parameter.TransactionHash) }
            }));
        }
        private string _transactionHash;

        public override string InfoTitle => Translate(TranslationKeys.Presale.PresaleParticipation);
        public override bool ShowRegularBehaviourText => false;

        protected override string AlternateBehaviourText => Translate(TranslationKeys.Presale.ShowTransaction);

        protected override Task SecondaryButtonAction()
        {
            return Task.FromResult(0);
        }
        protected override int GetRemainingTime()
        {
            return 0;
        }

        protected override async Task PrincipalButtonAction()
        {
            var baseEtherscanUrl = await _environmentInfoProvider.GetEtherscanUrl().ConfigureAwait(false);
            _appOpener.OpenLink($"{baseEtherscanUrl}/tx/{_transactionHash}");
        }
    }
}
