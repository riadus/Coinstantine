using System;
using System.Linq;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;
using Android.Widget;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using Coinstantine.Domain.Interfaces;
using Coinstantine.Domain.Interfaces.Translations;
using MvvmCross.Base;
using Plugin.CurrentActivity;
using static Android.Content.PM.PackageManager;

namespace Coinstantine.Droid.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class AppOpener : IAppOpener
    {
        private readonly ITranslationService _translationService;
        private readonly IMvxMainThreadAsyncDispatcher _mvxMainThreadAsyncDispatcher;

        private string TelegramBrowerUrl => $"https://web.telegram.org/#/im?p=@{_botName}";
        private readonly string _botName;
        public AppOpener(ITranslationService translationService,
                         ITelegramBotProvider telegramBotProvider,
                         IMvxMainThreadAsyncDispatcher mvxMainThreadAsyncDispatcher)
        {
            _translationService = translationService;
            _mvxMainThreadAsyncDispatcher = mvxMainThreadAsyncDispatcher;
            _botName = telegramBotProvider.GetBotLink();
        }

        public bool CanOpenTelegram => IsAppAvailable("org.telegram.messenger");

        public bool OpenEmailClient(string recipient, string subject)
        {
            try
            {
                var intent = new Intent(Intent.ActionSend);
                intent.SetType("plain/text");
                intent.PutExtra(Intent.ExtraEmail, new String[] { recipient });
                intent.PutExtra(Intent.ExtraSubject, subject);
                CrossCurrentActivity.Current.Activity.StartActivity(Intent.CreateChooser(intent, ""));
                return true;
            }
            catch(Exception)
            {
                return false;
            }

        }

        public void OpenLink(string url)
        {
            var intent = new Intent(Intent.ActionView);
            intent.SetData(Android.Net.Uri.Parse(url));
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        public void OpenTelegram()
        {
            if (CanOpenTelegram)
            {
                OpenTelegram(_botName);
            }
        }

        public void OpenTelegram(string userName)
        {
            var general = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://t.com/" + userName));
            var activity = CrossCurrentActivity.Current.Activity;
            var generalResolvers = activity.PackageManager.QueryIntentActivities(general, 0)
                                           .Where(x => x.ActivityInfo.PackageName != null)
                                           .Select(x => x.ActivityInfo.PackageName);

            Intent telegram = new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://t.me/" + userName));
            int goodResolver = 0;
            // gets the list of intents that can be loaded.
            var resInfo = activity.PackageManager.QueryIntentActivities(telegram, 0);
            foreach (var info in resInfo)
            {
                if (info.ActivityInfo.PackageName != null && !generalResolvers.Contains(info.ActivityInfo.PackageName))
                {
                    goodResolver++;
                    telegram.SetPackage(info.ActivityInfo.PackageName);
                }
            }
            //TODO: if there are several good resolvers create custom chooser
            if (goodResolver != 1)
            {
                telegram.SetPackage(null);
            }
            if (telegram.ResolveActivity(activity.PackageManager) != null)
            {
                activity.StartActivity(telegram);
            }
        }

        public void OpenTelegramInBrowser()
        {
            OpenLink(TelegramBrowerUrl);
        }

        public void ShareText(string text, object sender)
        {
            var sendIntent = new Intent();
            sendIntent.SetAction(Intent.ActionSend);
            sendIntent.PutExtra(Intent.ExtraText, text);
            sendIntent.SetType("text/plain");
            CrossCurrentActivity.Current.Activity.StartActivity(sendIntent);
        }

        public void ShareText(string text, object sender, ShareOption option)
        {
            switch(option)
            {
                case ShareOption.Copy:
                    CopyText(text);
                    break;
                case ShareOption.Send:
                    ShareText(text, sender);
                    break;
            }
        }

        public void CopyText(string text)
        {
            _mvxMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync(() =>
            {
                var clipboard = (ClipboardManager)CrossCurrentActivity.Current.AppContext.GetSystemService(Context.ClipboardService);
                clipboard.Text = text;
                var copiedText = _translationService.Translate(TranslationKeys.Home.AddressCopied);
                var toast = Toast.MakeText(CrossCurrentActivity.Current.Activity, copiedText, ToastLength.Short);
                toast.Show();
            });
        }

        private bool IsAppAvailable(string appName)
        {
            var pm = CrossCurrentActivity.Current.AppContext.PackageManager;
            try
            {
                var packageInfo = pm.GetPackageInfo(appName, PackageInfoFlags.Activities);
                return packageInfo != null;
            }
            catch (NameNotFoundException)
            {
                return false;
            }
        }

        public void OpenDocument(string pathToFile)
        {
            Java.IO.File file = new Java.IO.File(pathToFile);
            file.SetReadable(true);
            Android.Net.Uri uri = FileProvider.GetUriForFile(CrossCurrentActivity.Current.Activity, CrossCurrentActivity.Current.Activity.ApplicationContext.PackageName + ".coinstantine.provider", file);
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(uri, "application/pdf");
            intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }
    }
}