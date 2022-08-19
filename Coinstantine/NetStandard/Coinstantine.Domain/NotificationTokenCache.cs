using Coinstantine.Common.Attributes;
using Coinstantine.Domain.Extensions;
using Coinstantine.Domain.Interfaces;

namespace Coinstantine.Domain
{
    [RegisterInterfaceAsLazySingleton]
    public class NotificationTokenCache : INotificationTokenCache
    {
        public NotificationTokenCache(IFile file,
                                      IPathProvider pathProvider)
        {
            _file = file;
            _pathProvider = pathProvider;
        }

        private readonly object FileLock = new object();
        private readonly IFile _file;
        private readonly IPathProvider _pathProvider;

        public string GetCacheToken()
        {
            if (!_file.Exists(_pathProvider.CachePath))
            {
                return null;
            }
            try
            {
                var serializedUser = _file.ReadAllText(_pathProvider.CachePath);
                var cacheToken = serializedUser.DeserializeTo<CacheToken>();
                return cacheToken.Token;
            }
            catch
            {
                return null;
            }
        }

        public void SaveCacheToken(string token)
        {
            lock (FileLock)
            {
                var cacheToken = new CacheToken
                {
                    Token = token
                };
                var serializedUser = cacheToken.Serialize();
                _file.WriteAllText(_pathProvider.CachePath, serializedUser);
            }
        }

        public void DeleteCache()
        {
            lock(FileLock)
            {
                _file.DeleteFile(_pathProvider.CachePath);
            }
        }

        public class CacheToken
        {
            public string Token { get; set; }
        }
    }
}
