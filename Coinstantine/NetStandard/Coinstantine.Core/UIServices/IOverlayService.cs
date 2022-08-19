namespace Coinstantine.Core.UIServices
{
    public interface IOverlayService
    {
        void Wait(string message);
        void UpdateMessage(string message);
        void Dismiss();
    }
}
