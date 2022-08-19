using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Support.V4.App;
using Android.Util;
using Coinstantine.Common;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.Services;
using Coinstantine.Data;
using Coinstantine.Domain.Interfaces;
using Firebase.Iid;
using MvvmCross;
using MvvmCross.Base;
using Plugin.CurrentActivity;
using WindowsAzure.Messaging;

namespace Coinstantine.Droid.Notifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    [RegisterInterfaceAsLazySingleton]
    public class FirebaseService : FirebaseInstanceIdService, INotificationsRegistrationService
    {
        const string TAG = "FirebaseService";
        private IProfileProvider _profileProvider;
        private IProfileProvider ProfileProvider => _profileProvider ?? (Mvx.CanResolve<IProfileProvider>() ? (_profileProvider = Mvx.Resolve<IProfileProvider>()) : null);
        private INotificationTokenCache _notificationTokenCache;
        private INotificationTokenCache NotificationTokenCache => _notificationTokenCache ?? (Mvx.CanResolve<INotificationTokenCache>() ? (_notificationTokenCache = Mvx.Resolve<INotificationTokenCache>()) : null);
        private NotificationHub _hub; 

        public override async void OnTokenRefresh()
        {
            if(NotificationTokenCache != null && FirebaseInstanceId.Instance?.Token != null)
            {
                NotificationTokenCache.SaveCacheToken(FirebaseInstanceId.Instance.Token);
            }
            if(ProfileProvider == null)
            {
                return;
            }
            var profile = await ProfileProvider.GetUserProfile().ConfigureAwait(false);
            var email = profile?.Email;
            if(email == null)
            {
                return;
            }
            await RegisterForNotifications(email).ConfigureAwait(false);
        }

        public async Task RegisterForNotifications(string email = null)
        {
            if (NotificationTokenCache?.GetCacheToken() != null)
            {
                return;
            }
            var token = FirebaseInstanceId.Instance.Token;
            NotificationTokenCache?.SaveCacheToken(token);
            Log.Debug(TAG, "FCM token: " + token);
            var environmentProvider = Mvx.Resolve<IAppEnvironmentProvider>();
            _hub = new NotificationHub(environmentProvider.NotificationHub,
                                       environmentProvider.NotificationConnectionString, CrossCurrentActivity.Current.Activity);

            var tags = new List<string>();

            if(email.IsNotNull())
            {
                tags.Add(email);
            }
            await Task.Run(() =>
            {
                try
                {
                    var regID = _hub.Register(token, tags.ToArray()).RegistrationId;

                    Log.Debug(TAG, $"Successful registration of ID {regID}");
                }
                catch(Exception)
                {
                    //fails for Emulator
                }
            });
        }

        public Task<bool> AreNotificationsEnabled()
        {
            var hasPermissions = NotificationManagerCompat.From(CrossCurrentActivity.Current.Activity).AreNotificationsEnabled();
            return Task.FromResult(hasPermissions);
        }

        public async Task Unregister()
        {
            var token = FirebaseInstanceId.Instance.Token;
            if (token == null)
            {
                var tokenCache = NotificationTokenCache.GetCacheToken();
                if (tokenCache != null)
                {
                    token = tokenCache;
                }
                else
                {
                    return;
                }
            }
            NotificationTokenCache.DeleteCache();
            await Task.Factory.StartNew (() => _hub?.UnregisterAll(token));
        }
    }
}