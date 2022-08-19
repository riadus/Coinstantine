using Foundation;
using Coinstantine.Common.Attributes;
using Coinstantine.Core.UIServices;
using UIKit;
using System.Linq;
using MessageUI;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.iOS.UIServices
{
    [RegisterInterfaceAsDynamic]
    public class AppOpener : NSObject, IAppOpener
    {
        private string TelegramAppUrl => $"tg://resolve?domain={_botName}";
        private string TelegramBrowerUrl => $"https://web.telegram.org/#/im?p=@{_botName}";
        private readonly string _botName;

        public AppOpener(ITelegramBotProvider telegramBotProvider)
        {
            _botName = telegramBotProvider.GetBotLink();
        }

        public bool CanOpenTelegram => UIApplication.SharedApplication.CanOpenUrl(new NSUrl(TelegramAppUrl));

        public void OpenTelegram()
        {
            if (CanOpenTelegram)
            {
                OpenLink(TelegramAppUrl);
            }
        }

        public void OpenTelegramInBrowser()
        {
            OpenLink(TelegramBrowerUrl);
        }

        public void OpenLink(string url)
        {
            InvokeOnMainThread(() =>
            {
                if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url), new UIApplicationOpenUrlOptions(), null);
                }
                else
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
                }
            });
        }

        public void OpenDocument(string pathToFile)
        {
            InvokeOnMainThread(() =>
            {
                var fileData = NSUrl.FromFilename(pathToFile);
                var activityController = new UIActivityViewController(new NSObject[] { fileData }, null);
                var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                rootViewController.PresentViewController(activityController, true, () => { });
            });
        }

        public void ShareText(string text, object sender)
        {
            InvokeOnMainThread(() =>
            {
                var activityViewController = new UIActivityViewController(new[] { new NSString(text) }, null);

                switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIUserInterfaceIdiom.Phone:
                        var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        rootViewController.PresentViewController(activityViewController, true, null);
                        break;

                    case UIUserInterfaceIdiom.Pad:
                        var topViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                        while (topViewController.PresentedViewController != null)
                        {
                            topViewController = topViewController.PresentedViewController;
                        }

                        if (activityViewController?.PopoverPresentationController != null)
                        {
                            var view = sender as UIView ?? topViewController.View.Subviews.ElementAt(0);
                            activityViewController.PopoverPresentationController.SourceView = view;
                        }

                        topViewController.PresentViewController(activityViewController, true, null);
                        break;
                }
            });
        }

        public bool OpenEmailClient(string recipient, string subject)
        {
            if (MFMailComposeViewController.CanSendMail)
            {
                var mailController = new MFMailComposeViewController();
                mailController.SetToRecipients(new string[] { recipient });
                mailController.SetSubject(subject);

                var rootViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
                rootViewController.PresentViewController(mailController, true, null);

                mailController.Finished += (object s, MFComposeResultEventArgs args) =>
                {
                    args.Controller.DismissViewController(true, null);
                };
                return true;
            }
            return false;
        }

        public void ShareText(string text, object sender, ShareOption option)
        {
            ShareText(text, sender);
        }

    }
}
