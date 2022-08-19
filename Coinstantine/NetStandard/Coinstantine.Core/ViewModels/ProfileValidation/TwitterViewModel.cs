using System;
using System.Threading.Tasks;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Commands;

namespace Coinstantine.Core.ViewModels.ProfileValidation
{
	public class TwitterViewModel : BaseViewModel
    {
        private readonly ITwitterService _twitterService;
        private readonly IUserService _userService;
        private readonly IProfileProvider _profileProvider;
        private UserProfile _profile;
		public TwitterViewModel(IAppServices appServices,
                                IProfileProvider profilerProvider,
                                IUserService userService,
                                ITwitterService twitterService) : base(appServices)
        {
            _twitterService = twitterService;
            _profileProvider = profilerProvider;
            _userService = userService;
            TwitterButtonText = Translate(TranslationKeys.Twitter.AuthenticateAndTweet);
			ExplanationText = Translate(TranslationKeys.Twitter.ExplanationText);
			AlertText = TranslationKeys.Twitter.Alert;
			AuthenticateAndTweetCommand = new MvxCommand(AutenticateAndTweet);
            Title = TranslationKeys.General.TwitterAccount;
            TitleIcon = "twitter";
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();
            TrackPage("Twitter page");
        }

        public override async Task Initialize()
        {
            _profile = await _profileProvider.GetUserProfile().ConfigureAwait(false);
            TwitterAccount = Translate(TranslationKeys.General.NoLinkedAccount);

            RaiseAllPropertiesChanged();
        }

        public string TwitterAccount { get; set; }
        public string ExplanationText { get; }
        private TranslationKey AlertText { get; }
        public string TwitterButtonText { get; }
        private string TweetText => "Hey, I installed #CoinstantineApp app. @Coinstantine1";

        public IMvxCommand AuthenticateAndTweetCommand { get; }
        private void AutenticateAndTweet()
        {
			if(!AppServices.Connectivity.IsConnected)
			{
				ShowNoConnectionMessage();
				return;
			}

            AlertWithAction(AlertText, () =>
            {
                _twitterService.Authenticated -= _twitterService_Authenticated;
                _twitterService.Authenticated += _twitterService_Authenticated;
                _twitterService.Authenticate();
            });
        }

        async void _twitterService_Authenticated(object sender, EventArgs e)
        {
            Wait(TranslationKeys.Twitter.LoadingTwitter);
            _twitterService.Authenticated -= _twitterService_Authenticated;
            var tweet = _twitterService.Tweet(TweetText);
            if (tweet != null)
            {
                var (twitterProfile, success) = await _userService.GetTwitterProfile(tweet.TwitterId).ConfigureAwait(false);
                {
                    twitterProfile.TweetId = tweet.TweetId;
                    Close(this);
                    AppNavigationService.ShowTwitterPage(twitterProfile);
                }
                DismissWaitMessage();
            }
            else
            {
                DismissWaitMessage();
                Alert(TranslationKeys.Twitter.CouldNotTweet);
            }
        }
    }
}
