using System.Linq;
using System.Threading.Tasks;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Account.IconTextField;
using Coinstantine.Domain.Auth.Models;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Auth;
using Coinstantine.Domain.Interfaces.Auth.Validation;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Account.AccountCreation
{
    public class CreateAccountViewModel : BaseViewModel, IIconTextfieldFormViewModel
    {
        private readonly ICountriesProvider _countriesProvider;
        private readonly IValidators _validators;
        private readonly IAccountBackendService _accountBackendService;
        private bool _isLoading;

        public CreateAccountViewModel(IAppServices appServices,
                                      ICountriesProvider countriesProvider,
                                      IValidators validators,
                                      IAccountBackendService accountBackendService) : base(appServices)
        {
            ButtonText = NextText;
            ButtonCommand = new MvxAsyncCommand(Next);
            _countriesProvider = countriesProvider;
            _validators = validators;
            _accountBackendService = accountBackendService;
        }

        public async override Task Initialize()
        {
            IconTextfieldCollectionViewModels = new MvxObservableCollection<IIconTextfieldCollectionViewModel>
            {
                new CreateUsernameViewModel(AppServices, _validators),
                new CreateDetailsViewModel(AppServices, _validators)
            };
            var countries = await _countriesProvider.GetCountries().ConfigureAwait(false);
            var iconTextfieldItemsViewModels = countries.Select(x => new IconTextfieldItemsViewModel { Key = x.Langugage, Value = $"{x.Flag} - {x.Name}" });
            IconTextfieldCollectionViewModels.Add(new CreateAddressViewModel(AppServices, iconTextfieldItemsViewModels, _validators));
            RaisePropertyChanged(nameof(Count));
            await base.Initialize();
        }

        public MvxObservableCollection<IIconTextfieldCollectionViewModel> IconTextfieldCollectionViewModels { get; set; }

        public int Count => IconTextfieldCollectionViewModels.Count();
        public int CurrentIndex { get; private set; }

        public IMvxCommand ButtonCommand { get; }
        private string NextText => Translate(TranslationKeys.UserAccount.Next);
        private string SubmitText => Translate(TranslationKeys.UserAccount.Submit);
        public string ButtonText { get; set; }

        private async Task Next()
        {
            IsLoading = true;
            var validForm = await ValidateForm();
            IsLoading = false;

            if (!validForm)
            {
                return;
            }
            if (CurrentIndex == Count - 1)
            {
                await CreateAccount().ConfigureAwait(false);
                return;
            }
            CurrentIndex = (CurrentIndex + 1) % Count;
            if (CurrentIndex == Count - 1)
            {
                ButtonText = SubmitText;
            }
            else
            {
                ButtonText = NextText;
            }
            RaisePropertyChanged(nameof(CurrentIndex));
            RaisePropertyChanged(nameof(ButtonText));
            RaisePropertyChanged(nameof(IsLoading));
        }

        private async Task CreateAccount()
        {
            var accountCreationModel = new AccountCreationModel();
            IconTextfieldCollectionViewModels.Foreach(x =>
            {
                var vm = x as IIconTextfieldCollectionViewModel<AccountCreationModel>;
                vm.SetData(accountCreationModel);
            });
            Wait(TranslationKeys.LandingPage.Loading);
            var accountCorrect = await _accountBackendService.CreateAccount(accountCreationModel).ConfigureAwait(false);
            DismissWaitMessage();
            if (accountCorrect.AllGood)
            {
                AlertWithAction(TranslationKeys.UserAccount.AccountCreated, () => Close(this));
            }
            else
            {
                AlertWithAction(TranslationKeys.UserAccount.ErrorWhileCreatingAccount, () => Close(this));
            }
        }

        private Task<bool> ValidateForm()
        {
            return CurrentIconTextfieldCollectionViewModel?.ValidateForm();
        }

        public IIconTextfieldCollectionViewModel CurrentIconTextfieldCollectionViewModel => IconTextfieldCollectionViewModels[CurrentIndex];

        public bool IsMultiPage => true;

        public bool IsLoading
        {
            get => _isLoading;
            private set
            {
                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }
    }
}
