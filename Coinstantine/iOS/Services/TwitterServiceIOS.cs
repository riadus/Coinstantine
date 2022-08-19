using System;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain;
using Tweetinvi;
using UIKit;
using Xamarin.Auth;

namespace Coinstantine.iOS.Services
{
    [RegisterInterfaceAsDynamic]
    public class TwitterServiceIOS : TwitterService
    {
        public override void Authenticate()
        {
            var auth = new OAuth1Authenticator(
                    ConsumerKey,
                    ConsumerSecretKey,
                    new Uri(RequestTokenUrl),
                    new Uri(AuthorizeUrl),
                    new Uri(AccessTokenUrl),
                    new Uri(CallbackUrl));

            //save the account data in the authorization completed even handler
            auth.Completed += (s, eventArgs) =>
            {
                if (eventArgs.IsAuthenticated)
                {
                    //save the account data for a later session, according to Twitter docs, this doesn't expire
                    UIApplication.SharedApplication.KeyWindow.RootViewController.DismissViewController(true, null);
                    _account = eventArgs.Account;
                    OnAuthenticated();
                }
            };
            var ui = auth.GetUI();
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(ui, true, null);
        }
    }
}