using System;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.Home{    public class AirdropItemViewModel : BaseViewModel    {
        private readonly IAirdropDefinitionsService _airdropDefinitionsService;
        private readonly AirdropDefinition _airdropDefinition;
        private readonly AirdropStatus _airdropStatus;

        public  AirdropItemViewModel(IAppServices appServices,
                                    IAirdropDefinitionsService airdropDefinitionsService,
                                    AirdropDefinition airdropDefinition,
                                    AirdropStatus airdropStatus) : base(appServices)
        {
            _airdropDefinitionsService = airdropDefinitionsService;
            _airdropDefinition = airdropDefinition;
            _airdropStatus = airdropStatus;
            AirdropTitle = airdropDefinition.AirdropName;
            AmountToAirdrop = airdropDefinition.Amount;
            AdditionalInfo = airdropDefinition.OtherInfoToDisplay;
            SelectedCommand = new MvxCommand(() => NavigateToDetails(airdropDefinition));
        }

        private string GetInfoToShow()
        {
            switch(_airdropStatus)
            {
                case AirdropStatus.Full:
                    return AirdropNotAvailableAnynore;
                case AirdropStatus.NotStarted:  
                    return AirdropNotStarted;
                case AirdropStatus.SoonToBeFull:
                    return AirdropSoonNotAvailable;
                case AirdropStatus.Expired:
                    return AirdropExpired;
                case AirdropStatus.Ok:
                default:
                    return AirdropWillExpire;
            }
        }

        public string AirdropTitle { get; }        public string AmountToAirdropStr => AmountToAirdrop.ToString("N");
        public float AmountToAirdrop { get; }        public string LatestUpdate => GetInfoToShow();
        private string AirdropExpired => string.Format(Translate(TranslationKeys.Airdrop.ExpiredDetail));
        private string AirdropWillExpire => string.Format(Translate(TranslationKeys.Airdrop.ExpiresOn), FormatD(_airdropDefinition.ExpirationDate));
        private string AirdropNotStarted => string.Format(Translate(TranslationKeys.Airdrop.AvailableOn), FormatD(_airdropDefinition.StartDate));
        private string AirdropSoonNotAvailable => Translate(TranslationKeys.Airdrop.NotAvailableSoon);
        private string AirdropNotAvailableAnynore => Translate(TranslationKeys.Airdrop.NotAvailableAnymore);
        public string StatusLabel => _airdropStatus == AirdropStatus.AlreadySubscribed ? "subscribed" : "laptop";
        public string ButtonText => _airdropStatus == AirdropStatus.AlreadySubscribed ? Translate(TranslationKeys.Home.Details) : Translate(TranslationKeys.Home.Subscribe);
        public string AdditionalInfo { get; }
        public IMvxCommand SelectedCommand { get; }

        private void NavigateToDetails(AirdropDefinition airdropDefinition)
        {
            AppNavigationService.ShowAidropDetailPage(airdropDefinition);
        }
    }

    public class PurchaseItemViewModel : BaseViewModel
    {
        private readonly BuyingReceipt _buyingReceipt;

        public PurchaseItemViewModel(IAppServices appServices, BuyingReceipt buyingReceipt) : base(appServices)
        {
            PurchasePhase = Translate(TranslationKeys.Buy.Presale);
            StatusLabel = "shopping-cart";
            ButtonText = Translate(TranslationKeys.Home.Details);
            AmountBought = buyingReceipt.AmountBought;
            Cost = buyingReceipt.Value;
            PurchaseDate = buyingReceipt.PurchaseDate;
            _buyingReceipt = buyingReceipt;
            SelectedCommand = new MvxCommand(NavigateToDetails);
        }

        public string PurchasePhase { get; }
        public string ButtonText { get; }
        public string StatusLabel { get; }
        public string AmountBoughtStr => $"{AmountBought.ToString("N")} CSN";
        private decimal AmountBought { get; }
        private decimal Cost { get; }
        public string CostStr => $"{Cost.ToString("N")} ETH";
        public string PurchaseDateStr => FormatD(PurchaseDate);
        private DateTime PurchaseDate { get; }
        public IMvxCommand SelectedCommand { get; }

        private void NavigateToDetails()
        {
            AppNavigationService.ShowPurchasePage(_buyingReceipt);
        }
    }
}