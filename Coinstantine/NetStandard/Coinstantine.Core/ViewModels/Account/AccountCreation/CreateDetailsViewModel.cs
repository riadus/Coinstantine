using System;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.AccountCreation
{
    public class CreateDetailsViewModel : BaseIconTextfieldCollectionViewModel<AccountCreationModel>
    {
        private readonly IconTextfieldViewModel _firstNameTextfieldViewModel;
        private readonly IconTextfieldViewModel _lastNameTextfieldViewModel;
        private readonly IconDateTextfieldViewModel _dateOfBirthTextfieldViewModel;

        public CreateDetailsViewModel(IAppServices appServices, IValidators validators) : base(appServices)
        {
            _firstNameTextfieldViewModel = new IconTextfieldViewModel(appServices, validators.NotNullOrEmptyValidator) { Icon = "user", Placeholder = Translate(TranslationKeys.UserAccount.FirstName) };
            _lastNameTextfieldViewModel = new IconTextfieldViewModel(appServices, validators.NotNullOrEmptyValidator) { Icon = "user", Placeholder = Translate(TranslationKeys.UserAccount.LastName) };
            _dateOfBirthTextfieldViewModel = new IconDateTextfieldViewModel(appServices, validators.DateOfBirthValidator) { Icon = "calendar-alt", Placeholder = Translate(TranslationKeys.UserAccount.DateOfBirth), Type = IconTextFieldType.Date };

            IconTextFields = new MvxObservableCollection<IIconTextfieldViewModel>
            {
                _firstNameTextfieldViewModel,
                _lastNameTextfieldViewModel,
                _dateOfBirthTextfieldViewModel
            };
        }

        public override AccountCreationModel SetData(AccountCreationModel accountCreationModel)
        {
            accountCreationModel = accountCreationModel ?? new AccountCreationModel();

            accountCreationModel.FirstName = _firstNameTextfieldViewModel.Value;
            accountCreationModel.LastName = _lastNameTextfieldViewModel.Value;
            accountCreationModel.DateOfBirth = (DateTime) _dateOfBirthTextfieldViewModel.SelectedDate;

            return accountCreationModel;
        }
    }
}
