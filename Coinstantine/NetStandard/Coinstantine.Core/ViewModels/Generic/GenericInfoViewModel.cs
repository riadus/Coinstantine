using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.Generic
{
    public abstract class GenericInfoViewModel<T> : BaseViewModel<T>, IGenericInfoViewModel
    {
        protected T _subject;
        protected UserProfile _userProfile;
        private bool _enabledAction = true;
        protected readonly IGenericInfoItemViewModelConstructor _itemInfoViewModelConstructor;
        protected readonly IProfileProvider _profileProvider;
        protected readonly IUserService _userService;
        private bool _isRefreshing;

        protected GenericInfoViewModel(IAppServices appServices,
                                       IProfileProvider profileProvider,
                                       IUserService userService,
                                       IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor) : base(appServices)
        {
            _profileProvider = profileProvider;
            _userService = userService;
            _itemInfoViewModelConstructor = itemInfoViewModelConstructor;
            SecondaryButtonCommand = new MvxAsyncCommand(SecondaryButtonAction);
            PrincipalButtonCommand = new MvxAsyncCommand(PrincipalButtonAction);
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>();
            RefreshCommand = new MvxAsyncCommand(Refresh);
            HasRefreshingCapability = false;
        }

        public override async void Prepare(T parameter)
        {
            _subject = parameter;
            _userProfile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
        }

        public abstract string InfoTitle { get; }
        public MvxObservableCollection<GenericInfoItemViewModel> GenericInfoItems { get; protected set; }

        public IMvxCommand PrincipalButtonCommand { get; }
        public IMvxCommand SecondaryButtonCommand { get; }
        public IMvxCommand RefreshCommand { get; }
        public bool HasRefreshingCapability { get; protected set; }

        protected abstract Task SecondaryButtonAction();
        protected abstract Task PrincipalButtonAction();
        protected virtual Task Refresh()
        {
            return Task.FromResult(0);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public string RefreshText { get; protected set; }

        public bool EnabledAction
        {
            get => _enabledAction;
            protected set => SetProperty(ref _enabledAction, value);
        }

        public bool ShowSecondaryButton => ShowRegularBehaviourText || StillTimeToEdit;

        public string SecondaryButtonText => $"{Translate(TranslationKeys.General.Edit)}";
        public string RemainingTime => $"{GetRemainingTime()} sec";
        public bool StillTimeToEdit => !ShowRegularBehaviourText && GetRemainingTime() > 0;
        public abstract bool ShowRegularBehaviourText { get; }
        public virtual bool ShowPrincipalButton => true;

        protected virtual string RegularBehaviourText => Translate(TranslationKeys.General.Confirm);
        protected virtual string AlternateBehaviourText => Translate(TranslationKeys.General.Update);

        public string PrincipalButtonText => ShowRegularBehaviourText ? RegularBehaviourText : AlternateBehaviourText;
        protected abstract int GetRemainingTime();

        protected async Task Timer()
        {
            while (StillTimeToEdit)
            {
                RaisePropertyChanged(nameof(RemainingTime));
                await Task.Delay(1000).ConfigureAwait(false);
                RaisePropertyChanged(nameof(StillTimeToEdit));
                RaisePropertyChanged(nameof(ShowSecondaryButton));
            }
        }

        protected virtual async Task Reload(T profile)
        {
            _subject = profile;
            RaiseAllPropertiesChanged();
            await Timer().ConfigureAwait(false);
        }

        public IMvxViewModel ParentViewModel { get; set; }
    }
}
