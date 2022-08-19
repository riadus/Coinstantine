using System;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Tweetinvi;

namespace Coinstantine.Domain
{
    public abstract class TwitterService : ITwitterService
    {
        protected string ConsumerKey = "erCtN34A6YbXDi9IYl30FmM0R";
        protected string ConsumerSecretKey = "ArhhHeQBQxIBwu0FW16Hxt6oSX7y38DpkmoyVaULUhW4UuTKdl";
        protected string RequestTokenUrl = "https://api.twitter.com/oauth/request_token";
        protected string AuthorizeUrl = "https://api.twitter.com/oauth/authorize";
        protected string AccessTokenUrl = "https://api.twitter.com/oauth/access_token";
        protected string CallbackUrl = "https://mobile.twitter.com/home";

        protected Xamarin.Auth.Account _account;

        public event EventHandler Authenticated;
        protected void OnAuthenticated()
        {
            Authenticated?.Invoke(this, EventArgs.Empty);
        }
        public abstract void Authenticate();

        public TwitterProfile Tweet(string text)
        {
            try
            {
                if (_account == null)
                {
                    throw new Exception("User not authenticated");
                }
                var accessToken = _account.Properties["oauth_token"];
                var accessTokenSecret = _account.Properties["oauth_token_secret"];
                var key = _account.Properties["oauth_consumer_key"];
                var secret = _account.Properties["oauth_consumer_secret"];

                Tweetinvi.Auth.SetUserCredentials(key, secret, accessToken, accessTokenSecret);
                var authenticatedUser = User.GetAuthenticatedUser();
                var tweet = User.GetAuthenticatedUser().PublishTweet(text);
                return new TwitterProfile
                {
                    ScreenName = authenticatedUser.ScreenName,
                    Followers = authenticatedUser.FollowersCount,
                    CreationDate = authenticatedUser.CreatedAt,
                    Username = authenticatedUser.Name,
                    TwitterId = authenticatedUser.Id,
                    TweetId = tweet.Id
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
