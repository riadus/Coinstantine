using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Core.ViewModels.Generic;
using Coinstantine.Data;
using Coinstantine.Domain.Airdrops;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.ViewModels;

namespace Coinstantine.Core.ViewModels.ProfileValidation
{
    public class ValidateTelegramProfileViewModel : GenericInfoViewModel<TelegramProfile>
    {
        public ValidateTelegramProfileViewModel(IAppServices appServices,
                                                IProfileProvider profileProvider,
                                                IUserService userService,
                                                IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor) : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.General.TelegramAccount;
            TitleIcon = "telegram-plane";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Telegram validation page");
        }

        public override async void Prepare(TelegramProfile parameter)
        {
            base.Prepare(parameter);

            _subject.Id = _userProfile?.TelegramProfile?.Id ?? 0;

            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (UsernameText, Username) },
                { (FirstNameText, FirstName) },
                { (LastNameText, LastName, Display.Grouped) },
            }));

            await Timer().ConfigureAwait(false);
        }

        public override string InfoTitle => _subject.TelegramId.ToString();
        private string Username => _subject.Username;
        private string FirstName => _subject.FirstName;
        private string LastName => _subject.LastName;

        private TranslationKey UsernameText => TranslationKeys.Telegram.Username;
        private TranslationKey FirstNameText => TranslationKeys.Telegram.FirstName;
        private TranslationKey LastNameText => TranslationKeys.Telegram.LastName;

        public override bool ShowRegularBehaviourText => !_subject?.Validated ?? true;
        public override bool ShowPrincipalButton => ShowRegularBehaviourText;
        protected override async Task SecondaryButtonAction()
        {
            if (_subject.Validated)
            {
                Wait(TranslationKeys.General.Canceling);
                var sucessfullyCanceled = await _userService.CancelTelegramProfile(_subject.Username).ConfigureAwait(false);
                DismissWaitMessage();
                if (sucessfullyCanceled)
                {
                    Close(ParentViewModel);
                    AppNavigationService.ShowTelegramPage();
                }
            }
            else
            {
                _userProfile.TelegramProfile = new TelegramProfile
                {
                    Id = _subject.Id,
                    Validated = false,
                    ValidationDate = null
                };
                await _profileProvider.SaveUserProfile(_userProfile).ConfigureAwait(false);
                Close(ParentViewModel);
                AppNavigationService.ShowTelegramPage();
            }
        }

        protected override Task PrincipalButtonAction()
        {
            if (ShowRegularBehaviourText)
            {
                return Validate();
            }
            return Task.FromResult(0);
        }

        private async Task Validate()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.Telegram.SettingProfile);

            var (profile, success) = await _userService.SetTelegramProfile(_subject.Username).ConfigureAwait(false);
            DismissWaitMessage();
            if (success)
            {
                _subject = profile;
                await Reload(_subject).ConfigureAwait(false);
            }
            else
            {
                Alert(TranslationKeys.Telegram.AccountNotValidated);
                Close(ParentViewModel);
                AppNavigationService.ShowTelegramPage();
            }
        }

        protected override int GetRemainingTime()
        {
            if (_subject.ValidationDate.HasValue)
            {
                var diff = _subject.ValidationDate?.AddMinutes(1) - DateTime.Now;
                return diff?.Seconds ?? 0;
            }
            return 0;
        }

        protected override async Task Reload(TelegramProfile profile)
        {
            await base.Reload(profile);

            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (UsernameText, Username) },
                { (FirstNameText, FirstName) },
                { (LastNameText, LastName, Display.Grouped) },
            }));

            RaiseAllPropertiesChanged();
        }
    }
}
