using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces.Auth.Validation;

namespace Coinstantine.Core.ViewModels.Account.IconTextField
{
    public class IconDateTextfieldViewModel : BaseIconTextfieldViewModel<DateTime?, bool>
    {
        private object _grossValue;

        public IconDateTextfieldViewModel(IAppServices appServices, IDateOfBirthValidator dateOfBirthValidator) : base(appServices, dateOfBirthValidator)
        {
        }

        public override object GrossValue
        {
            get => _grossValue;
            set
            {
                if (value is DateTime selectedDateTime)
                {
                    SelectedDate = selectedDateTime;
                    _grossValue = value;
                    Value = FormatD(selectedDateTime);
                    RaisePropertyChanged(nameof(Value));
                }
            }
        }
        public DateTime? SelectedDate { get; private set; }

        public override async Task<bool> Validate()
        {
            var validationResult = await Validator.IsValid(SelectedDate).ConfigureAwait(false);
            IsError = !validationResult.Result;
            if(IsError)
            {
                ErrorMessage = Translate(validationResult.ErrorMessage);
            }
            return !IsError;
        }
    }
}
