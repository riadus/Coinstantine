using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Core.Services;using Coinstantine.Core.UIServices;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Airdrops;
using Coinstantine.Domain.Interfaces.Blockchain;
using Coinstantine.Domain.Interfaces.Translations;
using Coinstantine.Domain.Messages;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Home{    public class PrincipalViewModel : BaseViewModel    {
        private readonly IProfileProvider _profileProvider;
        private readonly IAppOpener _appOpener;
        private readonly IBlockchainService _blockchainService;
        private readonly IMvxMessenger _mvxMessenger;
        private readonly IBuyingService _buyingService;
        private readonly IAirdropDefinitionsService _airdropDefinitionsService;
        private readonly IAppVersion _appVersion;
        private readonly IAppEnvironmentProvider _appEnvironmentInfoProvider;
        private readonly IUserOptionService _userOptionService;
        protected MvxSubscriptionToken _weakReferenceToSyncDoneMessage;
        protected MvxSubscriptionToken _weakReferenceToBuyingDoneMessage;

        public PrincipalViewModel(IAppServices appServices,
                                  IProfileProvider profileProvider,
                                  IAppOpener appOpener,
                                  IBlockchainService blockchainService,
                                  IMvxMessenger mvxMessenger,
                                  IBuyingService buyingService,
                                  IAirdropDefinitionsService airdropDefinitionsService,
                                  IAppVersion appVersion,
                                  IAppEnvironmentProvider appEnvironmentInfoProvider,
                                  IUserOptionService userOptionService) : base(appServices)
        {
            Airdrops = new MvxObservableCollection<IBaseViewModel>();

            CoinstantineBalance = new TokenBalanceViewModel(appServices)
            {
                Symbole = "C",
                Name = "Coinstantine",
                AvailableLater = true
            };

            EtherBalance = new TokenBalanceViewModel(appServices)
            {
                Symbole = "ethereum",
                Name = "Ether",
                AvailableLater = false
            };
            _profileProvider = profileProvider;
            _appOpener = appOpener;
            _blockchainService = blockchainService;
            _mvxMessenger = mvxMessenger;
            _buyingService = buyingService;
            _airdropDefinitionsService = airdropDefinitionsService;
            _appVersion = appVersion;
            _appEnvironmentInfoProvider = appEnvironmentInfoProvider;
            _userOptionService = userOptionService;
            BuyCsnCommand = new MvxCommand(OnBuyCoinstantine);
            ShareCommand = new MvxAsyncCommand<object>(Share);
            RefreshCommand = new MvxAsyncCommand(Refresh);
            RefreshListCommand = new MvxAsyncCommand(async () => await LoadAirdrops(true).ConfigureAwait(false));

            _weakReferenceToSyncDoneMessage = mvxMessenger.Subscribe<SyncDoneMessage>(async obj =>
            {
                await LoadAirdrops(false).ConfigureAwait(false);
                await Refresh().ConfigureAwait(false);
                RaiseAllPropertiesChanged();
            });

            _weakReferenceToBuyingDoneMessage = mvxMessenger.Subscribe<BuyingDoneMessage>(async obj =>
            {
                await Refresh().ConfigureAwait(false);
                await LoadAirdrops(true).ConfigureAwait(false);
            });
        }

        public async new Task Start()
        {
            base.Start();
            await LoadEnvironmentVariables().ConfigureAwait(false);
            await LoadBalances().ConfigureAwait(false);
            await LoadAirdrops(false).ConfigureAwait(false);
            await Refresh().ConfigureAwait(false);
        }

        private async Task LoadEnvironmentVariables()
        {
            var ethereumNetwork = await _appVersion.GetEthereumNetwork().ConfigureAwait(false);
            var apiEnvironment = _appEnvironmentInfoProvider.ApiEnvironment;

            EthereumNetwork = ethereumNetwork.ToString();
            ApiEnvironment = apiEnvironment.ToString();

            ShowEnvironment = ethereumNetwork != Data.EthereumNetwork.Mainnet || apiEnvironment != Data.ApiEnvironment.Production;
        }

        private bool _isListLoading;

        public bool IsListLoading
        {
            get
            {
                return _isListLoading;
            }
            set
            {
                SetProperty(ref _isListLoading, value);
            }
        }

        private async Task LoadAirdrops(bool forceSync)
        {
            IsListLoading = true;
            var airdropDefinitions = await _airdropDefinitionsService.GetAirdropDefinitions().ConfigureAwait(false);
            Airdrops = new MvxObservableCollection<IBaseViewModel>();
            foreach (var item in airdropDefinitions)
            {
                var status = await _airdropDefinitionsService.GetStatus(item).ConfigureAwait(false);
                Airdrops.Add(new AirdropItemViewModel(AppServices, _airdropDefinitionsService, item, status));
                await Task.Delay(300);

            }
            await LoadPurchases(forceSync).ConfigureAwait(false);
            IsListLoading = false;
        }

        private async Task LoadPurchases(bool forceSync)
        {
            IEnumerable<BuyingReceipt> purchases;
            if (forceSync)
            {
                purchases  = await _buyingService.SyncReceipts().ConfigureAwait(false);
            }
            else
            {
                purchases = await _buyingService.GetReceipts().ConfigureAwait(false);
            }
            Airdrops.AddRange(purchases.Select(purchase => new PurchaseItemViewModel(AppServices, purchase)));
            RaisePropertyChanged(nameof(Airdrops));
        }

        private async Task LoadBalances()
        {
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            EtherBalance.Balance = profile.BlockchainInfo.Ether;
            CoinstantineBalance.Balance = profile.BlockchainInfo.Coinstantine;
            UsernameLabel = profile.Username;
            EthAddress = profile.BlockchainInfo.Address;
            RaiseAllPropertiesChanged();
            EtherBalance.RaiseAllPropertiesChanged();
            CoinstantineBalance.RaiseAllPropertiesChanged();
        }
        public string UsernameLabel { get; private set; }        public string EthAddress { get; private set; }
        private string EthereumNetwork { get; set; }
        private string ApiEnvironment { get; set; }
        public string Environment => $"{EthereumNetwork} : {ApiEnvironment}";
        public bool ShowEnvironment { get; private set; }

        public TokenBalanceViewModel CoinstantineBalance { get; }
        public TokenBalanceViewModel EtherBalance { get; }

        public string AirdropsLabel => "";//Translate(TranslationKeys.Home.Aidrops).ToUpper();        public AttributedName BuyCsnText => BuyCsnTextFunc();
        public Func<AttributedName> BuyCsnTextFunc { get; set; }        public string ShareButtonText => "share-square";        public IMvxCommand BuyCsnCommand { get; }        public MvxObservableCollection<IBaseViewModel> Airdrops { get; private set; }

        public event EventHandler BuyCoinstantine;

        private void OnBuyCoinstantine()
        {
            BuyCoinstantine?.Invoke(this, EventArgs.Empty);
        }

        public IMvxCommand<object> ShareCommand { get; }
        public IMvxCommand RefreshCommand { get; }
        public IMvxCommand RefreshListCommand { get; }
        bool _isRefreshing;

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }

            private set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }

        public string RefreshingMessage => Translate(TranslationKeys.Home.RefreshingMessage);
        public string RefreshingListMessage => Translate(TranslationKeys.Home.RefreshingListMessage);

        private async Task Share(object sender)
        {
            var selectedOption = await _userOptionService.ShowShareOptions(Translate(TranslationKeys.Home.SelectShareOption), Translate(TranslationKeys.General.Cancel)).ConfigureAwait(false);
            _appOpener.ShareText(EthAddress, sender, selectedOption);
        }

        private async Task Refresh()
        {
            IsRefreshing = true;
            try
            {
                var finishedRefreshing = await _blockchainService.UpdateBalance().ConfigureAwait(false);
                if (finishedRefreshing)
                {
                    var previousBalance = EtherBalance.Balance;
                    await LoadBalances().ConfigureAwait(false);
                    if (previousBalance != EtherBalance.Balance)
                    {
                        _mvxMessenger.Publish(new BalanceChangedMessage(this));
                    }
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }}