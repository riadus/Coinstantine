using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Auth.Validation;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public abstract class BaseIconTextfieldViewModel<TParam, TReturnType> : BaseViewModel, IIconTextfieldViewModel
    {
        private bool _isError;
        private string _errorMessage;

        protected BaseIconTextfieldViewModel(IAppServices appServices, IAccountValidator<TParam, TReturnType> validator) : base(appServices)
        {
            Validator = validator;
        }

        public virtual string Icon { get; set; }
        public virtual string Placeholder { get; set; }
        public virtual string Value { get; set; }
        public virtual IconTextFieldType Type { get; set; }
        public virtual object GrossValue { get; set; }
        public virtual IEnumerable<IconTextfieldItemsViewModel> Items { get; set; }

        public bool IsError
        {
            get => _isError; 
            set
            {
                _isError = value;
                RaisePropertyChanged(nameof(IsError));
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage; 
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }

        protected IAccountValidator<TParam, TReturnType> Validator { get; }

        public abstract Task<bool> Validate();
    }

    public class IconTextfieldViewModel : BaseIconTextfieldViewModel<string, bool>
    {
        public IconTextfieldViewModel(IAppServices appServices, IAccountValidator<string, bool> validator) : base(appServices, validator)
        {
        }

        public async override Task<bool> Validate()
        {
            var validationResult = await Validator.IsValid(Value).ConfigureAwait(false);
            IsError = !validationResult.Result;
            if(IsError)
            {
                ErrorMessage = Translate(validationResult.ErrorMessage);
            }
            return !IsError;
        }
    }
}
