using System;
using System.Threading.Tasks;
using Android.Content;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.LifeTime;
using Coinstantine.Domain;
using Plugin.CurrentActivity;
using Xamarin.Auth;

namespace Coinstantine.Droid.Services
{
    [RegisterInterfaceAsDynamic]
    public class DroidTwitterService : TwitterService
    {
        private readonly ILifeCycle _lifeCycle;

        public DroidTwitterService(ILifeCycle lifeCycle)
        {
            _lifeCycle = lifeCycle;
        }

        public override void Authenticate()
        {
            Intent intent = null;
            var auth = new OAuth1Authenticator(
                    ConsumerKey,
                    ConsumerSecretKey,
                    new Uri(RequestTokenUrl),
                    new Uri(AuthorizeUrl),
                    new Uri(AccessTokenUrl),
                    new Uri(CallbackUrl));

            //save the account data in the authorization completed even handler
            auth.Completed += async (s, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    //save the account data for a later session, according to Twitter docs, this doesn't expire
                    _account = eventArgs.Account;
                    var webAuthenticator = CrossCurrentActivity.Current.Activity;
                    webAuthenticator.Finish();
                    await Task.Delay(200);
                    OnAuthenticated();
                }
            };
            _lifeCycle.FireOnClose();
            var currentContext = CrossCurrentActivity.Current.Activity;
            intent = auth.AuthenticationUIPlatformSpecificEmbeddedBrowser(currentContext);
           //intent = auth.GetUI(currentContext);
            currentContext.StartActivity(intent);
        }
    }
}