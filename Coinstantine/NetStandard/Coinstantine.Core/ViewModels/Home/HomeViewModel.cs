﻿using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.Home
    public class HomeViewModel : BaseViewModel
        private readonly INotificationsAnalyserService _notificationsAnalyserService;
        private readonly ISyncService _syncService;

        public HomeViewModel(IAppServices appServices,
                             IUserService userService,
                             INotificationsRegistrationService notificationsService,
                             IProfileProvider profileProvider,
                             INotificationsAnalyserService notificationsAnalyserService,
                             ISyncService syncService,
                             PrincipalViewModel principalViewModel,
                             BuyViewModel buyViewModel) : base(appServices)
            _userService = userService;
            _notificationsAnalyserService = notificationsAnalyserService;
            _syncService = syncService;
            HasMenu = true;
            PrincipalViewModel.BuyCoinstantine += PrincipalViewModel_BuyCoinstantine;
            BuyViewModel = buyViewModel;
            BuyViewModel.BackToPrincipal += BuyViewModel_BackToPrincipal;
            ViewModels = new ObservableCollection<BaseViewModel>(new BaseViewModel[] { PrincipalViewModel, BuyViewModel });
            ChangePageCommand = new MvxCommand(ChangePage);
            ArrowButton = RightArrow;
        }

        public override async void ViewAppeared()
        {
            TrackPage("Home Page");
            await SyncIfNeeded().ConfigureAwait(false);
            var profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if (CheckConnectivitySilently())
            {
                await _notificationsService.RegisterForNotifications(profile.Email).ConfigureAwait(false);
            }
            PrincipalViewModel.ViewAppeared();
            base.ViewAppeared();
        }
        {
            base.Start();
            PrincipalViewModel.BuyCsnTextFunc = () => BuildAttributedName(true);
            BuyViewModel.BuyCsnTextFunc = () => BuildAttributedName(false);
            await PrincipalViewModel.Start();
            await BuyViewModel.Start();
        public BuyViewModel BuyViewModel { get; }

        public ObservableCollection<BaseViewModel> ViewModels { get; }

        public BaseViewModel ActiveViewModel { get; set; }

        void PrincipalViewModel_BuyCoinstantine(object sender, System.EventArgs e)
        {
            ActiveViewModel = BuyViewModel;
            TrackPage("Buy Page");
            RaisePropertyChanged(nameof(ActiveViewModel));
        }

        void BuyViewModel_BackToPrincipal(object sender, System.EventArgs e)
        {
            ActiveViewModel = PrincipalViewModel;
            TrackPage("Principal Page");
            RaisePropertyChanged(nameof(ActiveViewModel));
        }

        private AttributedName BuildAttributedName(bool withGo)
        {
            var baseString = withGo ? GoBuyCsn : BuyCsn;
            var text = baseString.Replace("@", "");
            var pos = baseString.IndexOf("@", 0, StringComparison.OrdinalIgnoreCase);
            return new AttributedName
            {
                WholeText = text,
                SpecificText = CsnSymbole,
                Config = (pos, pos + 1)
            };
        }
        public string CsnSymbole => "C";
        private string BuyCsn => Translate(TranslationKeys.Home.BuyCsn);
        private string GoBuyCsn => Translate(TranslationKeys.Home.GoBuyCsn);

        public IMvxCommand ChangePageCommand { get; }
        public string ChangePageText => "C";

        private void ChangePage()
        {
            if(ActiveViewModel is BuyViewModel)
            {
                BuyViewModel.BackToPrincipalCommand.Execute();
                ArrowButton = RightArrow;
            }
            else
            {
                PrincipalViewModel.BuyCsnCommand.Execute();
                ArrowButton = LeftArrow;
            }
            RaisePropertyChanged(nameof(ArrowButton));
        }

        public string ArrowButton { get; private set; }
        private string LeftArrow => "arrow-circle-left";
        private string RightArrow => "arrow-circle-right";
    }
}