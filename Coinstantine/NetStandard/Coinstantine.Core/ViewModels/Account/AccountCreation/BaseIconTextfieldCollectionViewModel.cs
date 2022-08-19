using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.AccountCreation
{
    public abstract class BaseIconTextfieldCollectionViewModel<T> : BaseViewModel, IIconTextfieldCollectionViewModel<T>
    {
        private string _error;

        protected BaseIconTextfieldCollectionViewModel(IAppServices appServices) : base(appServices)
        {
        }

        public MvxObservableCollection<IIconTextfieldViewModel> IconTextFields { get; set; }

        public string Error
        {
            get => _error;
            protected set
            {
                _error = value;
                RaisePropertyChanged(nameof(Error));
            }
        }

        public abstract T SetData(T accountCreationModel);

        public virtual async Task<bool> ValidateForm()
        {
            Error = "";
            var result = await Task.WhenAll(IconTextFields.Select(iconTextfield =>
            {
                var valid = iconTextfield.Validate();
                if (iconTextfield.IsError)
                {
                    Error = Error.AddReturnLine();
                    Error += iconTextfield.ErrorMessage;
                }
                return valid;
            })).ConfigureAwait(false);
            return result.All(x => x);
        }

        public virtual string ButtonText => "";
        public virtual IMvxCommand ButtonCommand { get; protected set; }
        public virtual bool ButtonVisible => false;
    }
}
