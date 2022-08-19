using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.ProfileValidation
{
    public class BitcoinTalkProfileViewModel : BaseViewModel
    {
        private readonly IProfileProvider _profileProvider;
        private readonly IUserService _userService;

        public BitcoinTalkProfileViewModel(IAppServices appServices,
                                           IProfileProvider profileProvider,
                                           IUserService userService) : base(appServices)
        {
            _profileProvider = profileProvider;
            _userService = userService;
            CheckButtonCommand = new MvxCommand(Check);
            Title = TranslationKeys.General.BitcoinTalkAccount;
            TitleIcon = "bitcoin";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Bitcoin talk page");
        }

        public override async void Prepare()
        {
            base.Prepare();
            var user = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            UserId = user.BitcoinTalkProfile?.UserId ?? user.BitcoinTalkProfile?.InAppUserId;
            RaisePropertyChanged(nameof(UserId));
        }

        public string UserId { get; set; }

        public string CheckButtonText => Translate(TranslationKeys.BitcoinTalk.CheckButtonText);
        public string UserIdPlaceholder => Translate(TranslationKeys.BitcoinTalk.UserId);
        public string ExplanationText => Translate(TranslationKeys.BitcoinTalk.ExplanationText);

        public IMvxCommand CheckButtonCommand { get; }

        private void Check()
        {
            if (int.TryParse(UserId, out int result))
            {
                if (!CheckConnectivity())
                {
                    return;
                }

                AlertWithAction(TranslationKeys.BitcoinTalk.Alert, async () =>
                {
                    Wait(TranslationKeys.BitcoinTalk.GettingUser);
                    var (profile, success) = await _userService.GetBitcoinTalkProfile(UserId).ConfigureAwait(false);
                    DismissWaitMessage();

                    if (success)
                    {
                        Close(this);
                        AppNavigationService.ShowBitcoinTalkPage(profile);
                    }
                    else
                    {
                        Alert(TranslationKeys.BitcoinTalk.UserNotFoud);
                    }
                });
            }
        }
    }
}