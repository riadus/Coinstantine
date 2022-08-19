namespace Coinstantine.Domain.Interfaces
{
    public interface INotificationTokenCache
    {
        string GetCacheToken();
        void SaveCacheToken(string token);
        void DeleteCache();
    }
}