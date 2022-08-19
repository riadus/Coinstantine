using System;
using System.Globalization;
using System.Threading.Tasks;
using Coinstantine.Core.Services;using Coinstantine.Core.UIServices;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Blockchain;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Domain.Messages;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;

namespace Coinstantine.Core.ViewModels.Home{    public class BuyViewModel : BaseViewModel
    {
        private readonly IPriceProvider _priceProvider;
        private readonly IProfileProvider _profileProvider;
        private readonly IBuyingService _buyingService;
        private readonly IAppOpener _appOpener;
        private readonly IUserOptionService _userOptionService;
        private UserProfile _userProfile;
        private decimal EtherBalance { get; set; }
        protected MvxSubscriptionToken _weakReferenceToSyncDoneMessage;
        protected MvxSubscriptionToken _weakReferenceToBalanceLoadedMessage;
        private decimal MinimumEtherRequired { get; set; }

        public BuyViewModel(IAppServices appServices,
                            IPriceProvider priceProvider,
                            IProfileProvider profileProvider,
                            IMvxMessenger mvxMessenger,
                            IBuyingService buyingService,
                            IAppOpener appOpener,
                            IUserOptionService userOptionService) : base(appServices)
        {
            BackToPrincipalCommand = new MvxCommand(OnBackToPrincipal);
            BuyCommand = new MvxCommand(DoBuy);
            _priceProvider = priceProvider;
            _profileProvider = profileProvider;
            _buyingService = buyingService;
            _appOpener = appOpener;
            _userOptionService = userOptionService;
            _weakReferenceToSyncDoneMessage = mvxMessenger.Subscribe<SyncDoneMessage>(async obj =>
            {
                await LoadPrices().ConfigureAwait(false);
                RaiseAllPropertiesChanged();
            });

            _weakReferenceToBalanceLoadedMessage = mvxMessenger.Subscribe<BalanceChangedMessage>(async obj =>
            {
                await UpdateStatusFromBalance().ConfigureAwait(false);
                RaiseAllPropertiesChanged();
            });
        }

        public IMvxCommand BackToPrincipalCommand { get; }
        public IMvxCommand BuyCommand { get; }
        public event EventHandler BackToPrincipal;

        private void OnBackToPrincipal()
        {
            BackToPrincipal?.Invoke(this, EventArgs.Empty);
        }

        public async new Task Start()
        {
            base.Start();
            _userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            await LoadPrices().ConfigureAwait(false);
        }

        private async Task LoadPrices()
        {
            var csnConfig = await _priceProvider.GetCoinstantinePriceConfig().ConfigureAwait(false);
            var prices = await _priceProvider.GetEthPrice().ConfigureAwait(false);
            if (csnConfig != null)
            {
                CsnPriceInETH = csnConfig.Eth;
                Bonus = csnConfig.Bonus;
                MinimumEtherRequired = csnConfig.MinimumEth;
                _startDate = csnConfig.StartDate;
                _endDate = csnConfig.EndDate;
            }
            if (prices != null)
            {
                EthPrice = prices.Usd;
            }
            await UpdateStatusFromBalance().ConfigureAwait(false);
        }

        private async Task UpdateStatusFromBalance()
        {
            _userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            EtherBalance = _userProfile.BlockchainInfo.Ether;

            InfoLabel = Translate(TranslationKeys.Buy.PricesNotSynced);
            Status = BuyStatus.NotOk;
            if (EthPrice != 0)
            {
                if (EtherBalance >= 0.5m)
                {
                    Status = BuyStatus.Ok;
                }
                else
                {
                    InfoLabel = NotEnoughEther;
                    Status = BuyStatus.NotEnoughEther;
                }
            }
            RaiseAllPropertiesChanged();
        }

        public AttributedName BuyCsnText => BuyCsnTextFunc();
        public Func<AttributedName> BuyCsnTextFunc { get; set; }

        public string BackToPrincipalText => "chevron-up";
        public string CsnValueText => Translate(TranslationKeys.Buy.CsnValue);

        private decimal CsnPriceInETH { get; set; }
        public string CsnPriceInETHText => $"ETH {CsnPriceInETH}";

        private DateTime _startDate;
        private DateTime _endDate;

        private decimal EthPrice { get; set; }
        private decimal CsnPriceDollar => CsnPriceInETH * EthPrice;
        public string CsnPriceDollarText => $"$ {CsnPriceDollar}";

        public string CoinstantineAmountText => Translate(TranslationKeys.Buy.CsnAmount);
        public string CoinstantineCostText => Translate(TranslationKeys.Buy.CsnCost);

        private decimal? _coinstantineAmount;

        public string CoinstantineAmount
        {
            get
            {
                if (_coinstantineAmount.HasValue)
                {
                    return _coinstantineAmount.Value.ToString("#0.###");
                }
                return string.Empty;
            }

            set
            {
                _coinstantineAmount = null;
                value = value.Replace(",", ".");
                if (decimal.TryParse(value, out decimal decimalValue))
                {
                    _coinstantineAmount = decimalValue;
                }
                UpdateCoinstantineCost();
                CheckValidity();
            }
        }

        private void UpdateCoinstantineCost()
        {
            _coinstantineCost = _coinstantineAmount * CsnPriceInETH;
            RaisePropertyChanged(nameof(CoinstantineCost));
            RaisePropertyChanged(nameof(TotalInETHText));
            RaisePropertyChanged(nameof(TotalCsnText));
        }

        private decimal? _coinstantineCost;

        public string CoinstantineCost
        {
            get
            {
                if (_coinstantineCost.HasValue)
                {
                    return _coinstantineCost.Value.ToString("#0.###");
                }
                return string.Empty;
            }

            set
            {
                _coinstantineCost = null;
                value = value.Replace(",", ".");
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
                {
                    _coinstantineCost = decimalValue;
                }
                UpdateCoinstantineAmount();
                CheckValidity();
            }
        }

        private void UpdateCoinstantineAmount()
        {
            _coinstantineAmount = _coinstantineCost / CsnPriceInETH;
            RaisePropertyChanged(nameof(CoinstantineAmount));
            RaisePropertyChanged(nameof(TotalInETHText));
            RaisePropertyChanged(nameof(TotalCsnText));
        }

        public string TotalLabel => "Total";
        public string TotalInETHText => $"ETH {TotalInETH}";
        public string TotalCsnText => $"CSN {TotalCsn}";

        public string BonusText => $"{Bonus}% {Translate(TranslationKeys.Buy.Bonus)}";
        private decimal Bonus { get; set; }

        private decimal? TotalInETH => _coinstantineCost;
        private decimal? TotalCsn => _coinstantineAmount + (_coinstantineAmount * Bonus / 100);

        public string InfoLabel { get; private set; }
        public string InfoCharacter => "exclamation-circle";
        public BuyStatus Status { get; set; }

        public enum BuyStatus
        {
            Ok,
            NotEnoughEther,
            NotOk
        }

        private string NotEnoughEther => string.Format(Translate(TranslationKeys.Buy.NotEnoughETH), string.Empty);
        private decimal MaxCsn => CsnPriceInETH != 0 ? EtherBalance / CsnPriceInETH : 0;

        private void NormalizeInput()
        {
            var message = string.Empty;
            if(DateTime.Now <= _startDate)
            {
            message = string.Format(Translate(TranslationKeys.Buy.PresaleNotOpenYet), FormatD(_startDate), FormatT(_startDate));
                Alert(message);
                return;
            }
            if (DateTime.Now >= _endDate)
            {
                message = string.Format(Translate(TranslationKeys.Buy.PresaleClosed), FormatD(_endDate));
                Alert(message);
                return;
            }
            if (TotalInETH == null || TotalInETH < MinimumEtherRequired)
            {
                Alert(TranslationKeys.Buy.MinimumEtherRequired, MinimumEtherRequired.ToString());
                return;
            }
            if (_coinstantineCost > EtherBalance)
            {
                _coinstantineCost = EtherBalance;
                UpdateCoinstantineAmount();
                UpdateCoinstantineCost();
                CheckValidity();
                message = string.Format(Translate(TranslationKeys.Buy.NotEnoughETH), _userProfile.BlockchainInfo.Address);
                Alert(message, () =>
                {
                    DismissAlert();
                    Share(_userProfile.BlockchainInfo.Address);
                }, TranslationKeys.General.ShareAddress, TranslationKeys.General.Ok);
                return;
            }
            var value = TotalInETH.Value;
            message += string.Format(Translate(TranslationKeys.Buy.AlertBuy), TotalCsn, TotalInETH);
            Alert(message, async () =>
            {
                DismissAlert(); 
                await SendTransaction(value);
            });
        }

        private async Task SendTransaction(decimal value)
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.Buy.SendingTransaction);
            var receipt = await _buyingService.Buy(value).ConfigureAwait(false);
            DismissWaitMessage();

            if (receipt == null)
            {
                Alert(TranslationKeys.Buy.AlertBuyError);
                return;
            }

            var messenger = Mvx.Resolve<IMvxMessenger>();
            messenger.Publish(new BuyingDoneMessage(this));
            var sucessMessage = string.Format(Translate(TranslationKeys.Buy.AlertBuyPending), receipt.TransactionHash);
            Alert(sucessMessage, () => Share(receipt.TransactionHash), TranslationKeys.Buy.ShareHash, TranslationKeys.General.Ok);
        }

        private async void Share(string transactionHash)
        {
            var selectedOption = await _userOptionService.ShowShareOptions(Translate(TranslationKeys.Home.SelectShareOption), Translate(TranslationKeys.General.Cancel)).ConfigureAwait(false);
            _appOpener.ShareText(transactionHash, null, selectedOption);
        }

        private void DoBuy()
        {
            NormalizeInput();
        }

        private void CheckValidity()
        {
            CorrectInput = false;
            CorrectInput |= _coinstantineCost == null;
            CorrectInput |= _coinstantineCost <= EtherBalance && _coinstantineCost >= MinimumEtherRequired;
            RaisePropertyChanged(nameof(CorrectInput));
        }

        public bool CorrectInput { get; set; } = true;
    }

    public class AttributedName
    {
        public string WholeText { get; set; }
        public string SpecificText { get; set; }
        public (int Start, int End) Config { get; set; }
    }}