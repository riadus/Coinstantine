using Coinstantine.Core.Services;
using Coinstantine.Core.UIServices;
using Coinstantine.Core.ViewModels.Account.AccountCreation;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.Login
{
    public class LoginPageViewModel : BaseIconTextfieldCollectionViewModel<LoginModel>
    {
        private readonly IconTextfieldViewModel _usernameTextfiledViewModel;
        private readonly IconTextfieldViewModel _passwordTextfiledViewModel;
        private readonly IAppOpener _appOpener;
        private readonly IEndpointProvider _endpointProvider;

        public LoginPageViewModel(IAppServices appServices, IValidators validators, IAppOpener appOpener, IEndpointProvider endpointProvider) : base(appServices)
        {
            _usernameTextfiledViewModel = new IconTextfieldViewModel(appServices, validators.NotNullOrEmptyValidator) { Icon = "user", Placeholder = Translate(TranslationKeys.UserAccount.Username), Type = IconTextFieldType.Username };
            _passwordTextfiledViewModel = new IconTextfieldViewModel(appServices, validators.NotNullOrEmptyValidator) { Icon = "unlock-alt", Placeholder = Translate(TranslationKeys.UserAccount.Password), Type = IconTextFieldType.Password };

            IconTextFields = new MvxObservableCollection<IIconTextfieldViewModel>
            {
                _usernameTextfiledViewModel,
                _passwordTextfiledViewModel
            };
            _appOpener = appOpener;
            _endpointProvider = endpointProvider;
            ButtonCommand = new MvxCommand(OpenLink);
        }

        public override LoginModel SetData(LoginModel accountCreationModel)
        {
            accountCreationModel = accountCreationModel ?? new LoginModel();

            accountCreationModel.Username = _usernameTextfiledViewModel.Value;
            accountCreationModel.Password = _passwordTextfiledViewModel.Value;

            return accountCreationModel;
        }

        public override string ButtonText => Translate(TranslationKeys.UserAccount.ForgotCredentials);
        public override bool ButtonVisible => true;

        private void OpenLink()
        {
            _appOpener.OpenLink($"{_endpointProvider.WebsiteEndpoint}account/forgot-credentials");
        }
    }
}
