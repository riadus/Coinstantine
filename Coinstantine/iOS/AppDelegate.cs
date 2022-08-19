using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Coinstantine.Core;
using Coinstantine.Core.LifeTime;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Coinstantine.iOS.Views.Extensions;
using Foundation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MvvmCross;
using MvvmCross.Platforms.Ios.Core;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace Coinstantine.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : MvxApplicationDelegate<Setup<App>, App>, IUNUserNotificationCenterDelegate, INotificationsRegistrationService
    {
        private SBNotificationHub _hub;
        private NSData _notificationToken;
        private ILifeCycle _lifeCycle;
        private INotificationTokenCache _notificationTokenCache;

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            var result = base.FinishedLaunching(application, launchOptions);

            Mvx.RegisterSingleton<INotificationsRegistrationService>(this);
            _lifeCycle = Mvx.Resolve<ILifeCycle>();
            SetAppCenter();
            var rootController = new UIViewController();
            rootController.View.BackgroundColor = AppColorDefinition.MainBlue.ToUIColor();
            Window.RootViewController = rootController;
            return result;
        }

		private void SetAppCenter()
		{
            var key = NSBundle.MainBundle.ObjectForInfoDictionary("AppCenterKey")?.ToString();
            AppCenter.Start(key, typeof(Analytics), typeof(Crashes));
		}

        private INotificationTokenCache NotificationTokenCache
        {
            get
            {
                return _notificationTokenCache ?? (_notificationTokenCache = Mvx.Resolve<INotificationTokenCache>());
            }
        }

        private void SubscribeForNotifications()
        {
            if(NotificationTokenCache.GetCacheToken() != null)
            {
                return;
            }

            BeginInvokeOnMainThread(() =>
			{
				if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
				{
					UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, HandleAuthorization);
					UNUserNotificationCenter.Current.Delegate = this;
				}
				else
				{
					var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, new NSSet());
					UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
				}

				UIApplication.SharedApplication.RegisterForRemoteNotifications();
			});
        }

        private void HandleAuthorization(bool approved, NSError error)
        {
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var environmentProvider = Mvx.Resolve<IAppEnvironmentProvider>();
            // Create a new notification hub with the connection string and hub path
            _notificationToken = deviceToken;
            NotificationTokenCache.SaveCacheToken(deviceToken.GetBase64EncodedString(NSDataBase64EncodingOptions.None));
            _hub = new SBNotificationHub(environmentProvider.NotificationConnectionString, environmentProvider.NotificationHub);

            // Unregister any previous instances using the device token
            _hub.UnregisterAllAsync(deviceToken, (error) =>
            {
                if (error != null || _email == null)
                {
                    return;
                }

                // Register this device with the notification hub
				_hub.RegisterNativeAsync(deviceToken, new NSSet(new []{ _email }), (registerError) =>
                {

                });
            });
        }

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
			// This method is called when a remote notification is received and the
			// App is in the foreground - i.e., not backgrounded
			Debug.WriteLine($"Notification received at {DateTime.Now}");
			Debug.WriteLine(userInfo);
            // We need to check that the notification has a payload (userInfo) and the payload
            // has the root "aps" key in the dictionary - this "aps" dictionary contains defined
            // keys by Apple which allows the system to determine how to handle the alert
            if (null != userInfo && userInfo.ContainsKey(new NSString("aps")))
            {
                // Get the aps dictionary from the alert payload
                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
				if(aps.ContainsKey(new NSString("part-to-update")))
				{
					var notificationService = Mvx.Resolve<INotificationsAnalyserService>();
					notificationService.HandlePartToUpdate(aps[new NSString("part-to-update")].ToString(), application.ApplicationState == UIApplicationState.Active);
				}
                if(aps.ContainsKey(new NSString("translationKey")))
                {
                    ShowLocalNotification(aps[new NSString("translationKey")].ToString());
                }
                // Here we can do any additional processing upon receiving the notification
                // As the app is in the foreground, we can handle this alert manually
                // here by creating a UIAlert for example
            }
        }

        private void ShowLocalNotification(string translationKey)
        {
            var translationService = Mvx.Resolve<ITranslationService>();
            var notification = new UNMutableNotificationContent
            {
                Title = "Coinstantine",
                Body = translationService.Translate(translationKey),
                Sound = UNNotificationSound.Default
            };

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(20, false);
            var request = UNNotificationRequest.FromIdentifier("NotificationForTranslationKey", notification, trigger);

            UNUserNotificationCenter.Current.AddNotificationRequest(request, error =>
            {
            });
        }

        private string _email;
        public Task RegisterForNotifications(string email)
        {
            _email = email;
            SubscribeForNotifications();
			return Task.FromResult(0);
        }

		public override void DidEnterBackground(UIApplication application)
        {
            _lifeCycle.FireOnClose();
        }

        public override void WillEnterForeground(UIApplication application)
        {
            _lifeCycle.FireOnRestart();
        }

        public Task<bool> AreNotificationsEnabled()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                TaskCompletionSource<bool> result = new TaskCompletionSource<bool>();
                UNUserNotificationCenter.Current.GetNotificationSettings((setting) => result.SetResult(setting.AuthorizationStatus == UNAuthorizationStatus.Authorized));
                return result.Task;
            }
            else
            {
                return Task.FromResult(UIApplication.SharedApplication.IsRegisteredForRemoteNotifications);
            }
        }

        public async Task Unregister()
        {
            if(_hub == null)
            {
                return;
            }
            if(_notificationToken == null)
            {
                var tokenCache = NotificationTokenCache.GetCacheToken();
                if (tokenCache != null)
                {
                    _notificationToken = new NSData(tokenCache, NSDataBase64DecodingOptions.None);
                }
                else
                {
                    return;
                }
            }
            NotificationTokenCache.DeleteCache();
            await _hub.UnregisterAllAsyncAsync(_notificationToken).ConfigureAwait(false);
        }
    }
}
