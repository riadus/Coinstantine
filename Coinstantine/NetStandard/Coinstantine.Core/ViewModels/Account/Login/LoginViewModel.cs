using System;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.Login
{
    public class LoginViewModel : BaseViewModel, IIconTextfieldFormViewModel
    {
        private readonly IProfileProvider _profileProvider;
        private readonly IAccountBackendService _accountBackendService;

        public LoginViewModel(IAppServices appServices, 
                              IProfileProvider profileProvider,
                              IValidators validators,
                              IAccountBackendService accountBackendService, 
                              IAppOpener appOpener,
                              IEndpointProvider endpointProvider) : base(appServices)
        {
            IconTextfieldCollectionViewModels = new MvxObservableCollection<IIconTextfieldCollectionViewModel>
            {
                new LoginPageViewModel(AppServices, validators, appOpener, endpointProvider)
            };
            CurrentIconTextfieldCollectionViewModel = IconTextfieldCollectionViewModels[0];
            _profileProvider = profileProvider;
            _accountBackendService = accountBackendService;
            ButtonText = Translate(TranslationKeys.LandingPage.Login);
            ButtonCommand = new MvxAsyncCommand(Login);
        }

        public MvxObservableCollection<IIconTextfieldCollectionViewModel> IconTextfieldCollectionViewModels { get; set; }
        public IIconTextfieldCollectionViewModel CurrentIconTextfieldCollectionViewModel { get; }
        public bool IsMultiPage => false;
        public int Count { get; }
        public int CurrentIndex { get; }
        public IMvxCommand ButtonCommand { get; }
        public string ButtonText { get; }

        private async Task Login()
        {
            if (!await ValidateForm())
            {
                return;
            }
            var loginModel = new LoginModel();
            IconTextfieldCollectionViewModels.Foreach(x =>
            {
                var vm = x as IIconTextfieldCollectionViewModel<LoginModel>;
                vm.SetData(loginModel);
            });
            Wait(TranslationKeys.LandingPage.Loading);
            var loginStatus = await _accountBackendService.Login(loginModel);
            switch (loginStatus)
            {
                case LoginStatus.AuthenticationSucceeded:
                    await Proceed().ConfigureAwait(false);
                    return;
                case LoginStatus.AuthenticationFailed:
                    DismissWaitMessage();
                    Alert(TranslationKeys.UserAccount.LoginFailed);
                    return;
                case LoginStatus.AccountNotConfirmed:
                    DismissWaitMessage();
                    Alert(TranslationKeys.UserAccount.AccountNotConfirmed);
                    return;
            }
        }

        private Task<bool> ValidateForm()
        {
            return CurrentIconTextfieldCollectionViewModel?.ValidateForm();
        }

        private async Task Proceed()
        {
            var syncTask = AppServices.SyncService.SyncTranslations();
            var syncProfile = AppServices.SyncService.CheckOnlineProfileAndUnpdateIfNeeded();
            Task.WaitAll(new[] { syncTask, syncProfile });
            DismissWaitMessage();

            var userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            if (userProfile.NeedsReset)
            {
                AppNavigationService.ShowSetPincode();
            }
            else if (userProfile.PinCode.IsNullOrEmpty())
            {
                AppNavigationService.ShowSetPincode();
            }
            else
            {
                userProfile.LoggedIn = true;
                await _profileProvider.SaveUserProfile(userProfile);
                AppNavigationService.ShowHomePage();
            }
        }

        public bool IsLoading { get; }
    }
}
