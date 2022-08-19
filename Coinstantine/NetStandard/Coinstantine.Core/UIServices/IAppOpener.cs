namespace Coinstantine.Core.UIServices
{
    public interface IAppOpener
    {
        bool CanOpenTelegram { get; }
        void OpenTelegram();
        void OpenTelegramInBrowser();
        void ShareText(string text, object sender);
        void OpenLink(string url);
        void OpenDocument(string pathToFile);
        bool OpenEmailClient(string recipient, string subject);
        void ShareText(string text, object sender, ShareOption option);
    }
}
