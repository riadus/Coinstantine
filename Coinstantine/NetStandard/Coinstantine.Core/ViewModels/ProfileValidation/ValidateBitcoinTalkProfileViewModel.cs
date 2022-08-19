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
    public class ValidateBitcoinTalkProfileViewModel : GenericInfoViewModel<BitcoinTalkProfile>
    {
        public ValidateBitcoinTalkProfileViewModel(IAppServices appServices,
                                                   IProfileProvider profileProvider,
                                                   IUserService userService,
                                                   IGenericInfoItemViewModelConstructor itemInfoViewModelConstructor) : base(appServices, profileProvider,userService, itemInfoViewModelConstructor)
        {
            Title = TranslationKeys.General.BitcoinTalkAccount;
            TitleIcon = "bitcoin";
        }


        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Bitcoin talk validation page");
        }

        public override async void Prepare(BitcoinTalkProfile parameter)
        {
            base.Prepare(parameter);

            _subject.Id = _userProfile?.BitcoinTalkProfile?.Id ?? 0;

            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (UsernameText, Username) },
                { (PostsText, Posts, Display.Grouped) },
                { (ActivityText, Activity) },
                { (RankText, Rank, Display.Grouped) },
                { (RegistrationDateText, RegistrationDate) },
                { (LocationText, Location) },
            }));

            await Timer().ConfigureAwait(false);
        }

        public override string InfoTitle => _subject.UserId;
        private string Username => _subject.Username;
        private string Posts => _subject.Posts.ToString();
        private string Activity => _subject.Activity.ToString();
        private string Rank => _subject.Rank;
        private string RegistrationDate => FormatF(_subject.RegistredDate);
        private string Location => _subject.Location;

        private TranslationKey UsernameText => TranslationKeys.BitcoinTalk.Username;
        private TranslationKey PostsText => TranslationKeys.BitcoinTalk.Posts;
        private TranslationKey ActivityText => TranslationKeys.BitcoinTalk.Activity;
        private TranslationKey RankText => TranslationKeys.BitcoinTalk.Rank;
        private TranslationKey RegistrationDateText => TranslationKeys.BitcoinTalk.RegistrationDate;
        private TranslationKey LocationText => TranslationKeys.BitcoinTalk.Location;

        public override bool ShowRegularBehaviourText => !_subject?.Validated ?? true;

        protected override async Task SecondaryButtonAction()
        {
            if (_subject.Validated)
            {
                Wait(TranslationKeys.General.Canceling);
                var sucessfullyCanceled = await _userService.CancelBitcoinTalkProfile(_subject.UserId).ConfigureAwait(false);
                DismissWaitMessage();
                if (sucessfullyCanceled)
                {
                    Close(ParentViewModel);
                    AppNavigationService.ShowBitcoinTalkPage();
                }
            }
            else
            {
                _userProfile.BitcoinTalkProfile = new BitcoinTalkProfile
                {
                    Id = _subject.Id,
                    Validated = false,
                    ValidationDate = null
                };
                await _profileProvider.SaveUserProfile(_userProfile).ConfigureAwait(false);
                Close(ParentViewModel);
                AppNavigationService.ShowBitcoinTalkPage();
            }
        } 

        protected override Task PrincipalButtonAction()
        {
            if(ShowRegularBehaviourText)
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
            Wait(TranslationKeys.BitcoinTalk.SettingProfile);

            var (profile, success) = await _userService.SetBitcoinTalkProfile(_subject.UserId).ConfigureAwait(false);
            DismissWaitMessage();
            if (success)
            {
                _subject = profile;
                await Reload(_subject).ConfigureAwait(false);
            }
            else
            {
                Alert(TranslationKeys.BitcoinTalk.AccountNotValidated);
                Close(ParentViewModel);
                AppNavigationService.ShowBitcoinTalkPage();
            }
        }

        private async Task Update()
        {
            if (!AppServices.Connectivity.IsConnected)
            {
                ShowNoConnectionMessage();
                return;
            }
            Wait(TranslationKeys.BitcoinTalk.UpdatingProfile);
            var (profile, success) = await _userService.UpdateBitcoinTalkProfile(_subject.UserId).ConfigureAwait(false);
            DismissWaitMessage();
            if (success)
            {
                await Reload(profile).ConfigureAwait(false);
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

        protected async override Task Reload(BitcoinTalkProfile profile)
        {
            await base.Reload(profile);
            GenericInfoItems = new MvxObservableCollection<GenericInfoItemViewModel>(_itemInfoViewModelConstructor.Construct(new List<ItemInfo>{
                { (UsernameText, Username) },
                { (PostsText, Posts, Display.Grouped) },
                { (ActivityText, Activity) },
                { (RankText, Rank, Display.Grouped) },
                { (RegistrationDateText, RegistrationDate) },
                { (LocationText, Location) },
            }));

            RaiseAllPropertiesChanged();
        }
    }
}
