using System.Collections.Generic;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.AccountCreation
{
    public class CreateAddressViewModel : BaseIconTextfieldCollectionViewModel<AccountCreationModel>
    {
        private readonly IconTextfieldViewModel _addressTextfieldViewModel;
        private readonly IconTextfieldViewModel _cityTextfieldViewModel;
        private readonly IconCountriesTextfiledViewModel _countryTextfieldViewModel;

        public CreateAddressViewModel(IAppServices appServices,
                                      IEnumerable<IconTextfieldItemsViewModel> iconTextfieldItemsViewModels,
                                      IValidators validators) : base(appServices)
        {
            _addressTextfieldViewModel = new IconTextfieldViewModel(AppServices, validators.NotNullOrEmptyValidator) { Icon = "address-card", Placeholder = Translate(TranslationKeys.UserAccount.Address) };
            _cityTextfieldViewModel = new IconTextfieldViewModel(AppServices, validators.NotNullOrEmptyValidator) { Icon = "mail-bulk", Placeholder = Translate(TranslationKeys.UserAccount.City) };
            _countryTextfieldViewModel = new IconCountriesTextfiledViewModel(AppServices, iconTextfieldItemsViewModels, validators.NotNullOrEmptyValidator) { Icon = "flag", Placeholder = Translate(TranslationKeys.UserAccount.Country), Type = IconTextFieldType.List };

            IconTextFields = new MvxObservableCollection<IIconTextfieldViewModel>
            {
                _addressTextfieldViewModel,
                _cityTextfieldViewModel,
                _countryTextfieldViewModel
            };
        }

        public override AccountCreationModel SetData(AccountCreationModel accountCreationModel)
        {
            accountCreationModel = accountCreationModel ?? new AccountCreationModel();

            accountCreationModel.Address = _addressTextfieldViewModel.Value;
            accountCreationModel.City = _cityTextfieldViewModel.Value;
            accountCreationModel.Country = _countryTextfieldViewModel.SelectedCountry.Key;

            return accountCreationModel;
        }
    }
}
