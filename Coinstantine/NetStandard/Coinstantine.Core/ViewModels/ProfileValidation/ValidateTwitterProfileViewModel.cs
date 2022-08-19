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
    public class ValidateTwitterProfileViewModel : GenericInfoViewModel<TwitterProfile>
    {
        public ValidateTwitterProfileViewModel(IAppServices appServices,
                                               IProfileProvider profileProvider,
                                               IUserService userService,
                                               IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor) : base(appServices, profileProvider, userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.General.TwitterAccount;
            TitleIcon = "twitter";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Twitter validation page");
        }

        public override async void Prepare(TwitterProfile parameter)
        {
            base.Prepare(parameter);

            _subject.Id = _userProfile?.TwitterProfile?.Id ?? 0;
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (ScreenNameText, ScreenName) },
                { (FollowersText, Followers, Display.Grouped) },
                { (CreationDateText, CreationDate) }
            }));

            await Timer().ConfigureAwait(false);
        }

        private TranslationKey FollowersText => TranslationKeys.Twitter.Followers;
        private TranslationKey CreationDateText => TranslationKeys.Twitter.CreationDate;
        private TranslationKey ScreenNameText => TranslationKeys.Twitter.ScreenName;

        public override string InfoTitle => _subject.Username;

        public override bool ShowRegularBehaviourText => !_subject?.Validated ?? true;

        private string Followers => _subject.Followers.ToString();
        private string CreationDate => FormatF(_subject.CreationDate);
        private string ScreenName => _subject.ScreenName;

        protected override async Task SecondaryButtonAction()
        {
            if (_subject.Validated)
            {
                Wait(TranslationKeys.General.Canceling);
                var sucessfullyCanceled = await _userService.CancelTwitterProfile(_subject.TwitterId).ConfigureAwait(false);
                DismissWaitMessage();
                if (sucessfullyCanceled)
                {
                    Close(ParentViewModel);
                    AppNavigationService.ShowTwitterPage();
                }
            }
            else
            {
                _userProfile.TwitterProfile = new TwitterProfile
                {
                    Id = _subject.Id,
                    Validated = false,
                    ValidationDate = null
                };
                await _profileProvider.SaveUserProfile(_userProfile).ConfigureAwait(false);
                Close(ParentViewModel);
                AppNavigationService.ShowTwitterPage();
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

        protected override Task PrincipalButtonAction()
        {
            if (ShowRegularBehaviourText)
            {
                return Validate();
            }
            return Update();
        }

        private async Task Validate()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.Twitter.SettingProfile);

            var (profile, success) = await _userService.SetTwitterProfile(_subject).ConfigureAwait(false);
            DismissWaitMessage();
            if (success)
            {
                _subject = profile;
                await Reload(_subject).ConfigureAwait(false);
            }
            else
            {
                Alert(TranslationKeys.Twitter.AccountNotValidated);
                Close(ParentViewModel);
                AppNavigationService.ShowTwitterPage();
            }
        }

        private async Task Update()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.Twitter.UpdatingProfile);
            var (profile, success) = await _userService.UpdateTwitterProfile(_subject.TwitterId).ConfigureAwait(false);
            DismissWaitMessage();
            if (success)
            {
                await Reload(profile).ConfigureAwait(false);
            }
        }

        protected override async Task Reload(TwitterProfile profile)
        {
            await base.Reload(profile).ConfigureAwait(false);
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (ScreenNameText, ScreenName) },
                { (FollowersText, Followers, Display.Grouped) },
                { (CreationDateText, CreationDate) }
            }));

            RaiseAllPropertiesChanged();
        }
    }
}
