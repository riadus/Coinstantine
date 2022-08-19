using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Auth.Validation;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public class IconCountriesTextfiledViewModel : BaseIconTextfieldViewModel<string, bool>
    {
        private object _grossValue;

        public IconCountriesTextfiledViewModel(IAppServices appServices, 
                                               IEnumerable<IconTextfieldItemsViewModel> items, 
                                               INotNullOrEmptyValidator notNullOrEmptyValidator) 
            : base(appServices, notNullOrEmptyValidator)
        {
            Items = items;
        }

        public override object GrossValue
        {
            get => _grossValue;
            set
            {
                if (value is IconTextfieldItemsViewModel country)
                {
                    _grossValue = value;
                    SelectedCountry = country;
                    Value = country.Value;
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }

        public IconTextfieldItemsViewModel SelectedCountry { get; private set; }

        public async override Task<bool> Validate()
        {
            var validationResult = await Validator.IsValid(Value).ConfigureAwait(false);
            IsError = !validationResult.Result;
            if (IsError)
            {
                ErrorMessage = Translate(validationResult.ErrorMessage);
            }
            return !IsError;
        }
    }
}
