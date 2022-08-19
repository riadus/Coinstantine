using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using Android.Util;
using Coinstantine.Common;
using Coinstantine.Core.Services;
using Coinstantine.Domain.Interfaces;
using Firebase.Messaging;
using MvvmCross;
using Plugin.CurrentActivity;

namespace Coinstantine.Droid.Notifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class CoinstantineFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        private readonly INotificationsAnalyserService _notificationsAnalyserService;

        public CoinstantineFirebaseMessagingService()
        {
            _notificationsAnalyserService = Mvx.Resolve<INotificationsAnalyserService>();
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            if (message.GetNotification() != null)
            {
                //These is how most messages will be received
                Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                // SendNotification(message.GetNotification().Body);
            }
            else
            {
                //Only used for debugging payloads sent from the Azure portal
                SendNotification(message.Data);
            }
        }

        void SendNotification(IDictionary<string, string> message)
        {
            message.TryGetValue("title", out var title);
            message.TryGetValue("body", out var body);
            message.TryGetValue("part-to-update", out var partToUpdate);
            message.TryGetValue("silent", out var silentString);
            message.TryGetValue("translationKey", out var translationKey);
            int.TryParse(silentString, out var silent);
            var translatedText = string.Empty;

            var builder = new NotificationCompat.Builder(CrossCurrentActivity.Current.Activity, "Coinstantine Channel")
                                                  .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
                                                  .SetContentTitle(title) // Set the title
                                                  .SetNumber(1) // Display the count in the Content Info
                                                  .SetSmallIcon(Resource.Mipmap.Icon); // This is the icon to display

            if (translationKey != null)
            {
                translatedText = Mvx.Resolve<ITranslationService>().Translate(translationKey);
                builder.SetContentText(translatedText); // the message to display.
            }
            else
            {
                builder.SetContentText(body); // the message to display.
            }

            CreateNotificationChannel();
            // Finally, publish the notification:
            if (silent > 0 || translatedText.IsNotNull())
            {
                var notificationManager = NotificationManagerCompat.From(CrossCurrentActivity.Current.Activity);
                notificationManager.Notify(0, builder.Build());
            }
            _notificationsAnalyserService.HandlePartToUpdate(partToUpdate, IsAppInForeground());
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = "Coinstantine Channel";
            var channelDescription = "Coinstantine channel";
            var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = NotificationManager.FromContext(CrossCurrentActivity.Current.Activity);
            notificationManager.CreateNotificationChannel(channel);
        }

        private bool IsAppInForeground()
        {
            var appProcessInfo = new ActivityManager.RunningAppProcessInfo();
            ActivityManager.GetMyMemoryState(appProcessInfo);
            if (appProcessInfo.Importance == Importance.Foreground || appProcessInfo.Importance == Importance.Visible)
            {
                return true;
            }

            var km = (KeyguardManager)CrossCurrentActivity.Current.Activity.GetSystemService(Context.KeyguardService);
            // App is foreground, but screen is locked, so show notification
            return km.InKeyguardRestrictedInputMode();
        }
    }
}
