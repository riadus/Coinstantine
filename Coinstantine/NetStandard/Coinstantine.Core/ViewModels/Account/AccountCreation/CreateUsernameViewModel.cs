using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.AccountCreation
{
    public class CreateUsernameViewModel : BaseIconTextfieldCollectionViewModel<AccountCreationModel>
    {
        private readonly IValidators _validators;
        private readonly IconTextfieldViewModel _usernameTextfiledViewModel;
        private readonly IconTextfieldViewModel _emailTextfiledViewModel;
        private readonly IconTextfieldViewModel _passwordTextfiledViewModel;
        public CreateUsernameViewModel(IAppServices appServices, IValidators validators) : base(appServices)
        {
            _usernameTextfiledViewModel = new IconTextfieldViewModel(appServices, validators.NotNullOrEmptyValidator) { Icon = "user", Placeholder = Translate(TranslationKeys.UserAccount.Username), Type = IconTextFieldType.Username };
            _emailTextfiledViewModel = new IconTextfieldViewModel(appServices, validators.EmailFormatValidator) { Icon = "at", Placeholder = Translate(TranslationKeys.UserAccount.Email), Type = IconTextFieldType.Email };
            _passwordTextfiledViewModel = new IconTextfieldViewModel(appServices, validators.PasswordValidator) { Icon = "unlock-alt", Placeholder = Translate(TranslationKeys.UserAccount.Password), Type = IconTextFieldType.NewPassword };

            IconTextFields = new MvxObservableCollection<IIconTextfieldViewModel>
            {
                _usernameTextfiledViewModel,
                _emailTextfiledViewModel,
                _passwordTextfiledViewModel
            };
            _validators = validators;
        }

        public async override Task<bool> ValidateForm()
        {
            var baseValidation = await base.ValidateForm().ConfigureAwait(false);
            if(baseValidation)
            {
                var accountCreationModel = new AccountCreationModel
                {
                    Username = _usernameTextfiledViewModel.Value,
                    Email = _emailTextfiledViewModel.Value,
                    Password = _passwordTextfiledViewModel.Value
                };
                var accountCorrect = await _validators.AccountValidator.IsValid(accountCreationModel).ConfigureAwait(false);
                HandleAccountCorrect(accountCorrect);
                return accountCorrect.Result.AllGood;
            }
            return false;
        }

        private void HandleAccountCorrect(ValidationResult<AccountCorrect> accountCorrect)
        {
            if (accountCorrect.Result.AllGood)
            {
                return;
            }
            _usernameTextfiledViewModel.IsError = !accountCorrect.Result.UsernameAvailable;
            _emailTextfiledViewModel.IsError = !accountCorrect.Result.EmailAvailable;
            _passwordTextfiledViewModel.IsError = !accountCorrect.Result.PasswordCorrect;
            Error = Translate(accountCorrect.ErrorMessage);
        }

        public override AccountCreationModel SetData(AccountCreationModel accountCreationModel)
        {
            accountCreationModel = accountCreationModel ?? new AccountCreationModel();

            accountCreationModel.Username = _usernameTextfiledViewModel.Value;
            accountCreationModel.Email = _emailTextfiledViewModel.Value;
            accountCreationModel.Password = _passwordTextfiledViewModel.Value;

            return accountCreationModel;
        }
    }
}
